using System;
using Microsoft.Xna.Framework;
using NervDog.Animations;
using NervDog.Common;
using NervDog.Soul;

namespace NervDog.Cameras
{
    public class UnitCamera : EventListener
    {
        #region Fields

        private readonly BaseCamera _cam = new BaseCamera();

        private bool _bResetX;
        private bool _bResetY;
        private float _count = 300.0f;
        private float _currentX;
        private float _currentY;
        private Vector2 _followBound;
        private Vector2 _keepBound;
        private Vector2 _lowerBound;
        private Vector3 _offset;
        private Vector3 _position;
        private Vector2 _resetBound;
        private Vector3 _resetStart;
        private DrawUnit _target;
        private Vector2 _upperBound;

        #endregion

        #region Properties

        public bool ResetX
        {
            set
            {
                if (value)
                {
                    Vector3 targetPos = Vector3.Transform(_target.Sprite.Position, _target.Sprite.Parent.World);
                    if (Math.Abs(targetPos.X - _position.X) > _keepBound.X)
                    {
                        _bResetX = true;
                        _currentX = 0.0f;
                        _resetStart.X = _position.X;
                    }
                }
                else
                {
                    _bResetX = false;
                }
            }
            get { return _bResetX; }
        }

        public bool ResetY
        {
            set
            {
                if (value)
                {
                    Vector3 targetPos = Vector3.Transform(_target.Sprite.Position, _target.Sprite.Parent.World);
                    if (Math.Abs(targetPos.Y - _position.Y) > _keepBound.Y)
                    {
                        _bResetY = true;
                        _currentY = 0.0f;
                        _resetStart.Y = _position.Y;
                    }
                }
                else
                {
                    _bResetY = false;
                }
            }
            get { return _bResetY; }
        }

        public uint ResetTime
        {
            set { _count = value/Variables.TargetFrameMilliseconds; }
        }

        public float Distance
        {
            set { _offset.Z = value; }
            get { return _offset.Z; }
        }

        public float OffsetX
        {
            set { _offset.X = value; }
            get { return _offset.X; }
        }

        public float OffsetY
        {
            set { _offset.Y = value; }
            get { return _offset.Y; }
        }

        public Vector3 Offset
        {
            set { _offset = value; }
            get { return _offset; }
        }

        public Vector3 Position
        {
            get { return _position; }
        }

        public Vector2 UpperBound
        {
            set { _upperBound = value; }
            get { return _upperBound; }
        }

        public Vector2 LowerBound
        {
            set { _lowerBound = value; }
            get { return _lowerBound; }
        }

        public Vector2 KeepBound
        {
            set { _keepBound = value; }
            get { return _keepBound; }
        }

        public Vector2 FollowBound
        {
            set { _followBound = value; }
            get { return _followBound; }
        }

        public bool IsRunning
        {
            get { return Enable; }
        }

        public Vector2 ResetBound
        {
            set { _resetBound = value; }
            get { return _resetBound; }
        }

        public DrawUnit Target
        {
            set { _target = value; }
            get { return _target; }
        }

        public override EventFunction Handler
        {
            set { _handler = value; }
            get { return e => Update(); }
        }

        #endregion

        public UnitCamera(
            DrawUnit target,
            Vector2 keepBound,
            Vector2 followBound,
            Vector2 upperBound,
            Vector2 lowerBound,
            Vector2 resetBound,
            Vector3 offset)
            : base(Constants.TIME_TICKS_EVENT)
        {
            _target = target;
            _offset = offset;
            _keepBound = keepBound;
            _followBound = followBound;
            _upperBound = upperBound;
            _lowerBound = lowerBound;
            _resetBound = resetBound;
        }

        public void Update()
        {
            Vector2 v = _target.Velocity;
            if (Math.Abs(v.X) < _resetBound.X)
            {
                if (!ResetX)
                {
                    ResetX = true;
                }
            }
            else
            {
                ResetX = false;
            }
            if (Math.Abs(v.Y) < _resetBound.Y)
            {
                if (!ResetY)
                {
                    ResetY = true;
                }
            }
            else
            {
                ResetY = false;
            }

            Vector3 targetPos = Vector3.Transform(_target.Sprite.Position, _target.Sprite.Parent.World);
            if (Math.Abs(targetPos.X - _position.X) > _followBound.X)
            {
                _bResetX = false;
                _position.X = targetPos.X + (targetPos.X - _position.X > 0 ? -_followBound.X : _followBound.X);
            }

            if (Math.Abs(targetPos.Y - _position.Y) > _followBound.Y)
            {
                _bResetY = false;
                _position.Y = targetPos.Y + (targetPos.Y - _position.Y > 0 ? -_followBound.Y : _followBound.Y);
            }

            if (_bResetX)
            {
                float changeX = targetPos.X - _resetStart.X;
                _position.X = EaseFunction.Out_Cubic.Func(_currentX, _resetStart.X, changeX, _count);
                _currentX++;
                if (_currentX >= _count)
                {
                    _bResetX = false;
                }
            }

            if (_bResetY)
            {
                float changeY = targetPos.Y - _resetStart.Y;
                _position.Y = EaseFunction.Out_Cubic.Func(_currentY, _resetStart.Y, changeY, _count);
                _currentY++;
                if (_currentY >= _count)
                {
                    _bResetY = false;
                }
            }

            _position.X += _offset.X;
            _position.Y += _offset.Y;

            if (_position.X < _upperBound.X)
            {
                _position.X = _upperBound.X;
            }
            else if (_position.X > _lowerBound.X)
            {
                _position.X = _lowerBound.X;
            }

            if (_position.Y < _lowerBound.Y)
            {
                _position.Y = _lowerBound.Y;
            }
            else if (_position.Y > _upperBound.Y)
            {
                _position.Y = _upperBound.Y;
            }

            Vector3 target = _position;
            target.Z = targetPos.Z;
            _position.Z = targetPos.Z + _offset.Z;
            _cam.Set(ref _position, ref target);
            _cam.Apply();
        }
    }
}