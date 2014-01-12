using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using NervDog.Managers;

namespace NervDog.Render
{
    public class Sprite : DrawNode
    {
        #region Fields

        private static readonly short[] _indices = new short[12] {0, 1, 2, 2, 1, 3, 7, 5, 6, 6, 5, 4};
        private bool _ZWriteEnable;

        private float _alpha = 1.0f;

        private Vector3 _color = Vector3.One;
        private Rectangle _drawRectangle;

        private IEffect _effect;
        private int _height;
        private bool _isRotated = true;
        private bool _isScaled = true;
        private bool _isTranslated = true;
        private Matrix _mLocalTransform;

        private Matrix _mRotateX = Matrix.Identity;
        private Matrix _mRotateY = Matrix.Identity;
        private Matrix _mRotateZ = Matrix.Identity;
        private Matrix _mScale;
        private Matrix _mTranslate;
        private Vector3 _position;
        private Vector3 _rotate;
        private Vector3 _scale = Vector3.One;
        private Texture2D _texture;
        private Vector2 _transformOrigin = new Vector2(0.5f, 0.5f);
        protected VertexPositionNormalTexture[] _vertexPosition = new VertexPositionNormalTexture[8];
        private int _width;

        #endregion

        #region Properties

        public Texture2D Texture
        {
            set { _texture = value; }
            get { return _texture; }
        }

        public virtual float X
        {
            set
            {
                _position.X = value;
                _isTranslated = true;
            }
            get { return _position.X; }
        }

        public virtual float Y
        {
            set
            {
                _position.Y = value;
                _isTranslated = true;
            }
            get { return _position.Y; }
        }

        public virtual float Z
        {
            set
            {
                _position.Z = value;
                base.DrawOrder = value;
                _isTranslated = true;
            }
            get { return _position.Z; }
        }

        public virtual Vector3 Position
        {
            set
            {
                _position = value;
                base.DrawOrder = value.Z;
                _isTranslated = true;
            }
            get { return _position; }
        }

        public float Alpha
        {
            set
            {
                if (value < 0)
                {
                    _alpha = 0.0f;
                }
                else if (value > 1.0f)
                {
                    _alpha = 1.0f;
                }
                else
                {
                    _alpha = value;
                }
            }
            get { return _alpha; }
        }

        public float RotateX
        {
            set
            {
                _rotate.X = value;
                Matrix.CreateRotationX(value, out _mRotateX);
                _isRotated = true;
            }
            get { return _rotate.X; }
        }

        public float RotateY
        {
            set
            {
                _rotate.Y = value;
                Matrix.CreateRotationY(value, out _mRotateY);
                _isRotated = true;
            }
            get { return _rotate.Y; }
        }

        public float RotateZ
        {
            set
            {
                _rotate.Z = value;
                Matrix.CreateRotationZ(value, out _mRotateZ);
                _isRotated = true;
            }
            get { return _rotate.Z; }
        }

        public Vector2 TransformOrigin
        {
            set
            {
                _transformOrigin = value;
                UpdateVertices();
            }
            get { return _transformOrigin; }
        }

        public float ScaleX
        {
            set
            {
                _scale.X = (value > 0.0f ? value : 0.0f);
                _isScaled = true;
            }
            get { return _scale.X; }
        }

        public float ScaleY
        {
            set
            {
                _scale.Y = (value > 0.0f ? value : 0.0f);
                _isScaled = true;
            }
            get { return _scale.Y; }
        }

        public float Width
        {
            set { ScaleX = value/_width; }
            get { return _width*Math.Abs(_scale.X); }
        }

        public float Height
        {
            set { ScaleY = value/_height; }
            get { return _height*Math.Abs(_scale.Y); }
        }

        public float R
        {
            set { _color.X = value; }
            get { return _color.X; }
        }

        public float G
        {
            set { _color.Y = value; }
            get { return _color.Y; }
        }

        public float B
        {
            set { _color.Z = value; }
            get { return _color.Z; }
        }

        public Rectangle DrawRectangle
        {
            set
            {
                _drawRectangle = value;

                _width = _drawRectangle.Width;
                _height = _drawRectangle.Height;

                //calculate texture coordinate
                float left = (float) _drawRectangle.X/_texture.Width;
                float top = (float) _drawRectangle.Y/_texture.Height;
                float right = (float) (_drawRectangle.X + _drawRectangle.Width)/_texture.Width;
                float bottom = (float) (_drawRectangle.Y + _drawRectangle.Height)/_texture.Height;

                //left top
                _vertexPosition[0].TextureCoordinate.X = _vertexPosition[4].TextureCoordinate.X = left;
                _vertexPosition[0].TextureCoordinate.Y = _vertexPosition[4].TextureCoordinate.Y = top;
                //right top
                _vertexPosition[1].TextureCoordinate.X = _vertexPosition[5].TextureCoordinate.X = right;
                _vertexPosition[1].TextureCoordinate.Y = _vertexPosition[5].TextureCoordinate.Y = top;
                //left bottom
                _vertexPosition[2].TextureCoordinate.X = _vertexPosition[6].TextureCoordinate.X = left;
                _vertexPosition[2].TextureCoordinate.Y = _vertexPosition[6].TextureCoordinate.Y = bottom;
                //right bottom
                _vertexPosition[3].TextureCoordinate.X = _vertexPosition[7].TextureCoordinate.X = right;
                _vertexPosition[3].TextureCoordinate.Y = _vertexPosition[7].TextureCoordinate.Y = bottom;

                UpdateVertices();
            }
            get { return _drawRectangle; }
        }

