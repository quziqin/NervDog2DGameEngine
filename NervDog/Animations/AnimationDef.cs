using System;

namespace NervDog.Animations
{
    [Serializable]
    public abstract class AnimationDef
    {
        public string Name = string.Empty;
        public abstract Animation ToAnimation(object data = null);
    }
}