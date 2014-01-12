using Box2D.XNA;
using Microsoft.Xna.Framework;
using NervDog.Common;
using NervDog.Soul;
using World = NervDog.Soul.World;

namespace NervDog.AI
{
    public static class AIHelper
    {
        public static void CharGoToChar(Character A, Character target)
        {
            AABB aabb;
            if (A.Direction == Direction.Left)
            {
                aabb.upperBound = World.B2Value(new Vector2(A.Position.X - A.Width / 2, A.Position.Y + A.Height / 2 - 10));
                aabb.lowerBound =
                    World.B2Value(new Vector2(A.Position.X - A.Width / 2 - 80, A.Position.Y - A.Height / 2 + 10));
            }
            else
            {
                aabb.upperBound =
                    World.B2Value(new Vector2(A.Position.X + A.Width / 2 + 80, A.Position.Y + A.Height / 2 - 10));
                aabb.lowerBound = World.B2Value(new Vector2(A.Position.X + A.Width / 2, A.Position.Y - A.Height / 2 + 10));
            }

            A.World.PhysicsWorld.QueryAABB(fixtureProxy =>
            {
                if (fixtureProxy.fixture.IsSensor())
                {
                    return true;
                }
                var unit = fixtureProxy.fixture.GetBody().GetUserData() as Unit;
                if (unit != null)
                {
                    if (unit.Group == Group.Destructable)
                    {
                        A.Do("Walk");
                        A.Do("Jump");
                    }
                }
                return true;
            }, ref aabb);

            if (target.Position.X > A.Position.X)
            {
                if (A.Direction != Direction.Right)
                {
                    A.Do("TurnRight");
                }
            }
            else
            {
                if (A.Direction != Direction.Left)
                {
                    A.Do("TurnLeft");
                }
            }

            A.Do("Walk");
        }
    }
}