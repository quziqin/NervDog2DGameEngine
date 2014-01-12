using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using NervDog.Animations;
using NervDog.Cameras;
using NervDog.Common;
using NervDog.Managers;
using NervDog.Render;

namespace NervDog.Misc
{
    public class Logo : EventListener
    {
        private readonly Scale _scale;
        private readonly Sprite _sprite;
        private readonly Timer _timer;
        private readonly UnitCamera _unitCam;

        public Logo(string texName, UnitCamera cam)
            : base(Constants.TIME_TICKS_EVENT)
        {
            var tex = XNADevicesManager.Instance.ContentManager.Load<Texture2D>(texName);
            _unitCam = cam;
            _sprite = new Sprite(tex);
            _sprite.DrawRectangle = new Rectangle(0, 0, tex.Width, tex.Height);
            _scale = new Scale(0.2f, 1.0f, 0.2f, 1.0f, 1500);
            _scale.EaseFunction = EaseFunction.Out_Elastic;
            _scale.Target = _sprite;
            _timer = new Timer(2000);
            _timer.OnTimer += _timer_OnTimer;
        }

        public Sprite Sprite
        {
            get { return _sprite; }
        }

        public float Distance { set; get; }

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

        private void _timer_OnTimer(Timer sender)
        {
            Hide();
        }

        public void Show()
        {
            Enable = true;
            _sprite.IsVisible = true;
            _scale.EaseFunction = EaseFunction.Out_Elastic;
            _scale.ScaleXFrom = 0.2f;
            _scale.ScaleYFrom = 0.2f;
            _scale.ScaleXTo = 1.0f;
            _scale.ScaleYTo = 1.0f;
            _scale.OnEnd -= _scale_OnEnd;
            _scale.OnEnd += _scale_OnEnd1;
            _scale.Start();
        }

        private void _scale_OnEnd1(Animation sender)
        {
            _timer.Start();
        }

        private void _scale_OnEnd(Animation sender)
        {
            _sprite.Parent.Remove(_sprite);
        }

        public void Hide()
        {
            Enable = true;
            _sprite.IsVisible = true;
            _scale.EaseFunction = EaseFunction.In_Elastic;
            _scale.ScaleXFrom = 1.0f;
            _scale.ScaleYFrom = 1.0f;
            _scale.ScaleXTo = 0.2f;
            _scale.ScaleYTo = 0.2f;
            _scale.OnEnd -= _scale_OnEnd1;
            _scale.OnEnd += _scale_OnEnd;
            _scale.Start();
        }

        public void Update()
        {
            _sprite.Position = _unitCam.Position;
            _sprite.Z -= Distance;
        }

        //Override
    }
}