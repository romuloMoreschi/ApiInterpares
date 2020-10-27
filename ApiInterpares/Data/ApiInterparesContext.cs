using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;

namespace ApiInterpares.Data
{
    public class ApiInterparesContext : IdentityDbContext<IdentityUser>
    {
        public ApiInterparesContext(DbContextOptions<ApiInterparesContext> options)
            : base(options)
        {
        }

        public DbSet<IdentityUser> Login { get; set; }
    }
}