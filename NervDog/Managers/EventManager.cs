using System.Collections.Generic;
using NervDog.Common;

namespace NervDog.Managers
{
    public class EventManager
    {
        #region Fields

        private readonly Dictionary<string, IEventController> _eventMap = new Dictionary<string, IEventController>();
        private readonly Queue<Event> _eventQueue = new Queue<Event>();

        #endregion

        #region Constructors

        public EventManager()
        {
            //TODO:Load predefined event;
        }

        public EventManager(object def)
        {
        }

        #endregion

        #region Public Functions

        public void AddEventType(IEventController type)
        {
            if (!_eventMap.ContainsKey(type.EventName))
            {
                _eventMap.Add(type.EventName, type);
            }
        }

        public void RemoveEventType(IEventController type)
        {
            if (!_eventMap.ContainsKey(type.EventName))
            {
                _eventMap.Remove(type.EventName);
            }
        }

        public void PostEvent(Event e)
        {
            _eventQueue.Enqueue(e);
        }

        public void SendEvent(Event e)
        {
            _eventMap[e.Name].Update(e.Data);
        }

        public void Register(IListener listener)
        {
            _eventMap[listener.EventName].Register(listener);
        }

        public void UnRegister(IListener listener)
        {
            _eventMap[listener.EventName].UnRegister(listener);
        }

        public void Update()
        {
            while (_eventQueue.Count != 0)
            {
                Event e = _eventQueue.Dequeue();
                _eventMap[e.Name].Update(e.Data);
            }
        }

        #endregion
    }
}