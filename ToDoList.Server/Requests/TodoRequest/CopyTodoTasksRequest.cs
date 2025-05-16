using System.ComponentModel.DataAnnotations;

namespace ToDoList.Requests.TodoRequest
{
    public class CopyTodoTasksRequest
    {
        public DateTime FromDate { get; set; }
        public DateTime ToDate { get; set; }
    }
}
