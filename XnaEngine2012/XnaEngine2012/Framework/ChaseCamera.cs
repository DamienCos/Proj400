using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using BEPUphysics.Entities;
using BEPUphysics.BroadPhaseEntries;
using BEPUphysics.CollisionRuleManagement;
using BEPUphysics.MathExtensions;
using BEPUphysics;

namespace AndroidTest
{
    public class ChaseCamera: BaseCamera
    {
        #region Chase Camera Mode

        private float yaw;
        private float pitch;

        // Gets or sets the yaw rotation of the camera.
        public float Yaw
        {
            get { return yaw; }
            set { yaw = MathHelper.WrapAngle(value); }
        }

        // Gets or sets the pitch rotation of the camera.
        public float Pitch
        {
            get { return pitch; }
            set
            {
                pitch = value;
                if (pitch > MathHelper.PiOver2 * .99f)
                    pitch = MathHelper.PiOver2 * .99f;
                else if (pitch < -MathHelper.PiOver2 * .99f)
                    pitch = -MathHelper.PiOver2 * .99f;
            }
        }

        // Gets or sets the speed at which the camera moves.
        public float Speed { get; set; }

        // Entity to follow around and point at.
        private Entity entityToChase;

        // Offset vector from the center of the target chase entity to look at.
        private Vector3 offsetFromChaseTarget;

        // Whether or not to transform the offset vector with the rotation of the entity.
        private bool transformOffset;

        // Distance away from the target entity to try to maintain.  The distance will be shorter at times if the ray hits an object.
        private float distanceToTarget;

        // Whether or not the camera is currently in chase camera mode.
        //private bool isChaseCameraMode;
        public bool UseMovementControls = false;
        /// <summary>
        /// Sets up all the information required by the chase camera.
        /// </summary>
        /// <param name="target">Target to follow.</param>
        /// <param name="offset">Offset from the center of the entity target to point at.</param>
        /// <param name="transform">Whether or not to transform the offset with the target entity's rotation.</param>
        /// <param name="distance">Distance from the target position to try to maintain.</param>
        public void ActivateChaseCameraMode(Entity target, Vector3 offset, bool transform, float distance)
        {
            entityToChase = target;
            offsetFromChaseTarget = offset;
            transformOffset = transform;
            distanceToTarget = distance;
            //isChaseCameraMode = true;
        }

        // Disable the chase camera mode, returning it to first person perspective.
        public void DeactivateChaseCameraMode()
        {
            //isChaseCameraMode = false;
        }

        #endregion


        public override void BuildViewMatrix()
        {
            //Vector3 offset;
            Vector3 lookAt;
            //if (transformOffset)
            //    offset = Matrix3X3.Transform(offsetFromChaseTarget, entityToChase.BufferedStates.InterpolatedStates.OrientationMatrix);
            //else
            //    offset = offsetFromChaseTarget;
            //Vector3 lookAt = entityToChase.BufferedStates.InterpolatedStates.Position + offset;
            //Vector3 backwards = WorldMatrix.Backward;

            ////Find the earliest ray hit that isn't the chase target to position the camera appropriately.
            //RayCastResult result;
            //if (entityToChase.Space.RayCast(new Ray(lookAt, backwards), distanceToTarget, rayCastFilter, out result))
            //{
            //    LocalPosition = lookAt + (result.HitData.T) * backwards; //Put the camera just before any hit spot.
            //}
            //else
            //    LocalPosition = lookAt + (distanceToTarget) * backwards;






            lookAt = Vector3.Transform(Vector3.Forward, WorldRotation);
            lookAt.Normalize();
            View = Matrix.CreateLookAt(WorldPosition, (WorldPosition + lookAt), Vector3.Up);
        }


        public ChaseCamera()
        {
            Projection = Matrix.CreatePerspectiveFieldOfView(MathHelper.PiOver4, 1.6f, 1f, 10000);
            Speed = 0;
            Pitch = 0;
            Yaw = 0;
            rayCastFilter = RayCastFilter;
        }

        public override void Update(RenderContext renderContext)
        {
            float dt = renderContext.GameTime.ElapsedGameTime.Seconds;
            base.Update(renderContext);

            BuildViewMatrix();
        }

        // Moves the camera forward.
        public void MoveForward(float distance)
        {
            LocalPosition += WorldMatrix.Forward * distance;
        }

        // Moves the camera to the right.
        public void MoveRight(float distance)
        {
            LocalPosition += WorldMatrix.Right * distance;
        }

        // Moves the camera up.
        public void MoveUp(float distance)
        {
            LocalPosition += new Vector3(0, distance, 0);
        }

        Func<BroadPhaseEntry, bool> rayCastFilter;
        bool RayCastFilter(BroadPhaseEntry entry)
        {
            return entry != entityToChase.CollisionInformation && (entry.CollisionRules.Personal <= CollisionRule.Normal);
        }
        
    }
}
