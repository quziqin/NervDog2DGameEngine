using System;
using System.Collections.Generic;
using NervDog.Common;
using NervDog.Soul;

namespace NervDog.Managers
{
    public class AIManager : IEventController
    {
        #region Fields

        private readonly Dictionary<Guid, AI.AI> _AIs = new Dictionary<Guid, AI.AI>();

        #endregion

        #region Properties

        public string EventName
        {
            get { return Constants.AI_EVENT; }
        }

        public IEnumerable<IListener> Listeners
        {
            get { return _AIs.Values; }
        }

        #endregion

        #region Constructors

        #endregion

        #region Public Functions

        public void Register(IListener listener)
        {
            if (listener is AI.AI)
            {
                Register(listener as AI.AI);
            }
        }

        public void UnRegister(IListener listener)
        {
            if (listener is AI.AI)
            {
                UnRegister(listener as AI.AI);
            }
        }

        public void Clear()
        {
            _AIs.Clear();
        }

        public void Update(object data = null)
        {
            foreach (AI.AI ai in _AIs.Values)
            {
                ai.Execute();
            }
        }

        public void Register(AI.AI ai)
        {
            if (!_AIs.ContainsKey(ai.ID))
            {
                _AIs.Add(ai.ID, ai);
            }
        }

        public void Register(Character character)
        {
            Register(character.AI);
        }

        public void UnRegister(AI.AI ai)
        {
            UnRegister(ai.ID);
        }

        public void UnRegister(Character character)
        {
            UnRegister(character.AI);
        }

        public void UnRegister(Guid id)
        {
            if (!_AIs.ContainsKey(id))
            {
                _AIs.Remove(id);
            }
        }

        #endregion
    }
}