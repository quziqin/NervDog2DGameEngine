using Box2D.XNA;
using IronPython.Modules;
using NervDog.Common;
using NervDog.Managers;

namespace NervDog.Soul
{
    public class ContactListener : IContactListener
    {
        public void BeginContact(Contact contact)
        {
            Fixture fixtureA = contact.GetFixtureA();
            Fixture fixtureB = contact.GetFixtureB();
            var unitA = (Unit) fixtureA.GetBody().GetUserData();
            var unitB = (Unit) fixtureB.GetBody().GetUserData();

            bool shouldContact = GroupManager.Instance.ShouldContact(unitA.Group, unitB.Group);
            if (shouldContact)
            {
                if (fixtureA.IsSensor())
                {
                    var sensorData = (SensorInfo) fixtureA.GetUserData();
                    if (sensorData.Enable && !fixtureB.IsSensor())
                    {
                        if (unitB is Character && sensorData.SensorName == Constants.SURFACE_SENSOR)
                        {
                            
                        }
                        sensorData.Add(unitB);
                    }
                }
                if (fixtureB.IsSensor())
                {
                    var sensorData = (SensorInfo) fixtureB.GetUserData();
                    if (sensorData.Enable && !fixtureA.IsSensor())
                    {
                        sensorData.Add(unitA);
                    }
                }
            }
            else
            {
                if (fixtureA.IsSensor())
                {
                    var sensorData = (SensorInfo)fixtureA.GetUserData();
                    if (sensorData.SensorName == Constants.VISUAL_SENSOR && sensorData.Enable && !fixtureB.IsSensor() && unitB is Character)
                    {
                        sensorData.Add(unitB);
                    }
                }
                if (fixtureB.IsSensor())
                {
                    var sensorData = (SensorInfo)fixtureB.GetUserData();
                    if (sensorData.SensorName == Constants.VISUAL_SENSOR && sensorData.Enable && !fixtureA.IsSensor() && unitA is Character)
                    {
                        sensorData.Add(unitA);
                    }
                }
            }
        }

        public void EndContact(Contact contact)
        {
            Fixture fixtureA = contact.GetFixtureA();
            Fixture fixtureB = contact.GetFixtureB();
            if (fixtureA.IsSensor())
            {
                var sensorData = (SensorInfo) fixtureA.GetUserData();
                var unitB = (Unit) fixtureB.GetBody().GetUserData();
                if (sensorData.Enable)
                {
                    sensorData.Remove(unitB);
                }
            }
            if (fixtureB.IsSensor())
            {
                var sensorData = (SensorInfo) fixtureB.GetUserData();
                var unitA = (Unit) fixtureA.GetBody().GetUserData();
                if (sensorData.Enable)
                {
                    sensorData.Remove(unitA);
                }
            }
        }

        public void PreSolve(Contact contact, ref Manifold oldManifold)
        {
            var unitA = (Unit) contact.GetFixtureA().GetBody().GetUserData();
            var unitB = (Unit) contact.GetFixtureB().GetBody().GetUserData();
            if (!(unitA.EnableCollision && unitB.EnableCollision
                  && GroupManager.Instance.ShouldContact(unitA.Group, unitB.Group)))
            {
                contact.SetEnabled(false);
            }
        }

        public void PostSolve(Contact contact, ref ContactImpulse impulse)
        {
        }
    }
}