        public IEffect Effect
        {
            set { _effect = value; }
            get { return _effect; }
        }

        public bool ZWriteEnable
        {
            set { _ZWriteEnable = value; }
            get { return _ZWriteEnable; }
        }

        #endregion

        #region Constructors

        public Sprite(Texture2D texture, IEffect effect = null)
        {
            _texture = texture;
            _effect = effect ?? EffectManager.Instance.CurrentEffect;

            for (int i = 0; i < 4; i++)
            {
                _vertexPosition[i].Normal = Vector3.Backward;
            }

            for (int i = 4; i < 8; i++)
            {
                _vertexPosition[i].Normal = Vector3.Forward;
            }
        }

        public Sprite(SpriteDef def)
        {
            var tex = XNADevicesManager.Instance.ContentManager.Load<Texture2D>(def.TexName);

            _texture = tex;
            _effect = EffectManager.Instance.CurrentEffect;

            for (int i = 0; i < 4; i++)
            {
                _vertexPosition[i].Normal = Vector3.Backward;
            }

            for (int i = 4; i < 8; i++)
            {
                _vertexPosition[i].Normal = Vector3.Forward;
            }

            DrawRectangle = def.DrawRectangle;
            Alpha = def.Alpha;
            ScaleX = def.ScaleX;
            ScaleY = def.ScaleY;
            RotateX = def.RotateX;
            RotateY = def.RotateY;
            RotateZ = def.RotateZ;
            R = def.R;
            G = def.G;
            B = def.B;
            X = def.X;
            Y = def.Y;
            Z = def.Z;
            TransformOrigin = def.TransformOrigin;
            ZWriteEnable = def.ZWriteEnable;
        }

        #endregion

        #region Private Functions

        private void UpdateVertices()
        {
            float hotX = _drawRectangle.Width*_transformOrigin.X;
            _vertexPosition[0].Position.X = _vertexPosition[4].Position.X = -hotX;
            _vertexPosition[1].Position.X = _vertexPosition[5].Position.X = _drawRectangle.Width - hotX;
            _vertexPosition[2].Position.X = _vertexPosition[6].Position.X = -hotX;
            _vertexPosition[3].Position.X = _vertexPosition[7].Position.X = _drawRectangle.Width - hotX;

            float hotY = _drawRectangle.Height*_transformOrigin.Y;
            _vertexPosition[0].Position.Y = _vertexPosition[4].Position.Y = hotY;
            _vertexPosition[1].Position.Y = _vertexPosition[5].Position.Y = hotY;
            _vertexPosition[2].Position.Y = _vertexPosition[6].Position.Y = hotY - _drawRectangle.Height;
            _vertexPosition[3].Position.Y = _vertexPosition[7].Position.Y = hotY - _drawRectangle.Height;
        }

        #endregion

        #region Public Functions

        public override void DrawSelf()
        {
            if (_isRotated || _isTranslated || _isScaled)
            {
                if (_isRotated)
                {
                    _isRotated = false;
                }
                if (_isTranslated)
                {
                    _isTranslated = false;
                    Matrix.CreateTranslation(ref _position, out _mTranslate);
                }
                if (_isScaled)
                {
                    _isScaled = false;
                    Matrix.CreateScale(ref _scale, out _mScale);
                }
                _mLocalTransform = _mScale*_mRotateX*_mRotateY*_mRotateZ*_mTranslate;
                _mWorld = _parent != null ? _mLocalTransform*_parent.World : _mLocalTransform*Matrix.Identity;
            }
            else
            {
                _mWorld = _parent != null ? _mLocalTransform*_parent.World : _mLocalTransform*Matrix.Identity;
            }

            _effect.Alpha = _alpha;
            _effect.Texture = _texture;
            _effect.Color = _color;
            _effect.World = _mWorld;
            _effect.ZWriteEnable = _ZWriteEnable;
            _effect.Apply();

            //Use XNA API to draw sprite.
            XNADevicesManager.Instance.GraphicsDevice.DrawUserIndexedPrimitives(PrimitiveType.TriangleList,
                _vertexPosition, 0, 8, _indices, 0, 4);
        }

        #endregion
    }
}