using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using LoginApi.Models;
using Microsoft.AspNetCore.Authorization;
using ApiInterpares.Services;
using ApiInterpares.Data;
using Microsoft.AspNetCore.Identity;
using System.IdentityModel.Tokens.Jwt;

namespace WebApplication1.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TodosLoginController : ControllerBase{
        private readonly ApiInterparesContext _context;
        private readonly UserManager<Login> _signInManager;

        public TodosLoginController(ApiInterparesContext context, UserManager<Login> signInManager)
        {
            _context = context;
            _signInManager = signInManager;
        }

        [HttpPost]
        [Route("Login")]
        [AllowAnonymous]
        public async Task<ActionResult<dynamic>> AutenticaLogin([FromBody] Login login)
        {
            var user = await _signInManager.FindByNameAsync(login.UserName).ConfigureAwait(false);
            if (user == null)
            {
                return BadRequest("Usuario nao encontrado");
            }
            if (await _signInManager.CheckPasswordAsync(user, login.UserName).ConfigureAwait(false))
            {
                var token = TokenService.GenerateToken(user);
                return new
                {
                    user = user,
                    token = token
                };
            }

            return BadRequest("Usuario nao encontrado");
        }

        // GET: api/TodoLogin
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Login>>> GetTodosLogins(){
            return await _context.Login.ToListAsync();
        }

        // GET: api/TodoLogin/5
        [HttpGet("{id:long}")]
        public async Task<ActionResult<Login>> GetLoginId(long id){
            var todoLogin = await _context.Login.FindAsync(id);

            if (todoLogin == null){
                return NotFound();
            }

            return todoLogin;
        }

        // POST: api/TodoLogin
        [HttpPost]
        public async Task<ActionResult<Login>> PostLogin(Login newLogin){
            _context.Login.Add(newLogin);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetLoginId), new { id = newLogin.Id }, newLogin);
        }        

        [HttpPut("{id}")]
        public async Task<IActionResult> PutTodoLogin(long id, Login todoLogin){
            if (id.Equals(todoLogin.Id)){
                return BadRequest();
            }

            _context.Entry(todoLogin).State = EntityState.Modified;

            try{
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException){
                if (!TodoLoginExists(id)){
                    return NotFound();
                }
                else{
                    throw;
                }
            }

            return NoContent();
        }
        
        // DELETE: api/TodoLogin/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<Login>> DeleteTodoLogin(long id){
            var todoLogin = await _context.Login.FindAsync(id);
            if (todoLogin == null){
                return NotFound();
            }

            _context.Login.Remove(todoLogin);
            await _context.SaveChangesAsync();

            return todoLogin;
        }

        private bool TodoLoginExists(long id){
            return _context.Login.Any(e => e.Id.Equals(id));
        }
    }
}
