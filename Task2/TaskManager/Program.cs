using System;
using System.Collections.Generic;
using System.Linq;

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
        public string? Title { get; set; }
        public string? Description { get; set; }
        public DateTime CreationDate { get; set; }
        public DateTime DueDate { get; set; }
        public Priority Priority { get; set; }
        public DateTime? Completed { get; set; }
    }

    class Program
    {
        static void Main(string[] args)
        {
            List<Task> tasks = GetSampleTasks();

            // Existing closest task logic remains
            Task? closestTask = FindClosestTask(tasks);
            if (closestTask != null)
            {
                Console.WriteLine("Closest Task:");
                PrintTaskDetails(closestTask);
            }
            else
            {
                Console.WriteLine("No tasks found!");
            }

            Console.WriteLine("==== Reports ====");

            // Total number of tasks
            int totalTasks = tasks.Count;
            Console.WriteLine($"Total number of tasks: {totalTasks}");

            // Number of completed tasks
            int completedTasks = tasks.Count(t => t.Completed != null);
            Console.WriteLine($"Number of completed tasks: {completedTasks}");

            // Number of completed tasks within a specified date range
            DateTime fromDate = DateTime.Today.AddDays(-10); // for example
            DateTime toDate = DateTime.Today;

            int completedInRange = tasks.Count(t => t.Completed != null && t.Completed.Value.Date >= fromDate && t.Completed.Value.Date <= toDate);
            Console.WriteLine($"Number of completed tasks from {fromDate:d} to {toDate:d}: {completedInRange}");

            // Number of incomplete tasks
            int incompleteTasks = tasks.Count(t => t.Completed == null);
            Console.WriteLine($"Number of incomplete tasks: {incompleteTasks}");

            // Number of overdue tasks
            int overdueTasks = tasks.Count(t => t.Completed == null && t.DueDate.Date < DateTime.Today);
            Console.WriteLine($"Number of overdue tasks: {overdueTasks}");

            // The last three tasks that were created more than five days ago and are still incomplete
            var oldIncompleteTasks = tasks.Where(t => t.Completed == null && t.CreationDate <= DateTime.Today.AddDays(-5))
                                          .OrderByDescending(t => t.CreationDate)
                                          .Take(3)
                                          .ToList();
            Console.WriteLine("Last three tasks created more than five days ago and still incomplete:");
            foreach (var task in oldIncompleteTasks)
            {
                PrintTaskDetails(task);
            }

            // The last three tasks that were created and completed on the same day
            var sameDayTasks = tasks.Where(t => t.Completed != null && t.CreationDate.Date == t.Completed.Value.Date)
                                    .OrderByDescending(t => t.CreationDate)
                                    .Take(3)
                                    .ToList();
            Console.WriteLine("Last three tasks that were created and completed on the same day:");
            foreach (var task in sameDayTasks)
            {
                PrintTaskDetails(task);
            }

            // Number of incomplete tasks categorized by priority
            var incompleteByPriority = tasks.Where(t => t.Completed == null)
                                            .GroupBy(t => t.Priority)
                                            .Select(g => new { Priority = g.Key, Count = g.Count() });
            Console.WriteLine("Incomplete tasks categorized by priority:");
            foreach (var group in incompleteByPriority)
            {
                Console.WriteLine($"{group.Priority}: {group.Count}");
            }
        }

        static Task? FindClosestTask(List<Task> tasks)
        {
            if (tasks == null || tasks.Count == 0)
                return null;
        
            DateTime today = DateTime.Today;
            Task? closestTask = null;
            int bestDifference = int.MaxValue;
        
            foreach (var task in tasks)
            {
                int difference = Math.Abs((task.DueDate.Date - today).Days);
                if (difference < bestDifference ||
                   (difference == bestDifference && (closestTask == null || task.Priority > closestTask.Priority)))
                {
                    bestDifference = difference;
                    closestTask = task;
                }
            }
        
            return closestTask;
        }

        static List<Task> GetSampleTasks()
        {
            // Hardcoded sample data illustrating various scenarios (some completed, some not)
            return new List<Task>
            {
                new Task
                {
                    Title = "Complete Project",
                    Description = "Finish the priority task manager",
                    CreationDate = DateTime.Today.AddDays(-7),
                    DueDate = DateTime.Today.AddDays(1),
                    Priority = Priority.High,
                    Completed = DateTime.Today.AddDays(-6) // Completed task (different day)
                },
                new Task
                {
                    Title = "Team Meeting",
                    Description = "Weekly status update",
                    CreationDate = DateTime.Today.AddDays(-8),
                    DueDate = DateTime.Today.AddDays(2),
                    Priority = Priority.Medium,
                    Completed = DateTime.Today.AddDays(-8) // Created and completed on the same day
                },
                new Task
                {
                    Title = "Code Review",
                    Description = "Review new feature implementation",
                    CreationDate = DateTime.Today.AddDays(-3),
                    DueDate = DateTime.Today.AddDays(3),
                    Priority = Priority.High,
                    Completed = null // Incomplete
                },
                new Task
                {
                    Title = "Documentation Update",
                    Description = "Update documentation",
                    CreationDate = DateTime.Today.AddDays(-10),
                    DueDate = DateTime.Today.AddDays(-2),
                    Priority = Priority.Low,
                    Completed = null // Incomplete and overdue
                },
                new Task
                {
                    Title = "Bug Fixing",
                    Description = "Fix reported issues",
                    CreationDate = DateTime.Today.AddDays(-12),
                    DueDate = DateTime.Today.AddDays(-1),
                    Priority = Priority.Medium,
                    Completed = DateTime.Today.AddDays(-10) // Completed task (different day)
                },
                new Task
                {
                    Title = "New Feature Design",
                    Description = "Design the interface",
                    CreationDate = DateTime.Today.AddDays(-15),
                    DueDate = DateTime.Today.AddDays(5),
                    Priority = Priority.High,
                    Completed = DateTime.Today.AddDays(-15) // Created and completed on the same day
                },
                new Task
                {
                    Title = "Client Feedback",
                    Description = "Review client feedback",
                    CreationDate = DateTime.Today.AddDays(-20),
                    DueDate = DateTime.Today.AddDays(-5),
                    Priority = Priority.Low,
                    Completed = null // Incomplete and overdue
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
            Console.WriteLine($"Completed: {(task.Completed.HasValue ? task.Completed.Value.ToString("d") : "Not Completed")}");
            Console.WriteLine();
        }
    }
}