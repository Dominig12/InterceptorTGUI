using System.Text.Json.Serialization;

namespace InterceptorTGUI.Types.CrewMonitor;

public class CrewMonitor
{
    [JsonPropertyName("config")]
    public Config Config { get; set; }
    
    [JsonPropertyName("data")]
    public Data Data { get; set; }
}

public class Config
    {
        [JsonPropertyName("title")]
        public string Title { get; set; }

        [JsonPropertyName("srcObject")]
        public SrcObject SrcObject { get; set; }

        [JsonPropertyName("stateKey")]
        public string StateKey { get; set; }

        [JsonPropertyName("status")]
        public int Status { get; set; }

        [JsonPropertyName("autoUpdateLayout")]
        public int AutoUpdateLayout { get; set; }

        [JsonPropertyName("autoUpdateContent")]
        public int AutoUpdateContent { get; set; }

        [JsonPropertyName("showMap")]
        public int ShowMap { get; set; }

        [JsonPropertyName("mapZLevel")]
        public int MapZLevel { get; set; }

        [JsonPropertyName("mapName")]
        public string MapName { get; set; }

        [JsonPropertyName("user")]
        public User User { get; set; }
    }

    public class Crewmember
    {
        [JsonPropertyName("dead")]
        public int Dead { get; set; }

        [JsonPropertyName("oxy")]
        public int Oxy { get; set; }

        [JsonPropertyName("tox")]
        public int Tox { get; set; }

        [JsonPropertyName("fire")]
        public int Fire { get; set; }

        [JsonPropertyName("brute")]
        public int Brute { get; set; }

        [JsonPropertyName("area")]
        public string Area { get; set; }

        [JsonPropertyName("x")]
        public int X { get; set; }

        [JsonPropertyName("y")]
        public int Y { get; set; }

        [JsonPropertyName("sensor_type")]
        public int SensorType { get; set; }

        [JsonPropertyName("name")]
        public string Name { get; set; }

        [JsonPropertyName("rank")]
        public string Rank { get; set; }

        [JsonPropertyName("assignment")]
        public string Assignment { get; set; }
    }

    public class Data
    {
        [JsonPropertyName("crewmembers")]
        public List<Crewmember> Crewmembers { get; set; }
    }

    public class Root
    {
        [JsonPropertyName("config")]
        public Config Config { get; set; }

        [JsonPropertyName("data")]
        public Data Data { get; set; }
    }

    public class SrcObject
    {
        [JsonPropertyName("name")]
        public string Name { get; set; }
    }

    public class User
    {
        [JsonPropertyName("name")]
        public string Name { get; set; }
    }
