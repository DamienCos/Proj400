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
        //public ChaseCamera Camera;
        public Character player;
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
        public CharacterControllerInput(Space owningSpace, Character p)
        {
            CharacterController = new CharacterController();

            Space = owningSpace;
            Space.Add(CharacterController);

            player = p;
            //Camera = cameraToUse;
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


        /// <summary>
        /// Handles the input and movement of the character.
        /// </summary>
        /// <param name="dt">Time since last frame in simulation seconds.</param>
        /// <param name="previousKeyboardInput">The last frame's keyboard state.</param>
        /// <param name="keyboardInput">The current frame's keyboard state.</param>
        /// <param name="previousGamePadInput">The last frame's gamepad state.</param>
        /// <param name="gamePadInput">The current frame's keyboard state.</param>
        public void Update(float dt)
        {
            if (IsActive)
            {

                #region unused
                //if (UseCameraSmoothing)
                //{
                //    //First, find where the camera is expected to be based on the last position and the current velocity.
                //    //Note: if the character were a free-floating 6DOF character, this would need to include an angular velocity contribution.
                //    //And of course, the camera orientation would be based on the character's orientation.
                //    Camera.LocalPosition = Camera.LocalPosition + CharacterController.Body.LinearVelocity * dt;
                //    //Now compute where it should be according the physical body of the character.
                //    Vector3 up = CharacterController.Body.OrientationMatrix.Up;
                //    Vector3 bodyPosition = CharacterController.Body.BufferedStates.InterpolatedStates.Position;
                //    Vector3 goalPosition = bodyPosition + up * (CharacterController.StanceManager.CurrentStance == Stance.Standing ? StandingCameraOffset : CrouchingCameraOffset);

                //    //Usually, the camera position and the goal will be very close, if not matching completely.
                //    //However, if the character steps or has its position otherwise modified, then they will not match.
                //    //In this case, we need to correct the camera position.

                //    //To do this, first note that we can't correct infinite errors.  We need to define a bounding region that is relative to the character
                //    //in which the camera can interpolate around.  The most common discontinuous motions are those of upstepping and downstepping.
                //    //In downstepping, the character can teleport up to the character's MaximumStepHeight downwards.
                //    //In upstepping, the character can teleport up to the character's MaximumStepHeight upwards, and the body's CollisionMargin horizontally.
                //    //Picking those as bounds creates a constraining cylinder.

                //    Vector3 error = goalPosition - Camera.LocalPosition;
                //    float verticalError = Vector3.Dot(error, up);
                //    Vector3 horizontalError = error - verticalError * up;
                //    //Clamp the vertical component of the camera position within the bounding cylinder.
                //    if (verticalError > CharacterController.StepManager.MaximumStepHeight)
                //    {
                //        Camera.LocalPosition -= up * (CharacterController.StepManager.MaximumStepHeight - verticalError);
                //        verticalError = CharacterController.StepManager.MaximumStepHeight;
                //    }
                //    else if (verticalError < -CharacterController.StepManager.MaximumStepHeight)
                //    {
                //        Camera.LocalPosition -= up * (-CharacterController.StepManager.MaximumStepHeight - verticalError);
                //        verticalError = -CharacterController.StepManager.MaximumStepHeight;
                //    }
                //    //Clamp the horizontal distance too.
                //    float horizontalErrorLength = horizontalError.LengthSquared();
                //    float margin = CharacterController.Body.CollisionInformation.Shape.CollisionMargin;
                //    if (horizontalErrorLength > margin * margin)
                //    {
                //        Vector3 previousHorizontalError = horizontalError;
                //        Vector3.Multiply(ref horizontalError, margin / (float)Math.Sqrt(horizontalErrorLength), out horizontalError);
                //        Camera.LocalPosition -= horizontalError - previousHorizontalError;
                //    }
                //    //Now that the error/camera position is known to lie within the constraining cylinder, we can perform a smooth correction.

                //    //This removes a portion of the error each frame.
                //    //Note that this is not framerate independent.  If fixed time step is not enabled,
                //    //a different smoothing method should be applied to the final error values.
                //    //float errorCorrectionFactor = .3f;

                //    //This version is framerate independent, although it is more expensive.
                //    float errorCorrectionFactor = (float)(1 - Math.Pow(.000000001, dt));
                //    Camera.LocalPosition += up * (verticalError * errorCorrectionFactor);
                //    Camera.LocalPosition += horizontalError * errorCorrectionFactor;


                //} 
                #endregion
               // else
              //  {
                player.LocalPosition = CharacterController.Body.Position + (CharacterController.StanceManager.CurrentStance == Stance.Standing ? StandingCameraOffset : CrouchingCameraOffset) * CharacterController.Body.OrientationMatrix.Up;
               // }

                Vector2 totalMovement = Vector2.Zero;

#if XBOX360
                Vector3 forward = Camera.WorldMatrix.Forward;
                forward.Y = 0;
                forward.Normalize();
                Vector3 right = Camera.WorldMatrix.Right;
                totalMovement += gamePadInput.ThumbSticks.Left.Y * new Vector2(forward.X, forward.Z);
                totalMovement += gamePadInput.ThumbSticks.Left.X * new Vector2(right.X, right.Z);
                CharacterController.HorizontalMotionConstraint.MovementDirection = Vector2.Normalize(totalMovement);
                
                CharacterController.StanceManager.DesiredStance = gamePadInput.IsButtonDown(Buttons.RightStick) ? Stance.Crouching : Stance.Standing;

                //Jumping
                if (previousGamePadInput.IsButtonUp(Buttons.LeftStick) && gamePadInput.IsButtonDown(Buttons.LeftStick))
                {
                    CharacterController.Jump();
                }
#else

                //Collect the movement impulses.

                Vector3 movementDir ;

                totalMovement += Vector2.Normalize(new Vector2(player.LocalPosition.X, player.LocalPosition.Z));
                //if (keyboardInput.IsKeyDown(Keys.E))
                //{
                //    movementDir = Camera.WorldMatrix.Forward;
                //    totalMovement += Vector2.Normalize(new Vector2(movementDir.X, movementDir.Z));
                //}
                //if (keyboardInput.IsKeyDown(Keys.D))
                //{
                //    movementDir = Camera.WorldMatrix.Forward;
                //    totalMovement -= Vector2.Normalize(new Vector2(movementDir.X, movementDir.Z));
                //}
                //if (keyboardInput.IsKeyDown(Keys.S))
                //{
                //    movementDir = Camera.WorldMatrix.Left;
                //    totalMovement += Vector2.Normalize(new Vector2(movementDir.X, movementDir.Z));
                //}
                //if (keyboardInput.IsKeyDown(Keys.F))
                //{
                //    movementDir = Camera.WorldMatrix.Right;
                //    totalMovement += Vector2.Normalize(new Vector2(movementDir.X, movementDir.Z));
                //}
                if (totalMovement == Vector2.Zero)
                    CharacterController.HorizontalMotionConstraint.MovementDirection = Vector2.Zero;
                else
                    CharacterController.HorizontalMotionConstraint.MovementDirection = Vector2.Normalize(totalMovement);

                //CharacterController.StanceManager.DesiredStance = keyboardInput.IsKeyDown(Keys.Z) ? Stance.Crouching : Stance.Standing;

                ////Jumping
                //if (previousKeyboardInput.IsKeyUp(Keys.A) && keyboardInput.IsKeyDown(Keys.A))
                //{
                //    CharacterController.Jump();
                //}
#endif

            }
        }
    }
}