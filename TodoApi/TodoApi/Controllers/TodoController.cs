using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TodoApi.Models;


namespace TodoApi.Controllers
{
    [Authorize]
    [Route("api/todo")]
    [ApiController]
    public class TodoController : ControllerBase
    {
        private readonly TodoContext _context;

        public TodoController(TodoContext context)
        {
            _context = context;

            if (_context.TodoItems.Count() == 0)
            {
                _context.TodoItems.Add(new TodoItem { Model = "Neon", Brand = "Dodge", Type = "medium", isAvailable = true });
                _context.TodoItems.Add(new TodoItem { Model = "Spark", Brand = "Chevrolet", Type = "small", isAvailable = true });
                _context.TodoItems.Add(new TodoItem { Model = "Figo", Brand = "Ford", Type = "medium", isAvailable = true });
                _context.TodoItems.Add(new TodoItem { Model = "Civic", Brand = "Honda", Type = "medium", isAvailable = true });
                _context.TodoItems.Add(new TodoItem { Model = "Accord", Brand = "Honda", Type = "medium", isAvailable = true });
                _context.SaveChanges();
            }
        }


        [HttpPost]
        public async Task<ActionResult<IEnumerable<TodoItemOut>>> GetTodoItems(TodoItemEntry itemEntry)
        {

            var listItems =  await _context.TodoItems.ToListAsync();
            List<TodoItemOut> listOut = new List<TodoItemOut>();
            
            foreach (TodoItem item in listItems)
            {
                if(item.isAvailable == true && (item.Type == itemEntry.Type || itemEntry.Type == null) && (item.Brand == itemEntry.Brand || itemEntry.Brand == null) && (item.Model == itemEntry.Model || itemEntry.Model == null) )
                //if(item.isAvailable == true)
                {
                    TodoItemOut itemOut = new TodoItemOut();
                    itemOut.Id = item.Id;
                    itemOut.Type = item.Type;
                    itemOut.Model = item.Model;
                    itemOut.Brand = item.Brand;

                    listOut.Add(itemOut);
                }
            }

            return listOut;

        }

        /*
        // GET: api/Todo/5
        [HttpGet("{id}")]
        public async Task<ActionResult<TodoItem>> GetTodoItem(long id)
        {
            var todoItem = await _context.TodoItems.FindAsync(id);

            if (todoItem == null)
            {
                return NotFound();
            }

            return todoItem;
        }
        */


        // PUT: api/Todo/5
        [HttpPut("{id}")]
        public async Task<ActionResult<bool>> PutTodoItem(long id)
        {
            var todoItem = await _context.TodoItems.FindAsync(id);

            if (todoItem == null)
            {
                return BadRequest();
            }

            if (todoItem.isAvailable == false)
            {
                todoItem.isAvailable = true;
            }
            else if (todoItem.isAvailable == true)
            {
                todoItem.isAvailable = false;
            }

            
            await _context.SaveChangesAsync();

            return true;
        }



    }
}
