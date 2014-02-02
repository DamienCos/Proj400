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
        /// <summary>
        /// Full speed at which ship can rotate; measured in radians per second.
        /// </summary>
        private const float RotationRate = 1.5f;

        private float _runAcceleration;
        private Vector2 _velocity;
        private int _direction;
        float temp = 0;
        /// <summary>
        /// Direction player is facing.
        /// </summary>
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

            Vector2 rotationAmount = new Vector2(0, renderContext.Input.screenPad.LeftStick.Y);

       

            #region Player input
            
            //Handle RUN_LEFT
            if (renderContext.Input.IsActionTriggered((int)InputActionIds.Rotate))
            {

                if (IsGrounded)
                    char_Model.PlayAnimation("Run", true, RUN_ACCELERATION_TIME);
                else
                    char_Model.PlayAnimation("Idle", true, RUN_ACCELERATION_TIME);

                _direction = -1;
                Rotate(0, -0, 0);

                temp += renderContext.Input.screenPad.LeftStick.X * 8;
                _velocity.X += _runAcceleration * (renderContext.Input.screenPad.LeftStick.Y * 2) * (float)renderContext.GameTime.ElapsedGameTime.TotalSeconds;
                Rotate(0, -temp, 0);
                Direction = DirectionToTravel(false, new Vector3(0, -temp, 0));

                #region MyRegion
              
                Up =new Vector3(0,1,0);

                // Re-normalize orientation vectors ,without this, the matrix transformations may introduce small rounding
                // errors which add up over time and could destabilize the ship.
                if (Direction != Vector3.Zero)
                    Direction.Normalize();
                if (Up != Vector3.Zero)
                    Up.Normalize();

                // Re-calculate Right
                right = Vector3.Cross(Direction, Up);

                // The same instability may cause the 3 orientation vectors may also diverge. Either the Up or Direction vector needs to be
                // re-computed with a cross product to ensure orthagonality
                Up = Vector3.Cross(Right, Direction); 
                #endregion
            }
            if (renderContext.Input.screenPad.LeftStick.Y == 0)
                _velocity.X = 0;
         
            //Clamp Velocity X
            _velocity.X = MathHelper.Clamp(_velocity.X, -RUN_SPEED, RUN_SPEED);

            //Handle JUMPING
            //... 
            #endregion

            //Calculate new position, based on the current velocity
            var totalMovement = _velocity * (float)renderContext.GameTime.ElapsedGameTime.TotalSeconds;
            var newPosition = LocalPosition + new Vector3(totalMovement, 0);
            //if (newPosition.X > 240)
            //    newPosition.X = 240;
            //if (newPosition.X < -240)
            //    newPosition.X = -240;
            Translate(newPosition);
            charInput.Update(dt);
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

        public void Reset(Vector3 position)
        {
            Translate(position);
            _velocity = Vector2.Zero;
        }

    }
}
