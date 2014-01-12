using Microsoft.Xna.Framework;
using NervDog.Common;
using NervDog.Render;

namespace NervDog.Cameras
{
    public class TargetCamera : EventListener
    {
        #region Fields

        private readonly BaseCamera _cam = new BaseCamera();
        private Sprite _target;

        #endregion

        #region Properties

        public Sprite Target
        {
            set { _target = value; }
            get { return _target; }
        }

        #endregion

        public TargetCamera(Sprite target)
            : base(Constants.TIME_TICKS_EVENT)
        {
            _target = target;
        }

        public override EventFunction Handler
        {
            set { _handler = value; }
            get { return e => Update(); }
        }

        public void Update()
        {
            _cam.Target = Vector3.Transform(_target.Position, _target.World);
            _cam.Apply();
        }
    }
}