using System.Collections.Generic;
using NervDog.Render;

namespace NervDog.Managers
{
    public class SceneManager
    {
        #region Fields

        private static readonly SceneManager _instance = new SceneManager();
        private readonly Dictionary<string, Scene> _scenes = new Dictionary<string, Scene>();
        private Scene _currentScene;

        #endregion

        #region Properties

        public Scene CurrentScene
        {
            get { return _currentScene; }
        }

        public static SceneManager Instance
        {
            get { return _instance; }
        }

        #endregion

        #region Constructors

        private SceneManager()
        {
        }

        #endregion

        #region Public Functions

        public void Add(Scene scene)
        {
            _scenes.Add(scene.Name, scene);
        }

        public void Remove(Scene scene)
        {
            if (_scenes.ContainsKey(scene.Name))
            {
                _scenes.Remove(scene.Name);
            }
        }

        public void Remove(string name)
        {
            if (_scenes.ContainsKey(name))
            {
                _scenes.Remove(name);
            }
        }

        public bool SwitchScene(string name)
        {
            if (_scenes.ContainsKey(name))
            {
                if (_currentScene != null)
                {
                    _currentScene.Dispose();
                }

                _currentScene = _scenes[name];
                _currentScene.Initialize();

                return true;
            }

            return false;
        }

        public Scene GetScene(string name)
        {
            if (_scenes.ContainsKey(name))
            {
                return _scenes[name];
            }

            return null;
        }

        public void Draw()
        {
            _currentScene.Draw();
        }

        public void Update()
        {
            _currentScene.Update();
        }

        #endregion
    }
}