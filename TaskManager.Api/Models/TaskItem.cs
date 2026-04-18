namespace TaskManager.Api.Models
{
    public enum TaskStatus
    {
        Backlog,
        InWork,
        Testing,
        Done
    }

    public class TaskItem
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public string Title { get; set; }
        public string Description { get; set; }
        public TaskStatus Status { get; set; } = TaskStatus.Backlog;
    }
}
