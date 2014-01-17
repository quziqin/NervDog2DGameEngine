NervDog 2D Game Engine
===================
NervDog is a simple 2D game engine based on XNA and Box2D

General Code Structure:
  
NervDog

    |----Common           //Include EventSystem, Constants, etc
          |----EventSystem
          |----Constants and other common class
    |----Render           //Basic implementation of DrawNode, Sprite, Scene, etc
          |----DrawNode   //Simple tree node.
          |----Sprite
          |----Scene
    |----Animations
          |----Animation  //Animation base class
          |----EaseFunction   //Key point of animation, you can define your own animation based on different ease function
          |----Sequence   //Play a set of animation in sequence
          |----Parallel   //Play a set of animation in parallel
          |----Frame      //Frame animation
          |----MoveXY     //Move animation
          |----MoveZ
          |----RotateX    //Rotate animation
          |----RotateY
          |----RotateZ
          |----Scale      //Scale animation
          |----Fade       //Fade animation
    |----Soul             //This module combine the render system with physics system(BOX2D), it's the soul part of this engine
          |----Unit       //Physics wrapper of Box2D
          |----DrawUnit   //Combine of unit and render module
          |----Character  //The draw unit that has actions and other properties(e.g. HP)
          |----World      //World simulator
          |----[Actions]    //Define serveral basic actions for character
                |----IAciton  //Interface of action
                |----Walk
                |----Jump
                |----TurnRight
                |----TurnLeft
                |----Attack
                |----GotHit
                |----Dying
                |----Stop
    |----Cameras          //Different kinds of cameras
          |----BaseCamera     //Simple camera
          |----AnimatedCamera //Camera with animation, e.g. can be used for auto playing video
          |----TargetCamera   //Camera to simply follow a sprite
          |----UnitCamera     //Camera to follow a unit, with certain bound and animation.
    |----AI               //Use IronPython to build the ai system.
          |----AI         //Load AI script, assign things to py script and execute certain update function in py.
          |----AIHelper   //Define some helper ai function
          |----[AIScripts]  //Some sample Python ai file
                |-----Null.py   //AI does nothing
    |----Managers         //Managers
          |----AIManager
          |----AuditionManager
          |----CameraManager
          |----EffectManager
          |----EventManager
          |----GroupManager
          |----InputManager
          |----LoggerManager
          |----SceneManager
          |----XNADevicesManager
    |----SceneTemplates
          |----2DAVGScene
    |----Trace
          |----ILogger
          |----BaseLogger
          |----FileLogger
          |----XNALogger
    |----Utilities
          |----RandomHelper
          |----XmlHelper
    |-----Misc
          |----Logo
          |----Board
          |----HpBar
