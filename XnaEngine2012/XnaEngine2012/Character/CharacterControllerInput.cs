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

        public ChaseCamera Camera;
        public Character player;
        public Quaternion rotation;

        private const int RUN_SPEED = 170;
        private const float RUN_ACCELERATION_TIME = 0.2f;
        private float _runAcceleration;

        private Vector2 velocity = Vector2.Zero;
        //private int _direction;
        //float temp = 0;
        //public Vector3 Velocity;
        //private const float DragFactor = 0.27f;
        //private const float ThrustForce = 1.0f;
        //private const float Mass = 1.0f;
        //private const float RotationRate = 1.5f;
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


        #region MyRegion
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
        
        #endregion
      
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
                Space.Remove(CharacterController);
            }
        }

        //public Quaternion RotateC(float pitch, float yaw, float roll)
        //{
        //    return rotation= Quaternion.CreateFromYawPitchRoll(MathHelper.ToRadians(yaw), MathHelper.ToRadians(pitch), MathHelper.ToRadians(roll));
        //}

        //public Vector3 DirectionToTravel(bool rotationVecIsInRadians, Vector3 rotationVec)//rotation vec must not be normalized at this point
        //{
        //    Vector3 result;

        //    if (!rotationVecIsInRadians)
        //    {
        //        rotationVec *= MathHelper.Pi / 180f;
        //    }

        //    float angle = rotationVec.Length();
        //    rotationVec /= angle; //normalizes rotation vec

        //    result = Matrix.CreateFromAxisAngle(rotationVec, angle).Forward;

        //    return result;
        //}

        //public void Reset()
        //{
        //    Direction = Vector3.Forward;
        //    Up = CharacterController.Body.OrientationMatrix.Up;
        //    right = Vector3.Right;
        //    Velocity = Vector3.Zero;
        //}

        float elapsed;
        public void Update(float dt, RenderContext renderContext)
        {
            if (IsActive)
            {
                elapsed += (float)renderContext.GameTime.ElapsedGameTime.TotalSeconds;
                #region Player input
                if (elapsed > 1)
                {
                    Vector2 movement = Vector2.Zero;
                    Vector3 forward = player.WorldMatrix.Forward;
                    forward.Y = 0;
                    forward.Normalize();
                    Vector3 right = player.WorldMatrix.Right;
                    movement += -renderContext.Input.screenPad.LeftStick.Y * new Vector2(forward.X, forward.Z);

                    if (renderContext.Input.screenPad.LeftStick.X < -.70f || renderContext.Input.screenPad.LeftStick.X > .70f)
                    {
                        movement += renderContext.Input.screenPad.LeftStick.X * new Vector2(right.X, right.Z);
                    }
                    //CharacterController.HorizontalMotionConstraint.MovementDirection = Vector2.Normalize(movement);

                    if (renderContext.Input.screenPad.LeftStick.Y < -.70f || renderContext.Input.screenPad.LeftStick.Y > .70f)
                    {
                        velocity.X += _runAcceleration * (renderContext.Input.screenPad.LeftStick.Y * 2) * (float)renderContext.GameTime.ElapsedGameTime.TotalSeconds;
                    }
                    else
                        velocity = Vector2.Zero;

                    var dir = Vector2.Normalize(movement);
                    //Clamp Velocity X
                    velocity.X = MathHelper.Clamp(velocity.X, -RUN_SPEED, RUN_SPEED);

                    float direction = Vector2ToRadian(movement);
                    var totalMovement = velocity * (float)renderContext.GameTime.ElapsedGameTime.TotalSeconds;
                    var tempMovement = movement;

                    var newPosition = CharacterController.Body.Position + new Vector3(tempMovement.X, 0, tempMovement.Y);

                    //var newPosition = CharacterController.Body.Position * direction + velocity;// new Vector3(totalMovement, 0) ;

                    CharacterController.Body.Position = newPosition;
                }
                #endregion
            }
        }

      

        public static float Vector2ToRadian(Vector2 direction)
        {
            return (float)Math.Atan2(direction.X, -direction.Y);
        }
    }
}