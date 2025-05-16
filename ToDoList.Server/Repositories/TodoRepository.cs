using Microsoft.EntityFrameworkCore;
using ToDoList.Models;
using ToDoList.Repositories.Interfaces;
using ToDoList.Services.Interfaces;

namespace ToDoList.Repositories
{
    public class TodoRepository : ITodoRepository
    {
        private readonly ToDoDbContext _context;

        public TodoRepository(ToDoDbContext context)
        {
            _context = context;
        }

        public async Task<List<Todo>> GetTodoByDateAsync(DateTime? date = null)
        {
            var query = _context.Todos.OrderBy(t => t.Sequence).AsQueryable();
            if (date.HasValue)
                query = query.Where(t => t.Date.Date == date.Value.Date);

            return await query.ToListAsync();
        }

        public async Task<List<Todo>> GetTodoByIdsAsync(List<int>? ids = null)
        {
            var query = _context.Todos.AsQueryable();
            if (ids.Count() > 0)
                query = query.Where(t => ids.Contains(t.Id));

            return await query.ToListAsync();
        }

        public async Task<List<Todo>> GetTodosAsync(DateTime? date = null)
        {
            var query = _context.Todos.AsQueryable();
            if (date.HasValue)
                query = query.Where(t => t.Date.Date == date.Value.Date);

            return await query.ToListAsync();
        }

        public async Task<Todo?> GetByIdAsync(int id)
        {
            return await _context.Todos.FindAsync(id);
        }

        public async Task<Todo> AddAsync(Todo todo)
        {
            _context.Todos.Add(todo);
            await _context.SaveChangesAsync();
            return todo;
        }

        public async Task UpdateAsync(Todo todo)
        {
            _context.Todos.Update(todo);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(Todo todo)
        {
            _context.Todos.Remove(todo);
            await _context.SaveChangesAsync();
        }

        public async Task AddRangeAsync(IEnumerable<Todo> todos)
        {
            await _context.Todos.AddRangeAsync(todos);
            await _context.SaveChangesAsync();
        }

        public async Task<int> GetNextSequenceAsync(DateTime date)
        {
            var maxSequence = await _context.Todos
                .Where(t => t.Date.Date == date.Date)
                .MaxAsync(t => (int?)t.Sequence) ?? 0;

            return maxSequence + 1;
        }
    }
}
