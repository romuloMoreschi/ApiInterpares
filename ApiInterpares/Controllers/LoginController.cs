using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using ApiInterpares.Services;
using ApiInterpares.Settings;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.Extensions.Options;
using ApiInterpares.ViewModel;

namespace ApiInterpares.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LoginController : ControllerBase
    {
        private readonly UserManager<IdentityUser> _signInManager;
        private readonly JwtSecurityTokenSettings _jwt;

        public LoginController(UserManager<IdentityUser> signInManager, IOptions<JwtSecurityTokenSettings> jwt)
        {
            _signInManager = signInManager;
            this._jwt = jwt.Value;
        }

        [HttpPost]
        public async Task<IActionResult> Post(LoginViewModel loginModel)
        {
            TokenService tokenService = new TokenService(_signInManager, _jwt);

            var user = await _signInManager.FindByNameAsync(loginModel.Login).ConfigureAwait(false);
            if (user == null)
                return BadRequest("Usuário não encontrado.");
            if (await _signInManager.CheckPasswordAsync(user, loginModel.Password).ConfigureAwait(false))
            {
                JwtSecurityToken jwtSecurityToken = await tokenService.GenerateToken(user).ConfigureAwait(false);
                return Ok(new JwtSecurityTokenHandler().WriteToken(jwtSecurityToken));
            }
            return BadRequest(new string[] { "Senha inválida." });
        }
    }
}
