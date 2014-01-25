using BEPUutilities;
using Microsoft.Xna.Framework.Input;

namespace AndroidTest
{
    public class Camera2 :GameObject3D
    {
        /// <summary>
        /// Gets or sets the position of the camera.
        /// </summary>
        public Vector3 Position { get; set; }
        float yaw;
        float pitch,dt;
        /// <summary>
        /// Gets or sets the yaw rotation of the camera.
        /// </summary>
        public float Yaw
        {
            get
            {
                return yaw;
            }
            set
            {
                yaw = BEPUutilities.MathHelper.WrapAngle(value);
            }
        }
        /// <summary>
        /// Gets or sets the pitch rotation of the camera.
        /// </summary>
        public float Pitch
        {
            get
            {
                return pitch;
            }
            set
            {
                pitch = MathHelper.Clamp(value, -MathHelper.PiOver2, MathHelper.PiOver2);
            }
        }

        /// <summary>
        /// Gets or sets the speed at which the camera moves.
        /// </summary>
        public float Speed { get; set; }

        ///// <summary>
        ///// Gets the view matrix of the camera.
        ///// </summary>
        public Matrix View { get; private set; }
        ///// <summary>
        ///// Gets or sets the projection matrix of the camera.
        ///// </summary>
        public Matrix Projection { get; set; }

        /// <summary>
        /// Gets the world transformation of the camera.
        /// </summary>
        public Matrix WorldMatrix { get; private set; }

        /// <summary>
        /// Gets the game owning the camera.
        /// </summary>
        public Game1 Game { get; private set; }

        public InputManager Input { get; private set; }

        /// <summary>
        /// Constructs a new camera.
        /// </summary>
        /// <param name="game">Game that this camera belongs to.</param>
        /// <param name="position">Initial position of the camera.</param>
        /// <param name="speed">Initial movement speed of the camera.</param>
        public Camera2( Vector3 position, float speed, InputManager input)
        {
            Position = position;
            Speed = speed;
            Projection = Matrix.CreatePerspectiveFieldOfViewRH(MathHelper.PiOver4, 4f / 3f, .1f, 10000.0f);
            Input = input;
        }

        /// <summary>
        /// Moves the camera forward using its speed.
        /// </summary>
        /// <param name="dt">Timestep duration.</param>
        public void MoveForward(float dt)
        {
            Position += WorldMatrix.Forward * (dt * Speed);
        }
        /// <summary>
        /// Moves the camera right using its speed.
        /// </summary>
        /// <param name="dt">Timestep duration.</param>
        /// 
        public void MoveRight(float dt)
        {
            Position += WorldMatrix.Right * (dt * Speed);
        }
        /// <summary>
        /// Moves the camera up using its speed.
        /// </summary>
        /// <param name="dt">Timestep duration.</param>
        /// 
        public void MoveUp(float dt)
        {
            Position += new Vector3(0, (dt * Speed), 0);
        }

        /// <summary>
        /// Updates the camera's view matrix.
        /// </summary>
        /// <param name="dt">Timestep duration.</param>
        public  void Update(RenderContext renderContext)
        {
            dt = ((float)renderContext.GameTime.ElapsedGameTime.TotalSeconds);
            //Turn based on gamepad input.
            Yaw += Input.CurrentScreenPadState.ThumbSticks.Left.X * -1.5f * dt;
            Pitch += Input.CurrentScreenPadState.ThumbSticks.Left.Y * 1.5f * dt;

            float distance = Speed * dt;
            //Move based on gamepad input.
            if (Input.CurrentScreenPadState.Buttons.X == VirtualButtonState.Pressed)
                MoveForward(distance/10);
            if (Input.CurrentScreenPadState.Buttons.Y == VirtualButtonState.Pressed)
                MoveUp(distance/10);

            BuildViewMatrix();
            base.Update(renderContext);
        }

        public  void BuildViewMatrix()
        {
            WorldMatrix = Matrix.CreateFromAxisAngle(Vector3.Right, Pitch) * Matrix.CreateFromAxisAngle(Vector3.Up, Yaw);
            WorldMatrix = WorldMatrix * Matrix.CreateTranslation(Position);
            View = Matrix.Invert(WorldMatrix);
        }
        
    }
}
