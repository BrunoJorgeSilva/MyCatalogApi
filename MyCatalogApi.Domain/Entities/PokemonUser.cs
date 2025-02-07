using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyCatalogApi.Domain.Entities
{
    public class PokemonUser
    {
        public int PokemonUserId { get; set; }
        public int PokemonId { get; set; }
        public int UserId { get; set; }
    }
}
