namespace NervDog.Common
{
    public class EventListener : IListener
    {
        protected string _eventName = null;
        protected EventFunction _handler;
        protected int _order = Constants.INVALID_ORDER;

        public EventListener(string eventName)
        {
            _eventName = eventName;
        }

        public EventListener()
            : this(Constants.TIME_TICKS_EVENT)
        {
        }

        public int Order
        {
            set { _order = value; }
            get { return _order; }
        }

        public virtual EventFunction Handler
        {
            set { _handler = value; }
            get { return _handler; }
        }

        public string EventName
        {
            get { return _eventName; }
        }

        public bool Enable
        {
            get { return (_order != Constants.INVALID_ORDER); }
        }
    }
}