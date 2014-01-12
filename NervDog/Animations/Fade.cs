using System;
using NervDog.Common;
using NervDog.Render;

namespace NervDog.Animations
{
    public class Fade : Animation
    {
        private float _alphaChange;
        private float _alphaFrom;
        private float _count;

        private EaseFunction _ease = EaseFunction.NoEasing;

        private Sprite _target;

        public Fade(float alphaFrom, float alphaTo, uint duration)
        {
            _alphaFrom = alphaFrom;
            _alphaChange = alphaTo - _alphaFrom;
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

        public float AlphaFrom
        {
            set { _alphaFrom = value; }
            get { return _alphaFrom; }
        }

        public float AlphaTo
        {
            set { _alphaChange = value - _alphaFrom; }
            get { return _alphaFrom + _alphaChange; }
        }

        //Milliseconds
        public uint Duration
        {
            set { _count = value/Variables.TargetFrameMilliseconds; }
            get { return (uint) Math.Round(_count*Variables.TargetFrameMilliseconds); }
        }

        protected override void PreStart()
        {
            _current = 0.0f;
            _target.Alpha = _alphaFrom;
        }

        protected override bool UpdateFrame()
        {
            if (_reverse)
            {
                if (_isReversing)
                {
                    if (_current >= 0.0f)
                    {
                        _target.Alpha = _ease.Func(_current, _alphaFrom, _alphaChange, _count);
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
                        _target.Alpha = _ease.Func(_current, _alphaFrom, _alphaChange, _count);
                        _current += _delta;
                    }
                    else
                    {
                        _target.Alpha = AlphaTo;
                        _current = _count;
                        _isReversing = true;
                    }
                }
            }
            else
            {
                if (_current < _count)
                {
                    _target.Alpha = _ease.Func(_current, _alphaFrom, _alphaChange, _count);
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
                _target.Alpha = _alphaFrom;
            }
            else
            {
                _current = _count;
                _target.Alpha = AlphaTo;
            }
        }
    }
}