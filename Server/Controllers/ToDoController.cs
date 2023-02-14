using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Server.Entities;

namespace Server.Controllers {
    [ApiController]
    [Route("api/[controller]")]
    public class ToDoController : ControllerBase {
        private readonly ApplicationDbContext _context;

        public ToDoController(ApplicationDbContext context) {
            _context = context;
        }
        [HttpGet]
        public async Task<ActionResult<List<ToDo>>> GetToDo() {
            var response = await _context.ToDos!.ToListAsync();

            if (response == null)
                return BadRequest("There are not to do list");

            return Ok(response);
        }
        [HttpGet("{id}")]
        public async Task<ActionResult<ToDo>> GetToDoById(int id) {
            var response = await _context.ToDos!.FindAsync(id);

            if (response == null)
                return BadRequest($"There not exist or is {response}");

            return Ok(response);
        }
        [HttpPost]
        public async Task<ActionResult<List<ToDo>>> PostToDo(ToDoDto request) {
            if (request == null)
                return BadRequest($"The ToDo is {request}!!");

            var toDo = new ToDo {
                Title = request.Title,
                Description = request.Description,
                Done = request.Done,
                CreatedAt = DateTime.Now
            };

            _context.Add(toDo);
            await _context.SaveChangesAsync();

            return Ok(await _context.ToDos!.ToListAsync());
        }
        [HttpPut("{id}")]
        public async Task<ActionResult<ToDo>> PutToDo(int id, ToDoDto request) {
            if (request == null)
                return BadRequest($"The ToDo is {request}!!");

            var response = await _context.ToDos!.FindAsync(id);

            if (response == null)
                return BadRequest($"There not exist or is {response}");

            response.Title = request.Title;
            response.Description = request.Description;
            response.Done = request.Done;
            response.UpdatedAt = DateTime.Now;

            await _context.SaveChangesAsync();

            return Ok(response);
        }
        [HttpDelete("{id}")]
        public async Task<ActionResult<ToDo>> DeleteToDo(int id) {
            var response = await _context.ToDos!.FindAsync(id);

            if (response == null)
                return BadRequest($"The ToDo not exist or is {response}!!");

            _context.Remove(response);
            await _context.SaveChangesAsync();

            return Ok(response);
        }
    }
}