#region Using Statements
using System;
using System.Threading;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Input.Touch;

#endregion

namespace AndroidTest
{
   
    class GameplayScreen : GameScreen
    {
        #region Fields

        ContentManager content;
        SpriteFont font;
        SpriteBatch spriteBatch;
        Texture2D btn;
        /// <summary>
        /// The actual gamescreen controls
        /// </summary>
        ScreenPad screenPad;

        float screenWidth, screenHeight;
        RenderContext _renderContext;
       
        #endregion

        #region Initialization

        public GameplayScreen()
        {
            TransitionOnTime = TimeSpan.FromSeconds(1.5);
            TransitionOffTime = TimeSpan.FromSeconds(0.5);
            EnabledGestures = GestureType.Tap;
            _renderContext = new RenderContext();

            SceneManager.AddGameScene(new TestLevel());
            SceneManager.SetActiveScene("Test");
            SceneManager.Initialize();         
        }

        public override void LoadContent()
        {
            if (content == null)
                content = new ContentManager(ScreenManager.Game_.Services, "Content");
# if WINDOWS_PHONE
            screenWidth = ScreenManager.GraphicsDevice.Viewport.Width;
            screenHeight = ScreenManager.GraphicsDevice.Viewport.Height;
#endif
#if ANDROID/// this maybe display bug on android 
            screenWidth = 1280;
            screenHeight = 720; //800
#endif
            spriteBatch = ScreenManager.SpriteBatch;
            SceneManager.RenderContext.SpriteBatch = spriteBatch;
            SceneManager.RenderContext.GraphicsDevice = ScreenManager.GraphicsDevice;
            SceneManager.LoadContent(content);

            font = content.Load<SpriteFont>("menufont");

            #region Screenpads
           
            screenPad = new ScreenPad
            (
                ScreenManager.Game_,
                content.Load<Texture2D>("Textures_Menu/ThumbBase"),
                content.Load<Texture2D>("Textures_Menu/ThumbStick"),
                content.Load<Texture2D>("Textures_Menu/ABXY_buttons")
            );
            #endregion

            SceneManager.RenderContext.Input.screenPad = screenPad;
            
            ScreenManager.Game_.ResetElapsedTime();
        }

        public override void UnloadContent()
        {
            content.Unload();
        }


        #endregion

        #region Update and Draw


        /// <summary>
        /// Updates the state of the game. This method checks the GameScreen.IsActive
        /// property, so the game will stop updating when the pause menu is active,
        /// or if you tab away to a different application.
        /// </summary>
        public override void Update(GameTime gameTime, bool otherScreenHasFocus, bool coveredByOtherScreen)
        {
            base.Update(gameTime, otherScreenHasFocus, coveredByOtherScreen);

            if (IsActive)
            {
                float elapsedTime = (float)gameTime.ElapsedGameTime.TotalSeconds;
                _renderContext.GameTime = gameTime;

                screenPad.Update();
                SceneManager.Update(gameTime);
                SceneManager.RenderContext.Input.CurrentScreenPadState = screenPad.GetState();
            }
        }

        /// <summary>
        /// Draws the gameplay screen.
        /// </summary>
        public override void Draw(GameTime gameTime)
        {
            ScreenManager.GraphicsDevice.Clear(ClearOptions.Target, Color.CornflowerBlue, 0, 0);
            SceneManager.Draw();

            spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend);
            screenPad.Draw(gameTime, spriteBatch);
            spriteBatch.End();   

            if (TransitionPosition > 0)
                ScreenManager.FadeBackBufferToBlack(1f - TransitionAlpha);
        }


        /// <summary>
        /// Lets the game respond to player input. Unlike the Update method,
        /// this will only be called when the gameplay screen is active.
        /// </summary>
        public override void HandleInput(InputState input)
        {
            if (input == null)
                throw new ArgumentNullException("input");

            // Look up inputs for the active player profile.
            int playerIndex = (int)ControllingPlayer.Value;

            KeyboardState keyboardState = input.CurrentKeyboardStates[playerIndex];
            GamePadState gamePadState = input.CurrentGamePadStates[playerIndex];

            // if the user pressed the back button, we return to the main menu/Exit game
            PlayerIndex player;
            if (input.IsNewButtonPress(Buttons.Back, ControllingPlayer, out player))
            {
                LoadingScreen.Load(ScreenManager, false, ControllingPlayer, new BackgroundScreen(), new MainMenuScreen());
            }
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
            {
                SceneManager.RemoveGameScene("Test");  // change this to generic screen name
            }

        }

        #endregion
    }

}
