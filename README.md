# OrgChartBuilder

A Blazor WebAssembly application for creating, visualizing, and managing organizational hierarchies interactively in the browser.

## Features

### 1. Employee Management
- **Add Employees**: Create new employees with name, position, department, and email
- **Edit Employees**: Modify employee information via the side panel
- **Delete Employees**: Remove employees from the organization
- **Automatic Hierarchy Management**: Prevents circular references and maintains data integrity

### 2. Interactive Visualization
- **Hierarchical Chart View**: Visual representation of the organizational structure
- **Interactive Nodes**: Click on employee cards to view or edit details
- **Connection Lines**: Visual indicators showing reporting relationships
- **Responsive Layout**: Three-panel design (Employee List | Chart View | Details/Editor)

### 3. Search and Filtering
- **Real-time Search**: Find employees by name, position, department, or email
- **Department Filter**: Filter employees by department
- **Instant Results**: Dynamic filtering as you type

### 4. Report Generation
- **Liquid Templates**: Use Fluid.Core for flexible report templates
- **Customizable Templates**: Edit the Liquid template to customize output
- **Batch Reports**: Generate reports for all or filtered employees
- **Default Template Included**: Pre-configured template for common use cases

### 5. Data Persistence
- **Local Storage**: Save organization charts to browser localStorage
- **Save/Load**: Persist multiple organizational charts
- **Automatic Modification Tracking**: Tracks creation and modification dates

## Technology Stack

- **Blazor WebAssembly (.NET 9.0)**: 100% client-side web application
- **Fluid.Core**: Liquid template engine for report generation
- **Bootstrap 5**: Responsive UI framework
- **LocalStorage**: Browser-based data persistence

## Getting Started

### Prerequisites
- .NET 9.0 SDK or later

### Running the Application

1. Clone the repository:
```bash
git clone https://github.com/Netonia/OrgChartBuilder.git
cd OrgChartBuilder
```

2. Restore dependencies:
```bash
dotnet restore
```

3. Run the application:
```bash
dotnet run
```

4. Open your browser and navigate to `https://localhost:5001` or `http://localhost:5000`

### Building for Production

```bash
dotnet publish -c Release
```

The output will be in `bin/Release/net9.0/publish/wwwroot/`

## Usage Guide

### Adding Your First Employee

1. Click the **"+ Add Employee"** button in the left panel
2. Fill in the employee details (Name, Position, Department, Email)
3. Select a manager (or leave as "None" for top-level employees)
4. Click **"Save"**

### Creating a Hierarchy

1. Add a top-level employee (e.g., CEO)
2. Add subordinate employees and select the CEO as their manager
3. Continue adding employees at different levels
4. The chart automatically updates to show the hierarchy

### Searching and Filtering

- **Search**: Type in the search box to find employees by any field
- **Department Filter**: Select a department from the dropdown to filter employees

### Generating Reports

1. Select the employees you want to report on (use search/filter if needed)
2. Edit the Liquid template in the right panel (optional)
3. Click **"Generate Report"**
4. The report appears below the template

### Template Syntax

The default template uses Liquid syntax:

```liquid
Employee: {{ name }} - {{ position }} ({{ department }})
{% if manager %}Manager: {{ manager.name }}{% else %}Manager: None (Top Level){% endif %}
Email: {{ email }}
{% if subordinates.size > 0 %}
Direct Reports: {{ subordinates.size }}
{% for sub in subordinates %}- {{ sub.name }} ({{ sub.position }})
{% endfor %}
{% endif %}
```

Available properties:
- `name`, `position`, `department`, `email`
- `manager.name`, `manager.position`, `manager.department`
- `subordinates` (array with `name`, `position`, `department`)

### Saving and Loading Charts

- **Save**: Click "Save Chart" to persist to localStorage
- **Load**: Click "Load Chart" to restore saved charts

## Architecture

### Models
- `Employee`: Represents an employee with hierarchical relationships
- `OrgChart`: Represents an organizational chart with metadata

### Services
- `OrgChartService`: Manages CRUD operations and hierarchy validation
- `StorageService`: Handles localStorage persistence
- `TemplateService`: Processes Liquid templates for report generation

### Components
- `Home.razor`: Main application page with three-panel layout
- `EmployeeNode.razor`: Recursive component for rendering hierarchy

## Roadmap / Future Enhancements

As outlined in PRD.md, potential extensions include:

- **D3.js Integration**: Advanced interactive visualizations with zoom, pan, and drag-and-drop
- **Export Functionality**: SVG/PNG export of the organizational chart
- **Multi-user Collaboration**: API backend for shared access
- **Import/Export**: CSV or JSON import/export of employee data
- **Visual Indicators**: Performance metrics, tenure, or KPIs per employee
- **LDAP/Active Directory Integration**: Sync with existing directory services

## License

This project is open source and available under the MIT License.

## Contributing

Contributions are welcome! Please feel free to submit a Pull Request.
