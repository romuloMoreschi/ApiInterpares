using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using LoginApi.Models;

namespace WebApplication1.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TodoLoginController : ControllerBase{
        private readonly LoginContext _context;

        public TodoLoginController(LoginContext context){
            _context = context;
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

        // GET: api/TodoLogin/5
        [HttpGet("{nome}")]
        public async Task<ActionResult> PesquisarPorNome(String nome)
        {
            var encontrado = await _context.Login.AnyAsync(p => p.UserName.ToUpper().Contains(nome.ToUpper()));
            if (encontrado == null)
            {
                return NotFound();
            }

            return Ok(encontrado);
        }

        // PUT: api/TodoLogin/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see https://aka.ms/RazorPagesCRUD.
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

        // POST: api/TodoLogin
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see https://aka.ms/RazorPagesCRUD.
        [HttpPost]
        public async Task<ActionResult<Login>> PostTodoLogin(Login todoLogin){
            _context.Login.Add(todoLogin);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetTodoLogin), new { id = todoLogin.Id }, todoLogin);
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
