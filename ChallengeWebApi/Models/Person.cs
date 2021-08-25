using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ChallengeWebApi.Models
{
    // model de pessoa q vai conter os campos desejados na especificação com o required para a validação do model dos campos obrigatórios
    public class Person
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "Este campo é obrigatório")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Este campo é obrigatório")]
        public string BirthDate { get; set; }

        [Required(ErrorMessage = "Este campo é obrigatório")]
        public string Gender { get; set; }

        [Required(ErrorMessage = "Este campo é obrigatório")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Este campo é obrigatório")]
        public string Cpf { get; set; }

        [Required(ErrorMessage = "Este campo é obrigatório")]
        public string StartDate { get; set; }

        public string? Team { get; set; }
    }
}
