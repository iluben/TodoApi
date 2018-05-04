using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using TodoApi.Models;
using System.Linq;

namespace TodoApi.Controllers
{
    [Route("api/[controller]")]
    public class TodoController : Controller
    {
        private readonly TodoContext _context;

        public TodoController(TodoContext context)
        {
            this._context = context;

            if (_context.Todo.Count() == 0)
            {
                _context.Todo.Add(new Todo { Name = "Item1" });
                _context.SaveChanges();
            }
        }

        // GET /api/todo
        [HttpGet]
        public IEnumerable<Todo> GetAll() {
            return _context.Todo.ToList();
        }

         // GET /api/todo/{id}
        [HttpGet("{id}", Name = "GetTodo")]
        public IActionResult GetById(long id) {

            var item = _context.Todo.FirstOrDefault(t => t.Id == id);

            if (item == null)
            {
                return NotFound();
            }
            return new ObjectResult(item);
        }

        // // GET /api/todo/{id}
        // [HttpGet("{id}")]
        // public IActionResult GetById2(long id) {

        //     var item = _context.Todo.FirstOrDefault(t => t.Id == id);

        //     if (item == null)
        //     {
        //         return NotFound();
        //     }
        //     return new ObjectResult(item);
        // }

        [HttpPost]
        public IActionResult Create([FromBody] Todo item) {
            if (item == null) 
            {
                return BadRequest();
            }

            _context.Todo.Add(item);
            _context.SaveChanges();

            CreatedAtRouteResult route = CreatedAtRoute("GetTodo", new { id = item.Id }, item);
            return route;
        }
    }
}