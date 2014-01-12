using NervDog.Common;

namespace NervDog.Render
{
    public abstract class DrawComponent : DrawNode, IListener
    {
        private EventFunction _eventHandler;
        private int _order = Constants.INVALID_ORDER;

        public DrawComponent()
        {
            EventName = Constants.TIME_TICKS_EVENT;
        }

        public int Order
        {
            set { _order = value; }
            get { return _order; }
        }

        public EventFunction Handler
        {
            set { _eventHandler = value == null ? Update : value; }
            get { return _eventHandler == null ? Update : _eventHandler; }
        }

        public string EventName { get; set; }

        public bool Enable
        {
            get { return (_order != Constants.INVALID_ORDER); }
        }

        public abstract void Update(object data = null);
    }
}