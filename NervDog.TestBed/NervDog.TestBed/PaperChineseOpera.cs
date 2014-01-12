using System.Collections.Generic;
using System.Collections.ObjectModel;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using NervDog.Animations;
using NervDog.Cameras;
using NervDog.Common;
using NervDog.Managers;
using NervDog.Misc;
using NervDog.Render;
using NervDog.Soul;
using NervDog.Utilities;

namespace NervDog.TestBed
{
    public class PaperChineseOpera : Scene
    {
        #region Fields

        private static readonly Texture2D _soundTex = XNADevicesManager.Instance.ContentManager.Load<Texture2D>("sound");
        private static readonly FrameDef _soundFrameDef = XmlHelper.LoadFromFile<FrameDef>(@"Data\sound.xml");

        private AIManager _AIManager;

        private Board _board;
        private string[] _boardTexs;
        private Character _boss;
        private Sprite _clearScreen;
        private int _countBoard;
        private Collection<Character> _enemies;
        private Fade _fadeScreen;
        private Timer _introTimer;
        private Character _player1;
        private Character _player2;
        private Timer _startTimer;
        private DrawUnit _terrain;
        private Timer _timer;
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

        private void LoadTerrain()
        {
            var def = XmlHelper.LoadFromFile<TerrainDef>(@"Data\Terrain.xml");
            _terrain = _world.CreateTerrain(def);

            Root.Add(_terrain.Sprite);
        }

        #endregion

        #region LoadPlayer

        private void LoadPlayer()
        {
            //Player1
            var def = XmlHelper.LoadFromFile<CharacterDef>(@"Data\Hua.xml");
            def.ScaleX = 1.3f;
            def.ScaleY = 1.3f;
            def.Width *= 1.3f;
            def.Height *= 1.3f;
            def.MaxHP = 100;
            def.HP = 100;

            _player1 = _world.CreateCharacter(def);

            var hpBar = new HPBar("bloodframe", "blood");
            hpBar.Width = 186;
            hpBar.Height = 20;
            hpBar.X = -93;
            hpBar.Y = 100;
            hpBar.Target = _player1;

            Root.Add(_player1.Sprite);

            //Player2
            def = XmlHelper.LoadFromFile<CharacterDef>(@"Data\Dan1.xml");
            def.ScaleX = 1.2f;
            def.ScaleY = 1.2f;
            def.Width *= 1.2f;
            def.Height *= 1.2f;
            def.X = -9600;
            def.Y = 600;
            def.AI = @"AI\DuplicatePlayer.py";
            _player2 = _world.CreateCharacter(def);

            hpBar = new HPBar("bloodframe", "blood");
            hpBar.Width = 186;
            hpBar.Height = 20;
            hpBar.X = -93;
            hpBar.Y = 100;
            hpBar.Target = _player2;

            Root.Add(_player2.Sprite);
            EventManager.Register(_player2.AI);
        }

        #endregion

        #region LoadBoss

        private void LoadBoss()
        {
            //Boss
            _boss = CreateCharacter(@"Data\Hua.xml", Group.PlayerThree, 10000, 300, 99, 30, 300, 600, 2.0f, 2.0f, 2.0f,
                1.0f, 1.0f);

            _boss.Actions["Dying"].ActionEnd += s =>
            {
                _board.SwitchTex(_boardTexs[_countBoard]);
                _board.Show();
            };

            _boss.AI.LoadAIScript(@"AI\Boss.py");
            Root.Add(_boss.Sprite);
            EventManager.Register(_boss.AI);
        }

        #endregion

        #region LoadEnemies

