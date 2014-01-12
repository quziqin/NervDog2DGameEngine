using System;
using Box2D.XNA;
using Microsoft.Xna.Framework;
using NervDog.Common;

namespace NervDog.Soul
{
    //Physics wrapper
    public class Unit : IDisposable
    {
        #region Fields

        //Box2D member.

        private bool _bEnableCollision = true;
        private Body _body;
        private Group _group = Group.Decoration;
        private World _world;

        #endregion

        #region Properties

        public Body Body
        {
            get { return _body; }
        }

        public Vector2 Position
        {
            get { return World.GameValue(_body.Position); }
        }

        public float VelocityX
        {
            set { Velocity = new Vector2(value, Velocity.Y); }
            get { return Velocity.X; }
        }

        public float VelocityY
        {
            set { Velocity = new Vector2(Velocity.X, value); }
            get { return Velocity.Y; }
        }

        public Vector2 Velocity
        {
            set { _body.SetLinearVelocity(World.B2Value(ref value)); }
            get { return World.GameValue(_body.GetLinearVelocity()); }
        }

        public Group Group
        {
            set { _group = value; }
            get { return _group; }
        }

        public World World
        {
            get { return _world; }
        }

        public bool EnableCollision
        {
            set { _bEnableCollision = value; }
            get { return _bEnableCollision; }
        }

        public bool IsDisposed
        {
            get { return (_body == null); }
        }

        #endregion

        #region Constructors

        public Unit(Body body, World world)
        {
            _body = body;
            _world = world;
        }

        #endregion

        #region Protected Functions

        public Fixture AttachRectangle(float width, float height,
            Vector2 center, float angle,
            float density, float friction, float restitution)
        {
            var shape = new PolygonShape();
            shape.SetAsBox(
                World.B2Value(width*0.5f),
                World.B2Value(height*0.5f),
                World.B2Value(center),
                angle);
            var fixtureDef = new FixtureDef();
            fixtureDef.shape = shape;
            fixtureDef.density = density;
            fixtureDef.friction = friction;
            fixtureDef.restitution = restitution;
            return _body.CreateFixture(fixtureDef);
        }

        public Fixture AttachRectangle(float width, float height,
            float density, float friction, float restitution)
        {
            return AttachRectangle(width, height, Vector2.Zero, 0, density, friction, restitution);
        }

        public Fixture AttachPolygon(Vector2[] verteces,
            float density, float friction, float restitution)
        {
            var shape = new PolygonShape();
            shape.Set(verteces,
                (verteces.Length > Settings.b2_maxPolygonVertices
                    ? Settings.b2_maxPolygonVertices
                    : verteces.Length));
            var fixtureDef = new FixtureDef();
            fixtureDef.shape = shape;
            fixtureDef.density = density;
            fixtureDef.friction = friction;
            fixtureDef.restitution = restitution;
            return _body.CreateFixture(fixtureDef);
        }

        public Fixture AttachCircle(Vector2 center, float radius,
            float density, float friction, float restitution)
        {
            var shape = new CircleShape();
            shape._p = World.B2Value(center);
            shape._radius = World.B2Value(radius);
            var fixtureDef = new FixtureDef();
            fixtureDef.shape = shape;
            fixtureDef.density = density;
            fixtureDef.friction = friction;
            fixtureDef.restitution = restitution;
            return _body.CreateFixture(fixtureDef);
        }

        public Fixture AttachCircle(float radius,
            float density, float friction, float restitution)
        {
            return AttachCircle(Vector2.Zero, radius, density, friction, restitution);
        }

        public Fixture AttachEdge(float x1, float y1, float x2, float y2,
            float friction, float restitution)
        {
            var shape = new PolygonShape();
            shape.SetAsEdge(
                World.B2Value(new Vector2(x1, y1)),
                World.B2Value(new Vector2(x2, y2)));
            var fixtureDef = new FixtureDef();
            fixtureDef.shape = shape;
            fixtureDef.density = 0.0f;
            fixtureDef.friction = friction;
            fixtureDef.restitution = restitution;
            return _body.CreateFixture(fixtureDef);
        }

        #endregion

        #region Public Functions

        public virtual void Dispose()
        {
            if (_body != null)
            {
                _world.PhysicsWorld.DestroyBody(_body);
                _body = null;
                _world = null;
            }
        }

        public void ApplyForce(Vector2 direction, float power)
        {
            var angle = (float) Math.Atan2(direction.Y, direction.X);
            ApplyForce(angle, power);
        }

        public void ApplyForce(float angle, float power)
        {
            _body.ApplyForce(
                new Vector2(
                    power*(float) Math.Cos(angle),
                    power*(float) Math.Sin(angle)),
                _body.GetWorldCenter());
        }

        public void ApplyImpulse(Vector2 direction, float impulse)
        {
            var angle = (float) Math.Atan2(direction.Y, direction.X);
            ApplyImpulse(angle, impulse);
        }

        public void ApplyImpulse(float angle, float impulse)
        {
            _body.ApplyLinearImpulse(
                new Vector2(
                    impulse*(float) Math.Cos(angle),
                    impulse*(float) Math.Sin(angle)),
                _body.GetWorldCenter());
        }

        public SensorInfo AttachRectangleSensor(float width, float height,
            Vector2 center, float angle, int maxSense = 10)
        {
            Fixture fixture = AttachRectangle(width, height, center, angle, 0, 0, 0);
            fixture.SetSensor(true);
            var info = new SensorInfo(this, fixture, maxSense);
            return info;
        }

        public SensorInfo AttachPolygonSensor(Vector2[] verteces, int maxSense = 10)
        {
            Fixture fixture = AttachPolygon(verteces, 0, 0, 0);
            fixture.SetSensor(true);
            var info = new SensorInfo(this, fixture, maxSense);
            return info;
        }

        public SensorInfo AttachCircleSensor(Vector2 center, float radius, int maxSense = 10)
        {
            Fixture fixture = AttachCircle(center, radius, 0, 0, 0);
            fixture.SetSensor(true);
            var info = new SensorInfo(this, fixture, maxSense);
            return info;
        }

        #endregion
    }
}