using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using NervDog.Common;
using NervDog.Managers;
using NervDog.Render;

namespace NervDog.Trace
{
    public class XNALogger : DrawNode, ILogger
    {
        private const string _logStringFormat = "[{0}][{1}]:{2}";
        private const int _maxLine = 10;

        private readonly SpriteFont _font;

        private readonly Queue<string> _messages = new Queue<string>();
        private readonly SpriteBatch _spriteBatch;

        public XNALogger(SpriteBatch spriteBatch, SpriteFont font)
        {
            _spriteBatch = spriteBatch;
            _font = font;
        }

        public XNALogger(SpriteBatch spriteBatch, string font)
        {
            _spriteBatch = spriteBatch;
            _font = XNADevicesManager.Instance.ContentManager.Load<SpriteFont>(font);
            DrawOrder = 10000;
        }

        public XNALogger(string font)
            : this(XNADevicesManager.Instance.SpriteBatch, font)
        {
        }

        public XNALogger()
            : this(Constants.DEFAULT_FONT)
        {
        }

        public void Dispose()
        {
        }

        public override void DrawSelf()
        {
            _spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.NonPremultiplied);

            int y = 0;
            foreach (string line in _messages)
            {
                _spriteBatch.DrawString(_font, line, new Vector2(0, y), Color.Blue);
                y += _font.LineSpacing;
            }

            _spriteBatch.End();
        }

        #region Logger Functions

        public void Log(LogLevel level, object msg)
        {
            if (_messages.Count >= _maxLine)
            {
                _messages.Dequeue();
            }

            string line = string.Format(_logStringFormat, DateTime.Now, level, msg);

            _messages.Enqueue(line);
        }

        public void Log(LogLevel level, string format, params object[] args)
        {
            if (_messages.Count >= _maxLine)
            {
                _messages.Dequeue();
            }

            string line = string.Format(_logStringFormat, DateTime.Now, level, string.Format(format, args));

            _messages.Enqueue(line);
        }

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