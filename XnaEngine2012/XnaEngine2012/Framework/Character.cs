using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;

namespace Blocker
{
    public enum InputActionIds
    {
        Action,
        Jump,
        MoveLeft,
        MoveRight,
        MoveForWard,
        MoveForWardXButton,
        MoveBackward,
        Restart,
        Rotate,
        RotUp,
        RotDown,
        Pause,
        MenuScreen
    }


    public class Character : GameObject3D
    {
        private GameAnimatedModel char_Model ;
        public CharacterControllerInput charInput { get; set; }
        //Movement
        public bool IsGrounded { get; set; }

        private const int RUN_SPEED = 170;
        private const float RUN_ACCELERATION_TIME = 0.2f;
        private float _runAcceleration;
        float temp;
      
        //private Vector2 _velocity;
        private int _direction;
       
        public Vector3 Direction;

        /// <summary>
        /// player's up vector.
        /// </summary>
        public Vector3 Up;

        private Vector3 right;
        /// <summary>
        /// player's right vector.
        /// </summary>
        public Vector3 Right
        {
            get { return right; }
        }
        public Vector3 Velocity;
        private const float DragFactor = 0.97f;
        private const float ThrustForce = 1.0f;
        private const float Mass = 1.0f;
        private const float RotationRate = 1.5f;
        public RenderContext renderContext { get; set; }

        public override void Initialize()
        {
            char_Model = new GameAnimatedModel(modelPath);
            char_Model.Scale(1f, 1f, 1f);
            AddChild(char_Model);

            //Initialize Movement Parameters
            _runAcceleration = RUN_SPEED / RUN_ACCELERATION_TIME;
            Rotate(LocalRotation.X, LocalRotation.Y, LocalRotation.Z);

            charInput = new CharacterControllerInput(Scene.Space, this);
            charInput.Activate();
            #region ADD INPUTACTIONS
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


            inputAction = new InputAction((int)InputActionIds.MoveForWardXButton, VirtualButtonState.Pressed)
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
            #endregion
            
            base.Initialize();
        }

        public override void LoadContent(ContentManager contentManager)
        {
            base.LoadContent(contentManager);

            char_Model.PlayAnimation("Idle", false, RUN_ACCELERATION_TIME);
        }

        public override void Update(RenderContext renderContext)
        {
            float dt = renderContext.GameTime.TotalGameTime.Seconds;
            //Temporary set IsGrounded to True.
            if (renderContext.Input.screenPad.LeftStick.Y > 0.23 || renderContext.Input.screenPad.LeftStick.Y < -0.23)
                IsGrounded = true;
            else if (renderContext.Input.screenPad.LeftStick.Y == 0)
                IsGrounded = false;// true;

            #region Player input
          
            if (IsGrounded)
                char_Model.PlayAnimation("Run", true, RUN_ACCELERATION_TIME);
            else
                char_Model.PlayAnimation("Idle", true, RUN_ACCELERATION_TIME);
           
            #endregion
            if (renderContext.Input.screenPad.LeftStick.X < -.70f || renderContext.Input.screenPad.LeftStick.X > .70f)
            {
                temp += renderContext.Input.screenPad.LeftStick.X * 5;
            }
            
            ///Translate(newPosition);
            charInput.Update(dt,renderContext);
            //charInput.Direction = Direction ;
            Rotate(0, -temp, 0);
            Translate(charInput.CharacterController.Body.Position);
            base.Update(renderContext);
        }

        public Vector3 DirectionToTravel(bool rotationVecIsInRadians, Vector3 rotationVec)//rotation vec must not be normalized at this point
        {
            Vector3 result;

            if (!rotationVecIsInRadians)
            {
                rotationVec *= MathHelper.Pi / 180f;
            }

            float angle = rotationVec.Length();
            rotationVec /= angle; //normalizes rotation vec

            result = Matrix.CreateFromAxisAngle(rotationVec, angle).Forward;

            return result;
        }
       
        public void Reset()
        {
            Direction = Vector3.Forward;
            Up = charInput.CharacterController.Body.OrientationMatrix.Up;
            right = Vector3.Right;
            Velocity = Vector3.Zero;
        }
      

    }
}
