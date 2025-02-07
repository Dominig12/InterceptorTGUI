using InterceptorTGUI.Service;
using Microsoft.AspNetCore.Mvc;

namespace InterceptorTGUI.Controllers;

[ApiController]
[Route("api/[controller]")]
public class GetDataController(DataProvider dataProvider) : ControllerBase
{
    private DataProvider DataProvider { get; set; } = dataProvider;

    [HttpGet]
    public IActionResult Get([FromQuery] string type)
    {
        switch (type)
        {
            case "gps":
                return Ok(DataProvider.GetGpsData());
                break;
        }
        return Ok("ПРИВЕТ");
    }
}