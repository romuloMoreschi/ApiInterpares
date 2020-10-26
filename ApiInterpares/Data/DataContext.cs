using Microsoft.EntityFrameworkCore;
using LoginApi.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace ApiInterpares.Data
{
    public class DataContext : IdentityDbContext
    {
        public DataContext(DbContextOptions<DataContext> options)
            : base(options)
        {
        }

        public DbSet<Login> Login { get; set; }
    }
}