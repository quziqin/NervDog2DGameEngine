using NervDog.Render;

namespace NervDog.Animations
{
    public class Frame : Animation
    {
        private readonly FrameDef _frameDef;
        private float _count;
        private int _currentFrame;
        private Sprite _target;

        public Frame(FrameDef frameDef)
        {
            _frameDef = frameDef;
            Loop = frameDef.Loop;
            Reverse = frameDef.Reverse;
            Speed = frameDef.Speed;
        }

        public string Name
        {
            get { return _frameDef.Name; }
        }

        public Sprite Target
        {
            set { _target = value; }
            get { return _target; }
        }

        public uint Duration
        {
            get
            {
                float duration = 0.0f;
                for (int i = 0; i < _frameDef.FrameCount; i++)
                {
                    duration += _frameDef.KeepTimes[i];
                }
                return (uint) duration;
            }
        }

        public FrameDef FrameInfo
        {
            get { return _frameDef; }
        }

        protected override void PreStart()
        {
            _current = 0.0f;
            _currentFrame = 0;
            _target.Texture = _frameDef.Texture;
            _count = _frameDef.KeepTimes[0];
            _target.DrawRectangle = _frameDef.DrawRectangles[0];
        }

        protected override bool UpdateFrame()
        {
            if (_reverse)
            {
                if (_isReversing)
                {
                    if (_current >= 0.0f)
                    {
                        _current -= _delta;
                    }
                    else
                    {
                        _currentFrame--;
                        if (_currentFrame < 0)
                        {
                            _isReversing = false;
                            return true;
                        }
                        _current = _frameDef.KeepTimes[_currentFrame];
                        _target.DrawRectangle = _frameDef.DrawRectangles[_currentFrame];
                    }
                }
                else
                {
                    if (_current < _count)
                    {
                        _current += _delta;
                    }
                    else
                    {
                        _currentFrame++;
                        if (_currentFrame >= (int) _frameDef.FrameCount)
                        {
                            _currentFrame--;
                            _current = _frameDef.KeepTimes[_currentFrame];
                            _isReversing = true;
                        }
                        else
                        {
                            _current = 0.0f;
                            _count = _frameDef.KeepTimes[_currentFrame];
                            _target.DrawRectangle = _frameDef.DrawRectangles[_currentFrame];
                        }
                    }
                }
            }
            else
            {
                if (_current < _count)
                {
                    _current += _delta;
                }
                else
                {
                    _currentFrame++;
                    if (_currentFrame >= (int) _frameDef.FrameCount)
                    {
                        return true;
                    }
                    _current = 0.0f;
                    _count = _frameDef.KeepTimes[_currentFrame];
                    _target.DrawRectangle = _frameDef.DrawRectangles[_currentFrame];
                }
            }
            return false;
        }

        protected override void PreEnd()
        {
            if (_reverse)
            {
                _currentFrame = 0;
                _current = 0.0f;
                _target.DrawRectangle = _frameDef.DrawRectangles[0];
            }
            else
            {
                _currentFrame = (int) _frameDef.FrameCount - 1;
                _current = _frameDef.KeepTimes[_currentFrame];
                _target.DrawRectangle = _frameDef.DrawRectangles[_currentFrame];
            }
        }
    }
}