using System;
using Box2D.XNA;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using NervDog.Animations;
using NervDog.Common;
using NervDog.Managers;

namespace NervDog.Soul.Actions
{
    public class Attack : IAction
    {
        private static readonly string _name = "Attack";
        private Animation _attack;
        private bool _attacking;
        private bool _bHit;
        private Character _character;
        private uint _current;
        private uint _hitDelay = 10;
        private int _priority = 3;

        public uint HitDelay
        {
            set { _hitDelay = (uint) Math.Round((double) value/Variables.TargetFrameMilliseconds); }
        }

        public SoundEffect Sound
        {
            get
            {
                if (_character.SoundEffects == null || !_character.SoundEffects.ContainsKey("Attack"))
                {
                    return null;
                }
                else
                {
                    return _character.SoundEffects["Attack"];
                }
            }
        }

        public string Name
        {
            get { return _name; }
        }

        public int Priority
        {
            set { _priority = value; }
            get { return _priority; }
        }

        public bool IsDoing
        {
            get { return _attacking; }
        }

        public Character Character
        {
            set
            {
                if (_character == null)
                {
                    _character = value;
                    _attack = _character.Animations["Attack"];
                    _attack.OnEnd += Attack_OnEnd;
                }
            }
            get { return _character; }
        }

        public void Do()
        {
            if (!_attacking)
            {
                _attacking = true;
                _current = 0;
                _bHit = false;
                if (!_attack.IsPlaying)
                {
                    if (_character.CurrentAnimation != null)
                    {
                        if (_character.CurrentAnimation.IsPlaying)
                        {
                            _character.CurrentAnimation.Stop();
                        }
                    }
                    _character.CurrentAnimation = _attack;
                    _attack.Start();
                }
                if (ActionStart != null)
                {
                    ActionStart(this);
                }
            }
        }

        public void Update()
        {
            if (!_bHit)
            {
                if (_current < _hitDelay)
                {
                    _current++;
                }
                else
                {
                    _bHit = true;
                    if (Sound != null)
                    {
                        Sound.Play();
                    }
                    AABB aabb;
                    if (_character.Direction == Direction.Left)
                    {
                        aabb.upperBound =
                            World.B2Value(new Vector2(_character.Position.X - _character.Width/2,
                                _character.Position.Y + 50));
                        aabb.lowerBound =
                            World.B2Value(new Vector2(_character.Position.X - _character.Width/2 - 60,
                                _character.Position.Y - 50));
                    }
                    else
                    {
                        aabb.upperBound =
                            World.B2Value(new Vector2(_character.Position.X + _character.Width/2 + 60,
                                _character.Position.Y + 50));
                        aabb.lowerBound =
                            World.B2Value(new Vector2(_character.Position.X + _character.Width/2,
                                _character.Position.Y - 50));
                    }
                    _character.World.PhysicsWorld.QueryAABB(HitUnit, ref aabb);
                }
            }
        }

        public void Break()
        {
            _bHit = false;
            _attacking = false;
            _attack.Stop();
        }

        public event ActionHandler ActionStart = null;

        public event ActionHandler ActionEnd = null;

        public bool HitUnit(FixtureProxy fixtureProxy)
        {
            if (fixtureProxy.fixture.GetUserData() is SensorInfo)
            {
                return true;
            }

            var unit = fixtureProxy.fixture.GetBody().GetUserData() as Unit;
            if (unit != null)
            {
                if (unit.Group == Group.Destructable)
                {
                    unit.ApplyImpulse(unit.Position - _character.Position, 5);
                }
                else
                {
                    var character = unit as Character;
                    if (character != null)
                    {
                        if (GroupManager.Instance.GetRelationShip(_character.Group, character.Group) ==
                            RelationShip.Enemy)
                        {
                            character.HP -= _character.Damage;
                            character.ApplyImpulse(character.Position - _character.Position, 5);
                            if (character.HP <= 0)
                            {
                                character.Do("Dying");
                            }
                            else
                            {
                                character.Do("GotHit");
                            }
                        }
                    }
                }
            }
            return true;
        }

        private void Attack_OnEnd(Animation sender)
        {
            if (_attacking)
            {
                _attacking = false;
                if (ActionEnd != null)
                {
                    ActionEnd(this);
                }
            }
        }
    }
}