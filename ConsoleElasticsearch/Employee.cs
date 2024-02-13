// See https://aka.ms/new-console-template for more information

public class Employee
{
    public Guid Id { get; set; } = Guid.NewGuid();
    //[Keyword]
    public string Name { get; set; } = string.Empty;
    public int Age { get; set; }
    public string Department { get; set; } = string.Empty;
    public bool IsDeleted { get; set; }
    public double Salary { get; set; }
    public DateTime Created { get; set; } = DateTime.Now;


    public override string ToString()
    { 
        const int padding = 5;
        return $"Id: {Id.ToString().PadRight(padding)}, Name: {Name.PadRight(padding)}, Age: {Age.ToString().PadRight(padding)}, Salary: {Salary.ToString().PadRight(padding)}, Created: {Created.ToString().PadRight(padding)},Department: {Department.PadRight(padding)}";
    }
}


public static class EmployeesExtensions
{
    public static void Print(this IReadOnlyCollection<Employee> employees)
    {
        if (!employees.Any())
        {
            Console.WriteLine("not data");
        }
        foreach (Employee employee in employees)
        {
            Console.WriteLine(employee);
        }
        Console.WriteLine("________________________________________________________________________");
    }
}