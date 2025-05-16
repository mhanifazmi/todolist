using ToDoList.Models;
using ToDoList.Requests.TodoRequest;
using ToDoList.Repositories.Interfaces;
using ToDoList.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;

namespace ToDoList.Services
{
    public class TodoService : ITodoService
    {
        private readonly ITodoRepository _repo;

        public TodoService(ITodoRepository repo)
        {
            _repo = repo;
        }

        public async Task<List<Todo>?> GetTodosByDateAsync(DateTime? date = null)
        {
            return await _repo.GetTodoByDateAsync(date);
        }

        public async Task<List<Todo>?> GetTodosByIdsAsync(List<int>? ids = null)
        {
            return await _repo.GetTodoByIdsAsync(ids);
        }

        public async Task<Todo?> GetTodoAsync(int id)
        {
            return await _repo.GetByIdAsync(id);
        }

        public async Task<Todo> CreateTodoAsync(CreateTodoRequest request)
        {
            var sequence = await _repo.GetNextSequenceAsync(request.Date);

            var todo = new Todo
            {
                Title = request.Title,
                IsCompleted = false,
                Date = request.Date,
                CreatedAt = DateTime.UtcNow,
                Sequence = sequence
            };

            return await _repo.AddAsync(todo);
        }

        public async Task<Todo?> UpdateTodoAsync(int id, EditTodoRequest request)
        {
            var todo = await _repo.GetByIdAsync(id);
            if (todo == null) return null;

            todo.Title = request.Title;
            todo.Date = request.Date;

            await _repo.UpdateAsync(todo);

            return todo;
        }

        public async Task<Todo?> UpdateStatusAsync(int id, UpdateStatusTodoRequest request)
        {
            var todo = await _repo.GetByIdAsync(id);
            if (todo == null) return null;

            todo.IsCompleted = request.IsCompleted;
            todo.CompletedAt = request.IsCompleted ? request.CompletedAt ?? DateTime.UtcNow : null;

            await _repo.UpdateAsync(todo);
            return todo;
        }

        public async Task<bool> DeleteTodoAsync(int id)
        {
            var todo = await _repo.GetByIdAsync(id);
            if (todo == null) return false;

            await _repo.DeleteAsync(todo);
            return true;
        }

        public async Task<List<Todo>> CopyTasksAsync(CopyTodoTasksRequest request)
        {
            var tasksToCopy = await _repo.GetTodoByDateAsync(request.FromDate);

            if (!tasksToCopy.Any())
                return new List<Todo>();

            var copiedTasks = tasksToCopy.Select(t => new Todo
            {
                Title = t.Title,
                Date = request.ToDate,
                IsCompleted = false,
                CompletedAt = null,
            }).ToList();

            await _repo.AddRangeAsync(copiedTasks);

            return copiedTasks;
        }

        public async Task<bool> ReorderTodosAsync(ReorderTodoRequest request)
        {
            for (int i = 0; i < request.Ids.Count; i++)
            {
                int id = request.Ids[i];
                var todo = await _repo.GetByIdAsync(id);
                if (todo == null) continue;

                todo.Sequence = i + 1;

                await _repo.UpdateAsync(todo);
            }

            return true;
        }
    }
}

