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
    /// <summary>
    /// This screen implements the actual game logic.
    /// 
    /// </summary>
    //class GameplayScreen : GameScreen
    //{
    //    #region Fields
       
    //    ContentManager content;
    //    SpriteFont font;
    //    SpriteBatch spriteBatch;
    
    //    Texture2D btn;
       
    //    /// <summary>
    //    /// The actual gamescreen controls
    //    /// </summary>
    //    ScreenPad screenPad;

    //    /// <summary>
    //    /// the value that <b>screenPad.GetState()</b> returns
    //    /// </summary>
    //    ScreenPadState current_PadState;

    //    float screenWidth, screenHeight;
    //    string text = "Button";
    //    RenderContext _renderContext;
    //    InputManager inputManager;
    //    GameSprite _background;
    //    //GameAnimatedModel _hero;
    //    Character _hero;
    //    BaseCamera _camera;
    //    #endregion

    //    #region Initialization

    //    public GameplayScreen()
    //    {
    //        TransitionOnTime = TimeSpan.FromSeconds(1.5);
    //        TransitionOffTime = TimeSpan.FromSeconds(0.5);
    //        EnabledGestures = GestureType.Tap;
    //        _renderContext = new RenderContext();
    //        _hero = new Character();
    //        _hero.renderContext = _renderContext;
    //        _hero.Initialize();
    //    }

    //    public override void LoadContent()
    //    {
    //        if (content == null)
    //            content = new ContentManager(ScreenManager.Game_.Services, "Content");
    //        screenWidth = ScreenManager.GraphicsDevice.Viewport.Width;
    //        screenHeight = ScreenManager.GraphicsDevice.Viewport.Height;


    //        spriteBatch = ScreenManager.SpriteBatch;
    //        font = content.Load<SpriteFont>("menufont");

    //        #region Screenpads
    //        #region OtherScreenPad
    //        //screenPad = new ScreenPad
    //        //(
    //        //    ScreenManager.Game_,
    //        //    content.Load<Texture2D>("Textures_Menu/ThumbBase"),
    //        //    content.Load<Texture2D>("Textures_Menu/ThumbStick"),
    //        //    content.Load<Texture2D>("Textures_Menu/Dpad_All"),
    //        //    Color.Blue,
    //        //    Color.Red,
    //        //    Color.Green
    //        //); 
    //        #endregion

    //        screenPad = new ScreenPad
    //        (
    //            ScreenManager.Game_,
    //            content.Load<Texture2D>("Textures_Menu/ThumbBase"),
    //            content.Load<Texture2D>("Textures_Menu/ThumbStick"),
    //            content.Load<Texture2D>("Textures_Menu/ABXY_buttons")
    //        );
    //        #endregion

           
    //        _camera = new BaseCamera();
    //        _camera.Translate(new Vector3(0, 0, 20));
    //        _renderContext.Camera = _camera;
    //        _background = new GameSprite(@"Textures/BackGround");
    //        //_hero = new GameAnimatedModel(@"Models/Vampire");
    //        //_hero.PlayAnimation("Run", true, 0f);
    //        _hero.LoadContent(content);
    //        _hero.Rotate(0f, 180f, 0f);

    //        inputManager = new InputManager();

    //        _renderContext.SpriteBatch = spriteBatch;
    //        _renderContext.GraphicsDevice = ScreenManager.GraphicsDevice;
    //        _renderContext.Input = inputManager;

    //        _renderContext.Input.screenPad = screenPad;
    //       // _renderContext.Input.CurrentScreenPadState = screenPad.GetState();
    //        // TODO: use this.Content to load your game content here
    //       // _hero = new Character(
            
    //        _background.LoadContent(content);

    //        ScreenManager.Game_.ResetElapsedTime();
    //    }
   
    //    public override void UnloadContent()
    //    {
    //        content.Unload();
          
    //    }

        
    //    #endregion

    //    #region Update and Draw


    //    /// <summary>
    //    /// Updates the state of the game. This method checks the GameScreen.IsActive
    //    /// property, so the game will stop updating when the pause menu is active,
    //    /// or if you tab away to a different application.
    //    /// </summary>
    //    public override void Update(GameTime gameTime, bool otherScreenHasFocus,bool coveredByOtherScreen)
    //    {
            
                
    //        base.Update(gameTime, otherScreenHasFocus, coveredByOtherScreen);

    //        if (IsActive)
    //        {
    //            float elapsedTime = (float)gameTime.ElapsedGameTime.TotalSeconds;
    //            _renderContext.GameTime = gameTime;

    //            screenPad.Update();
    //            //current_PadState = screenPad.GetState();
    //            _renderContext.Input.CurrentScreenPadState = screenPad.GetState();
                
                
    //            _camera.Update(_renderContext);

    //            _hero.Update(_renderContext);
    //            if (_renderContext.Input.CurrentScreenPadState.Buttons.A == VirtualButtonState.Pressed)
    //            LoadingScreen.Load(ScreenManager, false, ControllingPlayer, new BackgroundScreen(), new MainMenuScreen());
               
    //        }



    //    }


    //    /// <summary>
    //    /// Lets the game respond to player input. Unlike the Update method,
    //    /// this will only be called when the gameplay screen is active.
    //    /// </summary>
    //    public override void HandleInput(InputState input)
    //    {
    //        if (input == null)
    //            throw new ArgumentNullException("input");

    //        // Look up inputs for the active player profile.
    //        int playerIndex = (int)ControllingPlayer.Value;

    //        KeyboardState keyboardState = input.CurrentKeyboardStates[playerIndex];
    //        GamePadState gamePadState = input.CurrentGamePadStates[playerIndex];

    //        // if the user pressed the back button, we return to the main menu/Exit game
    //        PlayerIndex player;
    //        if (input.IsNewButtonPress(Buttons.Back, ControllingPlayer, out player))
    //        {
    //            LoadingScreen.Load(ScreenManager, false, ControllingPlayer, new BackgroundScreen(), new MainMenuScreen());
    //        }
    //        //else
    //        //{
    //        //    // conManager.UpdateInput(input);
    //        //    //a = conManager.GetButtonStatus("A");
    //        //    //b = conManager.GetButtonStatus("B");

    //        //}
    //    }


    //    /// <summary>
    //    /// Draws the gameplay screen.
    //    /// </summary>
    //    public override void Draw(GameTime gameTime)
    //    {
    //        ScreenManager.GraphicsDevice.Clear(ClearOptions.Target,Color.CornflowerBlue, 0, 0);

    //        spriteBatch.Begin();
    //        //if (current_PadState.Buttons.B != VirtualButtonState.Pressed) //for testing
    //        if(_renderContext.Input.CurrentScreenPadState.Buttons.B != VirtualButtonState.Pressed)
    //        _background.Draw(_renderContext);
    //        spriteBatch.End();

    //        //Reset the render states
    //        ScreenManager.GraphicsDevice.BlendState = BlendState.Opaque;
    //        ScreenManager.GraphicsDevice.DepthStencilState = DepthStencilState.Default;
    //        ScreenManager.GraphicsDevice.SamplerStates[0] = SamplerState.LinearWrap;

    //        _hero.Draw(_renderContext);

    //        spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend);
    //        screenPad.Draw(gameTime, spriteBatch);

    //        spriteBatch.DrawString(font, string.Format("Left Stick: {0}", current_PadState.ThumbSticks.Left.ToString()), Vector2.Zero, Color.Black);

    //        if (screenPad.XTYPE)//Screen pad with buttons
    //        {
    //            spriteBatch.DrawString(font, string.Format("X: {0}", (current_PadState.Buttons.X == VirtualButtonState.Pressed).ToString()), new Vector2(0, 50), Color.Black);
    //            spriteBatch.DrawString(font, string.Format("Y: {0}", (current_PadState.Buttons.Y == VirtualButtonState.Pressed).ToString()), new Vector2(0, 80), Color.Black);
    //            spriteBatch.DrawString(font, string.Format("A: {0}", (current_PadState.Buttons.A == VirtualButtonState.Pressed).ToString()), new Vector2(0, 110), Color.Black);
    //            spriteBatch.DrawString(font, string.Format("B: {0}", (current_PadState.Buttons.B == VirtualButtonState.Pressed).ToString()), new Vector2(0, 140), Color.Black);
    //        }
    //        else
    //        {
    //            spriteBatch.DrawString(font, string.Format("Right Stick: {0}", current_PadState.ThumbSticks.Right.ToString()), new Vector2(0, 50), Color.Black);
    //        }

    //        spriteBatch.End();   

    //        if (TransitionPosition > 0)
    //            ScreenManager.FadeBackBufferToBlack(1f - TransitionAlpha);
    //    }


    //    #endregion
    //}


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

        /// <summary>
        /// the value that <b>screenPad.GetState()</b> returns
        /// </summary>
       // ScreenPadState current_PadState;

        float screenWidth, screenHeight;
      //  string text = "Button";
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
# if WINDOWSPHONE
            screenWidth = ScreenManager.GraphicsDevice.Viewport.Width;
            screenHeight = ScreenManager.GraphicsDevice.Viewport.Height;
#endif
#if ANDROID
            screenWidth = 1280;
            screenHeight = 720;
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
            //else
            //{
            //    // conManager.UpdateInput(input);
            //    //a = conManager.GetButtonStatus("A");
            //    //b = conManager.GetButtonStatus("B");

            //}
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


        #endregion
    }

}
