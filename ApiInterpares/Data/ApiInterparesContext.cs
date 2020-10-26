using Microsoft.EntityFrameworkCore;
using LoginApi.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using System.Threading.Tasks;
using System;

namespace ApiInterpares.Data
{
    public class ApiInterparesContext : IdentityDbContext
    {
        public ApiInterparesContext(DbContextOptions<ApiInterparesContext> options)
            : base(options)
        {
        }

        public DbSet<Login> Login { get; set; }

        internal Task PasswordSignInManager(string userName, string password)
        {
            throw new NotImplementedException();
        }
    }
}