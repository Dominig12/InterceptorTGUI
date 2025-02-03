using System.Diagnostics;
using System.Text.RegularExpressions;
using System.Timers;
using Timer = System.Timers.Timer;

namespace InterceptorTGUI;

public class FileInjector : IDisposable
{
    private Timer _timer;
    private int _lastProcessId = -1;

    public void Start()
    {
        // Создаем таймер с интервалом 1 секунда
        _timer = new Timer(1000);
        _timer.Elapsed += OnTimedEvent;
        _timer.AutoReset = true;
        _timer.Start();
    }

    private void OnTimedEvent(object? sender, ElapsedEventArgs e)
    {
        CheckAndInject();
    }

    private void CheckAndInject()
    {
        try
        {
            var processes = Process.GetProcessesByName("dreamseeker");
            if (processes.Length == 0) return;

            var process = processes[0];
            
            // Если процесс перезапускался - обновляем путь
            if (process.Id != _lastProcessId)
            {
                _lastProcessId = process.Id;
                Console.WriteLine($"Detected new process ID: {process.Id}");
            }

            string cachePath = Config.PathCacheByond + process.Id.ToString();
            ProcessDirectory(cachePath);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Scan error: {ex.Message}");
        }
    }

    private void ProcessDirectory(string path)
    {
        if (!Directory.Exists(path)) return;

        var files = Directory.GetFiles(path, "tgui-window-*.html")
            .Where(f => Regex.IsMatch(Path.GetFileName(f), @"^tgui-window-\d+\.html$"))
            .ToList();

        foreach (var file in files)
        {
            ProcessFile(file);
        }
    }

    private void ProcessFile(string filePath)
    {
        try
        {
            if (IsFileLocked(filePath)) return;
            
            string content = File.ReadAllText(filePath);
            string target =
                $"var message = Byond.parseJson(rawMessage);\n      fetch(\"http://localhost:5000/api/data\", {{\n        method: 'POST',\n        headers: {{ 'Content-Type': 'application/json' }},\n        body: JSON.stringify(message)\n      }});";

            if (content.Contains(target))
            {
                return;
            }
            
            // Пример модификации: добавление JS в конец файла
            string modifiedContent = content.Replace("var message = Byond.parseJson(rawMessage);",
                target);
        
            File.WriteAllText(filePath, modifiedContent);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Failed to process {filePath}: {ex.Message}");
        }
    }

    private bool IsFileLocked(string path)
    {
        try
        {
            using (var stream = File.Open(path, FileMode.Open, FileAccess.ReadWrite, FileShare.None))
            {
                return false;
            }
        }
        catch (IOException)
        {
            return true;
        }
    }

    public void Stop()
    {
        _timer?.Stop();
        _timer?.Dispose();
    }

    public void Dispose()
    {
        _timer?.Dispose();
    }
}