using ToDoList.Models;

namespace ToDoList.Repositories.Interfaces
{
    public interface ITodoRepository
    {
        Task<List<Todo>> GetTodoByDateAsync(DateTime? date = null);
        Task<List<Todo>> GetTodoByIdsAsync(List<int>? ids = null);
        Task<Todo?> GetByIdAsync(int id);
        Task<Todo> AddAsync(Todo todo);
        Task UpdateAsync(Todo todo);
        Task DeleteAsync(Todo todo);
        Task AddRangeAsync(IEnumerable<Todo> todo);

        Task<int> GetNextSequenceAsync(DateTime date);
    }
}
