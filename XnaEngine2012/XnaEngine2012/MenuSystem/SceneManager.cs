using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.IO.IsolatedStorage;
using System.IO;
using System;
using System.Xml.Linq;

namespace AndroidTest
{
    static class SceneManager
    {
        public static Microsoft.Xna.Framework.Game MainGame { get; set; }
        public static List<GameScene> GameScenes { get; private set; }
        public static GameScene ActiveScene { get; private set; }
        public static RenderContext RenderContext { get; private set; }
        public static InputManager Input { get { return RenderContext.Input; } }
        public static SpriteFont font;
        public static Character tempChar { get;  set; }
        public static LevelData thisLevel { get; set; }

        static SceneManager()
        {         
            GameScenes = new List<GameScene>();
            RenderContext = new RenderContext
            {
                //Default Camera
                Camera = new ChaseCam(),
                Input = new InputManager(),
                Hero = tempChar
            };
        }

        public static void AddGameScene(GameScene gameScene)
        {
            if (!GameScenes.Contains(gameScene))
                GameScenes.Add(gameScene);
        }

        public static void RemoveGameScene(GameScene gameScene)
        {
            GameScenes.Remove(gameScene);
            if (ActiveScene == gameScene) ActiveScene = null;
        }

        public static void RemoveGameScene(string name)
        {
            var chosenScene = GameScenes.FirstOrDefault(scene => scene.SceneName.Equals(name));
            if (chosenScene != null)
            {
                GameScenes.Remove(chosenScene);
            }
            if (ActiveScene == chosenScene) ActiveScene = null;
            //GameScenes.Remove(ActiveScene);
        }

        public static void RemoveActiveScene()
        {
            GameScenes.Remove(ActiveScene);
        }

        public static bool SetActiveScene(string name)
        {
            var chosenScene = GameScenes.FirstOrDefault(scene => scene.SceneName.Equals(name));

            if (chosenScene != null)
            {
                if (ActiveScene != null)
                    ActiveScene.Deactivated();

                ActiveScene = chosenScene;
                chosenScene.Activated();
            }

            return chosenScene != null;
        }

        public static void Initialize()
        {
            GameScenes.ForEach(scene => scene.Initialize());
        }

        public static void LoadContent(ContentManager contentManager)
        {
            GameScenes.ForEach(scene => scene.LoadContent(contentManager));
            //Debug2D.LoadContent(contentManager);
            font = contentManager.Load<SpriteFont>("menufont");          
        }

        public static void Update(GameTime gameTime)
        {
            if (ActiveScene != null)
            {
                RenderContext.GameTime = gameTime;
                RenderContext.Input.Update();
                ActiveScene.Update(RenderContext);
            }
        }

        public static void Draw()
        {
            if (ActiveScene != null)
            {
                //2D Behind 3D
                RenderContext.SpriteBatch.Begin();
                ActiveScene.Draw2D(RenderContext, false);
#if DEBUG
                #region Debug info
                //Debug2D.Draw(RenderContext.SpriteBatch);
                if (Input.CurrentScreenPadState.Buttons.B == VirtualButtonState.Released)
                {
                    RenderContext.SpriteBatch.DrawString
                        (font, string.Format("Left Stick: {0}",
                        Input.CurrentScreenPadState.ThumbSticks.Left.ToString()), Vector2.Zero, Color.White);
                    RenderContext.SpriteBatch.DrawString
                        (font, string.Format("X: {0}", (Input.CurrentScreenPadState.Buttons.X == VirtualButtonState.Pressed).ToString()), new Vector2(0, 50), Color.Black);
                    RenderContext.SpriteBatch.DrawString
                        (font, string.Format("Y: {0}", (Input.CurrentScreenPadState.Buttons.Y == VirtualButtonState.Pressed).ToString()), new Vector2(0, 80), Color.Black);
                    RenderContext.SpriteBatch.DrawString
                        (font, string.Format("A: {0}", (Input.CurrentScreenPadState.Buttons.A == VirtualButtonState.Pressed).ToString()), new Vector2(0, 110), Color.Black);
                    RenderContext.SpriteBatch.DrawString
                        (font, string.Format("B: {0}", (Input.CurrentScreenPadState.Buttons.B == VirtualButtonState.Pressed).ToString()), new Vector2(0, 140), Color.Black);
                    RenderContext.SpriteBatch.DrawString
                        (font, string.Format("Char Postion: {0}",new Vector3(thisLevel.CharX,thisLevel.CharY,thisLevel.CharZ).ToString())
                        , new Vector2(0, 170), Color.White);
                } 
                #endregion

#endif
                RenderContext.SpriteBatch.End();

                //DRAW 3D
                //Reset Renderstate
                RenderContext.GraphicsDevice.BlendState = BlendState.Opaque;
                RenderContext.GraphicsDevice.DepthStencilState = DepthStencilState.Default;
                RenderContext.GraphicsDevice.SamplerStates[0] = SamplerState.LinearWrap;
                ActiveScene.Draw3D(RenderContext);

                //2D In front 3D
                RenderContext.SpriteBatch.Begin();
                ActiveScene.Draw2D(RenderContext, true);

                RenderContext.SpriteBatch.End();
            }
        }