        private void LoadEnemies()
        {
            _enemies = new Collection<Character>();

            for (int i = 0; i < 30; i++)
            {
                Character enemy;
                int rand = RandomHelper.NextInt(0, 3);
                if (rand == 1)
                {
                    float size = RandomHelper.NextFloat(1.0f, 1.5f);
                    enemy = CreateCharacter(@"Data\Dan.xml", Group.PlayerTwo, RandomHelper.NextFloat(-8000, 9500), 200,
                        100, 5, 120, 400, size, size, RandomHelper.NextFloat(0.9f, 1.3f),
                        RandomHelper.NextFloat(0.5f, 1.5f), RandomHelper.NextFloat(0.9f, 1.3f));
                    enemy.Actions["Attack"].ActionStart += sender =>
                    {
                        var timer = new Timer(500);
                        timer.EventData = sender.Character;
                        timer.OnTimer += timerSender =>
                        {
                            var dan = (Character) timerSender.EventData;
                            if (dan.Actions["Attack"].IsDoing)
                            {
                                var sprite = new Sprite(_soundTex);
                                sprite.DrawRectangle = new Rectangle(0, 0, _soundTex.Width, _soundTex.Height);
                                sprite.Position = dan.Sprite.Position;
                                sprite.ScaleX = dan.Sprite.ScaleX;
                                sprite.ScaleY = dan.Sprite.ScaleY;
                                sprite.R = dan.Sprite.R;
                                sprite.G = dan.Sprite.G;
                                sprite.B = dan.Sprite.B;
                                sprite.Z += 2.0f;
                                Root.Add(sprite);
                                var soundFrame = new Frame(_soundFrameDef);
                                soundFrame.Speed = 2.0f;
                                soundFrame.OnEnd += frameSender =>
                                {
                                    var frame = (Frame) frameSender;
                                    frame.Target.Parent.Remove(frame.Target);
                                };
                                soundFrame.Target = sprite;
                                soundFrame.Start();
                            }
                        };
                        timer.Start();
                    };
                }
                else if (rand == 2)
                {
                    float size = RandomHelper.NextFloat(1.0f, 1.5f);
                    enemy = CreateCharacter(@"Data\Dan1.xml", Group.PlayerTwo, RandomHelper.NextFloat(-8000, 9500), 200,
                        100, 5, 150, 500, size, size, RandomHelper.NextFloat(0.9f, 1.3f),
                        RandomHelper.NextFloat(0.5f, 1.5f), RandomHelper.NextFloat(0.9f, 1.3f));
                }
                else //if (rand == 3)
                {
                    float size = RandomHelper.NextFloat(1.0f, 1.5f);
                    enemy = CreateCharacter(@"Data\Hua.xml", Group.PlayerTwo, RandomHelper.NextFloat(-8000, 9500), 200,
                        100, 5, 100, 450, size, size, RandomHelper.NextFloat(-0.1f, 0.3f),
                        RandomHelper.NextFloat(0.5f, 1.5f), RandomHelper.NextFloat(0.9f, 1.3f));
                }

                enemy.AI.LoadAIScript(@"AI\Enemy.py");
                _enemies.Add(enemy);

                Root.Add(enemy.Sprite);
                EventManager.Register(enemy.AI);
            }
        }

        #endregion

        #region LoadRelations

        private void LoadRalations()
        {
            GroupManager.Instance.SetRelationShip(Group.PlayerOne, Group.PlayerTwo, RelationShip.Enemy);
            GroupManager.Instance.SetRelationShip(Group.PlayerTwo, Group.PlayerOne, RelationShip.Enemy);
            GroupManager.Instance.SetRelationShip(Group.PlayerOne, Group.PlayerOne, RelationShip.Friend);

            GroupManager.Instance.SetRelationShip(Group.PlayerThree, Group.PlayerTwo, RelationShip.Friend);
            GroupManager.Instance.SetRelationShip(Group.PlayerTwo, Group.PlayerThree, RelationShip.Friend);
            GroupManager.Instance.SetRelationShip(Group.PlayerThree, Group.PlayerOne, RelationShip.Enemy);
            GroupManager.Instance.SetRelationShip(Group.PlayerOne, Group.PlayerThree, RelationShip.Enemy);
            GroupManager.Instance.SetRelationShip(Group.PlayerThree, Group.PlayerThree, RelationShip.Friend);

            GroupManager.Instance.SetRelationShip(Group.PlayerTwo, Group.Terrain, RelationShip.Neutral);
            GroupManager.Instance.SetRelationShip(Group.Terrain, Group.PlayerTwo, RelationShip.Neutral);
            GroupManager.Instance.SetRelationShip(Group.PlayerOne, Group.Terrain, RelationShip.Neutral);
            GroupManager.Instance.SetRelationShip(Group.Terrain, Group.PlayerOne, RelationShip.Neutral);
            GroupManager.Instance.SetRelationShip(Group.PlayerThree, Group.Terrain, RelationShip.Neutral);
            GroupManager.Instance.SetRelationShip(Group.Terrain, Group.PlayerThree, RelationShip.Neutral);

            GroupManager.Instance.SetRelationShip(Group.Standby, Group.PlayerTwo, RelationShip.Neutral);
            GroupManager.Instance.SetRelationShip(Group.PlayerTwo, Group.Standby, RelationShip.Neutral);
            GroupManager.Instance.SetRelationShip(Group.Standby, Group.PlayerOne, RelationShip.Neutral);
            GroupManager.Instance.SetRelationShip(Group.PlayerOne, Group.Standby, RelationShip.Neutral);
            GroupManager.Instance.SetRelationShip(Group.Standby, Group.PlayerThree, RelationShip.Neutral);
            GroupManager.Instance.SetRelationShip(Group.PlayerThree, Group.Standby, RelationShip.Neutral);
        }

