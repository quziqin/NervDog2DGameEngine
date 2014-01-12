using NervDog.Animations;

namespace NervDog.Soul.Actions
{
    public class Jump : IAction
    {
        private Character _character;
        private bool _isJumping;
        private Animation _jump;
        private int _priority = 1;

        public string Name
        {
            get { return "Jump"; }
        }

        public int Priority
        {
            get { return _priority; }
            set { _priority = value; }
        }

        public bool IsDoing
        {
            get
            {
                CheckEnd();
                return _isJumping;
            }
        }

        public Character Character
        {
            set
            {
                if (_character == null)
                {
                    _character = value;
                    _jump = _character.Animations["Jump"];
                    //_jump.OnEnd += new AnimationHandler(Jump_OnEnd);
                }
            }
            get { return _character; }
        }

        public void Do()
        {
            if (_character.IsOnSurface)
            {
                _isJumping = true;
                _jump.Loop = true;
                _character.IsOnSurface = false;
                _character.VelocityY = _character.MoveSpeed.Y;
                if (_character.CurrentAnimation != null)
                {
                    if (_character.CurrentAnimation.IsPlaying)
                    {
                        _character.CurrentAnimation.Stop();
                    }
                }
                _character.CurrentAnimation = _jump;
                _jump.Start();
                if (ActionStart != null)
                {
                    ActionStart(this);
                }
            }
        }

        public void Update()
        {
            CheckEnd();
        }

        public void Break()
        {
            _isJumping = false;
        }

        public event ActionHandler ActionStart;
        public event ActionHandler ActionEnd;

        private void CheckEnd()
        {
            if (_isJumping && _character.IsOnSurface)
            {
                _isJumping = false;
                if (ActionEnd != null)
                {
                    ActionEnd(this);
                }
            }
        }
    }
}