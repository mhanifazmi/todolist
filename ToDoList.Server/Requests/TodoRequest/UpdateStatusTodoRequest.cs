using System.ComponentModel.DataAnnotations;

namespace ToDoList.Requests.TodoRequest
{
    public class UpdateStatusTodoRequest
    {
        [Required]
        public bool IsCompleted { get; set; }

        public DateTime? CompletedAt { get; set; } = DateTime.UtcNow;
    }
}
