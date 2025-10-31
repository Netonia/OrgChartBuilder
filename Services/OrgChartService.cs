using OrgChartBuilder.Models;

namespace OrgChartBuilder.Services;

public class OrgChartService
{
    private OrgChart _currentChart = new() { Name = "Default Organization" };
    public event Action? OnChange;

    public OrgChart GetCurrentChart() => _currentChart;

    public void SetCurrentChart(OrgChart chart)
    {
        _currentChart = chart;
        NotifyStateChanged();
    }

    public void AddEmployee(Employee employee)
    {
        _currentChart.Employees.Add(employee);
        _currentChart.ModifiedDate = DateTime.UtcNow;
        
        if (employee.ManagerId.HasValue)
        {
            var manager = GetEmployeeById(employee.ManagerId.Value);
            if (manager != null)
            {
                manager.Subordinates.Add(employee);
                employee.Manager = manager;
            }
        }
        
        NotifyStateChanged();
    }

    public void UpdateEmployee(Employee employee)
    {
        var existing = GetEmployeeById(employee.Id);
        if (existing != null)
        {
            var oldManagerId = existing.ManagerId;
            
            // Remove from old manager's subordinates
            if (oldManagerId.HasValue && oldManagerId != employee.ManagerId)
            {
                var oldManager = GetEmployeeById(oldManagerId.Value);
                if (oldManager != null)
                {
                    oldManager.Subordinates.Remove(existing);
                }
            }
            
            // Update properties
            existing.Name = employee.Name;
            existing.Position = employee.Position;
            existing.Department = employee.Department;
            existing.Email = employee.Email;
            existing.ManagerId = employee.ManagerId;
            
            // Add to new manager's subordinates
            if (employee.ManagerId.HasValue && oldManagerId != employee.ManagerId)
            {
                if (!WouldCreateCycle(existing.Id, employee.ManagerId.Value))
                {
                    var newManager = GetEmployeeById(employee.ManagerId.Value);
                    if (newManager != null)
                    {
                        newManager.Subordinates.Add(existing);
                        existing.Manager = newManager;
                    }
                }
                else
                {
                    // Revert manager change if it would create a cycle
                    existing.ManagerId = oldManagerId;
                    throw new InvalidOperationException("This change would create a cycle in the hierarchy.");
                }
            }
            
            _currentChart.ModifiedDate = DateTime.UtcNow;
            NotifyStateChanged();
        }
    }

    public void RemoveEmployee(Guid employeeId)
    {
        var employee = GetEmployeeById(employeeId);
        if (employee != null)
        {
            // Remove from manager's subordinates
            if (employee.ManagerId.HasValue)
            {
                var manager = GetEmployeeById(employee.ManagerId.Value);
                if (manager != null)
                {
                    manager.Subordinates.Remove(employee);
                }
            }
            
            // Reassign subordinates to the removed employee's manager
            foreach (var subordinate in employee.Subordinates.ToList())
            {
                subordinate.ManagerId = employee.ManagerId;
                subordinate.Manager = employee.Manager;
                if (employee.Manager != null)
                {
                    employee.Manager.Subordinates.Add(subordinate);
                }
            }
            
            _currentChart.Employees.Remove(employee);
            _currentChart.ModifiedDate = DateTime.UtcNow;
            NotifyStateChanged();
        }
    }

    public Employee? GetEmployeeById(Guid id)
    {
        return _currentChart.Employees.FirstOrDefault(e => e.Id == id);
    }

    public List<Employee> GetRootEmployees()
    {
        return _currentChart.Employees.Where(e => e.ManagerId == null).ToList();
    }

    public List<Employee> SearchEmployees(string searchTerm)
    {
        if (string.IsNullOrWhiteSpace(searchTerm))
            return _currentChart.Employees.ToList();
        
        searchTerm = searchTerm.ToLower();
        return _currentChart.Employees.Where(e =>
            e.Name.ToLower().Contains(searchTerm) ||
            e.Position.ToLower().Contains(searchTerm) ||
            e.Department.ToLower().Contains(searchTerm) ||
            e.Email.ToLower().Contains(searchTerm)
        ).ToList();
    }

    public List<Employee> FilterByDepartment(string department)
    {
        if (string.IsNullOrWhiteSpace(department))
            return _currentChart.Employees.ToList();
        
        return _currentChart.Employees
            .Where(e => e.Department.Equals(department, StringComparison.OrdinalIgnoreCase))
            .ToList();
    }

    public List<string> GetAllDepartments()
    {
        return _currentChart.Employees
            .Select(e => e.Department)
            .Where(d => !string.IsNullOrWhiteSpace(d))
            .Distinct()
            .OrderBy(d => d)
            .ToList();
    }

    private bool WouldCreateCycle(Guid employeeId, Guid newManagerId)
    {
        var current = GetEmployeeById(newManagerId);
        while (current != null)
        {
            if (current.Id == employeeId)
                return true;
            current = current.ManagerId.HasValue ? GetEmployeeById(current.ManagerId.Value) : null;
        }
        return false;
    }

    private void NotifyStateChanged() => OnChange?.Invoke();
}
