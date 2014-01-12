using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using NervDog.Render;

namespace NervDog.Managers
{
    public class EffectManager
    {
        private static readonly EffectManager _instance = new EffectManager();
        private readonly Dictionary<string, IEffect> _effects;
        private IEffect _currentEffect;

        private EffectManager()
        {
            _effects = new Dictionary<string, IEffect>();
        }

        public static EffectManager Instance
        {
            get { return _instance; }
        }

        public IEffect CurrentEffect
        {
            get
            {
                if (_currentEffect == null && _effects.Count > 0)
                {
                    _currentEffect = _effects.FirstOrDefault().Value;
                }

                return _currentEffect;
            }
        }

        public void SetView(ref Matrix view)
        {
            foreach (var item in _effects)
            {
                item.Value.View = view;
            }
        }

        public void SetProjection(ref Matrix projection)
        {
            foreach (var item in _effects)
            {
                item.Value.Projection = projection;
            }
        }

        public IEffect GetEffect(string name)
        {
            if (_effects.ContainsKey(name))
            {
                return _effects[name];
            }
            return null;
        }

        public void SwitchCurrentEffect(string name)
        {
            if (_currentEffect != null && _currentEffect.Name == name)
            {
                return;
            }

            IEffect newEffect = GetEffect(name);

            if (newEffect != null)
            {
                _currentEffect = newEffect;
            }
        }

        public void Add(IEffect effect)
        {
            _effects.Add(effect.Name, effect);
        }

        public void Remove(IEffect effect)
        {
            _effects.Remove(effect.Name);
        }

        public void Clear()
        {
            _effects.Clear();
        }
    }
}