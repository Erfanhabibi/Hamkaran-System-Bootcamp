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

    public class TaskItem : IComparable<TaskItem>
    {
        public string? Title { get; set; }
        public string? Description { get; set; }
        public DateTime CreationDate { get; set; }
        public DateTime DueDate { get; set; }
        public Priority Priority { get; set; }

        // Compares tasks first by closeness of due date and then by priority.
        public int CompareTo(TaskItem? other)
        {
            if (other == null) return 1;

            int dateComparison = Math.Abs((DueDate - DateTime.Today).Days)
                .CompareTo(Math.Abs((other.DueDate - DateTime.Today).Days));

            return dateComparison == 0 
                ? other.Priority.CompareTo(Priority) 
                : dateComparison;
        }
    }

    public class ComparerHeap<T> where T : IComparable<T>
    {
        private readonly List<T> _elements;
        private readonly IComparer<T> _comparer;

        public ComparerHeap(IComparer<T> comparer)
        {
            _elements = new List<T>();
            _comparer = comparer;
        }

        public int Count => _elements.Count;

        public void Insert(T item)
        {
            _elements.Add(item);
            HeapifyUp(_elements.Count - 1);
        }

        public T RemoveTop()
        {
            if (_elements.Count == 0)
                throw new InvalidOperationException("Heap is empty.");

            T top = _elements[0];
            _elements[0] = _elements[_elements.Count - 1];
            _elements.RemoveAt(_elements.Count - 1);
            HeapifyDown(0);
            return top;
        }

        public T Peek()
        {
            if (_elements.Count == 0)
                throw new InvalidOperationException("Heap is empty.");
            return _elements[0];
        }

        private void HeapifyUp(int index)
        {
            while (index > 0)
            {
                int parent = (index - 1) / 2;
                if (_comparer.Compare(_elements[parent], _elements[index]) <= 0)
                    break;
                Swap(parent, index);
                index = parent;
            }
        }

        private void HeapifyDown(int index)
        {
            while (true)
            {
                int leftChild = 2 * index + 1;
                int rightChild = 2 * index + 2;
                int smallest = index;

                if (leftChild < _elements.Count && _comparer.Compare(_elements[leftChild], _elements[smallest]) < 0)
                    smallest = leftChild;
                if (rightChild < _elements.Count && _comparer.Compare(_elements[rightChild], _elements[smallest]) < 0)
                    smallest = rightChild;
                if (smallest == index)
                    break;
                Swap(index, smallest);
                index = smallest;
            }
        }

        private void Swap(int a, int b)
        {
            T temp = _elements[a];
            _elements[a] = _elements[b];
            _elements[b] = temp;
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            List<TaskItem> tasks = GetSampleTasks();
            ComparerHeap<TaskItem> taskHeap = new ComparerHeap<TaskItem>(Comparer<TaskItem>.Default);
            
            foreach (var task in tasks)
            {
                taskHeap.Insert(task);
            }

            if (taskHeap.Count > 0)
            {
                Console.WriteLine("Closest Task:");
                PrintTaskDetails(taskHeap.Peek());
            }
            else
            {
                Console.WriteLine("No tasks found!");
            }
        }

        static List<TaskItem> GetSampleTasks()
        {
            return new List<TaskItem>
            {
                new TaskItem 
                { 
                    Title = "Complete Project", 
                    Description = "Finish the priority task manager", 
                    CreationDate = DateTime.Today.AddDays(-10), 
                    DueDate = DateTime.Today.AddDays(1), 
                    Priority = Priority.High 
                },
                new TaskItem 
                { 
                    Title = "Team Meeting", 
                    Description = "Weekly status update", 
                    CreationDate = DateTime.Today.AddDays(-5), 
                    DueDate = DateTime.Today.AddDays(2), 
                    Priority = Priority.Medium 
                },
                new TaskItem 
                { 
                    Title = "Code Review", 
                    Description = "Review new feature implementation", 
                    CreationDate = DateTime.Today.AddDays(-3), 
                    DueDate = DateTime.Today.AddDays(3), 
                    Priority = Priority.High 
                },
                new TaskItem 
                { 
                    Title = "Documentation Update", 
                    Description = "Update documentation", 
                    CreationDate = DateTime.Today.AddDays(-15), 
                    DueDate = DateTime.Today.AddDays(-2), 
                    Priority = Priority.Low 
                }
            };
        }

        static void PrintTaskDetails(TaskItem task)
        {
            Console.WriteLine($"Title: {task.Title}");
            Console.WriteLine($"Description: {task.Description}");
            Console.WriteLine($"Creation Date: {task.CreationDate:d}");
            Console.WriteLine($"Due Date: {task.DueDate:d}");
            Console.WriteLine($"Priority: {task.Priority}");
        }
    }
}