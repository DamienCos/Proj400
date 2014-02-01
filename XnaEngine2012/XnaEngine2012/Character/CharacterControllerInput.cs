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
                //Camera.LocalPosition = CharacterController.Body.Position + (CharacterController.StanceManager.CurrentStance == Stance.Standing ? StandingCameraOffset : CrouchingCameraOffset) * CharacterController.Body.OrientationMatrix.Up;
                CharacterController.Body.Position  = player.LocalPosition ;
            }
        }
    }
}