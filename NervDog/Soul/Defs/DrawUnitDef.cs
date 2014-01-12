using System;
using Microsoft.Xna.Framework;

namespace NervDog.Soul
{
    [Serializable]
    public class DrawUnitDef : UnitDef
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
        public float ScaleX = 1.0f;
        public float ScaleY = 1.0f;
        public string TexName = null;
        public Vector2 TransformOrigin = new Vector2(0.5f, 0.5f);
        public float Z = 0.0f;
        public bool ZWriteEnable = false;
    }
}