using NervDog.Common;
using NervDog.Managers;

namespace NervDog.Animations
{
    public delegate void AnimationHandler(Animation sender);

    public abstract class Animation : EventListener
    {
        #region 成员

        protected float _current = 0.0f;
        protected float _delta = 1.0f;
        protected bool _isPlaying = false;
        protected bool _isReversing = false;
        protected bool _loop = false;
        protected bool _reverse = false;

        #endregion

        #region 属性

        public float Speed
        {
            set { _delta = (value < 0.0f ? 0.0f : value); }
            get { return _delta; }
        }

        public bool Loop
        {
            set { _loop = value; }
            get { return _loop; }
        }

        public bool Reverse
        {
            set { _reverse = value; }
            get { return _reverse; }
        }

        public bool IsReversing
        {
            get { return _isReversing; }
        }

        public bool IsPlaying
        {
            get { return _isPlaying; }
        }

        public new bool Enable
        {
            set
            {
                if (value)
                {
                    SceneManager.Instance.CurrentScene.EventManager.Register(this);
                }
                else
                {
                    SceneManager.Instance.CurrentScene.EventManager.UnRegister(this);
                }
            }
            get { return Order != Constants.INVALID_ORDER; }
        }

        public override EventFunction Handler
        {
            set { _handler = value; }
            get { return e => Update(); }
        }

        public event AnimationHandler OnEnd;

        #endregion

        public Animation()
            : base(Constants.TIME_TICKS_EVENT)
        {
        }

        public void Start()
        {
            _isPlaying = true;
            PreStart();
            Enable = true;
        }

        public void End()
        {
            _isPlaying = false;
            PreEnd();
            Enable = false;
            if (OnEnd != null)
            {
                OnEnd(this);
            }
        }

        public virtual void Stop()
        {
            _isPlaying = false;
            Enable = false;
        }

        public void Update()
        {
            if (_isPlaying)
            {
                if (UpdateFrame())
                {
                    if (_loop)
                    {
                        PreStart();
                        UpdateFrame();
                    }
                    else
                    {
                        End();
                    }
                }
            }
        }

        protected abstract void PreStart();
        protected abstract bool UpdateFrame();
        protected abstract void PreEnd();
    }
}