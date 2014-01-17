using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;

namespace AndroidTest
{
    public enum InputActionIds
    {
        Action,
        Jump,
        MoveLeft,
        MoveRight,
        MoveForWard,
        MoveBackward,
        Restart,
        Rotate,
        RotUp,
        RotDown,
        Pause,
        MenuScreen
    }


    public class Character: GameObject3D
    {
        private GameAnimatedModel char_Model;
        //private InputManager im;
        //Movement
        public bool IsGrounded { get; set; }

        private const int   RUN_SPEED = 170;
        private const float RUN_ACCELERATION_TIME = 0.2f;

        private float   _runAcceleration;
        private Vector2 _velocity;
        private int     _direction;
        float temp = 0;
        public RenderContext renderContext { get; set; }

        public override void Initialize()
        {
            char_Model = new GameAnimatedModel("Models\\Vampire");
            char_Model.Scale(0.8f, 0.8f, 0.8f);
            AddChild(char_Model);
            
            //Initialize Movement Parameters
            _runAcceleration = RUN_SPEED / RUN_ACCELERATION_TIME;

            //ADD INPUTACTIONS
            var inputAction = new InputAction((int)InputActionIds.Rotate, VirtualButtonState.Pressed)
            {
#if WINDOWS
                GamePadButton = Buttons.LeftThumbstickLeft,
                KeyButton = Keys.Left
#endif
                S_Pad_Button = Virtual_Button.thumbstick
            };
            SceneManager.Input.MapAction(inputAction);
           

            inputAction = new InputAction((int)InputActionIds.MoveForWard, VirtualButtonState.Pressed)
            {
#if WINDOWS
                GamePadButton = Buttons.LeftThumbstickRight,
                KeyButton = Keys.Right
#endif
                S_Pad_Button = Virtual_Button.thumbstick
            };
            SceneManager.Input.MapAction(inputAction);
           

            inputAction = new InputAction((int)InputActionIds.Jump, VirtualButtonState.Pressed)
            {
                #if WINDOWS
                GamePadButton = Buttons.A,
                KeyButton = Keys.Space
                #endif
                S_Pad_Button = Virtual_Button.A
            };
            SceneManager.Input.MapAction(inputAction);
            

            inputAction = new InputAction((int)InputActionIds.MoveForWard, VirtualButtonState.Pressed)
            {
#if WINDOWS
                GamePadButton = Buttons.X,
                KeyButton = Keys.X
#endif
                S_Pad_Button = Virtual_Button.X
            };
            SceneManager.Input.MapAction(inputAction);

            inputAction = new InputAction((int)InputActionIds.Pause, VirtualButtonState.Pressed)
            {
#if WINDOWS
                GamePadButton = Buttons.X,
                KeyButton = Keys.X
#endif
                S_Pad_Button = Virtual_Button.B
            };
            SceneManager.Input.MapAction(inputAction);

            base.Initialize();
        }

        public override void LoadContent(ContentManager contentManager)
        {
            base.LoadContent(contentManager);

            char_Model.PlayAnimation("Idle", false, RUN_ACCELERATION_TIME);
        }

        public override void Update(RenderContext renderContext)
        {
            //Temporary set IsGrounded to True.
            if (renderContext.Input.screenPad.LeftStick.Y > 0.23 || renderContext.Input.screenPad.LeftStick.Y < -0.23)
                IsGrounded = true;
            else if (renderContext.Input.screenPad.LeftStick.Y == 0)
                IsGrounded = false;// true;

            //Handle RUN_LEFT
            if (renderContext.Input.IsActionTriggered((int)InputActionIds.Rotate))
            {
               
                if (IsGrounded) 
                    char_Model.PlayAnimation("Run", true, RUN_ACCELERATION_TIME);
                else
                    char_Model.PlayAnimation("Idle", true, RUN_ACCELERATION_TIME);

                _direction = -1;
                Rotate(0, -0, 0);
             
                //
               // _velocity.X -= _runAcceleration * (float)renderContext.GameTime.ElapsedGameTime.TotalSeconds/10;
                temp += renderContext.Input.screenPad.LeftStick.X *8;
                _velocity.X += _runAcceleration * (renderContext.Input.screenPad.LeftStick.Y *2) * (float)renderContext.GameTime.ElapsedGameTime.TotalSeconds;
                Rotate(0, -temp, 0);
            }
            if (renderContext.Input.screenPad.LeftStick.Y == 0)
                _velocity.X = 0;
            //Handle RUN_FORWARD
            //else if (renderContext.Input.IsActionTriggered((int)InputActionIds.MoveForWard))
            //{
            //    if (IsGrounded) char_Model.PlayAnimation("Run", true, RUN_ACCELERATION_TIME);

            //    _direction = 1;
            //    Rotate(0, 0, 0);
            //    _velocity.X -= _runAcceleration * (float)renderContext.GameTime.ElapsedGameTime.TotalSeconds / 10;
            //    //_velocity.X += renderContext.Input.screenPad.LeftStick.Y * (float)renderContext.GameTime.ElapsedGameTime.TotalSeconds;
            //}
            ////Handle IDLE
            //else if (IsGrounded)
            //{
            //    _velocity.X -= _direction * _runAcceleration * (float)renderContext.GameTime.ElapsedGameTime.TotalSeconds;

            //    if ((_direction < 0 && _velocity.X > 0) || (_direction > 0 && _velocity.X < 0)) _velocity.X = 0;
            //    char_Model.PlayAnimation("Idle", false, RUN_ACCELERATION_TIME);
            //}

            //Clamp Velocity X
            _velocity.X = MathHelper.Clamp(_velocity.X, -RUN_SPEED, RUN_SPEED);

            //Handle JUMPING
            //...

            //Calculate new position, based on the current velocity
            var totalMovement = _velocity * (float)renderContext.GameTime.ElapsedGameTime.TotalSeconds;
            var newPosition = LocalPosition + new Vector3(totalMovement, 0);
            Translate(newPosition);

           

            base.Update(renderContext);
        }

        public void Reset(Vector3 position)
        {
            Translate(position);
            _velocity = Vector2.Zero;
        }

    }
}
