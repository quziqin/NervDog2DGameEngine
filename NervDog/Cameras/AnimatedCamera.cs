using System;
using Microsoft.Xna.Framework;
using NervDog.Animations;
using NervDog.Common;

namespace NervDog.Cameras
{
    public class AnimatedCamera : Animation
    {
        #region Fields

        private readonly BaseCamera _cam = new BaseCamera();
        private Vector3 _change;
        private float _count;
        private EaseFunction _ease = EaseFunction.NoEasing;
        private Vector3 _from;

        #endregion

        #region Properties

        public EaseFunction EaseFunction
        {
            set { _ease = value; }
            get { return _ease; }
        }

        public Vector3 From
        {
            set { _from = value; }
            get { return _from; }
        }

        public Vector3 To
        {
            set { _change = value - _from; }
            get { return _from + _change; }
        }

        public uint Duration
        {
            set { _count = value/Variables.TargetFrameMilliseconds; }
            get { return (uint) Math.Round(_count*Variables.TargetFrameMilliseconds); }
        }

        #endregion

        #region Constructors

        public AnimatedCamera()
        {
            _cam.Set(new Vector3(0, 0, 200), Vector3.Zero);
        }

        public AnimatedCamera(Vector3 position, Vector3 target)
        {
            _cam.Set(ref position, ref target);
        }

        public AnimatedCamera(Vector3 position, Vector3 target, Vector3 from, Vector3 to, uint duration)
        {
            _cam.Set(ref position, ref target);
            _from = from;
            _change = to - from;
            Duration = duration;
        }

        #endregion

        #region Public Functions

        public void Set(Vector3 position, Vector3 target)
        {
            _cam.Set(ref position, ref target);
        }

        #endregion

        #region Override Animation Functions

        protected override void PreStart()
        {
            _current = 0.0f;
            _cam.MoveToPosition(_from);
            _cam.Apply();
        }

        protected override bool UpdateFrame()
        {
            if (_reverse)
            {
                if (_isReversing)
                {
                    if (_current >= 0.0f)
                    {
                        Vector3 pos;
                        pos.X = _ease.Func(_current, _from.X, _change.X, _count);
                        pos.Y = _ease.Func(_current, _from.Y, _change.Y, _count);
                        pos.Z = _ease.Func(_current, _from.Z, _change.Z, _count);
                        _cam.MoveToPosition(ref pos);
                        _cam.Apply();
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
                        Vector3 pos;
                        pos.X = _ease.Func(_current, _from.X, _change.X, _count);
                        pos.Y = _ease.Func(_current, _from.Y, _change.Y, _count);
                        pos.Z = _ease.Func(_current, _from.Z, _change.Z, _count);
                        _cam.MoveToPosition(ref pos);
                        _cam.Apply();
                        _current += _delta;
                    }
                    else
                    {
                        _cam.MoveToPosition(To);
                        _cam.Apply();
                        _current = _count;
                        _isReversing = true;
                    }
                }
            }
            else
            {
                if (_current < _count)
                {
                    Vector3 pos;
                    pos.X = _ease.Func(_current, _from.X, _change.X, _count);
                    pos.Y = _ease.Func(_current, _from.Y, _change.Y, _count);
                    pos.Z = _ease.Func(_current, _from.Z, _change.Z, _count);
                    _cam.MoveToPosition(ref pos);
                    _cam.Apply();
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
                _cam.MoveToPosition(_from);
                _cam.Apply();
            }
            else
            {
                _current = _count;
                _cam.MoveToPosition(To);
                _cam.Apply();
            }
        }

        #endregion
    }
}