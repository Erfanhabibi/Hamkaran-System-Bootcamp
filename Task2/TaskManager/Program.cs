namespace TaskPriorityManager
{
    public enum Priority
    {
        Low,
        Medium,
        High
    }

    public class Task
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime CreationDate { get; set; }
        public DateTime DueDate { get; set; }
        public Priority Priority { get; set; }
    }

    class Program
    {
        static void Main(string[] args)
        {
            List<Task> tasks = GetSampleTasks();
            Task closestTask = FindClosestTask(tasks);

            if (closestTask != null)
            {
                Console.WriteLine("Closest Task:");
                PrintTaskDetails(closestTask);
            }
            else
            {
                Console.WriteLine("No tasks found!");
            }
        }

        static Task FindClosestTask(List<Task> tasks)
        {
            if (!tasks.Any()) return null;

            DateTime today = DateTime.Today;

            // Find the closest due date (absolute difference from today)
            var closestDateGroup = tasks
                .GroupBy(t => Math.Abs((t.DueDate.Date - today).Days))
                .OrderBy(g => g.Key)
                .First();

            // Get tasks with the closest due date and select highest priority
            return closestDateGroup
                .OrderByDescending(t => t.Priority)
                .First();
        }

        static List<Task> GetSampleTasks()
        {
            return new List<Task>
            {
                new Task
                {
                    Title = "Complete Project",
                    Description = "Finish the priority task manager",
                    CreationDate = new DateTime(2023, 10, 1),
                    DueDate = DateTime.Today.AddDays(1),
                    Priority = Priority.High
                },
                new Task
                {
                    Title = "Team Meeting",
                    Description = "Weekly status update",
                    CreationDate = new DateTime(2023, 10, 2),
                    DueDate = DateTime.Today.AddDays(2),
                    Priority = Priority.Medium
                },
                new Task
                {
                    Title = "Code Review",
                    Description = "Review new feature implementation",
                    CreationDate = new DateTime(2023, 10, 3),
                    DueDate = DateTime.Today.AddDays(3),
                    Priority = Priority.High
                },
                new Task
                {
                    Title = "Documentation Update",
                    Description = "Update documentation",
                    CreationDate = new DateTime(2023, 10, 4),
                    DueDate = DateTime.Today.AddDays(-2),
                    Priority = Priority.Low
                }
            };
        }

        static void PrintTaskDetails(Task task)
        {
            Console.WriteLine($"Title: {task.Title}");
            Console.WriteLine($"Description: {task.Description}");
            Console.WriteLine($"Creation Date: {task.CreationDate:d}");
            Console.WriteLine($"Due Date: {task.DueDate:d}");
            Console.WriteLine($"Priority: {task.Priority}");
            Console.WriteLine();
        }

    }
}