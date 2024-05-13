using BepInEx.Logging;
using System;

namespace BloodyCore.Helper
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
                _logger.Log(logLevel, $"[{MyPluginInfo.PLUGIN_VERSION}] {message}");
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