        public static void LoadLevel()
        {
#if WINDOWS_PHONE
            IsolatedStorageFile storage = IsolatedStorageFile.GetUserStoreForApplication();
#else
            IsolatedStorageFile storage = IsolatedStorageFile.GetUserStoreForDomain();
#endif
            using (storage)
            {
                XDocument document;
                if (storage.FileExists("TestData.xml"))
                {
                    using (var stream1 = storage.OpenFile("TestData.xml", FileMode.Open))
                    {
                        document = XDocument.Load(stream1);
                    }

                    var data = (from query in document.Descendants("Levels")
                                select new LevelData()
                                {
                                    LevelName = (string)query.Element("LevelName"),
                                    CharX = (float)query.Element("CharX"),
                                    CharY = (float)query.Element("CharY"),
                                    CharZ = (float)query.Element("CharZ")
                                });

                    foreach (LevelData l in data)
                    {
                        ActiveScene.thisLevel = l;
                    }
                }
                else
                {   // if first time use, use default settings from content to seed level settings
                    Stream stream = TitleContainer.OpenStream("Content/TestData.xml");
                    XDocument doc = XDocument.Load(stream);
                    var data = (from query in doc.Descendants("Levels")
                                select new LevelData()
                                {
                                    LevelName = (string)query.Element("LevelName"),
                                    CharX = (float)query.Element("CharX"),
                                    CharY = (float)query.Element("CharY"),
                                    CharZ = (float)query.Element("CharZ")
                                });

                    foreach (LevelData l in data)
                    {
                        ActiveScene.thisLevel = l;
                    }
                }
            }
        }

        public static void SaveLevel(LevelData ld)
        {
#if WINDOWS_PHONE
            IsolatedStorageFile storage = IsolatedStorageFile.GetUserStoreForApplication();
#else
            IsolatedStorageFile storage = IsolatedStorageFile.GetUserStoreForDomain();
#endif

            using (storage)
            {
                XDocument document;
                XElement levelRootNode = null;
                // Check if there is a file to  write to
                if (storage.FileExists("TestData.xml"))
                {
                    using (var stream = storage.OpenFile("TestData.xml", FileMode.Open))
                    {
                        document = XDocument.Load(stream);
                    }
                    levelRootNode = document.Descendants("Levels").FirstOrDefault();
                }
                else
                {
                    document = new XDocument();
                }
                // If new file add data
                if (levelRootNode == null)
                {
                    levelRootNode = new XElement("Levels",
                                    new XElement("LevelName", "xyz"),
                                    new XElement("CharX", ld.CharX),
                                    new XElement("CharY", ld.CharY),
                                    new XElement("CharZ", ld.CharZ));
                    document.Add(levelRootNode);
                }
                else
                {   //If file exists, clear it and re-write new data
                    document.RemoveNodes();
                    //adds updated data to isolated storage
                    levelRootNode = new XElement("Levels",
                                    new XElement("LevelName", "Test"),
                                    new XElement("CharX", ld.CharX),
                                    new XElement("CharY", ld.CharY),
                                    new XElement("CharZ", ld.CharZ));
                    document.Add(levelRootNode);
                }
                using (Stream stream = storage.CreateFile("TestData.xml"))
                {
                    document.Save(stream);
                }
            }
        }

    }
}
