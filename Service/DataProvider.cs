using InterceptorTGUI.Types;
using Newtonsoft.Json;

namespace InterceptorTGUI.Service;

public class DataProvider
{
    public GPS GpsData { get; set; } = new GPS()
    {
        Signals = new List<Signal>(),
        CrewSignals = new List<Signal>(),
        Position = new List<int>()
        {
            0,0,0
        },
        Tag = "TEST",
        Area = "TESTAREA"
    };

    public string GetGpsData()
    {
        return JsonConvert.SerializeObject(GpsData);
    }
}