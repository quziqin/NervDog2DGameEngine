namespace NervDog.Trace
{
    public abstract class BaseLogger : ILogger
    {
        #region Abstract Functions

        public abstract void Log(LogLevel level, object msg);
        public abstract void Log(LogLevel level, string format, params object[] args);
        public abstract void Dispose();

        #endregion

        #region Virtual Functions

        public virtual void Info(object msg)
        {
            Log(LogLevel.INFO, msg);
        }

        public virtual void Warn(object msg)
        {
            Log(LogLevel.WARN, msg);
        }

        public virtual void Error(object msg)
        {
            Log(LogLevel.ERROR, msg);
        }

        public virtual void Fatal(object msg)
        {
            Log(LogLevel.FATAL, msg);
        }

        public virtual void Info(string format, params object[] args)
        {
            Log(LogLevel.INFO, format, args);
        }

        public virtual void Warn(string format, params object[] args)
        {
            Log(LogLevel.WARN, format, args);
        }

        public virtual void Error(string format, params object[] args)
        {
            Log(LogLevel.ERROR, format, args);
        }

        public virtual void Fatal(string format, params object[] args)
        {
            Log(LogLevel.FATAL, format, args);
        }

        #endregion
    }
}