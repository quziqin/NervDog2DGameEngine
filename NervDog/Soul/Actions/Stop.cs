namespace NervDog.Soul.Actions
{
    public class Stop : IAction
    {
        private Character _character;
        private int _priority = 2;

        public string Name
        {
            get { return "Stop"; }
        }

        public int Priority
        {
            set { _priority = value; }
            get { return _priority; }
        }

        public bool IsDoing
        {
            get { return false; }
        }

        public Character Character
        {
            set
            {
                if (_character == null)
                {
                    _character = value;
                }
            }
            get { return _character; }
        }

        public void Do()
        {
            if (ActionStart != null)
            {
                ActionStart(this);
            }

            if (ActionEnd != null)
            {
                ActionEnd(this);
            }
        }

        public void Update()
        {
        }

        public void Break()
        {
        }

        public event ActionHandler ActionStart = null;
        public event ActionHandler ActionEnd = null;
    }
}