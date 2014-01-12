using System;
using NervDog.Common;

namespace NervDog.Soul.Actions
{
    public class GotHit : IAction
    {
        private static readonly string _name = "GotHit";
        private Character _character;
        private uint _count = 60;
        private uint _current;
        private bool _hitting;
        private int _priority = 4;

        public uint Duration
        {
            set { _count = (uint) Math.Round((double) value/Variables.TargetFrameMilliseconds); }
        }

        public string Name
        {
            get { return _name; }
        }

        public int Priority
        {
            set { _priority = value; }
            get { return _priority; }
        }

        public bool IsDoing
        {
            get { return _hitting; }
        }

        public Character Character
        {
            set
            {
                if (_character != null)
                {
                    _character = value;
                }
            }
            get { return _character; }
        }

        public void Do()
        {
            if (!_hitting)
            {
                _current = 0;
                _hitting = true;
                if (ActionStart != null)
                {
                    ActionStart(this);
                }
            }
        }

        public void Update()
        {
            if (_current < _count)
            {
                _current++;
            }
            else
            {
                _hitting = false;
                if (ActionEnd != null)
                {
                    ActionEnd(this);
                }
            }
        }

        public void Break()
        {
            _hitting = false;
        }

        public event ActionHandler ActionStart = null;
        public event ActionHandler ActionEnd = null;
    }
}