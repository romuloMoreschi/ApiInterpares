using LoginApi.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using System.Threading.Tasks;

namespace ApiInterpares.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RegisterController : ControllerBase
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        public RegisterController (UserManager<IdentityUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            _userManager = userManager;
            _roleManager = roleManager;
        }

        [HttpPost]
        public async Task<ActionResult> Register(RegisterViewModel registerViewModel)
        {
            if (ModelState.IsValid)
            {
                var user = new IdentityUser
                {
                    NormalizedUserName = registerViewModel.Nome,
                    UserName = registerViewModel.Login,
                    PhoneNumber = registerViewModel.PhoneNumber,
                    Email = registerViewModel.Email
                };

                var result = await _userManager.CreateAsync(user, registerViewModel.Password).ConfigureAwait(false);
                if (result.Succeeded)
                {
                    bool roleExists = await _roleManager.RoleExistsAsync(registerViewModel.RoleName).ConfigureAwait(false);
                    if (!roleExists)
                    {
                        await _roleManager.CreateAsync(new IdentityRole(registerViewModel.RoleName)).ConfigureAwait(false);
                    }

                    if (!await _userManager.IsInRoleAsync(user, registerViewModel.RoleName).ConfigureAwait(false))
                    {
                        await _userManager.AddToRoleAsync(user, registerViewModel.RoleName).ConfigureAwait(false);
                    }
                    if (!string.IsNullOrWhiteSpace(user.Email))
                    {
                        Claim claim = new Claim(ClaimTypes.Email, user.Email);
                        await _userManager.AddClaimAsync(user, claim).ConfigureAwait(false);
                    }
                    return Ok();
                }
                foreach(var error in result.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }
                return BadRequest(ModelState);
            }

            return BadRequest(ModelState);
        }

    }
}