        #endregion

        #region LoadShouldContact

        private void LoadShouldContact()
        {
            GroupManager.Instance.SetShouldContact(Group.PlayerOne, Group.Terrain, true);
            GroupManager.Instance.SetShouldContact(Group.PlayerOne, Group.PlayerOne, false);
            GroupManager.Instance.SetShouldContact(Group.PlayerOne, Group.Destructable, true);
            GroupManager.Instance.SetShouldContact(Group.PlayerOne, Group.Standby, false);

            GroupManager.Instance.SetShouldContact(Group.PlayerTwo, Group.Terrain, true);
            GroupManager.Instance.SetShouldContact(Group.PlayerTwo, Group.PlayerTwo, true);
            GroupManager.Instance.SetShouldContact(Group.PlayerTwo, Group.Destructable, true);
            GroupManager.Instance.SetShouldContact(Group.PlayerTwo, Group.Standby, false);

            GroupManager.Instance.SetShouldContact(Group.PlayerThree, Group.Terrain, true);
            GroupManager.Instance.SetShouldContact(Group.PlayerThree, Group.PlayerThree, true);
            GroupManager.Instance.SetShouldContact(Group.PlayerThree, Group.Destructable, true);
            GroupManager.Instance.SetShouldContact(Group.PlayerThree, Group.Standby, false);
            GroupManager.Instance.SetShouldContact(Group.PlayerThree, Group.PlayerTwo, true);
            GroupManager.Instance.SetShouldContact(Group.PlayerThree, Group.PlayerOne, false);

            GroupManager.Instance.SetShouldContact(Group.PlayerOne, Group.PlayerTwo, false);
            GroupManager.Instance.SetShouldContact(Group.PlayerTwo, Group.PlayerOne, false);

            GroupManager.Instance.SetShouldContact(Group.Destructable, Group.Terrain, true);

            GroupManager.Instance.SetShouldContact(Group.Standby, Group.Destructable, false);
            GroupManager.Instance.SetShouldContact(Group.Standby, Group.Terrain, true);

            GroupManager.Instance.SetShouldContact(Group.Standby, Group.Standby, false);
            GroupManager.Instance.SetShouldContact(Group.Destructable, Group.Destructable, true);
        }

        #endregion

        #region LoadDecorations

