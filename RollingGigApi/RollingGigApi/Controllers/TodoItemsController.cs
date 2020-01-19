using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RollingGigApi.Models;
using RollingGigApi.ViewModels;

namespace RollingGigApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TodoItemsController : ControllerBase
    {
        private readonly RollingGigContext _context;

        public TodoItemsController(RollingGigContext context)
        {
            _context = context;
        }

        // GET: api/TodoItems
        [HttpGet]
        public async Task<ActionResult<IEnumerable<TodoItemDetailsViewModel>>> GetTodoItems()
        {
            var todoItems =  await _context.TodoItems.Include(x => x.TodoItemTags).ThenInclude(x => x.Tag).ToListAsync();
            
            var model = new List<TodoItemDetailsViewModel>();
            foreach (var todoItem in todoItems)
            {
                var item = new TodoItemDetailsViewModel
                {
                    Id = todoItem.Id,
                    Title = todoItem.Title,
                    IsComplete = todoItem.IsComplete,
                    LastModified = todoItem.LastModified
                };

                foreach (var tag in todoItem.TodoItemTags)
                {
                    item.Tags.Add(new TagDetailsViewModel {
                        Id = tag.Tag.Id,
                        Name = tag.Tag.Name,
                        LastModified = tag.Tag.LastModified
                    });
                }

                model.Add(item);
            }

            return model;
        }

        // GET: api/TodoItems/5
        [HttpGet("{id}")]
        public async Task<ActionResult<TodoItemDetailsViewModel>> GetTodoItem(long id)
        {
            var todoItem = await _context.TodoItems.Include(x => x.TodoItemTags).ThenInclude(x => x.Tag).FirstOrDefaultAsync(x => x.Id == id);

            if (todoItem == null)
            {
                return NotFound();
            }

            var model = new TodoItemDetailsViewModel
            {
                Id = todoItem.Id,
                IsComplete = todoItem.IsComplete,
                Title = todoItem.Title,
                LastModified = todoItem.LastModified
            };

            foreach (var tag in todoItem.TodoItemTags)
            {
                model.Tags.Add(new TagDetailsViewModel
                {
                    Id = tag.Tag.Id,
                    Name = tag.Tag.Name,
                    LastModified = tag.Tag.LastModified
                });
            }

            return model;
        }

        // PUT: api/TodoItems/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see https://aka.ms/RazorPagesCRUD.
        [HttpPut("{id}")]
        public async Task<IActionResult> PutTodoItem(long id, TodoItemCreateEditViewModel model)
        {
            if (id != model.Id)
            {
                return BadRequest();
            }

            var todoItem = await _context.TodoItems.Include(x => x.TodoItemTags).ThenInclude(x => x.Tag).FirstOrDefaultAsync(x => x.Id == id);
            if (todoItem == null)
            {
                return NotFound();
            }

            todoItem.Title = model.Title;
            todoItem.IsComplete = model.IsComplete;
            todoItem.LastModified = DateTime.Now;

            var existingTags = new HashSet<long>(todoItem.TodoItemTags.Select(t => t.TagId));
            var selectedTags = new HashSet<long>(model.Tags.Select(t => t.Id));
            foreach (var todoItemTag in todoItem.TodoItemTags)
            {
                if (!selectedTags.Contains(todoItemTag.TagId))
                {
                    todoItem.TodoItemTags.Remove(todoItemTag);
                }
            }
            foreach (var tag in model.Tags)
            {
                if (!existingTags.Contains(tag.Id))
                {
                    var todoItemTag = new TodoItemTag { TagId = tag.Id, TodoItemId = todoItem.Id };
                    todoItem.TodoItemTags.Add(todoItemTag);
                }
            }

            _context.Entry(todoItem).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!TodoItemExists(id))
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

        // POST: api/TodoItems
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see https://aka.ms/RazorPagesCRUD.
        [HttpPost]
        public async Task<ActionResult<TodoItem>> PostTodoItem(TodoItemCreateEditViewModel model)
        {
            var todoItem = new TodoItem
            {
                Title = model.Title,
                IsComplete = false,
                LastModified = DateTime.Now,
                TodoItemTags = new List<TodoItemTag>()
            };
            
            foreach (var tag in model.Tags)
            {
                todoItem.TodoItemTags.Add(new TodoItemTag { TagId = tag.Id });
            }

            _context.TodoItems.Add(todoItem);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetTodoItem), new { id = todoItem.Id }, todoItem);
        }

        // DELETE: api/TodoItems/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<TodoItem>> DeleteTodoItem(long id)
        {
            var todoItem = await _context.TodoItems.FindAsync(id);
            if (todoItem == null)
            {
                return NotFound();
            }

            _context.TodoItems.Remove(todoItem);
            await _context.SaveChangesAsync();

            return todoItem;
        }

        private bool TodoItemExists(long id)
        {
            return _context.TodoItems.Any(e => e.Id == id);
        }
    }
}
