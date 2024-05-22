using BepInEx.Logging;
using System;

namespace Bloody.Core.Helper.v1
{
    public class Logger
    {
        private readonly ManualLogSource _logger;
        private bool _enabled = true;

        public Logger(ManualLogSource logger)
        {
            _logger = logger;
        }

        public void Enable(bool enable)
        {
            _enabled = enable;
        }

        private void Log(LogLevel logLevel, string message)
        {
            if (_enabled)
            {
                var date = DateTime.Now;
                _logger.Log(logLevel, $"[{date.ToString("HH:mm:ss")}] [{MyPluginInfo.PLUGIN_VERSION}] {message}");
            }

        }

        public void LogInfo(string message) => Log(LogLevel.Info, message);
        public void LogWarning(string message) => Log(LogLevel.Warning, message);
        public void LogDebug(string message) => Log(LogLevel.Debug, message);
        public void LogFatal(string message) => Log(LogLevel.Fatal, message);
        public void LogError(string message) => Log(LogLevel.Error, message);
        public void LogError(Exception exception) => Log(LogLevel.Error, exception.ToString());
        public void LogMessage(string message) => Log(LogLevel.Message, message);
    }
}
