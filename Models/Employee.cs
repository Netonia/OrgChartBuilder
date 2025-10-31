namespace OrgChartBuilder.Models;

public class Employee
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public string Name { get; set; } = string.Empty;
    public string Position { get; set; } = string.Empty;
    public string Department { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public Guid? ManagerId { get; set; }
    public Employee? Manager { get; set; }
    public List<Employee> Subordinates { get; set; } = new();
}
