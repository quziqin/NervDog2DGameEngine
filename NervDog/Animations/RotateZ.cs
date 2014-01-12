using System;
using NervDog.Common;
using NervDog.Render;

namespace NervDog.Animations
{
    public class RotateZ : Animation
    {
        private float _angleChange;
        private float _angleFrom;
        private float _count;
        private EaseFunction _ease = EaseFunction.NoEasing;
        private Sprite _target;

        public RotateZ(float angleFrom, float angleTo, uint duration)
        {
            _angleFrom = angleFrom;
            _angleChange = angleTo - _angleFrom;
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

        public float AngleFrom
        {
            set { _angleFrom = value; }
            get { return _angleFrom; }
        }

        public float AngleTo
        {
            set { _angleChange = value - _angleFrom; }
            get { return _angleFrom + _angleChange; }
        }

        public uint Duration
        {
            set { _count = value/Variables.TargetFrameMilliseconds; }
            get { return (uint) Math.Round(_count*Variables.TargetFrameMilliseconds); }
        }

        protected override void PreStart()
        {
            _current = 0.0f;
            _target.RotateZ = _angleFrom;
        }

        protected override bool UpdateFrame()
        {
            if (_reverse)
            {
                if (_isReversing)
                {
                    if (_current >= 0.0f)
                    {
                        _target.RotateZ = _ease.Func(_current, _angleFrom, _angleChange, _count);
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
                        _target.RotateZ = _ease.Func(_current, _angleFrom, _angleChange, _count);
                        _current += _delta;
                    }
                    else
                    {
                        _target.RotateZ = AngleTo;
                        _current = _count;
                        _isReversing = true;
                    }
                }
            }
            else
            {
                if (_current < _count)
                {
                    _target.RotateZ = _ease.Func(_current, _angleFrom, _angleChange, _count);
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
                _target.RotateZ = _angleFrom;
            }
            else
            {
                _current = _count;
                _target.RotateZ = AngleTo;
            }
        }
    }
}