using System.Text;
using System.Text.Json.Serialization;
using System.Xml.Linq;
using InterceptorTGUI.Types;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using JsonSerializer = System.Text.Json.JsonSerializer;

namespace InterceptorTGUI.Controllers;

[ApiController]
[Route("api/[controller]")]
public class DataController : ControllerBase
{
    
    // Обработка GET-запросов
    [HttpGet]
    public IActionResult Get()
    {
        return Ok("ПРИВЕТ");
    }

    [HttpPost]
    public IActionResult PostData([FromBody] object data)
    {
        string? json = data.ToString();
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
        Console.WriteLine($"GPS: {data.Tag} {Converter(data.Position)}");
        PositionGps positionGps = new PositionGps();
        
        positionGps.SavedPosition = Converter(data.Saved);
        positionGps.Position = Converter(data.Position);
        positionGps.OtherGps = data.Signals.Select(x => Converter(x.Position)).Where(x => !string.IsNullOrWhiteSpace(x)).ToList();

        if (IsFileLocked(Config.PathGpsData))
        {
            return;
        }

        try
        {
            System.IO.File.WriteAllText(Config.PathGpsData, JsonConvert.SerializeObject(positionGps), Encoding.Unicode);
        }
        catch
        {
            // ignore
        }

        string Converter(List<int>? coors)
        {
            if (coors == null)
            {
                return "";
            }
            return $"({coors[0]}, {coors[1]}, {coors[2]})";
        }
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