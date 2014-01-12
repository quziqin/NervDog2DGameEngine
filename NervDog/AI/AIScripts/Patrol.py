import clr, sys
clr.AddReference("NervDog")
clr.AddReference("Microsoft.Xna.Framework")

from NervDog.Soul import *
from NervDog.Common import *
from NervDog.Managers import *
from NervDog.AI import *
from Microsoft.Xna.Framework import *

THIS = 0

COUNT = 0

#THIS must be set before calling Execute()
def Execute():
    global THIS
    global COUNT

    if THIS != 0:
        #pass
        units = THIS.VisualSense.SensedUnits;
        for unit in units:
            if isinstance(unit, Character):
                if unit.Group == Group.PlayerOne:
                    #Follow player1
                    followDistance = Vector2.Distance(THIS.Position, unit.Position);
                    if followDistance > 300:
                        AIHelper.CharGoToChar(THIS, unit)
                    else:
                        THIS.Do("Stop")
                elif GroupManager.Instance.GetRelationShip(THIS.Group, unit.Group) == RelationShip.Enemy:
                    xdistance = abs(THIS.Position.X - unit.Position.X) - unit.Width / 2 - THIS.Width / 2
                    p1distance = Vector2.Distance(THIS.Position, unit.Position)
                    if 50 < xdistance:
                        AIHelper.CharGoToChar(THIS, unit)
                    else:
                        if unit.Position.X > THIS.Position.X:
                            if THIS.Direction != Direction.Right:
                                THIS.Do("TurnRight")
                        else:
                            if THIS.Direction != Direction.Left:
                                THIS.Do("TurnLeft")
                        THIS.Do("Attack");
    return