        private void LoadDecorations()
        {
            var lantern1Def = XmlHelper.LoadFromFile<SpriteDef>(@"Data\lantern1.xml");
            var lantern2Def = XmlHelper.LoadFromFile<SpriteDef>(@"Data\lantern2.xml");
            var pillarDef = XmlHelper.LoadFromFile<SpriteDef>(@"Data\pillar.xml");

            for (int i = -10; i <= 10; i++)
            {
                Sprite lantern1 = lantern1Def.ToSprite();
                lantern1.X = i*951.333333f;
                Root.Add(lantern1);
            }
            for (int i = -11; i <= 11; i++)
            {
                Sprite lantern2 = lantern2Def.ToSprite();
                lantern2.X = 475.666666666f + i*951.333333f;
                Root.Add(lantern2);
            }
            for (int i = -22; i <= 22; i++)
            {
                Sprite pillar = pillarDef.ToSprite();
                pillar.X = 238 + i*475.666666f;
                Root.Add(pillar);
            }

            #region CreateDragonFly

            CreateDragonfly("dragonfly1", 50, -9450, -9000, 100, 250);
            CreateDragonfly("dragonfly1", 50, -4000, -3000, 200, 250);
            CreateDragonfly("dragonfly2", 50, -3930, -4300, 200, 140);
            CreateDragonfly("dragonfly2", 150, 0, 400, 280, 130, 4000);
            CreateDragonfly("dragonfly1", 150, 1400, 1000, 340, 270, 5000);
            CreateDragonfly("dragonfly2", 50, 2900, 3800, 200, 470, 5000);
            CreateDragonfly("dragonfly1", 50, 3050, 3200, 200, 500, 3000);
            CreateDragonfly("dragonfly2", 150, 7000, 6300, 130, 240, 4000);
            CreateDragonfly("dragonfly1", 150, 9000, 10200, 270, 300, 4000);
            CreateDecoration("dragonfly2", 9070, 200, 50);

            #endregion

            #region Plant

            CreateDecoration("plant1", -9980, 100, 50);
            CreateDecoration("plant2", -8372, 100, 50);
            CreateDecoration("plant3", -7263, 100, 50);
            CreateDecoration("plant10", -7000, 100, 50);
            CreateDecoration("plant4", -6573, 100, 50);
            CreateDecoration("plant3", -6000, 100, 50);
            CreateDecoration("plant5", -5555, 100, 50);
            CreateDecoration("plant6", -4238, 100, 50);
            CreateDecoration("plant7", -3997, 100, 50);
            CreateDecoration("plant8", -2345, 100, 50);
            CreateDecoration("plant9", -1234, 100, 50);
            CreateDecoration("plant1", 0, 100, 50);
            CreateDecoration("plant2", 1000, 100, 50);
            CreateDecoration("plant8", -1345, 100, 50);
            CreateDecoration("plant3", 2000, 100, 50);
            CreateDecoration("plant4", 3000, 100, 50);
            CreateDecoration("plant7", 3200, 100, 50);
            CreateDecoration("plant5", 4000, 100, 50);
            CreateDecoration("plant5", -4855, 100, 50);
            CreateDecoration("plant6", 5000, 100, 50);
            CreateDecoration("plant7", 6000, 100, 50);
            CreateDecoration("plant8", 7000, 100, 50);
            CreateDecoration("plant9", 8000, 100, 50);
            CreateDecoration("plant10", 9000, 100, 50);

            CreateDecoration("plant3", -9843, 100, 200);
            CreateDecoration("plant5", -7855, 100, 200);
            CreateDecoration("plant8", -4438, 100, 200);
            CreateDecoration("plant7", -3997, 100, 200);
            CreateDecoration("plant8", -1345, 100, 200);
            CreateDecoration("plant4", -234, 100, 200);
            CreateDecoration("plant1", 1433, 100, 200);
            CreateDecoration("plant7", 1800, 100, 200);
            CreateDecoration("plant5", 3345, 100, 200);
            CreateDecoration("plant3", 4700, 100, 200);
            CreateDecoration("plant4", 5300, 100, 200);
            CreateDecoration("plant7", 6800, 100, 200);
            CreateDecoration("plant10", 7000, 100, 200);
            CreateDecoration("plant4", 873, 100, 200);
            CreateDecoration("plant3", 9350, 100, 200);

            CreateDecoration("plant5", -9740, 100, 300);
            CreateDecoration("plant5", -6200, 100, 300);
            CreateDecoration("plant3", -3300, 100, 300);
            CreateDecoration("plant7", 1300, 100, 300);
            CreateDecoration("plant5", 3300, 100, 300);
            CreateDecoration("plant8", 7000, 100, 300);
            CreateDecoration("plant10", 8000, 100, 300);

            #endregion

            #region ContactableDecoration

            CreateContactableDecoration("chair3", -9271, 150*0.27f, 99.5f, 100, 70, 0.5f, 0.77f, UnitType.Dynamic);
            CreateContactableDecoration("table4", -8444, 67, 99, 151, 115, 0.5f, 0.57f);
            CreateContactableDecoration("chair2", -7000, 150*0.28f, 99, 108, 65, 0.5f, 0.78f, UnitType.Dynamic);
            CreateContactableDecoration("table3", -6857, 63, 99.5f, 180, 104, 0.5f, 0.58f);
            CreateContactableDecoration("chair2", -6677, 150*0.28f, 99, 108, 65, 0.5f, 0.78f, UnitType.Dynamic);
            CreateContactableDecoration("table1", -5000, 76, 99, 153, 120, 0.5f, 0.6f);
            CreateContactableDecoration("chair4", -4600, 150*0.28f, 99, 190, 65, 0.5f, 0.78f, UnitType.Dynamic);
            CreateContactableDecoration("chair1", -3000, 150*0.25f, 99, 98, 75, 0.5f, 0.75f, UnitType.Dynamic);
            CreateContactableDecoration("chair1", -2860, 150*0.25f, 99.5f, 98, 75, 0.5f, 0.75f, UnitType.Dynamic);
            CreateContactableDecoration("chair3", -1100, 150*0.27f, 99, 100, 70, 0.5f, 0.77f, UnitType.Dynamic);
            CreateContactableDecoration("table2", -943, 64, 99, 152, 106, 0.5f, 0.58f);
            CreateContactableDecoration("chair3", -773, 150*0.27f, 99, 100, 70, 0.5f, 0.77f, UnitType.Dynamic);
            CreateContactableDecoration("chair1", -30, 150*0.25f, 99, 98, 75, 0.5f, 0.75f, UnitType.Dynamic);
            CreateContactableDecoration("chair1", 100, 150*0.25f, 99.5f, 98, 75, 0.5f, 0.75f, UnitType.Dynamic);
            CreateContactableDecoration("chair2", 1100, 150*0.28f, 99, 108, 65, 0.5f, 0.78f, UnitType.Dynamic);
            CreateContactableDecoration("table3", 1255, 63, 99.5f, 180, 104, 0.5f, 0.58f);
            CreateContactableDecoration("chair2", 1429, 150*0.28f, 99, 108, 65, 0.5f, 0.78f, UnitType.Dynamic);
            CreateContactableDecoration("table1", -2600, 76, 99, 153, 120, 0.5f, 0.6f);
            CreateContactableDecoration("chair3", 3700, 150*0.27f, 99, 100, 70, 0.5f, 0.77f, UnitType.Dynamic);
            CreateContactableDecoration("chair3", 3830, 150*0.27f, 99.5f, 100, 70, 0.5f, 0.77f, UnitType.Dynamic);
            CreateContactableDecoration("table2", 4700, 64, 99, 152, 106, 0.5f, 0.58f);
            CreateContactableDecoration("chair2", 5350, 150*0.28f, 99, 108, 65, 0.5f, 0.78f, UnitType.Dynamic);
            CreateContactableDecoration("chair2", 5470, 150*0.28f, 99.5f, 108, 65, 0.5f, 0.78f, UnitType.Dynamic);
            CreateContactableDecoration("table3", 1255, 63, 99, 180, 104, 0.5f, 0.58f);
            CreateContactableDecoration("chair2", 5945, 150*0.28f, 99.5f, 108, 65, 0.5f, 0.78f, UnitType.Dynamic);
            CreateContactableDecoration("chair4", 6500, 150*0.28f, 99, 190, 65, 0.5f, 0.78f, UnitType.Dynamic);
            CreateContactableDecoration("table3", 7777, 63, 99, 180, 104, 0.5f, 0.58f);
            CreateContactableDecoration("chair1", 8700, 150*0.25f, 99, 98, 75, 0.5f, 0.75f, UnitType.Dynamic);
            CreateContactableDecoration("table4", 8850, 67, 99.5f, 151, 115, 0.5f, 0.57f);
            CreateContactableDecoration("chair1", 9029, 150*0.25f, 99, 98, 75, 0.5f, 0.75f, UnitType.Dynamic);
            CreateContactableDecoration("chair3", 9758, 150*0.27f, 99, 100, 70, 0.5f, 0.77f, UnitType.Dynamic);
            CreateContactableDecoration("chair3", 9888, 150*0.27f, 99.5f, 100, 70, 0.5f, 0.77f, UnitType.Dynamic);

            #endregion
        }

