using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LoginApi.Models
{
    public class Grupo
    {
        public int Id { get; set; }
        public string Nome { get; set; }
        public List<Login> Logins { get; set; }
    }
}
