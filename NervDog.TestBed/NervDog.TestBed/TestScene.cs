using Microsoft.Xna.Framework;
using NervDog.Cameras;
using NervDog.Common;
using NervDog.Render;
using NervDog.Soul;
using NervDog.Utilities;

namespace NervDog.TestBed
{
    public class TestScene : Scene
    {
        private IEffect _effect;
        private Character _player1;
        private Sprite _sprite;
        private UnitCamera _unitCam;
        private World _world;
        private DrawUnit uni;

        public TestScene()
            : base("Test")
        {
        }

        public override void Initialize()
        {
            _world = new World(new Vector2(0, 0), EventManager);
            //Player1
            var def = XmlHelper.LoadFromFile<CharacterDef>(@"Data\Hua.xml");
            def.ScaleX = 1.3f;
            def.ScaleY = 1.3f;
            def.Width *= 1.3f;
            def.Height *= 1.3f;
            def.SoundEffects = null;
            _player1 = _world.CreateCharacter(def);

            _player1.Sprite.IsVisible = false;
            Root.Add(_player1.Sprite);

            _unitCam = new UnitCamera(_player1, new Vector2(40.0f, 40.0f),
                new Vector2(80.0f, 80.0f),
                new Vector2(-9550, 465),
                new Vector2(9550, -9550),
                new Vector2(190, 190),
                new Vector3(0.0f, 120.0f, 500.0f));
        }

        public override void Update()
        {
            EventManager.PostEvent(new Event(Constants.TIME_TICKS_EVENT));
            EventManager.Update();

            base.Update();
        }

        public override void Dispose()
        {
        }
    }
}