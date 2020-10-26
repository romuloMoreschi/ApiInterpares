using Microsoft.AspNetCore.Identity;

namespace LoginApi.Models
{
    public class Login : IdentityUser
    {
        public string Name { get; set; }
    }
}