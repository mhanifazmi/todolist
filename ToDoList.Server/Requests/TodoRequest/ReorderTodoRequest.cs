using System.ComponentModel.DataAnnotations;

namespace ToDoList.Requests.TodoRequest
{
    public class ReorderTodoRequest
    {
        [Required]
        public List<int>? Ids { get; set; } = new();
    }
}
