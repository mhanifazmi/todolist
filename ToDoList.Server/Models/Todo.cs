namespace ToDoList.Models
{
    public class Todo
    {
        public int Id { get; set; }

        public string Title { get; set; } = string.Empty;

        public bool IsCompleted { get; set; }
        public int Sequence { get; set; } = 1;

        public DateTime? CompletedAt { get; set; } 

        public DateTime Date { get; set; } = DateTime.UtcNow.Date;

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}