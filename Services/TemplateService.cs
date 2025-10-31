using Fluid;
using OrgChartBuilder.Models;

namespace OrgChartBuilder.Services;

public class TemplateService
{
    private readonly FluidParser _parser;

    public TemplateService()
    {
        _parser = new FluidParser();
    }

    public string GenerateReport(string template, Employee employee)
    {
        if (_parser.TryParse(template, out var parsedTemplate, out var error))
        {
            var context = new TemplateContext(new
            {
                name = employee.Name,
                position = employee.Position,
                department = employee.Department,
                email = employee.Email,
                manager = employee.Manager != null ? new
                {
                    name = employee.Manager.Name,
                    position = employee.Manager.Position,
                    department = employee.Manager.Department
                } : null,
                subordinates = employee.Subordinates.Select(s => new
                {
                    name = s.Name,
                    position = s.Position,
                    department = s.Department
                }).ToList()
            });

            return parsedTemplate.Render(context);
        }

        return $"Template error: {error}";
    }

    public string GenerateReportForAll(string template, List<Employee> employees)
    {
        var results = new List<string>();
        
        foreach (var employee in employees)
        {
            results.Add(GenerateReport(template, employee));
        }
        
        return string.Join("\n\n", results);
    }

    public string GetDefaultTemplate()
    {
        return @"Employee: {{ name }} - {{ position }} ({{ department }})
{% if manager %}Manager: {{ manager.name }}{% else %}Manager: None (Top Level){% endif %}
Email: {{ email }}
{% if subordinates.size > 0 %}
Direct Reports: {{ subordinates.size }}
{% for sub in subordinates %}- {{ sub.name }} ({{ sub.position }})
{% endfor %}
{% else %}
Direct Reports: None
{% endif %}";
    }
}
