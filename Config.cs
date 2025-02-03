using System.Xml.Linq;

namespace InterceptorTGUI;

public static class Config
{
    public static string PathCacheByond { get; set; } = Environment.GetFolderPath(Environment.SpecialFolder.Personal) + @"\BYOND\cache\tmp";

    public static void SaveConfig()
    {
        XElement element = new XElement("Config");
        XElement el3 = new XElement("PathCacheByond");
        el3.Value = PathCacheByond;
        element.Add(el3);

        element.Save("config.xml");
    }

    public static void LoadConfig()
    {
        StreamReader reader = new StreamReader("config.xml");
        XElement element = XElement.Load(reader);
        reader.Close();

        PathCacheByond = element.Element("PathCacheByond")?.Value ?? $"{PathCacheByond}";

        element.Save("config.xml");
    }
}