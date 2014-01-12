using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using NervDog.Animations;
using NervDog.Cameras;
using NervDog.Common;
using NervDog.Managers;
using NervDog.Render;

namespace NervDog.Misc
{
    public class Board : EventListener
    {
        private readonly Fade _fade;
        private readonly Scale _scale;
        private readonly Sprite _sprite;
        private readonly UnitCamera _unitCam;

        public Board(string texName, UnitCamera cam)
            : base(Constants.TIME_TICKS_EVENT)
        {
            var tex = XNADevicesManager.Instance.ContentManager.Load<Texture2D>(texName);
            _unitCam = cam;
            _sprite = new Sprite(tex);
            _sprite.DrawRectangle = new Rectangle(0, 0, tex.Width, tex.Height);
            _sprite.IsVisible = false;
            _fade = new Fade(0.0f, 1.0f, 1000);
            _fade.Target = _sprite;
            _scale = new Scale(0.2f, 1.0f, 0.2f, 1.0f, 1000);
            _scale.Target = _sprite;
        }

        public Sprite Sprite
        {
            get { return _sprite; }
        }

        public float Distance { set; get; }

        public bool IsFading
        {
            get { return _fade.IsPlaying; }
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

        public event EventHandler OnHide = null;

        public void Show()
        {
            Enable = true;
            _sprite.IsVisible = true;
            _scale.EaseFunction = EaseFunction.Back_Out_Cubic;
            _scale.ScaleXFrom = 0.2f;
            _scale.ScaleYFrom = 0.2f;
            _scale.ScaleXTo = 1.0f;
            _scale.ScaleYTo = 1.0f;
            _fade.AlphaFrom = 0.0f;
            _fade.AlphaTo = 1.0f;
            _scale.Start();
            _fade.Start();
            _scale.OnEnd -= _scale_OnEnd;
        }

        public void SwitchTex(string texName)
        {
            var tex = XNADevicesManager.Instance.ContentManager.Load<Texture2D>(texName);
            _sprite.Texture = tex;
        }

        private void _scale_OnEnd(Animation sender)
        {
            _sprite.IsVisible = false;
            Enable = false;
            if (OnHide != null)
            {
                OnHide(null, null);
            }
        }

        public void Hide()
        {
            Enable = true;
            _sprite.IsVisible = true;
            _scale.EaseFunction = EaseFunction.Back_In_Cubic;
            _scale.ScaleXFrom = 1.0f;
            _scale.ScaleYFrom = 1.0f;
            _scale.ScaleXTo = 0.2f;
            _scale.ScaleYTo = 0.2f;
            _fade.AlphaFrom = 1.0f;
            _fade.AlphaTo = 0.0f;
            _scale.OnEnd += _scale_OnEnd;
            _scale.Start();
            _fade.Start();
        }

        public void Update()
        {
            _sprite.Position = _unitCam.Position;
            _sprite.Z -= Distance;
        }

        //Override
    }
}