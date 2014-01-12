import clr, sys

clr.AddReference("NervDog")
clr.AddReference("Microsoft.Xna.Framework")

from NervDog.Soul import *
from NervDog.Common import *
from NervDog.Managers import *
from NervDog.AI import *
from NervDog.Utilities import *
from Microsoft.Xna.Framework import *

THIS = 0

#AI Name: Null
#AI Description: Do nothing
#THIS must be set before calling Execute()
def Execute():
    global THIS

    if THIS != 0:
        #pass
        units = THIS.VisualSense.SensedUnits;
        for unit in units:
            if isinstance(unit, Character) and unit.IsAlive:
                if GroupManager.Instance.GetRelationShip(THIS.Group, unit.Group) == RelationShip.Enemy:
                    xdistance = abs(THIS.Position.X - unit.Position.X) - unit.Width / 2 - THIS.Width / 2
                    p1distance = Vector2.Distance(THIS.Position, unit.Position)
                    if xdistance < 800:
                        if unit.Position.X > THIS.Position.X:
                            if THIS.Direction != Direction.Right:
                                THIS.Do("TurnRight")
                        else:
                            if THIS.Direction != Direction.Left:
                                THIS.Do("TurnLeft")
                        if xdistance < 40:
                            if RandomHelper.NextInt(0, 10) != 0:
                                THIS.Do("Attack")
                        if RandomHelper.NextInt(0, 20) != 0:
                            THIS.Do("Walk")
                        else:
                            THIS.Do("Jump")
                    else:
                        THIS.Do("Stop")
    return