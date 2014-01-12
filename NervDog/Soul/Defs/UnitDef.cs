using System;
using Box2D.XNA;
using NervDog.Common;

namespace NervDog.Soul
{
    [Serializable]
    public class UnitDef
    {
        public BodyType BodyType = BodyType.Dynamic;
        public float Density = 1.0f;
        public bool FixedRotation = false;
        public float Fraction = 0.4f;
        public Group Group = Group.PlayerOne;
        public float Restitution = 0.4f;
        public float RotateZ = 0.0f;
        public float X = 0.0f;
        public float Y = 0.0f;

        public string UnitType
        {
            set
            {
                BodyType type;
                if (!Enum.TryParse(value, out type))
                {
                    type = BodyType.Dynamic;
                }
                BodyType = type;
            }

            get { return BodyType.ToString(); }
        }
    }

    public static class UnitType
    {
        public const string Static = "Static";
        public const string Kinematic = "Kinematic";
        public const string Dynamic = "Dynamic";
    }

    [Serializable]
    public class KeyValuePair
    {
        public string Key;
        public string Value;

        public KeyValuePair()
        {
            
        }

        public KeyValuePair(string key, string value)
        {
            Key = key;
            Value = value;
        }
    }
}