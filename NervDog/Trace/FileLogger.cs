using System;
using System.IO;
using NervDog.Common;
using Timer = System.Timers.Timer;

namespace NervDog.Trace
{
    public class FileLogger : BaseLogger
    {
        private const string _logStringFormat = "[{0}][{1}]:{2}";

        private readonly FileStream _fileStream;
        private readonly StreamWriter _streamWriter;
        private Timer _flushTimer;

        public FileLogger()
            : this(Constants.DEFAULT_LOG_NAME)
        {
        }

        public FileLogger(string filePath)
        {
            _fileStream = new FileStream(filePath, FileMode.OpenOrCreate, FileAccess.ReadWrite);
            _streamWriter = new StreamWriter(_fileStream);

            _flushTimer = new Timer(TimeSpan.FromSeconds(10).TotalMilliseconds);
            _flushTimer.Elapsed += (o, e) => { _streamWriter.Flush(); };
            _flushTimer.Start();
        }

        public override void Log(LogLevel level, object msg)
        {
            _streamWriter.WriteLine(_logStringFormat, DateTime.Now, level, msg);
        }

        public override void Log(LogLevel level, string format, params object[] args)
        {
            _streamWriter.WriteLine(_logStringFormat, DateTime.Now, level, string.Format(format, args));
        }

        public override void Dispose()
        {
            if (_streamWriter != null)
            {
                _streamWriter.Flush();
                _streamWriter.Dispose();
            }

            if (_fileStream != null)
            {
                _fileStream.Dispose();
            }
        }
    }
}