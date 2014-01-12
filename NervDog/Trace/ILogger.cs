using System;

namespace NervDog.Trace
{
    public interface ILogger : IDisposable
    {
        void Log(LogLevel level, object msg);

        void Info(object msg);
        void Warn(object msg);
        void Error(object msg);
        void Fatal(object msg);

        void Log(LogLevel level, string format, params object[] args);

        void Info(string format, params object[] args);
        void Warn(string format, params object[] args);
        void Error(string format, params object[] args);
        void Fatal(string format, params object[] args);
    }

    public enum LogLevel
    {
        INFO,
        WARN,
        ERROR,
        FATAL
    }
}