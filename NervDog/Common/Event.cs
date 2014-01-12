using System;

namespace NervDog.Common
{
    [Serializable]
    public class Event
    {
        //Use public fields for serialization.
        public object Data;
        public string Name;

        public Event()
        {
            Name = Constants.TIME_TICKS_EVENT;
            Data = null;
        }

        public Event(string name, object data = null)
        {
            Name = name;
            Data = data;
        }
    }
}