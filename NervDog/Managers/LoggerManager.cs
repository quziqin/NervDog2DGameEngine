using System.Collections.Generic;
using NervDog.Trace;

namespace NervDog.Managers
{
    public class LoggerManager : BaseLogger
    {
        private static readonly LoggerManager _instance = new LoggerManager();
        private readonly Dictionary<string, ILogger> _loggers = new Dictionary<string, ILogger>();

        private LoggerManager()
        {
            Register(new FileLogger());
            Register(new XNALogger());
        }

        public static LoggerManager Instance
        {
            get { return _instance; }
        }

        public void Register(ILogger logger)
        {
            _loggers.Add(logger.GetType().Name, logger);
        }

        public void UnRegister(ILogger logger)
        {
            _loggers.Remove(logger.GetType().Name);
        }

        public ILogger GetLogger(string name)
        {
            return _loggers[name];
        }

        public override void Log(LogLevel level, object msg)
        {
            foreach (ILogger logger in _loggers.Values)
            {
                logger.Log(level, msg);
            }
        }

        public override void Log(LogLevel level, string format, params object[] args)
        {
            foreach (ILogger logger in _loggers.Values)
            {
                logger.Log(level, format, args);
            }
        }

        public override void Dispose()
        {
            foreach (ILogger logger in _loggers.Values)
            {
                logger.Dispose();
            }

            _loggers.Clear();
        }
    }
}