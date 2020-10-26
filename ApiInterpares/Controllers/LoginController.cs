using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using LoginApi.Models;
using Microsoft.AspNetCore.Authorization;
using ApiInterpares.Services;

namespace WebApplication1.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TodosLoginController : ControllerBase{
        private readonly LoginContext _context;

        public TodosLoginController(LoginContext context){
            _context = context;
        }

        [HttpPost]
        [Route("Login")]
        [AllowAnonymous]
        public async Task<ActionResult<dynamic>> Authenticate([FromBody] Login model)
        {
            var user = await _context.Login.FindAsync(model.UserName, model.Password);

            if (user == null)
            {
                return NotFound(new { Message = "Usuário ou senha inválidos" });
            }

            var token = TokenService.GenerateToken(user);
            user.Password = "";
            return new
            {
                user = user,
                token = token
            };
        }

        // GET: api/TodoLogin
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Login>>> GetTodoItems(){
            return await _context.Login.ToListAsync();
        }

        // GET: api/TodoLogin/5
        [HttpGet("{id:long}")]
        public async Task<ActionResult<Login>> GetTodoLogin(long id){
            var todoLogin = await _context.Login.FindAsync(id);

            if (todoLogin == null){
                return NotFound();
            }

            return todoLogin;
        }

        // POST: api/TodoLogin
        [HttpPost]
        public async Task<ActionResult<Login>> PostTodoLogin(Login newLogin){
            _context.Login.Add(newLogin);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetTodoLogin), new { id = newLogin.Id }, newLogin);
        }        

        [HttpPut("{id}")]
        public async Task<IActionResult> PutTodoLogin(long id, Login todoLogin){
            if (id != todoLogin.Id){
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
            return _context.Login.Any(e => e.Id == id);
        }
    }
}
