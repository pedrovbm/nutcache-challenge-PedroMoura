using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using X.PagedList;

namespace ChallengeWebApi.Models
{
    // classe responsável por manter a lista de pessoas paginada
    public class PersonPageInfo
    {
        public int? pageSize;

        public StaticPagedList<Person> Persons { get; set; }

    }
}
