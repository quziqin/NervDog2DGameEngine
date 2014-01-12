using System;
using Microsoft.Xna.Framework;
using NervDog.Animations;

namespace NervDog.Soul
{
    [Serializable]
    public class CharacterDef : DrawUnitDef
    {
        public string AI = string.Empty;
        public string[] Actions = null;
        public FrameDef[] FrameDefs = null;
        public int HP = 10;
        public float Height = 0;
        public int MaxHP = 10;
        public Vector2 MoveSpeed = Vector2.Zero;
        public float Width = 0;
        public int Damage = 1;
        public KeyValuePair[] SoundEffects = null;
    }
}