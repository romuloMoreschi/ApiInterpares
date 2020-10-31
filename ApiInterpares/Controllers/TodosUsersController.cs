using ApiInterpares.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ApiInterpares.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TodosUsersController : ControllerBase
    {
        private readonly ApiInterparesContext _context;
        public TodosUsersController(ApiInterparesContext context)
        {
            _context = context;
        }

        [HttpGet]
        [Authorize]
        public async Task<List<IdentityUser>> GetTodosUsers()
        {
            return await _context.Users.ToListAsync();
        }

        [HttpGet("{id}")]
        [Authorize]
        public async Task<ActionResult<IdentityUser>> GerUserId(string id)
        {
            var login = await _context.Login.FindAsync(id);

            if (login.Equals(null))
            {
                return NotFound();
            }

            return login;
        }

        [HttpPut("{id}")]
        [Authorize]
        public async Task<IActionResult> AlteraUser(string id, IdentityUser login)
        {
            if (id.Equals(login.Id))
            {
                return BadRequest();
            }

            _context.Entry(login).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!TodoLoginExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        [HttpDelete("{id}")]
        [Authorize]
        public async Task<ActionResult<IdentityUser>> DeleteUser(string id)
        {
            var todoLogin = await _context.Login.FindAsync(id);
            if (todoLogin == null)
            {
                return NotFound();
            }

            _context.Login.Remove(todoLogin);
            await _context.SaveChangesAsync();

            return todoLogin;
        }

        private bool TodoLoginExists(string id)
        {
            return _context.Login.Any(e => e.Id.Equals(id));
        }

    }
}
