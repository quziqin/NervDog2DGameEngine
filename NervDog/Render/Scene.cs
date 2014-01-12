using System;
using NervDog.Common;
using NervDog.Managers;

namespace NervDog.Render
{
    public class Scene : IDisposable
    {
        #region Fields

        private readonly EventManager _eventManager;
        private readonly string _name;

        private readonly DrawNode _root;
        protected bool _isPlaying = true;

        #endregion

        #region Properties

        public string Name
        {
            get { return _name; }
        }

        public DrawNode Root
        {
            get { return _root; }
        }

        public EventManager EventManager
        {
            get { return _eventManager; }
        }

        public virtual bool Pause
        {
            set { _isPlaying = !value; }
            get { return !_isPlaying; }
        }

        #endregion

        #region Constructors

        public Scene(string name)
        {
            _name = name;
            _root = new DrawNode();
            _eventManager = new EventManager();
            //Add the default event;
            _eventManager.AddEventType(new EventController(Constants.TIME_TICKS_EVENT));
        }

        #endregion

        #region Public Functions

        public virtual void Dispose()
        {
        }

        public virtual void Initialize()
        {
            _isPlaying = true;
        }

        public virtual void Draw()
        {
            _root.Draw();
        }

        public virtual void Update()
        {
            if (_isPlaying)
            {
                _eventManager.Update();
            }
        }

        #endregion
    }
}