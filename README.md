NervDog 2D Game Engine
===================
NervDog is based on XNA 4.0 and Box2D, which can be used to made 2D/2.5D games. NervDog's predecessor was called “Harmony”. 2 of my classmates and I formed HIHGameStudio (2 artists, I’m the only dev) in university, “Harmony” was created. And a demo game named Paper Chinese Opera was made based on it.
Paper Chinese Opera puts paper like 2D characters in a 3D environment, and the traditional Chinese style puts you in an unprecedented aura. But the main features have not been implemented yet. After my recent optimization and refactor, it was renamed to NervDog 2D Game Engine.

A simple demo PaperChineseOpera has already been built based on NervDog.
You can check the demo video for PaperChineseOpera http://quziqin.com/projects/HIHGameDemo.flv
or the demo pic at http://quziqin.com/ PaperChineseOpera project area.
PaperChineseOpera.rar contains the demo binaries, you can try to run the PaperChineseOpera.exe, if you cannot run it successfully, please install xnafx40_redist.msi from http://www.microsoft.com/en-us/download/details.aspx?id=20914

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
