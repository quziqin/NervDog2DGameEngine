using System.Collections.Generic;
using NervDog.Common;

namespace NervDog.Managers
{
    public class GroupManager
    {
        private static readonly GroupManager _instance = new GroupManager();
        private readonly Dictionary<uint, RelatedGroupInfo> _relationTable = new Dictionary<uint, RelatedGroupInfo>();

        private GroupManager()
        {
        }

        public static GroupManager Instance
        {
            get { return _instance; }
        }

        public void SetRelationShip(Group fromGroup, Group toGroup, RelationShip relation)
        {
            uint key = ((uint) fromGroup << 16 | (uint) toGroup);
            if (_relationTable.ContainsKey(key))
            {
                _relationTable[key].RelationShip = relation;
            }
            else
            {
                var groupInfo = new RelatedGroupInfo();
                groupInfo.RelationShip = relation;
                _relationTable.Add(key, groupInfo);
            }
        }

        public RelationShip GetRelationShip(Group fromGroup, Group toGroup)
        {
            uint key = ((uint) fromGroup << 16 | (uint) toGroup);
            return _relationTable[key].RelationShip;
        }

        public void SetShouldContact(Group groupOne, Group groupTwo, bool shouldContact)
        {
            uint key = ((uint) groupOne << 16 | (uint) groupTwo);
            if (_relationTable.ContainsKey(key))
            {
                _relationTable[key].ShouldContact = shouldContact;
            }
            else
            {
                var groupInfo = new RelatedGroupInfo();
                groupInfo.ShouldContact = shouldContact;
                _relationTable.Add(key, groupInfo);
                if (groupTwo != groupOne)
                {
                    key = ((uint) groupTwo << 16 | (uint) groupOne);
                    if (_relationTable.ContainsKey(key))
                    {
                        _relationTable[key].ShouldContact = shouldContact;
                    }
                    else
                    {
                        _relationTable.Add(key, groupInfo);
                    }
                }
            }
        }

        public bool ShouldContact(Group groupOne, Group groupTwo)
        {
            uint key = ((uint) groupOne << 16 | (uint) groupTwo);
            return _relationTable[key].ShouldContact;
        }

        public void SetUserData(Group fromGroup, Group toGroup, object userData)
        {
            uint key = ((uint) fromGroup << 16 | (uint) toGroup);
            if (_relationTable.ContainsKey(key))
            {
                _relationTable[key].UserData = userData;
            }
            else
            {
                var groupInfo = new RelatedGroupInfo();
                groupInfo.UserData = userData;
                _relationTable.Add(key, groupInfo);
            }
        }

        public object GetUserData(Group fromGroup, Group toGroup)
        {
            uint key = ((uint) fromGroup << 16 | (uint) toGroup);
            return _relationTable[key].UserData;
        }

        public void ClearAllInfo()
        {
            _relationTable.Clear();
        }
    }
}