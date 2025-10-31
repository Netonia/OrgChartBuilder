using System.Text.Json;
using Microsoft.JSInterop;
using OrgChartBuilder.Models;

namespace OrgChartBuilder.Services;

public class StorageService
{
    private readonly IJSRuntime _jsRuntime;
    private const string StorageKey = "orgcharts";

    public StorageService(IJSRuntime jsRuntime)
    {
        _jsRuntime = jsRuntime;
    }

    public async Task<List<OrgChart>> LoadAllChartsAsync()
    {
        try
        {
            var json = await _jsRuntime.InvokeAsync<string>("localStorage.getItem", StorageKey);
            if (string.IsNullOrEmpty(json))
                return new List<OrgChart>();
            
            return JsonSerializer.Deserialize<List<OrgChart>>(json) ?? new List<OrgChart>();
        }
        catch
        {
            return new List<OrgChart>();
        }
    }

    public async Task SaveAllChartsAsync(List<OrgChart> charts)
    {
        var json = JsonSerializer.Serialize(charts);
        await _jsRuntime.InvokeVoidAsync("localStorage.setItem", StorageKey, json);
    }

    public async Task<OrgChart?> LoadChartAsync(Guid chartId)
    {
        var charts = await LoadAllChartsAsync();
        return charts.FirstOrDefault(c => c.Id == chartId);
    }

    public async Task SaveChartAsync(OrgChart chart)
    {
        var charts = await LoadAllChartsAsync();
        var existing = charts.FirstOrDefault(c => c.Id == chart.Id);
        
        if (existing != null)
        {
            charts.Remove(existing);
        }
        
        charts.Add(chart);
        await SaveAllChartsAsync(charts);
    }

    public async Task DeleteChartAsync(Guid chartId)
    {
        var charts = await LoadAllChartsAsync();
        var chart = charts.FirstOrDefault(c => c.Id == chartId);
        
        if (chart != null)
        {
            charts.Remove(chart);
            await SaveAllChartsAsync(charts);
        }
    }
}
