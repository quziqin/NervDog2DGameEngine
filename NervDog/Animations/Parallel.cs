using System.Collections.Generic;

namespace NervDog.Animations
{
    public class Parallel : Animation
    {
        private readonly List<Animation> _animationList = new List<Animation>();
        private int _animationCount;

        public void Add(Animation animation)
        {
            _animationList.Add(animation);
        }

        public void Remove(Animation animation)
        {
            _animationList.Remove(animation);
        }

        public void Clear()
        {
            _animationList.Clear();
        }

        public override void Stop()
        {
            for (int i = 0; i < _animationList.Count; i++)
            {
                _animationList[i].Stop();
            }
            base.Stop();
        }

        protected override void PreStart()
        {
            _animationCount = _animationList.Count;
            for (int i = 0; i < _animationList.Count; i++)
            {
                _animationList[i].Speed = _delta;
                _animationList[i].Reverse = _reverse;
                _animationList[i].Loop = false;
                _animationList[i].Start();
            }
        }

        protected override bool UpdateFrame()
        {
            int count = _animationCount;
            for (int i = 0; i < _animationList.Count; i++)
            {
                if (!_animationList[i].IsPlaying)
                {
                    count--;
                    if (count == 0)
                    {
                        return true;
                    }
                }
                //_isReversing = _animationList[i].IsReversing;
            }
            return false;
        }

        protected override void PreEnd()
        {
            _animationCount = 0;
            for (int i = 0; i < _animationList.Count; i++)
            {
                _animationList[i].End();
            }
        }
    }
}