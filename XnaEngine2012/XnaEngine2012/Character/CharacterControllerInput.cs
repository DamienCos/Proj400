using BEPUphysics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System;
using System.Diagnostics;
using BEPUphysics.BroadPhaseEntries;

namespace Blocker
{
     //<summary>
     //Handles input and movement of a character in the game.
     //Acts as a simple 'front end' for the bookkeeping and math of the character controller.
     //</summary>
    public class CharacterControllerInput
    {
        /// <summary>
        /// Camera to use for input.
        /// </summary>
       // public Camera Camera;
        public ChaseCamera Camera;
        //public RenderContext renderContext { get; set; }
        public Character player;
        public Quaternion rotation;
        /// <summary>
        /// Full speed at which ship can rotate; measured in radians per second.
        /// </summary>
        private const int RUN_SPEED = 170;
        private const float RUN_ACCELERATION_TIME = 0.2f;
        private float _runAcceleration;


        private Vector2 _velocity;
        private int _direction;
        float temp = 0;

        public Vector3 Rotatation;

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


        /// <summary>
        /// Offset from the position of the character to the 'eyes' while the character is standing.
        /// </summary>
        public float StandingCameraOffset = .7f;

        /// <summary>
        /// Offset from the position of the character to the 'eyes' while the character is crouching.
        /// </summary>
        public float CrouchingCameraOffset = .4f;

        /// <summary>
        /// Physics representation of the character.
        /// </summary>
        public CharacterController CharacterController;

        /// <summary>
        /// Whether or not to use the character controller's input.
        /// </summary>
        public bool IsActive = true;

        /// <summary>
        /// Whether or not to smooth out character steps and other discontinuous motion.
        /// </summary>
        public bool UseCameraSmoothing = true;

        /// <summary>
        /// Owning space of the character.
        /// </summary>
        public Space Space { get; private set; }

        /// <summary>
        /// Constructs the character and internal physics character controller.
        /// </summary>
        /// <param name="owningSpace">Space to add the character to.</param>
        /// <param name="cameraToUse">Camera to attach to the character.</param>
        //public CharacterControllerInput(Space owningSpace, Character p,RenderContext renderC)
        //{
        //    CharacterController = new CharacterController();
        //    _runAcceleration = RUN_SPEED / RUN_ACCELERATION_TIME;
        //    Space = owningSpace;
        //    Space.Add(CharacterController);
        //    renderContext = renderC;
        //    player = p;
        //    Deactivate();
        //}


        /// <summary>
        /// Constructs the character and internal physics character controller.
        /// </summary>
        /// <param name="owningSpace">Space to add the character to.</param>
        /// <param name="cameraToUse">Camera to attach to the character.</param>
        public CharacterControllerInput(Space owningSpace, Character p, ChaseCamera cam)
        {
            CharacterController = new CharacterController();

            Space = owningSpace;
            Space.Add(CharacterController);

            player = p;
            Camera = cam;
            Deactivate();
        }

        public CharacterControllerInput(Space owningSpace, Character p)
        {
            CharacterController = new CharacterController();

            Space = owningSpace;
            Space.Add(CharacterController);
            _runAcceleration = RUN_SPEED / RUN_ACCELERATION_TIME;
            player = p;
            Deactivate();
        }

        /// <summary>
        /// Gives the character control over the Camera and movement input.
        /// </summary>
        public void Activate()
        {
            if (!IsActive)
            {
                IsActive = true;
                //Camera.UseMovementControls = false;
                Space.Add(CharacterController);
                CharacterController.Body.Position = (player.LocalPosition - new Vector3(0, StandingCameraOffset, 0));
            }
        }

        /// <summary>
        /// Returns input control to the Camera.
        /// </summary>
        public void Deactivate()
        {
            if (IsActive)
            {
                IsActive = false;
                //Camera.UseMovementControls = true;
                Space.Remove(CharacterController);
            }
        }

        public Quaternion RotateC(float pitch, float yaw, float roll)
        {
            return rotation= Quaternion.CreateFromYawPitchRoll(MathHelper.ToRadians(yaw), MathHelper.ToRadians(pitch), MathHelper.ToRadians(roll));
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


        /// <summary>
        /// Handles the input and movement of the character.
        /// </summary>
        /// <param name="dt">Time since last frame in simulation seconds.</param>
        /// <param name="previousKeyboardInput">The last frame's keyboard state.</param>
        /// <param name="keyboardInput">The current frame's keyboard state.</param>
        /// <param name="previousGamePadInput">The last frame's gamepad state.</param>
        /// <param name="gamePadInput">The current frame's keyboard state.</param>
        public void Update(float dt, RenderContext renderContext)
        {
            if (IsActive)
            {
                #region Player input
                Vector2 rotationAmount = new Vector2(0, renderContext.Input.screenPad.LeftStick.Y);
                //Handle RUN_LEFT
                if (renderContext.Input.IsActionTriggered((int)InputActionIds.Rotate))
                {
                    _direction = -1;
                    RotateC(0, -0, 0);

                    temp += renderContext.Input.screenPad.LeftStick.X * 8;
                    _velocity.X += _runAcceleration * (renderContext.Input.screenPad.LeftStick.Y * 2) * (float)renderContext.GameTime.ElapsedGameTime.TotalSeconds;
                    RotateC(0, -temp, 0);
                    Direction = DirectionToTravel(false, new Vector3(0, -temp, 0));

                    #region MyRegion

                    Up = CharacterController.Body.OrientationMatrix.Up;// new Vector3(0, 1, 0);

                    // Re-normalize orientation vectors ,without this, the matrix transformations may introduce small rounding
                    // errors which add up over time and could destabilize the ship.
                    //if (Direction != Vector3.Zero)
                    //    Direction.Normalize();
                    //if (Up != Vector3.Zero)
                    //    Up.Normalize();

                    //// Re-calculate Right
                    //right = Vector3.Cross(Direction, Up);
                    //// The same instability may cause the 3 orientation vectors may also diverge. Either the Up or Direction vector needs to be
                    //// re-computed with a cross product to ensure orthagonality
                    //Up = Vector3.Cross(Right, Direction);
                    //Rotatation = new Vector3(0, -temp, 0);
                    
                    #endregion
                }
                if (renderContext.Input.screenPad.LeftStick.Y == 0)
                    _velocity.X = 0;

                //Clamp Velocity X
                _velocity.X = MathHelper.Clamp(_velocity.X, -RUN_SPEED, RUN_SPEED);

                //Calculate new position, based on the current velocity
                var totalMovement = _velocity * (float)renderContext.GameTime.ElapsedGameTime.TotalSeconds;
                var newPosition = CharacterController.Body.Position + new Vector3(totalMovement, 0);// +(CharacterController.StanceManager.CurrentStance == Stance.Standing ? StandingCameraOffset : CrouchingCameraOffset) * CharacterController.Body.OrientationMatrix.Up;

               // player.LocalRotation = Rotate(0, -temp, 0); 
                #endregion


                //Camera.LocalPosition = CharacterController.Body.Position + (CharacterController.StanceManager.CurrentStance == Stance.Standing ? StandingCameraOffset : CrouchingCameraOffset) * CharacterController.Body.OrientationMatrix.Up;
                CharacterController.Body.Position = newPosition;
            }
        }
    }
}