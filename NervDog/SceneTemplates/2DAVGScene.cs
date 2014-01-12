using Microsoft.Xna.Framework;
using NervDog.Cameras;
using NervDog.Managers;
using NervDog.Render;
using NervDog.Soul;

namespace NervDog.SceneTemplates
{
    public class _2DAVGScene : Scene
    {
        #region Fields

        private AIManager _AIManager;
        private Character _player1;
        private UnitCamera _unitCam;
        private World _world;

        #endregion

        #region Properties

        public override bool Pause
        {
            set
            {
                _isPlaying = !value;
                _world.Enable = !value;
            }
            get { return !_isPlaying; }
        }

        #endregion

        #region LoadTerrain

        protected virtual void LoadTerrain()
        {
        }

        #endregion

        #region LoadPlayer

        protected virtual void LoadPlayer()
        {
        }

        #endregion

        #region LoadBoss

        protected virtual void LoadBoss()
        {
            //Boss
        }

        #endregion

        #region LoadEnemies

        protected virtual void LoadEnemies()
        {
        }

        #endregion

        #region LoadRelations

        protected virtual void LoadRalations()
        {
        }

        #endregion

        #region LoadShouldContact

        protected virtual void LoadShouldContact()
        {
        }

        #endregion

        #region LoadDecorations

        protected virtual void LoadDecorations()
        {
        }

        #endregion

        #region LoadBackground

        protected virtual void LoadBackground()
        {
        }

        #endregion

        #region LoadWorld

        protected virtual void LoadWorld()
        {
        }

        #endregion

        #region LoadIntro

        protected virtual void LoadIntro()
        {
        }

        #endregion

        #region LoadAudition

        protected virtual void LoadAudition()
        {
        }

        #endregion

        #region Constructors

        public _2DAVGScene(string name)
            : base(name)
        {
        }

        #endregion

        #region Initialize

        public override void Initialize()
        {
            //print log on screen.
            Root.Add(LoggerManager.Instance.GetLogger("XNALogger") as DrawNode);
            _world = new World(new Vector2(0, -10.0f), EventManager);
            _AIManager = new AIManager();

            EventManager.AddEventType(_AIManager);

            LoadAudition();
            LoadShouldContact();
            LoadRalations();
            LoadTerrain();
            LoadPlayer();
            LoadDecorations();
            LoadBackground();
            LoadEnemies();
            LoadBoss();

            _unitCam = new UnitCamera(_player1, new Vector2(40.0f, 40.0f),
                new Vector2(80.0f, 80.0f),
                new Vector2(-9550, 465),
                new Vector2(9550, -9550),
                new Vector2(190, 190),
                new Vector3(0.0f, 120.0f, 500.0f));

            EventManager.Register(_world);
            EventManager.Register(_unitCam);

            LoadIntro();
        }

        #endregion

        #region Update

        public override void Update()
        {
            base.Update();
        }

        #endregion

        #region Dispose

        public override void Dispose()
        {
            base.Dispose();
        }

        #endregion
    }
}