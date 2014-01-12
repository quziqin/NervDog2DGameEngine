using System;

namespace NervDog.Common
{
    [Serializable]
    internal class RelatedGroupInfo
    {
        public RelationShip RelationShip = RelationShip.Neutral;
        public bool ShouldContact = true;
        public object UserData;
    }
}