using Microsoft.Xna.Framework;
using NervDog.Animations;
using NervDog.Common;
using NervDog.Render;

namespace NervDog.Soul.Actions
{
    public class Dying : IAction
    {
        private static readonly string _name = "Dying";
        private readonly Fade _fade;
        private readonly RotateX _rotateX;
        private Character _character;
        private bool _isDying;
        private bool _isOnSurface;
        private int _priority = 5;

        public Dying()
        {
            _rotateX = new RotateX(0, MathHelper.PiOver2, 600);
            _rotateX.EaseFunction = EaseFunction.In_Cubic;
            _rotateX.OnEnd += _rotateX_OnEnd;
            _fade = new Fade(1.0f, 0.0f, 3000);
            _fade.OnEnd += _fade_OnEnd;
        }

        public string Name
        {
            get { return _name; }
        }

        public int Priority
        {
            set { _priority = value; }
            get { return _priority; }
        }

        public bool IsDoing
        {
            get { return _isDying; }
        }

        public Character Character
        {
            set
            {
                if (_character == null)
                {
                    _character = value;
                    _rotateX.Target = value.Sprite;
                    _fade.Target = value.Sprite;
                }
            }
            get { return _character; }
        }

        public void Do()
        {
            if (!_isDying)
            {
                _isDying = true;
                _isOnSurface = false;
                _character.Group = Group.Standby;
                if (_character.IsOnSurface)
                {
                    _isOnSurface = true;
                    BeginQuit();
                }
                if (ActionStart != null)
                {
                    ActionStart(this);
                }
            }
        }

        public void Update()
        {
            if (!_isOnSurface)
            {
                if (_character.IsOnSurface)
                {
                    _isOnSurface = true;
                    BeginQuit();
                }
            }
        }

        public void Break()
        {
        }

        public event ActionHandler ActionStart = null;
        public event ActionHandler ActionEnd = null;

        private void _fade_OnEnd(Animation sender)
        {
            _isDying = false;
            _character.IsAlive = false;
            if (ActionEnd != null)
            {
                ActionEnd(this);
            }
        }

        private void _rotateX_OnEnd(Animation sender)
        {
            _fade.Start();
        }

        private void BeginQuit()
        {
            Sprite sprite = _character.Sprite;
            _character.EventManager.UnRegister(_character);
            sprite.Y = sprite.Y - sprite.Height/2.0f;
            sprite.Z = 110.0f;
            sprite.TransformOrigin = new Vector2(0.5f, 1.0f);
            if (_character.Direction == Direction.Left)
            {
                _rotateX.AngleTo = MathHelper.PiOver2;
            }
            else
            {
                _rotateX.AngleTo = -MathHelper.PiOver2;
            }
            _rotateX.Start();
        }
    }
}