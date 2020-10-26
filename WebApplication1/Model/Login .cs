using System.ComponentModel.DataAnnotations.Schema;

namespace LoginApi.Models
{
    public class Login
    {
        public long Id { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public string Nome { get; set; }

        [ForeignKey("GrupoId")]
        public Grupo Grupo { get; set; }
        public int GrupoId { get; set; }
    }
}