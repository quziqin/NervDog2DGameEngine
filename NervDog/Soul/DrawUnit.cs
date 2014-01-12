using Box2D.XNA;
using NervDog.Common;
using NervDog.Managers;
using NervDog.Render;

namespace NervDog.Soul
{
    public class DrawUnit : Unit, IListener
    {
        #region Fields

        private readonly EventManager _eventManager;
        private readonly EventListener _listener;
        private Sprite _sprite;

        #endregion

        #region Properties

        public Sprite Sprite
        {
            set { _sprite = value; }
            get { return _sprite; }
        }

        public EventManager EventManager
        {
            get { return _eventManager; }
        }

        #region IListener implementation

        public string EventName
        {
            get { return _listener.EventName; }
        }

        public bool Enable
        {
            get { return _listener.Enable; }
        }

        public int Order
        {
            set { _listener.Order = value; }
            get { return _listener.Order; }
        }

        public EventFunction Handler
        {
            set { _listener.Handler = value; }
            get { return _listener.Handler; }
        }

        #endregion

        #endregion

        #region Constructors

        public DrawUnit(Sprite sprite, Body body, World world)
            : base(body, world)
        {
            _sprite = sprite;
            _eventManager = world.EventManager;

            _listener = new EventListener(Constants.TIME_TICKS_EVENT);
            _listener.Handler = e => Update();
            _eventManager.Register(_listener);
        }

        #endregion

        #region Public Functions

        public virtual void Update()
        {
            Body body = base.Body;
            if (body.IsAwake() && body.GetType() != BodyType.Static)
            {
                _sprite.X = World.GameValue(body.Position.X);
                _sprite.Y = World.GameValue(body.Position.Y);
                _sprite.RotateZ = body.Rotation;
            }
        }

        public override void Dispose()
        {
            _eventManager.UnRegister(_listener);

            if (_sprite != null)
            {
                _sprite.Parent.Remove(_sprite);
                _sprite = null;
            }
            base.Dispose();
        }

        #endregion
    }
}