using System;
using Microsoft.Xna.Framework;

namespace NervDog.Soul
{
    [Serializable]
    public class EdgeDef
    {
        public Point A;
        public Point B;
        public float Friction;
        public float Restitution;
    }
}