using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using NervDog.Managers;
using NervDog.Utilities;

namespace NervDog.Render
{
    [Serializable]
    public class SpriteDef
    {
        public float Alpha = 1.0f;
        public float B = 1.0f;
        public Rectangle DrawRectangle = Rectangle.Empty;
        public float G = 1.0f;
        public float R = 1.0f;
        public float RepeatX = 0.0f;
        public float RepeatY = 0.0f;
        public float RotateX = 0.0f;
        public float RotateY = 0.0f;
        public float RotateZ = 0.0f;
        public float ScaleX = 1.0f;
        public float ScaleY = 1.0f;
        public string TexName = "particles";
        public Vector2 TransformOrigin = new Vector2(0.5f, 0.5f);
        public float X = 0.0f;
        public float Y = 0.0f;
        public float Z = 0.0f;
        public bool ZWriteEnable = false;

        #region Constructors

        public SpriteDef()
        {
        }

        public SpriteDef(SpriteDef sd)
        {
            TexName = sd.TexName;
            X = sd.X;
            Y = sd.Y;
            Z = sd.Z;
            Alpha = sd.Alpha;
            ScaleX = sd.ScaleX;
            ScaleY = sd.ScaleY;
            RotateX = sd.RotateX;
            RotateY = sd.RotateY;
            RotateZ = sd.RotateZ;
            R = sd.R;
            G = sd.G;
            B = sd.B;
            RepeatX = sd.RepeatX;
            RepeatY = sd.RepeatY;
            DrawRectangle = sd.DrawRectangle;
            TransformOrigin = sd.TransformOrigin;
            ZWriteEnable = sd.ZWriteEnable;
        }

        #endregion

        public Sprite ToSprite()
        {
            var tex = XNADevicesManager.Instance.ContentManager.Load<Texture2D>(TexName);
            Sprite sprite;
            if (RepeatX != 0.0f || RepeatY != 0.0f)
            {
                var rSprite = new RepeatSprite(tex);
                rSprite.RepeatX = RepeatX;
                rSprite.RepeatY = RepeatY;
                sprite = rSprite;
            }
            else
            {
                sprite = new Sprite(tex);
            }
            sprite.DrawRectangle = DrawRectangle;
            sprite.Alpha = Alpha;
            sprite.ScaleX = ScaleX;
            sprite.ScaleY = ScaleY;
            sprite.RotateX = RotateX;
            sprite.RotateY = RotateY;
            sprite.RotateZ = RotateZ;
            sprite.R = R;
            sprite.G = G;
            sprite.B = B;
            sprite.X = X;
            sprite.Y = Y;
            sprite.Z = Z;
            sprite.TransformOrigin = TransformOrigin;
            sprite.ZWriteEnable = ZWriteEnable;
            return sprite;
        }

        //Utilites
        public static void ExportToFile(SpriteDef def, string filename)
        {
            XmlHelper.ExportToFile(def, filename);
        }

        public static SpriteDef LoadFromFile(string filename)
        {
            return XmlHelper.LoadFromFile<SpriteDef>(filename);
        }
    }
}