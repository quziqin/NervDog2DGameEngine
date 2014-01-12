using System;
using Microsoft.Xna.Framework;
using NervDog.Managers;

namespace NervDog.Cameras
{
    public class BaseCamera
    {
        #region Fields

        private Matrix _mRotateX;
        private Matrix _mRotateY;
        private Matrix _mTransform;
        private Matrix _mView;
        private Vector3 _position;
        private Vector3 _target;
        private Vector3 _up = Vector3.Up;

        #endregion

        #region Properties

        public Vector3 Position
        {
            set
            {
                _position = value;
                SetTarget(ref _target);
            }
            get { return _position; }
        }

        public Vector3 Target
        {
            set { SetTarget(ref value); }
            get { return _target; }
        }

        public Vector3 Up
        {
            get { return _up; }
        }

        public Matrix View
        {
            get { return _mView; }
        }

        #endregion

        #region Private Functions

        private void SetTarget(ref Vector3 target)
        {
            Vector3 dest = target - _position;
            float l = dest.Length();
            if (l != 0.0f)
            {
                var rotateX = (float) Math.Asin(dest.Y/l);
                float rotateY = 0.0f;
                if (dest.X != 0.0f)
                {
                    rotateY = -(float) Math.Atan(dest.Z/dest.X);
                }
                Matrix.CreateRotationY(rotateX, out _mRotateX);
                Matrix.CreateRotationY(rotateY, out _mRotateY);
                _mTransform = _mRotateX*_mRotateY;
                _up = Vector3.Up;
                Vector3.Transform(ref _up, ref _mTransform, out _up);
                _target = target;
            }
        }

        #endregion

        #region Public Functions

        public void Set(Vector3 position, Vector3 target)
        {
            _position = position;
            SetTarget(ref target);
        }

        public void Set(ref Vector3 position, ref Vector3 target)
        {
            _position = position;
            SetTarget(ref target);
        }

        public void MoveToPosition(ref Vector3 position)
        {
            _target = (_target - _position + position);
            _position = position;
        }

        public void MoveToPosition(Vector3 position)
        {
            MoveToPosition(ref position);
        }

        public void Move(ref Vector3 delta)
        {
            _position = _position + delta;
            _target = _target + delta;

            SetTarget(ref _target);
        }

        public void Move(Vector3 delta)
        {
            Move(ref delta);
        }

        public void Apply()
        {
            Matrix.CreateLookAt(ref _position, ref _target, ref _up, out _mView);

            EffectManager.Instance.SetView(ref _mView);
        }

        #endregion
    }
}