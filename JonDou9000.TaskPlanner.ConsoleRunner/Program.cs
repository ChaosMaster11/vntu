using JonDou9000.TaskPlanner.Domain.Models;
using JonDou9000.TaskPlanner.Domain.Models.Enums;
using JonDou9000.TaskPlanner.Domain.Logic;

internal static class Program
{
    private static readonly SimpleTaskPlanner TaskPlanner = new SimpleTaskPlanner();
    private static readonly WorkItem[] WorkItems = new WorkItem[10];

    public static void Main(string[] args)
    {

        while (true)
        {
            Console.WriteLine("\nMenu:");
            Console.WriteLine("[A]dd work item");
            Console.WriteLine("[B]uild a plan");
            Console.WriteLine("[M]ark work item as completed");
            Console.WriteLine("[R]emove a work item");
            Console.WriteLine("[Q]uit the app");
            Console.Write("Choose an option: ");

            char option = Console.ReadKey().KeyChar;
            Console.WriteLine();

            switch (char.ToUpper(option))
            {
                case 'A':
                    AddWorkItem();
                    break;
                case 'B':
                    BuildPlan();
                    break;
                case 'M':
                    MarkAsCompleted();
                    break;
                case 'R':
                    RemoveWorkItem();
                    break;
                case 'Q':
                    Console.WriteLine("Exiting the app.");
                    return;
                default:
                    Console.WriteLine("Invalid option. Please try again.");
                    break;
            }
        }
    }

    private static void AddWorkItem()
    {
        Console.WriteLine("Creating a new WorkItem:");
        WorkItem newWorkItem = CreateWorkItemFromConsoleInput();
        Console.WriteLine("Work item added successfully.");
    }

    private static void BuildPlan()
    {
        WorkItem[] sortedWorkItems = TaskPlanner.CreatePlan(WorkItems);
        Console.WriteLine("Sorted WorkItems:");
        foreach (var item in sortedWorkItems)
        {
            Console.WriteLine(item.ToString());
        }
    }

    private static void MarkAsCompleted()
    {
        Console.WriteLine("Work item marked as completed.");
    }

    private static void RemoveWorkItem()
    {
        Console.WriteLine("Work item removed successfully.");
    }

    private static WorkItem CreateWorkItemFromConsoleInput()
    {
        Console.Write("Title: ");
        string title = Console.ReadLine();

        Console.Write("Description: ");
        string description = Console.ReadLine();

        Console.Write("Priority (None, Low, Medium, High, Urgent): ");
        if (!Enum.TryParse(Console.ReadLine(), true, out Priority priority))
        {
            Console.WriteLine("Invalid priority. Defaulting to None.");
            priority = Priority.None;
        }

        Console.Write("Complexity (None, Minutes, Hours, Days, Weeks): ");
        if (!Enum.TryParse(Console.ReadLine(), true, out Complexity complexity))
        {
            Console.WriteLine("Invalid complexity. Defaulting to None.");
            complexity = Complexity.None;
        }

        Console.Write("Due Date (dd/MM/yyyy): ");
        if (!DateTime.TryParseExact(Console.ReadLine(), "dd/MM/yyyy", null, System.Globalization.DateTimeStyles.None, out DateTime dueDate))
        {
            Console.WriteLine("Invalid date format. Defaulting to current date.");
            dueDate = DateTime.Today;
        }

        return new WorkItem
        {
            Id = Guid.NewGuid(),
            CreationDate = DateTime.Now,
            DueDate = dueDate,
            Priority = priority,
            Complexity = complexity,
            Title = title,
            Description = description,
            IsCompleted = false
        };
    }
}
