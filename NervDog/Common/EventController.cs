using System.Collections.Generic;

namespace NervDog.Common
{
    public class EventController : IEventController
    {
        #region Fields

        private readonly string _eventName = string.Empty;
        private readonly List<IListener> _listenerList = new List<IListener>();

        #endregion

        #region Properties

        public string EventName
        {
            get { return _eventName; }
        }

        public IEnumerable<IListener> Listeners
        {
            get { return _listenerList; }
        }

        #endregion

        #region Constructors

        public EventController(string name)
        {
            _eventName = name;
        }

        #endregion

        #region Public Functions

        public void Register(IListener listener)
        {
            if (listener.Order == Constants.INVALID_ORDER)
            {
                listener.Order = _listenerList.Count;
                _listenerList.Add(listener);
            }
        }

        public void UnRegister(IListener listener)
        {
            int order = listener.Order;
            if (order != Constants.INVALID_ORDER)
            {
                int last = _listenerList.Count - 1;
                if (order < last)
                {
                    _listenerList[last].Order = order;
                    _listenerList[order] = _listenerList[last];
                }
                _listenerList.RemoveAt(last);
                listener.Order = Constants.INVALID_ORDER;
            }
        }

        public void Clear()
        {
            for (int i = 0; i < _listenerList.Count; i++)
            {
                _listenerList[i].Order = Constants.INVALID_ORDER;
            }
            _listenerList.Clear();
        }

        public void Update(object data = null)
        {
            for (int i = 0; i < _listenerList.Count; i++)
            {
                IListener listener = _listenerList[i];
                if (listener.Order == Constants.INVALID_ORDER)
                {
                    continue;
                }
                listener.Handler(data);
            }
        }

        #endregion
    }
}