using System.Collections.Generic;

namespace NervDog.Animations
{
    public class Sequence : Animation
    {
        private readonly List<Animation> _animationList = new List<Animation>();
        private Animation _currentAnimation;

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
            _currentAnimation.Stop();
            base.Stop();
        }

        protected override void PreStart()
        {
            _current = 1;
            _currentAnimation = _animationList[0];
            for (int i = 0; i < _animationList.Count; i++)
            {
                _animationList[i].Speed = _delta;
                _animationList[i].Reverse = _reverse;
                _animationList[i].Loop = false;
            }
            _currentAnimation.Start();
        }

        protected override bool UpdateFrame()
        {
            if (_reverse)
            {
                if (_isReversing)
                {
                    if (!_currentAnimation.IsPlaying)
                    {
                        if (_current >= 0)
                        {
                            _currentAnimation = _animationList[(int) _current];
                            _currentAnimation.Enable = true;
                            _current--;
                        }
                        else
                        {
                            _isReversing = false;
                            return true;
                        }
                    }
                }
                else
                {
                    if (_currentAnimation.IsReversing)
                    {
                        if (_current < _animationList.Count)
                        {
                            _currentAnimation.Enable = false;
                            _currentAnimation = _animationList[(int) _current];
                            _currentAnimation.Start();
                            _current++;
                        }
                        else
                        {
                            _current = _animationList.Count - 1;
                            _isReversing = true;
                        }
                    }
                }
            }
            else
            {
                if (!_currentAnimation.IsPlaying)
                {
                    if (_current < _animationList.Count)
                    {
                        _currentAnimation = _animationList[(int) _current];
                        _currentAnimation.Start();
                        _current++;
                    }
                    else
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        protected override void PreEnd()
        {
            if (_reverse)
            {
                _animationList[0].End();
            }
            else
            {
                _animationList[_animationList.Count - 1].End();
            }
        }
    }
}