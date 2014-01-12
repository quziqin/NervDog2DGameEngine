using System;
using System.Collections.Generic;
using Box2D.XNA;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using NervDog.Animations;
using NervDog.Common;
using NervDog.Managers;
using NervDog.Render;
using NervDog.Soul.Actions;

namespace NervDog.Soul
{
    public enum Direction
    {
        Left,
        Right,
        Up,
        Down
    };

    public class Character : DrawUnit
    {
        private const float CHAR_SENSOR_WIDTH_FACTOR = 0.98f;
        private const float VISUAL_RANGE = 3.0f;
        private readonly AI.AI _ai;
        private readonly float _height;
        private readonly SensorInfo _visualSense;
        private readonly float _width;
        private Dictionary<string, IAction> _actions = new Dictionary<string, IAction>();
        private Dictionary<string, Animation> _animations = new Dictionary<string, Animation>();
        private Dictionary<string, SoundEffect> _soundEffects = new Dictionary<string, SoundEffect>();

        private IAction _currentAction;
        private Animation _currentAnimation;
        private int _damage;
        private Direction _direction = Direction.Right;

        private int _hp;
        private bool _isAlive = true;
        private int _maxHp = 1;
        private Vector2 _moveSpeed;

        //for detecting surface
        private SensorInfo _surfaceSense;
        //for detecting enemies

        public Character(Sprite sprite, Body body, World world, CharacterDef charDef)
            : base(sprite, body, world)
        {
            _width = charDef.Width;
            _height = charDef.Height;
            _moveSpeed = charDef.MoveSpeed;

            //create surface sensor
            float xtmp = _width*CHAR_SENSOR_WIDTH_FACTOR*0.5f;
            float x1 = World.B2Value(-xtmp);
            float x2 = World.B2Value(xtmp);
            float ytmp = -_height*0.5f - 2.0f;
            float y1 = World.B2Value(ytmp - 1.0f);
            float y2 = World.B2Value(ytmp);
            var vertices = new Vector2[4];
            vertices[0].X = x2;
            vertices[0].Y = y2;
            vertices[1].X = x1;
            vertices[1].Y = y2;
            vertices[2].X = x1;
            vertices[2].Y = y1;
            vertices[3].X = x2;
            vertices[3].Y = y1;
            _surfaceSense = AttachPolygonSensor(vertices);
            _surfaceSense.SensorName = Constants.SURFACE_SENSOR;

            //create visual sensor
            _visualSense = AttachCircleSensor(Vector2.Zero, _width*VISUAL_RANGE);
            _visualSense.SensorName = Constants.VISUAL_SENSOR;

            HP = charDef.HP;
            MaxHP = charDef.MaxHP;

            //Load animation
            if (charDef.FrameDefs != null)
            {
                for (int i = 0; i < charDef.FrameDefs.Length; i++)
                {
                    FrameDef frameDef = charDef.FrameDefs[i];
                    var frame = new Frame(frameDef);
                    frame.Target = sprite;

                    _animations.Add(frame.Name, frame);
                }
            }

            //Load actions
            if (charDef.Actions != null)
            {
                for (int i = 0; i < charDef.Actions.Length; i++)
                {
                    AttachAction(Activator.CreateInstance(Type.GetType(charDef.Actions[i])) as IAction);
                }
            }

            //Load sound effects
            if (charDef.SoundEffects != null)
            {
                foreach (var i in charDef.SoundEffects)
                {
                    _soundEffects.Add(i.Key, AuditionManager.Instance.SoundEffects[i.Value]);
                }
            }

            //Load AI
            if (!string.IsNullOrEmpty(charDef.AI))
            {
                _ai = new AI.AI(charDef.AI, this);
            }
        }

        public Dictionary<string, Animation> Animations
        {
            get { return _animations; }
        }

        public Dictionary<string, IAction> Actions
        {
            get { return _actions; }
        }

        public Dictionary<string, SoundEffect> SoundEffects
        {
            get { return _soundEffects; }
        }

        public AI.AI AI
        {
            get { return _ai; }
        }

        public Animation CurrentAnimation
        {
            set { _currentAnimation = value; }
            get { return _currentAnimation; }
        }

        public IAction CurrentAction
        {
            get { return _currentAction; }
        }

        public Vector2 MoveSpeed
        {
            set { _moveSpeed = value; }
            get { return _moveSpeed; }
        }

        public Direction Direction
        {
            set { _direction = value; }
            get { return _direction; }
        }

        public float Width
        {
            get { return _width; }
        }

        public float Height
        {
            get { return _height; }
        }

        public SensorInfo VisualSense
        {
            get { return _visualSense; }
        }

        public bool IsOnSurface
        {
            set
            {
                if (value == false)
                {
                    _surfaceSense.Clear();
                }
            }
            get { return (_surfaceSense.SensedCount > 0); }
        }

        public int MaxHP
        {
            set { _maxHp = value <= 0 ? 1 : value; }
            get { return _maxHp; }
        }

        public int HP
        {
            set { _hp = value <= 0 ? 0 : value; }
            get { return _hp; }
        }

        public int Damage
        {
            get { return _damage; }
            set { _damage = value <= 0 ? 0 : value; }
        }

        public bool IsAlive
        {
            set { _isAlive = value; }
            get { return _isAlive; }
        }

        public void AttachAction(IAction action)
        {
            action.Character = this;
            _actions.Add(action.Name, action);
        }

        public void Do(string name)
        {
            IAction action = _actions[name];
            if (!action.IsDoing)
            {
                if (_currentAction != null)
                {
                    if (_currentAction.IsDoing)
                    {
                        if (_currentAction.Priority <= action.Priority)
                        {
                            _currentAction.Break();
                        }
                        else
                        {
                            return;
                        }
                    }
                }
                _currentAction = action;
                action.Do();
            }
        }

        public override void Update()
        {
            base.Update();
            if (_currentAction != null)
            {
                _currentAction.Update();
                if (!_currentAction.IsDoing)
                {
                    _currentAction = null;
                }
            }
        }

        public override void Dispose()
        {
            _animations = null;
            _actions = null;
            _currentAnimation = null;
            _currentAction = null;
            _surfaceSense = null;
            base.Dispose();
        }
    }
}