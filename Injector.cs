using System;
using System.IO;
using System.Diagnostics;
using System.Threading;
using System.Timers;
using Timer = System.Timers.Timer;

namespace InterceptorTGUI
{
    public class FileInjector : IDisposable
    {
        private Timer _timer;
        private int _lastProcessId = -1;
        private string Target { get; }
        private string Inject { get; }
        private string SearchPattern { get; }
        
        private const int MaxRetries = 3;
        private const int RetryDelay = 300;

        public FileInjector(string target, string inject, string searchPattern = "*.html")
        {
            Target = target;
            Inject = inject;
            SearchPattern = searchPattern;
        }

        public void Start()
        {
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
                
                if (process.Id != _lastProcessId)
                {
                    _lastProcessId = process.Id;
                    Console.WriteLine($"New process detected: {process.Id}");
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

            foreach (var file in Directory.EnumerateFiles(path, SearchPattern))
            {
                ProcessFileWithRetries(file);
            }
        }

        private void ProcessFileWithRetries(string filePath)
        {
            for (int attempt = 0; attempt < MaxRetries; attempt++)
            {
                try
                {
                    if (ProcessFile(filePath)) return;
                }
                catch (IOException) when (attempt < MaxRetries - 1)
                {
                    Thread.Sleep(RetryDelay);
                }
            }
        }

        private bool ProcessFile(string filePath)
        {
            try
            {
                string content;
                using (var fs = new FileStream(filePath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
                using (var reader = new StreamReader(fs))
                {
                    content = reader.ReadToEnd();
                }

                if (content.Contains(Inject))
                    return true;

                string modifiedContent = content.Replace(Target, Inject);
                WriteToFileWithTemp(filePath, modifiedContent);
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error processing {filePath}: {ex.Message}");
                return false;
            }
        }

        private void WriteToFileWithTemp(string filePath, string content)
        {
            string tempFile = Path.GetTempFileName();
            
            try
            {
                File.WriteAllText(tempFile, content);
                File.Copy(tempFile, filePath, overwrite: true);
            }
            finally
            {
                if (File.Exists(tempFile))
                    File.Delete(tempFile);
            }
        }

        public void Stop()
        {
            _timer?.Stop();
            _timer?.Dispose();
        }

        public void Dispose()
        {
            Stop();
            GC.SuppressFinalize(this);
        }
    }
}