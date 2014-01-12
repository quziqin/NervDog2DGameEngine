namespace NervDog.Soul.Actions
{
    public delegate void ActionHandler(IAction sender);

    public interface IAction
    {
        string Name { get; }

        int Priority { set; get; }

        bool IsDoing { get; }

        Character Character { set; get; }

        void Do();
        void Update();
        void Break();

        event ActionHandler ActionStart;
        event ActionHandler ActionEnd;
    }
}