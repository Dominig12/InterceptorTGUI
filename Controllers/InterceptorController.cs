using System.Text;
using System.Text.Json;
using Microsoft.AspNetCore.Mvc;

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
        StreamWriter writer = new StreamWriter("GPSData.txt", false, Encoding.Unicode);
        writer.WriteLine(JsonSerializer.Serialize(data));
        writer.Close();
        return new JsonResult(JsonSerializer.Serialize(data));
    }
}

public class PositionGps
{
    public string Position { get; set; }
    public string SavedPosition { get; set; }
    public List<string>? OtherGps { get; set; }
}