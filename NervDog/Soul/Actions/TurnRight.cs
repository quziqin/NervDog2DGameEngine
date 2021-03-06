﻿namespace NervDog.Soul.Actions
{
    public class TurnRight : IAction
    {
        private Character _character;
        private int _priority = 1;

        public string Name
        {
            get { return "TurnRight"; }
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
            if (_character.Direction != Direction.Right)
            {
                _character.Direction = Direction.Right;
                _character.Sprite.RotateY = 0.0f; //MathHelper.Pi;
                //_character.Sprite.ScaleX = Math.Abs(_character.Sprite.ScaleX);
            }

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