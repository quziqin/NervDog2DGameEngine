using Microsoft.Xna.Framework.Input;

namespace NervDog.Managers
{
    public class InputManager
    {
        #region Fields

        private static readonly InputManager _instance = new InputManager();
        private KeyboardState _newState;
        private KeyboardState _oldState;

        #endregion

        #region Properties

        public static InputManager Instance
        {
            get { return _instance; }
        }

        #endregion

        #region Constructors

        private InputManager()
        {
            _newState = Keyboard.GetState();
        }

        #endregion

        #region Public Functions

        public bool IsKeyDown(Keys key)
        {
            return (_newState.IsKeyDown(key) && !_oldState.IsKeyDown(key));
        }

        public bool IsKeyUp(Keys key)
        {
            return (!_newState.IsKeyDown(key) && _oldState.IsKeyDown(key));
            ;
        }

        public bool GetKeyState(Keys key)
        {
            return _newState.IsKeyDown(key);
        }

        public void Update()
        {
            _oldState = _newState;
            _newState = Keyboard.GetState();
        }

        #endregion
    }
}