using System;
using NervDog.Animations;
using NervDog.Common;

namespace NervDog.Soul.Actions
{
    public class Walk : IAction
    {
        private Character _character;
        private bool _isWalking;
        private int _priority = 1;
        private Animation _walk;

        public string Name
        {
            get { return "Walk"; }
        }

        public int Priority
        {
            set { _priority = value; }
            get { return _priority; }
        }

        public bool IsDoing
        {
            get { return _isWalking; }
        }

        public Character Character
        {
            set
            {
                if (_character == null)
                {
                    _character = value;
                    _walk = _character.Animations["Walk"];
                    _walk.OnEnd += Walk_OnEnd;
                }
            }
            get { return _character; }
        }

        public void Do()
        {
            if (!_isWalking)
            {
                _isWalking = true;
                if (ActionStart != null)
                {
                    ActionStart(this);
                }
            }
        }

        public void Update()
        {
            if (_character.IsOnSurface)
            {
                if (!_walk.IsPlaying)
                {
                    _walk.Loop = true;
                    if (_character.CurrentAnimation != null)
                    {
                        if (_character.CurrentAnimation.IsPlaying)
                        {
                            _character.CurrentAnimation.Stop();
                        }
                    }
                    _character.CurrentAnimation = _walk;
                    _walk.Start();
                }

                _character.VelocityX = (_character.Direction == Direction.Left
                    ? -_character.MoveSpeed.X
                    : _character.MoveSpeed.X);
            }
            else
            {
                if (Math.Abs(_character.VelocityX) < Constants.EPSILON)
                {
                    _character.VelocityX = (_character.Direction == Direction.Left
                        ? -_character.MoveSpeed.X
                        : _character.MoveSpeed.X);
                }
            }
        }

        public void Break()
        {
            _isWalking = false;
            _walk.Loop = false;
        }

        public event ActionHandler ActionStart = null;
        public event ActionHandler ActionEnd = null;

        private void Walk_OnEnd(Animation sender)
        {
            if (ActionEnd != null)
            {
                ActionEnd(this);
            }
        }
    }
}