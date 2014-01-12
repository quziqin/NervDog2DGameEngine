using System;
using NervDog.Common;
using NervDog.Render;

namespace NervDog.Animations
{
    public class Scale : Animation
    {
        private float _count;
        private EaseFunction _ease = EaseFunction.NoEasing;
        private float _scaleXChange;
        private float _scaleXFrom;
        private float _scaleYChange;
        private float _scaleYFrom;
        private Sprite _target;

        public Scale(float scaleXFrom, float scaleXTo, float scaleYFrom, float scaleYTo, uint duration)
        {
            _scaleXFrom = scaleXFrom;
            _scaleXChange = scaleXTo - _scaleXFrom;
            _scaleYFrom = scaleYFrom;
            _scaleYChange = scaleYTo - _scaleYFrom;
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

        public float ScaleXFrom
        {
            set { _scaleXFrom = value; }
            get { return _scaleXFrom; }
        }

        public float ScaleXTo
        {
            set { _scaleXChange = value - _scaleXFrom; }
            get { return _scaleXFrom + _scaleXChange; }
        }

        public float ScaleYFrom
        {
            set { _scaleYFrom = value; }
            get { return _scaleYFrom; }
        }

        public float ScaleYTo
        {
            set { _scaleYChange = value - _scaleYFrom; }
            get { return _scaleYFrom + _scaleYChange; }
        }

        public uint Duration
        {
            set { _count = value/Variables.TargetFrameMilliseconds; }
            get { return (uint) Math.Round(_count*Variables.TargetFrameMilliseconds); }
        }

        protected override void PreStart()
        {
            _current = 0.0f;
            _target.ScaleX = _scaleXFrom;
        }

        protected override bool UpdateFrame()
        {
            if (_reverse)
            {
                if (_isReversing)
                {
                    if (_current >= 0.0f)
                    {
                        _target.ScaleX = _ease.Func(_current, _scaleXFrom, _scaleXChange, _count);
                        _target.ScaleY = _ease.Func(_current, _scaleYFrom, _scaleYChange, _count);
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
                        _target.ScaleX = _ease.Func(_current, _scaleXFrom, _scaleXChange, _count);
                        _target.ScaleY = _ease.Func(_current, _scaleYFrom, _scaleYChange, _count);
                        _current += _delta;
                    }
                    else
                    {
                        _target.ScaleX = ScaleXTo;
                        _target.ScaleY = ScaleYTo;
                        _current = _count;
                        _isReversing = true;
                    }
                }
            }
            else
            {
                if (_current < _count)
                {
                    _target.ScaleX = _ease.Func(_current, _scaleXFrom, _scaleXChange, _count);
                    _target.ScaleY = _ease.Func(_current, _scaleYFrom, _scaleYChange, _count);
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
                _target.ScaleX = _scaleXFrom;
                _target.ScaleY = _scaleYFrom;
            }
            else
            {
                _current = _count;
                _target.ScaleX = ScaleXTo;
                _target.ScaleY = ScaleYTo;
            }
        }
    }
}