        #endregion

        #region Helpers

        private Character CreateCharacter(string filePath, Group group, float x, float y, float z,
            int HP = 10, float speedX = 200, float speedY = 400,
            float scaleX = 1.0f, float scaleY = 1.0f,
            float r = 1.0f, float g = 1.0f, float b = 1.0f)
        {
            var charDef = XmlHelper.LoadFromFile<CharacterDef>(filePath);
            charDef.X = x;
            charDef.Y = y;
            charDef.Z = z;
            charDef.R = r;
            charDef.G = g;
            charDef.B = b;
            charDef.ScaleX = scaleX;
            charDef.ScaleY = scaleY;
            charDef.Width *= scaleX;
            charDef.Height *= scaleY;
            charDef.MoveSpeed = new Vector2(speedX, speedY);
            charDef.Group = group;
            charDef.MaxHP = HP;
            charDef.HP = HP;

            Character character = _world.CreateCharacter(charDef);

            var hpBar = new HPBar("bloodframe", "blood");
            hpBar.Width = 186;
            hpBar.Height = 20;
            hpBar.X = -93;
            hpBar.Y = 100;
            hpBar.Target = character;

            return character;
        }

        private void CreateContactableDecoration(string texName,
            float x, float y, float z,
            float w, float h,
            float transX = 0.5f, float transY = 0.5f,
            string unitType = UnitType.Static,
            float scaleX = 1.0f, float scaleY = 1.0f)
        {
            var def = new DrawUnitDef();
            def.TexName = texName;
            def.X = x;
            def.Y = y;
            def.Z = z;
            def.UnitType = unitType;
            def.TransformOrigin = new Vector2(transX, transY);
            DrawUnit unit = _world.CreateRectangle(def, w, h);
            Texture2D tex = unit.Sprite.Texture;
            unit.Sprite.DrawRectangle = new Rectangle(0, 0, tex.Width, tex.Height);
            unit.Group = Group.Destructable;

            Root.Add(unit.Sprite);
        }

