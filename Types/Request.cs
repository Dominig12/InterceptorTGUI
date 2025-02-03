using System.Text.Json.Serialization;

namespace InterceptorTGUI.Types;

public class Client
{
    [JsonPropertyName("ckey")]
    public string Ckey;

    [JsonPropertyName("address")]
    public string Address;

    [JsonPropertyName("computer_id")]
    public string ComputerId;
}

public class Config
{
    [JsonPropertyName("title")]
    public string Title;

    [JsonPropertyName("status")]
    public int Status;

    [JsonPropertyName("interface")]
    public string Interface;

    [JsonPropertyName("window")]
    public Window Window;

    [JsonPropertyName("client")]
    public Client Client;

    [JsonPropertyName("user")]
    public User User;
}

public class Payload<T>
{
    [JsonPropertyName("config")]
    public Config Config;

    [JsonPropertyName("data")]
    public T Data;
}

public class Root<T>
{
    [JsonPropertyName("type")]
    public string Type;

    [JsonPropertyName("payload")]
    public Payload<T> Payload;
}

public class Signal
{
    [JsonPropertyName("tag")]
    public string Tag;

    [JsonPropertyName("area")]
    public string Area;

    [JsonPropertyName("position")]
    public List<int>? Position;
}

public class User
{
    [JsonPropertyName("name")]
    public string Name;

    [JsonPropertyName("observer")]
    public int Observer;
}

public class Window
{
    [JsonPropertyName("key")]
    public string Key;

    [JsonPropertyName("size")]
    public List<int> Size;

    [JsonPropertyName("fancy")]
    public int Fancy;

    [JsonPropertyName("locked")]
    public int Locked;
}