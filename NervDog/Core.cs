using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using NervDog.Common;
using NervDog.Managers;
using NervDog.Render;

namespace NervDog
{
    public class Core : IDisposable
    {
        private static Core _instance;
        private readonly Game _game;
        private readonly float _targetFrameMilliseconds;
        private DefaultEffect _defaultEffect;
        private EffectManager _effectManager;
        private InputManager _input;
        private DrawNode _root;
        private SceneManager _sceneManager;

        public Core(Game game)
        {
            _game = game;

            XNADevicesManager.Instance.GraphicsDevice = game.GraphicsDevice;
            XNADevicesManager.Instance.ContentManager = game.Content;
            XNADevicesManager.Instance.SpriteBatch = new SpriteBatch(game.GraphicsDevice);

            _targetFrameMilliseconds = (float) game.TargetElapsedTime.TotalMilliseconds;
            Variables.TargetFrameMilliseconds = _targetFrameMilliseconds;
            _instance = this;
        }

        public static Core Instance
        {
            get { return _instance; }
        }

        public float TargetFrameMilliseconds
        {
            get { return _targetFrameMilliseconds; }
        }

        public SceneManager Scene
        {
            get { return _sceneManager; }
        }

        public DrawNode Root
        {
            get { return _root; }
        }

        public IEffect DefaultEffect
        {
            get { return _defaultEffect; }
        }

        public Game Game
        {
            get { return _game; }
        }

        public void Dispose()
        {
            LoggerManager.Instance.Dispose();
        }

        public void Initialize()
        {
            _root = new DrawNode();
            _defaultEffect = new DefaultEffect(XNADevicesManager.Instance.GraphicsDevice);
            _effectManager = EffectManager.Instance;
            _effectManager.Add(_defaultEffect);
            _input = InputManager.Instance;
            _sceneManager = SceneManager.Instance;
        }

        public void Update()
        {
            _input.Update();
            _sceneManager.Update();
            //Time ticks event will be sent by default.

            if (InputManager.Instance.IsKeyDown(Keys.Escape))
            {
                _game.Exit();
            }
        }

        public void Draw()
        {
            _sceneManager.Draw();
            _root.Draw();
        }
    }
}