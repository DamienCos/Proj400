//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using Microsoft.Xna.Framework;
//using BEPUphysics.Entities;
//using BEPUphysics.BroadPhaseEntries;
//using BEPUphysics.CollisionRuleManagement;
//using BEPUphysics.MathExtensions;
//using BEPUphysics;

//namespace Blocker
//{
//    public class ChaseCamera : GameObject3D
//    {

//        #region Chased object properties (set externally each frame)

//        public Matrix View { get; protected set; }
//        public Matrix Projection { get; protected set; }

//        /// <summary>
//        /// Position of object being chased.
//        /// </summary>
//        public Vector3 ChasePosition
//        {
//            get { return chasePosition; }
//            set { chasePosition = value; }
//        }
//        private Vector3 chasePosition;

//        /// <summary>
//        /// Direction the chased object is facing.
//        /// </summary>
//        public Vector3 ChaseDirection
//        {
//            get { return chaseDirection; }
//            set { chaseDirection = value; }
//        }
//        private Vector3 chaseDirection;

//        /// <summary>
//        /// Chased object's Up vector.
//        /// </summary>
//        public Vector3 Up
//        {
//            get { return up; }
//            set { up = value; }
//        }
//        private Vector3 up = Vector3.Up;

//        #endregion

//        #region Desired camera positioning (set when creating camera or changing view)

//        /// <summary>
//        /// Desired camera position in the chased object's coordinate system.
//        /// </summary>
//        public Vector3 DesiredPositionOffset
//        {
//            get { return desiredPositionOffset; }
//            set { desiredPositionOffset = value; }
//        }
//        private Vector3 desiredPositionOffset = new Vector3(0, 2.0f, 2.0f);

//        /// <summary>
//        /// Desired camera position in world space.
//        /// </summary>
//        public Vector3 DesiredPosition
//        {
//            get
//            {
//                // Ensure correct value even if update has not been called this frame
//                UpdateWorldPositions();
//                return desiredPosition;
//            }
//        }
//        private Vector3 desiredPosition;

//        /// <summary>
//        /// Look at point in the chased object's coordinate system.
//        /// </summary>
//        public Vector3 LookAtOffset
//        {
//            get { return lookAtOffset; }
//            set { lookAtOffset = value; }
//        }
//        private Vector3 lookAtOffset = new Vector3(0, 2.8f, 0);

//        /// <summary>
//        /// Look at point in world space.
//        /// </summary>
//        public Vector3 LookAt
//        {
//            get
//            {
//                // Ensure correct value even if update has not been called this frame
//                UpdateWorldPositions();
//                return lookAt;
//            }
//        }
//        private Vector3 lookAt;

//        #endregion

//        #region Camera physics (typically set when creating camera)

//        /// <summary>
//        /// Physics coefficient which controls the influence of the camera's position
//        /// over the spring force. The stiffer the spring, the closer it will stay to
//        /// the chased object.
//        /// </summary>
//        public float Stiffness
//        {
//            get { return stiffness; }
//            set { stiffness = value; }
//        }
//        private float stiffness = 1800.0f;

//        /// <summary>
//        /// Physics coefficient which approximates internal friction of the spring.
//        /// Sufficient damping will prevent the spring from oscillating infinitely.
//        /// </summary>
//        public float Damping
//        {
//            get { return damping; }
//            set { damping = value; }
//        }
//        private float damping = 600.0f;

//        /// <summary>
//        /// Mass of the camera body. Heaver objects require stiffer springs with less
//        /// damping to move at the same rate as lighter objects.
//        /// </summary>
//        public float Mass
//        {
//            get { return mass; }
//            set { mass = value; }
//        }
//        private float mass = 50.0f;

//        #endregion

//        #region Current camera properties (updated by camera physics)

      
//        /// <summary>
//        /// Velocity of camera.
//        /// </summary>
//        public Vector3 Velocity
//        {
//            get { return velocity; }
//        }
//        private Vector3 velocity;

//        #endregion


//        #region Perspective properties

//        /// <summary>
//        /// Perspective aspect ratio. Default value should be overriden by application.
//        /// </summary>
//        public float AspectRatio
//        {
//            get { return aspectRatio; }
//            set { aspectRatio = value; }
//        }
//        private float aspectRatio = 1.6f;

