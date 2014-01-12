using System;
using System.Collections.ObjectModel;

namespace NervDog.Soul.Defs
{
    [Serializable]
    public class AuditionDef
    {
        public Collection<string> Songs;
        public Collection<string> SoundEffects;
    }
}