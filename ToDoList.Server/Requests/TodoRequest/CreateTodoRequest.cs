using System.ComponentModel.DataAnnotations;

namespace ToDoList.Requests.TodoRequest
{
    public class CreateTodoRequest
    {
        [Required]
        public string Title { get; set; } = string.Empty;

        [Required]
        public DateTime Date { get; set; } = DateTime.UtcNow.Date;
    }
}
