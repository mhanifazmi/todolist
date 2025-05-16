using System.Collections.Generic;
using ToDoList.Models;
using ToDoList.Requests.TodoRequest;

namespace ToDoList.Services.Interfaces
{
    public interface ITodoService
    {
        Task<List<Todo>?> GetTodosByDateAsync(DateTime? date = null);
        Task<List<Todo>?> GetTodosByIdsAsync(List<int>? ids = null);
        Task<Todo?> GetTodoAsync(int id);
        Task<Todo> CreateTodoAsync(CreateTodoRequest request);
        Task<Todo?> UpdateTodoAsync(int id, EditTodoRequest request);
        Task<Todo?> UpdateStatusAsync(int id, UpdateStatusTodoRequest request);
        Task<bool> DeleteTodoAsync(int id);
        Task<List<Todo>> CopyTasksAsync(CopyTodoTasksRequest request);
        Task<bool> ReorderTodosAsync(ReorderTodoRequest request);
    }
}