        private Sprite CreateDecoration(string texName,
            float x, float y, float z,
            float scaleX = 1.0f, float scaleY = 1.0f)
        {
            var sd = new SpriteDef();
            sd.TexName = texName;
            sd.X = x;
            sd.Y = y;
            sd.Z = z;
            sd.ScaleX = scaleX;
            sd.ScaleY = scaleY;
            sd.ZWriteEnable = true;

            Sprite sprite = sd.ToSprite();
            sprite.DrawRectangle = new Rectangle(0, 0, sprite.Texture.Width, sprite.Texture.Height);
            sd.DrawRectangle = sprite.DrawRectangle;

            Root.Add(sprite);

            return sprite;
        }

        private void CreateDragonfly(string texName, float z,
            float xFrom, float xTo, float yFrom, float yTo,
            uint duration = 2000)
        {
            Sprite sprite = CreateDecoration(texName, xTo, yTo, z);

            var moveDragonfly = new MoveXY(xFrom, xTo, yFrom, yTo, duration);
            moveDragonfly.Loop = true;
            moveDragonfly.Reverse = true;
            moveDragonfly.EaseFunction = EaseFunction.In_Out_Cubic;
            moveDragonfly.Target = sprite;
            moveDragonfly.Start();

            var rotateDragonfly = new RotateZ(0.3f, -0.5f, 1000);
            rotateDragonfly.Loop = true;
            rotateDragonfly.Reverse = true;
            rotateDragonfly.Target = sprite;
            rotateDragonfly.Start();
        }

        #endregion

        #region LoadBackground

        private void LoadBackground()
        {
            var defFiles = new List<string>();
            defFiles.Add(@"Data\bg1.xml");
            defFiles.Add(@"Data\bg2.xml");
            defFiles.Add(@"Data\bg3.xml");
            defFiles.Add(@"Data\bg4.xml");
            defFiles.Add(@"Data\bg5.xml");
            defFiles.Add(@"Data\bg6.xml");
            defFiles.Add(@"Data\bg7.xml");
            defFiles.Add(@"Data\bg8.xml");
            defFiles.Add(@"Data\bg9.xml");
            defFiles.Add(@"Data\bg10.xml");
            defFiles.Add(@"Data\bg11.xml");
            defFiles.Add(@"Data\bg12.xml");

            defFiles.Add(@"Data\gallery.xml");

            defFiles.Add(@"Data\rail.xml");
            defFiles.Add(@"Data\roof.xml");
            foreach (string defFile in defFiles)
            {
                var def = XmlHelper.LoadFromFile<SpriteDef>(defFile);
                Sprite sprite = def.ToSprite();
                Root.Add(sprite);
            }
        }

