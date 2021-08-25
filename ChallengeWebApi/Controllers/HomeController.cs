using ChallengeWebApi.Data;
using ChallengeWebApi.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using X.PagedList;

namespace ChallengeWebApi.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        /* Metodo index é o endpoint responsável por trazer a listagem das pessoas cadastradas, vai receber um data context do serviço 
         * e a página, onde a ideia é carregar uma tabela paginada */
        public IActionResult Index([FromServices] DataContext context, int? page = 1)
        {
            PersonPageInfo personPageInfo = new PersonPageInfo();
            if (page < 0)
            {
                page = 1;
            }
            var pageIndex = (page ?? 1) - 1; // garantir q o pag n seja null, e por se tratar do index da menos 1
            var pageSize = 5; // tamanho de cada página
            int totalPersonCount = context.Persons.Count();
            var persons = PersonPagination(context.Persons, pageIndex, pageSize); //método responsável pelo select dos itens da página referente
            var personPagedList = new StaticPagedList<Person>(persons, pageIndex + 1, pageSize, totalPersonCount);
            personPageInfo.Persons = personPagedList;
            personPageInfo.pageSize = page;
            return View(personPageInfo); // retorno da view com a tabela preenchida 
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        // Um get feito para retornar todas as pessoas inclusas no context, no momento a utilização dele tornou-se obsoleta com o index trazendo a paginação dos dados 
        [HttpGet]
        public async Task<ActionResult<List<Person>>> Get([FromServices] DataContext context)
        {
            var persons = await context.Persons.ToListAsync();
            return persons;
        }

        /* Principal endpoint, o post vai servir tanto para inclusão dos novos dados como para update no caso de 
         * atualização das informações. vai receber como parametro todos os dados de pessoa */
        [HttpPost]
        public async Task<ActionResult<Person>> Post(
            [FromServices] DataContext context,
            int? id, string name, string birthdate, string email, string cpf, string gender, string startdate, string? team)
        {
            // faz a checagem se o modelo é válido, caso negativo já estoura um badrequest, para checar se o modelo
            // é valido leva em consideração os fields obrigatórios
            if (ModelState.IsValid)
            {
                // montagem do person q será atualizado ou inserido
                Person person = new Person();
                person.Name = name;
                person.BirthDate = birthdate;
                person.Email = email;
                person.Cpf = cpf;
                person.Gender = gender;
                person.StartDate = startdate;
                person.Team = team;
                // caso o id seja nulo é pq vai ser um insert, se n, será o update
                if (id != null)
                {
                    // atualiza as informações 
                    person.Id = id ?? 1;
                    context.Persons.Update(person);
                } else
                {
                    // realiza o insert do novo cadastro
                    context.Persons.Add(person);
                }
                // como o post se trata de uma operação assincrona, é necessário o await pra finalizar o save nas informações do context
                await context.SaveChangesAsync();
                //redireciona para a página inicial para entrar no index e recarregar as informações da tabela
                return Redirect("https://localhost:44370");
            }
            else
            {
                return BadRequest(ModelState);
            }
        }

        // endpoint para remover a pessoa desejada. optei por fazer uma requisição do tipo delete pra ficar diferente,
        // porém poderia ter feito um outro post com essa finalidade. Vai receber o context e o id do person a ser excluido
        [HttpDelete]
        public async Task<ActionResult<Person>> Delete(
            [FromServices] DataContext context,
            int id)
        {
            // as validações do model q também são feitas no post
            if (ModelState.IsValid)
            {
                // caso tudo ok, irá buscar o objeto na lista e entãoo excluir, depois rodar o await por se tratar de uma operação assincrona e por fim redirecionar
                // para a página principal
                Person person = context.Persons.Find(id);
                context.Persons.Remove(person);
                await context.SaveChangesAsync();
                return Redirect("https://localhost:44370");
            }
            else
            {
                return BadRequest(ModelState);
            }
        }

        // método auxiliar para buscar dentro de todo meu banco os dados referentes a uma página específica
        private List<Person> PersonPagination (DbSet<Person> persons, int page, int pageSize)
        {
            var index = page * pageSize;
            var lastPersons = persons.Count() - index;
            List<Person> personsPage = persons.ToList().GetRange(index, (lastPersons >= pageSize ? pageSize : lastPersons));
            return personsPage;
        }
    }
}
