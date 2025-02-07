using System.Text.Json.Serialization;

namespace InterceptorTGUI.Types;

public class GPS
{
    [JsonPropertyName("active")]
    public int Active;

    [JsonPropertyName("tag")]
    public string Tag;

    [JsonPropertyName("same_z")]
    public int SameZ;

    [JsonPropertyName("area")]
    public string Area;

    [JsonPropertyName("position")]
    public List<int>? Position;

    [JsonPropertyName("saved")]
    public List<int>? Saved;

    [JsonPropertyName("signals")]
    public List<Signal> Signals;

    [JsonPropertyName("crew_signals")]
    public List<Signal> CrewSignals;
}