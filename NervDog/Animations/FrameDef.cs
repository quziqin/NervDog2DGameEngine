using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using NervDog.Common;
using NervDog.Managers;
using NervDog.Render;

namespace NervDog.Animations
{
    [Serializable]
    public class FrameDef : AnimationDef
    {
        public Rectangle[] DrawRectangles;
        public uint FrameCount = 0;
        public float[] KeepTimes;
        public bool Loop = false;
        public bool Reverse = false;
        public float Speed = 1.0f;
        private string _texName = string.Empty;
        private Texture2D _texture;

        public string TexName
        {
            set
            {
                if (!string.IsNullOrEmpty(value))
                {
                    _texName = value;
                    _texture = XNADevicesManager.Instance.ContentManager.Load<Texture2D>(_texName);
                }
            }
            get { return _texName; }
        }

        public Texture2D Texture
        {
            get { return _texture; }
        }

        public void SetUp(string tex, uint frameCount)
        {
            TexName = tex;
            DrawRectangles = new Rectangle[frameCount];
            KeepTimes = new float[frameCount];
            FrameCount = frameCount;
        }

        public void SetFrame(int subScript, Rectangle rect, uint keepTime)
        {
            DrawRectangles[subScript] = rect;
            KeepTimes[subScript] = keepTime/Variables.TargetFrameMilliseconds;
        }

        public override Animation ToAnimation(object data)
        {
            var frame = new Frame(this);
            frame.Target = data as Sprite;
            return frame;
        }
    }
}