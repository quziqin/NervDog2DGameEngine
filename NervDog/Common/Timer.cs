using System;
using NervDog.Managers;

namespace NervDog.Common
{
    public delegate void TimerHandler(Timer sender);

    public class Timer : EventListener
    {
        #region 成员

        private uint _count;
        private uint _currentCount;
        private uint _currentLoop;
        private uint _loop;

        #endregion

        #region 属性

        public uint Delay
        {
            set { _count = (uint) Math.Round((double) value/Variables.TargetFrameMilliseconds); }
        }

        public uint LoopTimes
        {
            set { _loop = value; }
        }

        public uint CurrentLoop
        {
            get { return _currentLoop; }
        }

        public object EventData { get; set; }


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

        #endregion

        #region 方法

        public Timer(uint delay, uint loop = 1)
            : base(Constants.TIME_TICKS_EVENT)
        {
            Delay = delay;
            _loop = loop;
        }

        public void Start()
        {
            _currentLoop = 0;
            _currentCount = 0;
            Enable = true;
        }

        public void Stop()
        {
            Enable = false;
        }

        public void Update()
        {
            if (++_currentCount > _count)
            {
                if (++_currentLoop < _loop || _loop == 0)
                {
                    _currentCount = 0;
                }
                else
                {
                    Enable = false;
                }
                if (OnTimer != null)
                {
                    OnTimer(this);
                }
            }
        }

        #endregion

        public event TimerHandler OnTimer = null;
    }
}