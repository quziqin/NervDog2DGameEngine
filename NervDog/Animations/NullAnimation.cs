using System;
using NervDog.Common;

namespace NervDog.Animations
{
    //The animation that does nothing.
    public class NullAnimation : Animation
    {
        private float _count;

        public NullAnimation(uint duration)
        {
            Duration = duration;
        }

        public uint Duration
        {
            set { _count = value/Variables.TargetFrameMilliseconds; }
            get { return (uint) Math.Round(_count*Variables.TargetFrameMilliseconds); }
        }

        protected override void PreStart()
        {
            //do nothing.
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
                        _isReversing = false;
                        return true;
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
                        _current = _count;
                        _isReversing = true;
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
                    return true;
                }
            }
            return false;
        }

        protected override void PreEnd()
        {
            //do nothing
        }
    }
}