using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace NervDog.Render
{
    public class DefaultEffect : IEffect
    {
        private readonly BasicEffect _effect;
        private bool _ZWriteEnable;
        private TextureAddressMode _textureMode = TextureAddressMode.Clamp;

        public DefaultEffect(GraphicsDevice device)
        {
            _effect = new BasicEffect(device);
            _effect.Name = "Default";
            _effect.TextureEnabled = true;
            _effect.World = Matrix.Identity;
            _effect.View = Matrix.CreateLookAt(new Vector3(0, 0, 10), new Vector3(0, 0, 0), new Vector3(0, 1, 0));
            _effect.Projection = Matrix.CreatePerspectiveFieldOfView(MathHelper.PiOver2, device.Viewport.AspectRatio,
                0.1f, 800.0f);
            _effect.Alpha = 1.0f;
            _effect.LightingEnabled = false;
            //base.EnableDefaultLighting();
        }

        public string Name
        {
            set { _effect.Name = value; }
            get { return _effect.Name; }
        }

        public Texture2D Texture
        {
            set { _effect.Texture = value; }
            get { return _effect.Texture; }
        }

        public Matrix Projection
        {
            set { _effect.Projection = value; }
            get { return _effect.Projection; }
        }

        public Matrix View
        {
            set { _effect.View = value; }
            get { return _effect.View; }
        }

        public Matrix World
        {
            set { _effect.World = value; }
            get { return _effect.World; }
        }

        public float Alpha
        {
            set { _effect.Alpha = value; }
            get { return _effect.Alpha; }
        }

        public Vector3 Color
        {
            set { _effect.DiffuseColor = value; }
            get { return _effect.DiffuseColor; }
        }

        public bool ZWriteEnable
        {
            set
            {
                _ZWriteEnable = value;
                if (value)
                {
                    _effect.GraphicsDevice.DepthStencilState = DepthStencilState.Default;
                }
                else
                {
                    _effect.GraphicsDevice.DepthStencilState = DepthStencilState.DepthRead;
                }
            }
            get { return _ZWriteEnable; }
        }

        public TextureAddressMode TextureMode
        {
            set
            {
                _textureMode = value;
                switch (value)
                {
                    case TextureAddressMode.Clamp:
                        _effect.GraphicsDevice.SamplerStates[0] = SamplerState.LinearClamp;
                        break;
                    case TextureAddressMode.Wrap:
                        _effect.GraphicsDevice.SamplerStates[0] = SamplerState.LinearWrap;
                        break;
                    case TextureAddressMode.Mirror:
                        break;
                    default:
                        break;
                }
            }
            get { return _textureMode; }
        }

        public void Apply()
        {
            _effect.CurrentTechnique.Passes[0].Apply();
        }
    }
}