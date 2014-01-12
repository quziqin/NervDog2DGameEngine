using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace NervDog.Render
{
    public class RepeatSprite : Sprite
    {
        private float _repeatX = 1.0f;
        private float _repeatY = 1.0f;

        public RepeatSprite(Texture2D texture)
            : base(texture)
        {
            base.DrawRectangle = new Rectangle(0, 0, texture.Width, texture.Height);
        }

        public float RepeatX
        {
            set
            {
                _repeatX = value;
                _vertexPosition[1].TextureCoordinate.X = _vertexPosition[5].TextureCoordinate.X = value;
                _vertexPosition[3].TextureCoordinate.X = _vertexPosition[7].TextureCoordinate.X = value;
            }
            get { return _repeatX; }
        }

        public float RepeatY
        {
            set
            {
                _repeatY = value;
                _vertexPosition[2].TextureCoordinate.Y = _vertexPosition[6].TextureCoordinate.Y = value;
                _vertexPosition[3].TextureCoordinate.Y = _vertexPosition[7].TextureCoordinate.Y = value;
            }
            get { return _repeatY; }
        }

        public new Rectangle DrawRectangle
        {
            get { return base.DrawRectangle; }
        }

        public override void Draw()
        {
            TextureAddressMode mode = base.Effect.TextureMode;
            base.Effect.TextureMode = TextureAddressMode.Wrap;
            base.DrawSelf();
            base.Effect.TextureMode = mode;
            base.DrawChildren();
        }
    }
}