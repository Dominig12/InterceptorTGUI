using System.Xml.Linq;

namespace InterceptorTGUI;

public static class Config
{
    public static string PathCacheByond { get; set; } = Environment.GetFolderPath(Environment.SpecialFolder.Personal) + @"\BYOND\cache\tmp";

    public static string PathGpsData { get; set; } = "../GPSData.txt";

    public static void SaveConfig()
    {
        XElement element = new XElement("Config");
        XElement el3 = new XElement("PathCacheByond");
        el3.Value = PathCacheByond;
        element.Add(el3);
        
        XElement el2 = new XElement("PathGPSData");
        el2.Value = PathGpsData;
        element.Add(el2);

        element.Save("config.xml");
    }

    public static void LoadConfig()
    {
        StreamReader reader = new StreamReader("config.xml");
        XElement element = XElement.Load(reader);
        reader.Close();

        PathCacheByond = element.Element("PathCacheByond")?.Value ?? $"{PathCacheByond}";
        
        PathGpsData = element.Element("PathGPSData")?.Value ?? $"{PathGpsData}";

        element.Save("config.xml");
    }
}