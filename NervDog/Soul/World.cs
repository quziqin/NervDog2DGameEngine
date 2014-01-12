using Box2D.XNA;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using NervDog.Common;
using NervDog.Managers;
using NervDog.Render;

namespace NervDog.Soul
{
    public class World : EventListener
    {
        public const float B2FACTOR = 100.0f;
        private readonly EventManager _eventManager;
        private readonly Box2D.XNA.World _world;

        private int _positionIterations = 6;
        private float _timeStep;
        private int _velocityIterations = 8;

        public World(Vector2 gravity, EventManager eventManager)
            : base(Constants.TIME_TICKS_EVENT)
        {
            _world = new Box2D.XNA.World(gravity, true);
            _world.ContactListener = new ContactListener();
            _timeStep = Variables.TargetFrameMilliseconds/1000.0f;
            _eventManager = eventManager;
        }

        public override EventFunction Handler
        {
            set { _handler = value; }
            get { return e => Update(); }
        }

        public int VelocityIterations
        {
            set { _velocityIterations = value; }
            get { return _velocityIterations; }
        }

        public int PositionIterations
        {
            set { _positionIterations = value; }
            get { return _positionIterations; }
        }

        public float TimeStep
        {
            set { _timeStep = value; }
            get { return _timeStep; }
        }

        public bool IsRunning
        {
            get { return Enable; }
        }

        public Box2D.XNA.World PhysicsWorld
        {
            get { return _world; }
        }

        public EventManager EventManager
        {
            get { return _eventManager; }
        }

        public new bool Enable
        {
            set
            {
                if (value)
                {
                    EventManager.Register(this);
                }
                else
                {
                    EventManager.UnRegister(this);
                }
            }
            get { return Order != Constants.INVALID_ORDER; }
        }

        public static float B2Value(float value)
        {
            return value/B2FACTOR;
        }

        public static Vector2 B2Value(Vector2 value)
        {
            return new Vector2(value.X/B2FACTOR, value.Y/B2FACTOR);
        }

        public static Vector2 B2Value(ref Vector2 value)
        {
            return new Vector2(value.X/B2FACTOR, value.Y/B2FACTOR);
        }

        public static float GameValue(float value)
        {
            return value*B2FACTOR;
        }

        public static Vector2 GameValue(Vector2 value)
        {
            return new Vector2(value.X*B2FACTOR, value.Y*B2FACTOR);
        }

        public static Vector2 GameValue(ref Vector2 value)
        {
            return new Vector2(value.X*B2FACTOR, value.Y*B2FACTOR);
        }

        public Unit CreateUnit(UnitDef unitDef)
        {
            var bodydef = new BodyDef();
            bodydef.type = unitDef.BodyType;
            bodydef.angle = unitDef.RotateZ;
            bodydef.fixedRotation = unitDef.FixedRotation;
            bodydef.position.X = B2Value(unitDef.X);
            bodydef.position.Y = B2Value(unitDef.Y);
            Body body = _world.CreateBody(bodydef);

            var unit = new Unit(body, this);
            unit.Group = unitDef.Group;
            body.SetUserData(unit);
            return unit;
        }

        public Unit CreateRectangle(UnitDef unitDef, float width, float height)
        {
            Unit unit = CreateUnit(unitDef);
            unit.AttachRectangle(width, height, unitDef.Density, unitDef.Fraction, unitDef.Restitution);
            return unit;
        }

        public Unit CreateCircle(UnitDef unitDef, float radius)
        {
            Unit unit = CreateUnit(unitDef);
            unit.AttachCircle(radius, unitDef.Density, unitDef.Fraction, unitDef.Restitution);
            return unit;
        }

        public Unit CreatePolygon(UnitDef unitDef, Vector2[] verteces)
        {
            Unit unit = CreateUnit(unitDef);
            unit.AttachPolygon(verteces, unitDef.Density, unitDef.Fraction, unitDef.Restitution);
            return unit;
        }

        public DrawUnit CreateTerrain(TerrainDef def)
        {
            var bodydef = new BodyDef();
            bodydef.type = def.BodyType;
            bodydef.angle = def.RotateZ;
            bodydef.fixedRotation = def.FixedRotation;
            bodydef.position.X = B2Value(def.X);
            bodydef.position.Y = B2Value(def.Y);
            Body body = _world.CreateBody(bodydef);

            var tex = XNADevicesManager.Instance.ContentManager.Load<Texture2D>(def.TexName);
            Sprite sprite;
            if (def.RepeatX != 0.0f || def.RepeatY != 0.0f)
            {
                var rSprite = new RepeatSprite(tex);
                if (def.RepeatX != 0.0f)
                {
                    rSprite.RepeatX = def.RepeatX;
                }
                if (def.RepeatY != 0.0f)
                {
                    rSprite.RepeatY = def.RepeatY;
                }
                sprite = rSprite;
            }
            else
            {
                sprite = new Sprite(tex);
            }
            sprite.DrawRectangle = def.DrawRectangle;
            sprite.Alpha = def.Alpha;
            sprite.ScaleX = def.ScaleX;
            sprite.ScaleY = def.ScaleY;
            sprite.RotateX = def.RotateX;
            sprite.RotateY = def.RotateY;
            sprite.RotateZ = def.RotateZ;
            sprite.R = def.R;
            sprite.G = def.G;
            sprite.B = def.B;
            sprite.X = def.X;
            sprite.Y = def.Y;
            sprite.Z = def.Z;
            sprite.TransformOrigin = def.TransformOrigin;
            sprite.ZWriteEnable = def.ZWriteEnable;
            var unit = new DrawUnit(sprite, body, this);
            unit.Group = def.Group;
            body.SetUserData(unit);

            foreach (EdgeDef edge in def.Edges)
            {
                unit.AttachEdge(edge.A.X, edge.A.Y,
                    edge.B.X, edge.B.Y,
                    edge.Friction, edge.Restitution);
            }

            unit.Group = Group.Terrain;

            return unit;
        }