        #endregion

        #region LoadWorld

        private void LoadWorld()
        {
        }

        #endregion

        #region LoadIntro

        private void LoadIntro()
        {
            _clearScreen = new Sprite(XNADevicesManager.Instance.ContentManager.Load<Texture2D>("black1x1"));
            _clearScreen.DrawRectangle = new Rectangle(0, 0, 1, 1);
            _clearScreen.Position = _player1.Sprite.Position;
            _clearScreen.Z = 315;
            _clearScreen.ScaleX = 4096.0f;
            _clearScreen.ScaleY = 4096.0f;
            Root.Add(_clearScreen);

            _fadeScreen = new Fade(1.0f, 0.0f, 2000);
            _fadeScreen.EaseFunction = EaseFunction.In_Cubic;
            _fadeScreen.Target = _clearScreen;
            _fadeScreen.OnEnd += s => { _clearScreen.IsVisible = false; };

            _boardTexs = new string[4];
            _boardTexs[0] = "intro0";
            _boardTexs[1] = "intro1";
            _boardTexs[2] = "intro2";
            _boardTexs[3] = "intro3";

            _board = new Board(_boardTexs[0], _unitCam);
            _board.Distance = 285;
            _board.OnHide += (s, e) => { Pause = false; };
            Root.Add(_board.Sprite);

            var logo = new Logo("logo", _unitCam);
            logo.Distance = 284;
            logo.Show();
            AuditionManager.Instance.PlayBGM("surrounded");
            Root.Add(logo.Sprite);

            _startTimer = new Timer(5500);
            _startTimer.OnTimer += sender =>
            {
                _fadeScreen.Start();
                Pause = false;
                _introTimer.Start();
            };
            _startTimer.Start();

            _introTimer = new Timer(1000);
            _introTimer.OnTimer += sender =>
            {
                _board.SwitchTex(_boardTexs[_countBoard]);
                _board.Show();
                Pause = true;
            };

            Pause = true;
        }

        #endregion

        #region LoadAudition

        private void LoadAudition()
        {
            AuditionManager.Instance.LoadAuditionResource(@"Data\Audition.xml");
        }

        #endregion

        #region Constructors

        public PaperChineseOpera(string name)
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
            EventManager.PostEvent(new Event(Constants.TIME_TICKS_EVENT));
            EventManager.Update();

            if (!Pause)
            {
                EventManager.PostEvent(new Event(Constants.AI_EVENT));

                if (InputManager.Instance.IsKeyUp(Keys.Left) ||
                    InputManager.Instance.IsKeyUp(Keys.Right))
                {
                    _player1.Do("Stop");
                }

                if (InputManager.Instance.GetKeyState(Keys.Up))
                {
                    _player1.Do("Jump");
                }
                if (InputManager.Instance.GetKeyState(Keys.Down))
                {
                }
                if (InputManager.Instance.GetKeyState(Keys.Left))
                {
                    if (_player1.Direction != Direction.Left)
                    {
                        _player1.Do("TurnLeft");
                    }
                    _player1.Do("Walk");
                }
                if (InputManager.Instance.GetKeyState(Keys.Right))
                {
                    if (_player1.Direction != Direction.Right)
                    {
                        _player1.Do("TurnRight");
                    }
                    _player1.Do("Walk");
                }

                if (InputManager.Instance.GetKeyState(Keys.F))
                {
                    _player1.Do("Attack");
                }
            }
            else
            {
                if (!_board.IsFading && InputManager.Instance.IsKeyDown(Keys.F))
                {
                    _countBoard++;
                    if (_countBoard == 3)
                    {
                        _board.Hide();
                    }
                    else
                    {
                        _board.SwitchTex(_boardTexs[_countBoard]);
                        _board.Show();
                    }
                }
            }

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
