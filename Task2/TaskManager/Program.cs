using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
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
            string filePath = "tasks.csv";

            List<Task> tasks = ReadTasksFromCSV(filePath);

            if (tasks.Count == 0)
            {
                Console.WriteLine("No tasks found in CSV!");
                return;
            }

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

        static List<Task> ReadTasksFromCSV(string filePath)
        {
            List<Task> tasks = new List<Task>();

            if (!File.Exists(filePath))
            {
                Console.WriteLine("CSV file not found!");
                return tasks;
            }

            string[] lines = File.ReadAllLines(filePath);
            for (int i = 1; i < lines.Length; i++) // Skip header row
            {
                string[] values = lines[i].Split(',');

                if (values.Length < 5)
                {
                    Console.WriteLine($"Skipping invalid row: {lines[i]}");
                    continue;
                }

                try
                {
                    Task task = new Task
                    {
                        Title = values[0].Trim(),
                        Description = values[1].Trim(),
                        CreationDate = DateTime.ParseExact(values[2].Trim(), "yyyy-MM-dd", CultureInfo.InvariantCulture),
                        DueDate = DateTime.ParseExact(values[3].Trim(), "yyyy-MM-dd", CultureInfo.InvariantCulture),
                        Priority = Enum.Parse<Priority>(values[4].Trim(), true),
                        Completed = string.IsNullOrWhiteSpace(values[5]) ? (DateTime?)null : DateTime.ParseExact(values[5].Trim(), "yyyy-MM-dd", CultureInfo.InvariantCulture)
                    };

                    tasks.Add(task);
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error parsing row: {lines[i]}. Error: {ex.Message}");
                }
            }

            return tasks;
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