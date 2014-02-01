using System;
using System.Collections.Generic;
using System.IO;
using System.IO.IsolatedStorage;
using System.Linq;
using System.Xml.Linq;
using System.Xml.Serialization;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Blocker
{
    static class SceneManager
    {
        public static Game MainGame { get; set; }
        public static List<GameScene> GameScenes { get; set; }
        public static GameScene ActiveScene { get; private set; }
        public static RenderContext RenderContext { get; private set; }
        public static InputManager Input { get { return RenderContext.Input; } }
        public static SpriteFont font;
        public static LevelData thisLevel { get; set; }
        public static Character c { get; set; }

        static SceneManager()
        {
            GameScenes = new List<GameScene>();
            RenderContext = new RenderContext
            {
                //Default Camera
                //Camera = new BaseCamera(),
                Camera = new ChaseCamera(),
                Input = new InputManager(),
            };
        }

        #region Add/Remove/Activate GameScenes
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
        #endregion

        public static void Initialize()
        {
            GameScenes.ForEach(scene => scene.Initialize());
        }

        public static void LoadContent(ContentManager contentManager)
        {
            GameScenes.ForEach(scene => scene.LoadContent(contentManager));
            font = contentManager.Load<SpriteFont>("menufont"); // this is for debug
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

        /// <summary>
        /// This takes a snapshot of all the game object values to save to Isolated storage. Called when back button pressed
        /// </summary>
        public static void UpdateLevelData()
        {
            thisLevel.GameObject3D.Clear();
            foreach (GameObject3D c in ActiveScene.SceneObjects3D)
            {
                if (c.GetType() == typeof(ChaseCamera))
                {
                }
                else if (c.id == 1)
                {

                    thisLevel.character.PositionX = c.WorldPosition.X;
                    thisLevel.character.PositionY = c.WorldPosition.Y;
                    thisLevel.character.PositionZ = c.WorldPosition.Z;
                    thisLevel.character.RotationX = c.WorldRotation.X;
                    thisLevel.character.RotationY = c.WorldRotation.Y;
                    thisLevel.character.RotationZ = c.WorldRotation.Z;
                    thisLevel.character.RotationW = c.WorldRotation.W;
                }
                else
                {
                    Object3D_Data temp = new Object3D_Data();
                    temp.id = c.id;
                    temp.model_Path = c.modelPath;
                    temp.PositionX = c.WorldPosition.X;
                    temp.PositionY = c.WorldPosition.Y;
                    temp.PositionZ = c.WorldPosition.Z;
                    temp.RotationX = c.WorldRotation.X;
                    temp.RotationY = c.WorldRotation.Y;
                    temp.RotationZ = c.WorldRotation.Z;
                    temp.RotationW = c.WorldRotation.W;
                    thisLevel.GameObject3D.Add(temp);
                }
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
                        (font, string.Format("Cam Pos: {0}", ActiveScene.camera.LocalPosition.ToString())
                        , new Vector2(0, 50), Color.Black);
                    RenderContext.SpriteBatch.DrawString
                        (font, string.Format("Char Position: {0}", c.LocalPosition.ToString())
                        , new Vector2(0, 80), Color.Black);
                    RenderContext.SpriteBatch.DrawString
                       (font, string.Format("Controller Pos: {0}", c.charInput.CharacterController.Body.Position.ToString())
                       , new Vector2(0, 110), Color.Black);
                    RenderContext.SpriteBatch.DrawString
                      (font, string.Format("Controller Rot: {0}", c.charInput.CharacterController.Body.Orientation.ToString())
                      , new Vector2(0, 140), Color.Black);
                    RenderContext.SpriteBatch.DrawString
                      (font, string.Format("Char Rot: {0}", c.LocalRotation.ToString())
                      , new Vector2(0, 170), Color.Black);
                    //RenderContext.SpriteBatch.DrawString
                    //    (font, string.Format("A: {0}", (Input.CurrentScreenPadState.Buttons.A == VirtualButtonState.Pressed).ToString()), new Vector2(0, 110), Color.Black);
                    //RenderContext.SpriteBatch.DrawString
                    //    (font, string.Format("B: {0}", (Input.CurrentScreenPadState.Buttons.B == VirtualButtonState.Pressed).ToString()), new Vector2(0, 140), Color.Black);
                    //RenderContext.SpriteBatch.DrawString
                    //    (font, string.Format("Char Postion: {0}", new Vector3(thisLevel.character.PositionX, thisLevel.character.PositionY, thisLevel.character.PositionZ).ToString())
                    //    , new Vector2(0, 170), Color.White);
                    //RenderContext.SpriteBatch.DrawString
                    //    (font, string.Format("Char Postion: {0}", .ToString())
                    //    , new Vector2(0, 170), Color.White);
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

        public static void SaveLevel(LevelData saveData)
        {
#if WINDOWS_PHONE
            IsolatedStorageFile storage = IsolatedStorageFile.GetUserStoreForApplication();
#else
            IsolatedStorageFile storage = IsolatedStorageFile.GetUserStoreForDomain();
#endif
            using (storage)
            {
                //delete any existing file
                if (storage.FileExists("TestData.xml"))
                    storage.DeleteFile("TestData.xml");
                //create file for data
                if (!storage.FileExists("TestData.xml"))
                    storage.CreateFile("TestData.xml").Dispose();

                //create new savegame file
                if (storage.FileExists("TestData.xml"))
                {
                    using (var stream = storage.OpenFile("TestData.xml", FileMode.Open))
                    {
                        XmlSerializer serializer = new XmlSerializer(typeof(LevelData));
                        serializer.Serialize(stream, saveData);
                    }
                }
            }
        }

        public static LevelData LoadLevel()
        {
            LevelData data;
#if WINDOWS_PHONE
            IsolatedStorageFile storage = IsolatedStorageFile.GetUserStoreForApplication();
#else
            IsolatedStorageFile storage = IsolatedStorageFile.GetUserStoreForDomain();
#endif
            using (storage)
            {
                if (storage.FileExists("TestData.xml"))
                {
                    using (var fstream = storage.OpenFile("TestData.xml", FileMode.Open))
                    {
                        XmlSerializer serializer = new XmlSerializer(typeof(LevelData));
                        data = (LevelData)serializer.Deserialize(fstream);
                    }
                    return data;
                }
                else
                {
                    using (var stream = TitleContainer.OpenStream("Content/TestData.xml"))
                    {
                        XmlSerializer serializer = new XmlSerializer(typeof(LevelData));
                        data = (LevelData)serializer.Deserialize(stream);
                    }
                    return data;
                }
            }
        }
    }
}
