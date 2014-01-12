import clr, sys
clr.AddReference("NervDog")
from NervDog.Soul import *

THIS = 0

#AI Name: Null
#AI Description: Do nothing
#THIS must be set before calling Execute()
def Execute():
    global THIS

    if THIS != 0:
        #do nothing
        pass
    return