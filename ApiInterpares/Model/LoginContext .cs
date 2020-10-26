using Microsoft.EntityFrameworkCore;

namespace LoginApi.Models
{
    public class LoginContext : DbContext
    {
        public LoginContext(DbContextOptions<LoginContext> options)
            : base(options)
        {
        }

        public DbSet<Login> Login { get; set; }
        public DbSet<Grupo> Grupo { get; set; }
    }
}