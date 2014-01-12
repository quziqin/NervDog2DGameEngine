namespace NervDog.Common
{
    public delegate void EventFunction(object eventData = null);

    public interface IListener
    {
        //The event name that you want to listen.
        string EventName { get; }

        bool Enable { get; }

        int Order { set; get; }

        EventFunction Handler { set; get; }
    }
}