//        /// <summary>
//        /// Perspective field of view.
//        /// </summary>
//        public float FieldOfView
//        {
//            get { return fieldOfView; }
//            set { fieldOfView = value; }
//        }
//        private float fieldOfView = MathHelper.ToRadians(45.0f);

//        /// <summary>
//        /// Distance to the near clipping plane.
//        /// </summary>
//        public float NearPlaneDistance
//        {
//            get { return nearPlaneDistance; }
//            set { nearPlaneDistance = value; }
//        }
//        private float nearPlaneDistance = 1.0f;

//        /// <summary>
//        /// Distance to the far clipping plane.
//        /// </summary>
//        public float FarPlaneDistance
//        {
//            get { return farPlaneDistance; }
//            set { farPlaneDistance = value; }
//        }
//        private float farPlaneDistance = 10000.0f;

//        #endregion

//        #region Matrix properties

//        ///// <summary>
//        ///// View transform matrix.
//        ///// </summary>
//        //public Matrix View
//        //{
//        //    get { return view; }
//        //}
//        //private Matrix view;

//        ///// <summary>
//        ///// Projecton transform matrix.
//        ///// </summary>
//        //public Matrix Projection
//        //{
//        //    get { return projection; }
//        //}
//        //private Matrix projection;

//        #endregion

//        public ChaseCamera()
//        {
//            Projection = Matrix.CreatePerspectiveFieldOfView(MathHelper.PiOver4, 1.6f, 1f, 10000);
//        }


//        #region Methods

//        /// <summary>
//        /// Rebuilds object space values in world space. Invoke before publicly
//        /// returning or privately accessing world space values.
//        /// </summary>
//        private void UpdateWorldPositions()
//        {
//            // Construct a matrix to transform from object space to worldspace
//            Matrix transform = Matrix.Identity;
//            transform.Forward = ChaseDirection;
//            transform.Up = Up;
//            transform.Right = Vector3.Cross(Up, ChaseDirection);

//            // Calculate desired camera properties in world space
//            desiredPosition = ChasePosition +
//                Vector3.TransformNormal(DesiredPositionOffset, transform);
//            lookAt = ChasePosition +
//                Vector3.TransformNormal(LookAtOffset, transform);
//        }

//        /// <summary>
//        /// Rebuilds camera's view and projection matricies.
//        /// </summary>
//        public void BuildViewMatrix()
//        {
//            View = Matrix.CreateLookAt(this.LocalPosition, this.LookAt, this.Up);
//            //Projection = Matrix.CreatePerspectiveFieldOfView(FieldOfView,
//            //    AspectRatio, NearPlaneDistance, FarPlaneDistance);
//        }

//        /// <summary>
//        /// Forces camera to be at desired position and to stop moving. The is useful
//        /// when the chased object is first created or after it has been teleported.
//        /// Failing to call this after a large change to the chased object's position
//        /// will result in the camera quickly flying across the world.
//        /// </summary>
//        public void Reset()
//        {            
//            // Stop motion
//            velocity = Vector3.Zero;

//            // Force desired position
//            LocalPosition = desiredPosition;

//            UpdateWorldPositions();
//            BuildViewMatrix();
//        }

//        /// <summary>
//        /// Animates the camera from its current position towards the desired offset
//        /// behind the chased object. The camera's animation is controlled by a simple
//        /// physical spring attached to the camera and anchored to the desired position.
//        /// </summary>
//        public override void Update(RenderContext renderContext)
//        {
//            if (renderContext.GameTime == null)
//                throw new ArgumentNullException("gameTime");
          
//            float elapsed = (float)renderContext.GameTime.ElapsedGameTime.TotalSeconds;

//            // Calculate spring force
//            Vector3 stretch = LocalPosition - desiredPosition;
//            Vector3 force = -stiffness * stretch - damping * velocity;

//            // Apply acceleration
//            Vector3 acceleration = force / mass;
//            velocity += acceleration * elapsed;

//            // Apply velocity
//            LocalPosition += velocity * elapsed;

//            BuildViewMatrix();
//            UpdateWorldPositions();
//        }

//        #endregion

       
//    }
//}
