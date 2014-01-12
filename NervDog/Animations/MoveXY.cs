using System;
using NervDog.Common;
using NervDog.Render;

namespace NervDog.Animations
{
    public class MoveXY : Animation
    {
        private float _count;
        private EaseFunction _ease = EaseFunction.NoEasing;
        private Sprite _target;
        private float _xChange;
        private float _xFrom;
        private float _yChange;
        private float _yFrom;

        public MoveXY(float xFrom, float xTo, float yFrom, float yTo, uint duration)
        {
            _xFrom = xFrom;
            _xChange = xTo - _xFrom;
            _yFrom = yFrom;
            _yChange = yTo - _yFrom;
            Duration = duration;
        }

        public Sprite Target
        {
            set { _target = value; }
            get { return _target; }
        }

        public EaseFunction EaseFunction
        {
            set { _ease = value; }
            get { return _ease; }
        }

        public float XFrom
        {
            set { _xFrom = value; }
            get { return _xFrom; }
        }

        public float XTo
        {
            set { _xChange = value - _xFrom; }
            get { return _xFrom + _xChange; }
        }

        public float YFrom
        {
            set { _yFrom = value; }
            get { return _yFrom; }
        }

        public float YTo
        {
            set { _yChange = value - _yFrom; }
            get { return _yFrom + _yChange; }
        }

        public uint Duration
        {
            set { _count = value/Variables.TargetFrameMilliseconds; }
            get { return (uint) Math.Round(_count*Variables.TargetFrameMilliseconds); }
        }

        protected override void PreStart()
        {
            _current = 0.0f;
            _target.X = _xFrom;
            _target.Y = _yFrom;
        }

        protected override bool UpdateFrame()
        {
            if (_reverse)
            {
                if (_isReversing)
                {
                    if (_current >= 0.0f)
                    {
                        _target.X = _ease.Func(_current, _xFrom, _xChange, _count);
                        _target.Y = _ease.Func(_current, _yFrom, _yChange, _count);
                        _current -= _delta;
                    }
                    else
                    {
                        _isReversing = false;
                        return true;
                    }
                }
                else
                {
                    if (_current < _count)
                    {
                        _target.X = _ease.Func(_current, _xFrom, _xChange, _count);
                        _target.Y = _ease.Func(_current, _yFrom, _yChange, _count);
                        _current += _delta;
                    }
                    else
                    {
                        _target.X = XTo;
                        _target.Y = YTo;
                        _current = _count;
                        _isReversing = true;
                    }
                }
            }
            else
            {
                if (_current < _count)
                {
                    _target.X = _ease.Func(_current, _xFrom, _xChange, _count);
                    _target.Y = _ease.Func(_current, _yFrom, _yChange, _count);
                    _current += _delta;
                }
                else
                {
                    return true;
                }
            }
            return false;
        }

        protected override void PreEnd()
        {
            if (_reverse)
            {
                _current = 0.0f;
                _isReversing = false;
                _target.X = _xFrom;
                _target.Y = _yFrom;
            }
            else
            {
                _current = _count;
                _target.X = XTo;
                _target.Y = YTo;
            }
        }
    }
}