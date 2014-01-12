using System.Collections.Generic;

namespace NervDog.Common
{
    public interface IEventController
    {
        string EventName { get; }

        IEnumerable<IListener> Listeners { get; }

        void Register(IListener listener);

        void UnRegister(IListener listener);

        void Clear();

        void Update(object data = null);
    }
}