using System.ComponentModel.DataAnnotations;

namespace ToDoList.Requests.TodoRequest
{
    public class EditTodoRequest
    {
        [Required]
        public string Title { get; set; } = string.Empty;

        [Required]
        public bool IsCompleted { get; set; }

        [Required]
        public DateTime Date { get; set; } = DateTime.UtcNow.Date;
    }
}
