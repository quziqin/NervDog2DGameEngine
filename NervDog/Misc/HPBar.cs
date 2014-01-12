using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using NervDog.Animations;
using NervDog.Common;
using NervDog.Managers;
using NervDog.Render;
using NervDog.Soul;
using NervDog.Soul.Actions;

namespace NervDog.Misc
{
    public class HPBar : EventListener
    {
        private readonly Fade _fade;
        private readonly Sprite _hpFrame;
        private readonly Sprite _hpRect;
        private Character _character;
        private uint _count = 180;
        private uint _current;
        private float _height;
        private bool _isVisible = true;
        private float _remain;
        private float _width;
        private float _x;
        private float _y;

        public HPBar(string frameTexName, string rectTexName)
        {
            var frameTex = XNADevicesManager.Instance.ContentManager.Load<Texture2D>(frameTexName);
            var rectTex = XNADevicesManager.Instance.ContentManager.Load<Texture2D>(rectTexName);

            _hpFrame = new Sprite(frameTex);
            _hpFrame.Z = 2.0f;
            _hpFrame.DrawRectangle = new Rectangle(0, 0, frameTex.Width, frameTex.Height);
            _hpFrame.TransformOrigin = new Vector2(0.0f, 1.0f);
            _hpFrame.IsVisible = false;
            _hpRect = new Sprite(rectTex);
            _hpRect.Z = 1.0f;
            _hpRect.DrawRectangle = new Rectangle(0, 0, rectTex.Width, rectTex.Height);
            _hpRect.TransformOrigin = new Vector2(0.0f, 1.0f);
            _hpRect.IsVisible = false;
            _fade = new Fade(0, 1.0f, 400);
            _fade.Target = _hpFrame;
        }

        public float Remain
        {
            set
            {
                if (value < 0.0f)
                {
                    _remain = 0.0f;
                }
                else if (value > 1.0f)
                {
                    _remain = 1.0f;
                }
                else
                {
                    _remain = value;
                }
                _hpRect.Width = _width*_remain;
            }
            get { return _remain; }
        }

        public float X
        {
            set
            {
                _x = value;
                _hpFrame.X = value;
                _hpRect.X = value;
            }
            get { return _x; }
        }

        public float Y
        {
            set
            {
                _y = value;
                _hpFrame.Y = value;
                _hpRect.Y = value;
            }
            get { return _y; }
        }

        public float Width
        {
            set
            {
                _width = value;
                _hpFrame.Width = value;
            }
            get { return _width; }
        }

        public float Height
        {
            set
            {
                _height = value;
                _hpFrame.Height = value;
                _hpRect.Height = value;
            }
            get { return _height; }
        }

        public bool IsVisible
        {
            set
            {
                _isVisible = value;
                _hpFrame.IsVisible = value;
                _hpRect.IsVisible = value;
            }
            get { return _isVisible; }
        }

        public Character Target
        {
            set
            {
                if (_character != null)
                {
                    _character.Actions["GotHit"].ActionStart -= HPBar_ActionStart;
                }
                if (_hpFrame.Parent != null)
                {
                    _hpFrame.Parent.Remove(_hpFrame);
                }
                if (_hpRect.Parent != null)
                {
                    _hpRect.Parent.Remove(_hpRect);
                }
                _character = value;
                if (value != null)
                {
                    _character.Actions["GotHit"].ActionStart += HPBar_ActionStart;
                    _character.Sprite.Add(_hpFrame);
                    _character.Sprite.Add(_hpRect);
                }
            }
            get { return _character; }
        }

        public new bool Enable
        {
            set
            {
                if (value)
                {
                    SceneManager.Instance.CurrentScene.EventManager.Register(this);
                }
                else
                {
                    SceneManager.Instance.CurrentScene.EventManager.UnRegister(this);
                }
            }
            get { return Order != Constants.INVALID_ORDER; }
        }

        public override EventFunction Handler
        {
            set { _handler = value; }
            get { return e => Update(); }
        }

        public void Show()
        {
            _current = 0;
            _hpRect.Width = _character.HP*_width/_character.MaxHP;
            Enable = true;
            IsVisible = true;
            _fade.Start();
        }

        public void Hide()
        {
            Enable = false;
            IsVisible = false;
        }

        public void Update()
        {
            if (_current < _count)
            {
                if (_character.HP <= 0)
                {
                    Hide();
                }
                _current++;
                _hpRect.Width = _character.HP*_width/_character.MaxHP;
                _hpRect.Alpha = _hpFrame.Alpha;
            }
            else
            {
                Hide();
            }
        }

        private void HPBar_ActionStart(IAction sender)
        {
            Show();
        }
  }
}