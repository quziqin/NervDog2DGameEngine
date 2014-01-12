import clr, sys
clr.AddReference("NervDog")
clr.AddReference("Microsoft.Xna.Framework")

from NervDog.Soul import *
from NervDog.Common import *
from NervDog.Managers import *
from NervDog.AI import *
from Microsoft.Xna.Framework import *

THIS = 0
PLAYER_ONE = 0

#THIS must be set before calling Execute()
def Execute():
    global THIS
    global PLAYER_ONE

    if THIS != 0:
        #pass
        units = THIS.VisualSense.SensedUnits;
        for unit in units:
            if isinstance(unit, Character) and unit.IsAlive:
                if GroupManager.Instance.GetRelationShip(THIS.Group, unit.Group) == RelationShip.Enemy:
                    xdistance = abs(THIS.Position.X - unit.Position.X) - unit.Width / 2 - THIS.Width / 2
                    p1distance = Vector2.Distance(THIS.Position, unit.Position)
                    if xdistance > 50:
                        AIHelper.CharGoToChar(THIS, unit)
                    else:
                        if unit.Position.X > THIS.Position.X:
                            if THIS.Direction != Direction.Right:
                                THIS.Do("TurnRight")
                        else:
                            if THIS.Direction != Direction.Left:
                                THIS.Do("TurnLeft")
                        THIS.Do("Attack")
                    break
                elif unit.Group == Group.PlayerOne:
                    if PLAYER_ONE == 0:
                        PLAYER_ONE = unit
                    #Follow player1
                    followDistance = Vector2.Distance(THIS.Position, unit.Position);
                    if followDistance > 300:
                        AIHelper.CharGoToChar(THIS, unit)
                    else:
                        THIS.Do("Stop")
                    break
                elif PLAYER_ONE != 0:
                    followDistance = Vector2.Distance(THIS.Position, PLAYER_ONE.Position);
                    if followDistance > 300:
                        AIHelper.CharGoToChar(THIS, PLAYER_ONE)
                    else:
                        THIS.Do("Stop")
                    break
    return