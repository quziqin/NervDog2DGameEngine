using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using NervDog.Managers;

namespace NervDog.Render
{
    public class ColorEffect : IEffect
    {
        private readonly Effect _effect;
        private readonly GraphicsDevice _graphicDevice;
        private bool _ZWriteEnable;
        private float _alpha;
        private Vector3 _color;
        private Matrix _mProjection;
        private Matrix _mView;
        private Matrix _mWorld;
        private string _name = "Color";
        private Texture2D _texture;

        private TextureAddressMode _textureMode = TextureAddressMode.Clamp;

        public ColorEffect(GraphicsDevice device)
        {
            _effect = XNADevicesManager.Instance.ContentManager.Load<Effect>("ColorEffect");
            _graphicDevice = device;
            World = Matrix.Identity;
            View = Matrix.CreateLookAt(new Vector3(0, 0, 10), new Vector3(0, 0, 0), new Vector3(0, 1, 0));
            Projection = Matrix.CreatePerspectiveFieldOfView(MathHelper.PiOver2, _graphicDevice.Viewport.AspectRatio,
                0.1f, 800.0f);
            Alpha = 1.0f;
        }

        public string Name
        {
            set { _name = value; }
            get { return _name; }
        }

        public bool ZWriteEnable
        {
            set { _ZWriteEnable = value; }
            get { return _ZWriteEnable; }
        }

        public Vector3 Color
        {
            set
            {
                _color = value - Vector3.One;
                _effect.Parameters["Color"].SetValue(_color);
            }
            get { return (_color + Vector3.One); }
        }

        public Texture2D Texture
        {
            set
            {
                _texture = value;
                _effect.Parameters["Texture"].SetValue(value);
            }
            get { return _texture; }
        }

        public TextureAddressMode TextureMode
        {
            set { _textureMode = value; }
            get { return _textureMode; }
        }

        public float Alpha
        {
            set
            {
                _alpha = value;
                _effect.Parameters["Alpha"].SetValue(value);
            }
            get { return _alpha; }
        }

        public Matrix Projection
        {
            set
            {
                _mProjection = value;
                _effect.Parameters["Projection"].SetValue(value);
            }
            get { return _mProjection; }
        }

        public Matrix View
        {
            set
            {
                _mView = value;
                _effect.Parameters["View"].SetValue(value);
            }
            get { return _mView; }
        }

        public Matrix World
        {
            set
            {
                _mWorld = value;
                _effect.Parameters["World"].SetValue(value);
            }
            get { return _mWorld; }
        }

        public void Apply()
        {
            _effect.CurrentTechnique.Passes[0].Apply();
            if (_ZWriteEnable)
            {
                _graphicDevice.DepthStencilState = DepthStencilState.Default;
            }
            if (_textureMode == TextureAddressMode.Wrap)
            {
                _graphicDevice.SamplerStates[0] = SamplerState.LinearWrap;
            }
        }
    }
}