        public DrawUnit CreateUnit(DrawUnitDef def)
        {
            var bodydef = new BodyDef();
            bodydef.type = def.BodyType;
            bodydef.angle = def.RotateZ;
            bodydef.fixedRotation = def.FixedRotation;
            bodydef.position.X = B2Value(def.X);
            bodydef.position.Y = B2Value(def.Y);
            Body body = _world.CreateBody(bodydef);

            var tex = XNADevicesManager.Instance.ContentManager.Load<Texture2D>(def.TexName);
            Sprite sprite;
            if (def.RepeatX != 0.0f || def.RepeatY != 0.0f)
            {
                var rSprite = new RepeatSprite(tex);
                if (def.RepeatX != 0.0f)
                {
                    rSprite.RepeatX = def.RepeatX;
                }
                if (def.RepeatY != 0.0f)
                {
                    rSprite.RepeatY = def.RepeatY;
                }
                sprite = rSprite;
            }
            else
            {
                sprite = new Sprite(tex);
                sprite.DrawRectangle = def.DrawRectangle;
            }
            sprite.Alpha = def.Alpha;
            sprite.ScaleX = def.ScaleX;
            sprite.ScaleY = def.ScaleY;
            sprite.RotateX = def.RotateX;
            sprite.RotateY = def.RotateY;
            sprite.RotateZ = def.RotateZ;
            sprite.R = def.R;
            sprite.G = def.G;
            sprite.B = def.B;
            sprite.X = def.X;
            sprite.Y = def.Y;
            sprite.Z = def.Z;
            sprite.TransformOrigin = def.TransformOrigin;
            sprite.ZWriteEnable = def.ZWriteEnable;
            var unit = new DrawUnit(sprite, body, this);
            unit.Group = def.Group;
            body.SetUserData(unit);
            return unit;
        }

        public DrawUnit CreateRectangle(DrawUnitDef def, float width, float height)
        {
            DrawUnit unit = CreateUnit(def);
            unit.AttachRectangle(width, height, def.Density, def.Fraction, def.Restitution);
            return unit;
        }

        public DrawUnit CreateCircle(DrawUnitDef def, float radius)
        {
            DrawUnit unit = CreateUnit(def);
            unit.AttachCircle(radius, def.Density, def.Fraction, def.Restitution);
            return unit;
        }

        public DrawUnit CreatePolygon(DrawUnitDef def, Vector2[] verteces)
        {
            DrawUnit unit = CreateUnit(def);
            unit.AttachPolygon(verteces, def.Density, def.Fraction, def.Restitution);
            return unit;
        }

        public Character CreateCharacter(CharacterDef charDef)
        {
            var bodydef = new BodyDef();
            bodydef.type = charDef.BodyType;
            bodydef.angle = charDef.RotateZ;
            bodydef.fixedRotation = charDef.FixedRotation;
            bodydef.position.X = B2Value(charDef.X);
            bodydef.position.Y = B2Value(charDef.Y);
            Body body = _world.CreateBody(bodydef);
            var shape = new PolygonShape();
            shape.SetAsBox(B2Value(charDef.Width*0.5f), B2Value(charDef.Height*0.5f));
            var fixtureDef = new FixtureDef();
            fixtureDef.shape = shape;
            fixtureDef.density = charDef.Density;
            fixtureDef.friction = charDef.Fraction;
            fixtureDef.restitution = charDef.Restitution;
            body.CreateFixture(fixtureDef);

            var tex = XNADevicesManager.Instance.ContentManager.Load<Texture2D>(charDef.TexName);
            var sprite = new Sprite(tex);
            sprite.Alpha = charDef.Alpha;
            sprite.ScaleX = charDef.ScaleX;
            sprite.ScaleY = charDef.ScaleY;
            sprite.RotateX = charDef.RotateX;
            sprite.RotateY = charDef.RotateY;
            sprite.RotateZ = charDef.RotateZ;
            sprite.R = charDef.R;
            sprite.G = charDef.G;
            sprite.B = charDef.B;
            sprite.X = charDef.X;
            sprite.Y = charDef.Y;
            sprite.Z = charDef.Z;
            sprite.DrawRectangle = charDef.DrawRectangle;
            sprite.TransformOrigin = charDef.TransformOrigin;
            sprite.ZWriteEnable = charDef.ZWriteEnable;
            var character = new Character(sprite, body, this, charDef);
            character.Group = charDef.Group;
            character.Damage = charDef.Damage;
            character.HP = charDef.HP;
            character.MaxHP = charDef.MaxHP;
            body.SetUserData(character);

            return character;
        }

        public void Update()
        {
            _world.ClearForces();
            _world.Step(_timeStep, _velocityIterations, _positionIterations);
        }
    }
}