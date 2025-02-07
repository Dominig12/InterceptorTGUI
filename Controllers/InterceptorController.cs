using System.Text;
using System.Text.Json.Serialization;
using System.Xml.Linq;
using InterceptorTGUI.Service;
using InterceptorTGUI.Types;
using InterceptorTGUI.Types.CrewMonitor;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using JsonSerializer = System.Text.Json.JsonSerializer;

namespace InterceptorTGUI.Controllers;

[ApiController]
[Route("api/[controller]")]
public class DataController(DataProvider provider) : ControllerBase
{
    private DataProvider DataProvider { get; set; } = provider;

    // Обработка GET-запросов
    [HttpGet]
    public IActionResult Get([FromQuery] string data)
    {
        Console.WriteLine(data);
        CrewMonitor? crewMonitor = JsonConvert.DeserializeObject<CrewMonitor>(data);
        DataProvider.GpsData.CrewSignals = new List<Signal>();
        if (crewMonitor != null)
            foreach (Crewmember dataCrewmember in crewMonitor.Data.Crewmembers)
            {
                DataProvider.GpsData.CrewSignals.Add(new Signal()
                {
                    Position = new List<int>()
                    {
                        dataCrewmember.X, dataCrewmember.Y, crewMonitor.Config.MapZLevel
                    },
                    Tag = dataCrewmember.Name,
                    Area = dataCrewmember.Area
                });
            }

        return Ok();
    }

    [HttpPost]
    public IActionResult PostData([FromBody] object data)
    {
        string? json = data.ToString();
        Console.WriteLine(json);
        if (json == null)
        {
            return new AcceptedResult();
        }
        Root<object>? root = JsonConvert.DeserializeObject<Root<object>>(json);

        if (root?.Type != "update")
        {
            Console.WriteLine(root?.Type);
            return new AcceptedResult();
        }

        try
        {
            switch (root?.Payload.Config.Title)
            {
                case "GPS":
                    GpsProcess(JsonConvert.DeserializeObject<Root<GPS>>(json));
                    break;
                default:
                    Console.WriteLine(json);
                    break;
            }
        }
        catch
        {
            // ignore
        }
        return new AcceptedResult();
    }

    private void GpsProcess(Root<GPS>? gps)
    {
        if (gps == null)
        {
            return;
        }
        
        GPS data = gps.Payload.Data;
        Console.WriteLine($"GPS: {data.Tag} {data.Area}");

        DataProvider.GpsData.Signals = data.Signals;
        DataProvider.GpsData.Position = data.Position;
        DataProvider.GpsData.Tag = data.Tag;
    }
    
    private bool IsFileLocked(string path)
    {
        try
        {
            using (var stream = System.IO.File.Open(path, FileMode.Open, FileAccess.ReadWrite, FileShare.None))
            {
                return false;
            }
        }
        catch (IOException)
        {
            return true;
        }
    }
}

public class PositionGps
{
    public string Position { get; set; }
    public string SavedPosition { get; set; }
    public List<string> OtherGps { get; set; }
}