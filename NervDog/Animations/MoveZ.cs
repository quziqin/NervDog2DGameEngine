using System;
using NervDog.Common;
using NervDog.Render;

namespace NervDog.Animations
{
    public class MoveZ : Animation
    {
        private float _count;
        private EaseFunction _ease = EaseFunction.NoEasing;
        private Sprite _target;
        private float _zChange;
        private float _zFrom;

        public MoveZ(float zFrom, float zTo, uint duration)
        {
            _zFrom = zFrom;
            _zChange = zTo - _zFrom;
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

        public float ZFrom
        {
            set { _zFrom = value; }
            get { return _zFrom; }
        }

        public float ZTo
        {
            set { _zChange = value - _zFrom; }
            get { return _zFrom + _zChange; }
        }

        public uint Duration
        {
            set { _count = value/Variables.TargetFrameMilliseconds; }
            get { return (uint) Math.Round(_count*Variables.TargetFrameMilliseconds); }
        }

        protected override void PreStart()
        {
            _current = 0.0f;
            _target.Z = _zFrom;
        }

        protected override bool UpdateFrame()
        {
            if (_reverse)
            {
                if (_isReversing)
                {
                    if (_current >= 0.0f)
                    {
                        _target.Z = _ease.Func(_current, _zFrom, _zChange, _count);
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
                        _target.Z = _ease.Func(_current, _zFrom, _zChange, _count);
                        _current += _delta;
                    }
                    else
                    {
                        _target.Z = ZTo;
                        _current = _count;
                        _isReversing = true;
                    }
                }
            }
            else
            {
                if (_current < _count)
                {
                    _target.Z = _ease.Func(_current, _zFrom, _zChange, _count);
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
                _target.Z = _zFrom;
            }
            else
            {
                _current = _count;
                _target.Z = ZTo;
            }
        }
    }
}