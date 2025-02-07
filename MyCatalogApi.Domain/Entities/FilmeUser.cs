using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyCatalogApi.Domain.Entities
{
    public class FilmeUser
    {
        public int  FilmeUserId  { get; set; }
        public int FilmeId { get; set; }
        public int UserId { get; set; }
    }
}
