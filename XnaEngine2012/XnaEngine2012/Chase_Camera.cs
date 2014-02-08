//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using Microsoft.Xna.Framework;
//using Microsoft.Xna.Framework.Input;

//namespace Blocker
//{
//    public class ChaseCamera
//    {
//        public Vector3 LocalPosition;
//        private Vector3 target;
//        public Matrix View, Projection;
//        private float yaw, pitch, roll;
//        private float speed;
//        private Matrix cameraRotation;
//        private Vector3 desiredPosition;
//        private Vector3 desiredTarget;
//        private Vector3 offsetDistance;

//        public ChaseCamera()
//        {
//            ResetCamera();
//        }

//        public void ResetCamera()
//        {
//            desiredPosition = LocalPosition;
//            desiredTarget = target;
//            offsetDistance = new Vector3(0.0f, 200.0f, -400.0f);
//        }

//        public void Update(Matrix chasedObjectsWorld)
//        {
//            UpdateViewMatrix(chasedObjectsWorld);
//        }
 
//        private void UpdateViewMatrix(Matrix chasedObjectsWorld)
//        {
//            cameraRotation.Forward.Normalize();
//            chasedObjectsWorld.Right.Normalize();
//            chasedObjectsWorld.Up.Normalize();

//            cameraRotation = Matrix.CreateFromAxisAngle(cameraRotation.Forward, roll);

//            desiredTarget = chasedObjectsWorld.Translation;
//            target = desiredTarget;
//            target += chasedObjectsWorld.Right * yaw;
//            target += chasedObjectsWorld.Up * pitch;

//            desiredPosition = Vector3.Transform(offsetDistance, chasedObjectsWorld);
//            LocalPosition = Vector3.SmoothStep(LocalPosition, desiredPosition, .15f);

//            yaw = MathHelper.SmoothStep(yaw, 0f, .1f);
//            pitch = MathHelper.SmoothStep(pitch, 0f, .1f);
//            roll = MathHelper.SmoothStep(roll, 0f, .2f);

//        }

//        private void HandleInput()
//        {
//            KeyboardState keyboardState = Keyboard.GetState();

//            if (keyboardState.IsKeyDown(Keys.J))
//            {
//                yaw += .02f;
//            }
//            if (keyboardState.IsKeyDown(Keys.L))
//            {
//                yaw += -.02f;
//            }
//            if (keyboardState.IsKeyDown(Keys.I))
//            {
//                pitch += -.02f;
//            }
//            if (keyboardState.IsKeyDown(Keys.K))
//            {
//                pitch += .02f;
//            }
//            if (keyboardState.IsKeyDown(Keys.U))
//            {
//                roll += -.02f;
//            }
//            if (keyboardState.IsKeyDown(Keys.O))
//            {
//                roll += .02f;
//            }

//            if (keyboardState.IsKeyDown(Keys.W))
//            {
//                MoveCamera(cameraRotation.Forward);
//            }
//            if (keyboardState.IsKeyDown(Keys.S))
//            {
//                MoveCamera(-cameraRotation.Forward);
//            }
//            if (keyboardState.IsKeyDown(Keys.A))
//            {
//                MoveCamera(-cameraRotation.Right);
//            }
//            if (keyboardState.IsKeyDown(Keys.D))
//            {
//                MoveCamera(cameraRotation.Right);
//            }
//            if (keyboardState.IsKeyDown(Keys.E))
//            {
//                MoveCamera(cameraRotation.Up);
//            }
//            if (keyboardState.IsKeyDown(Keys.Q))
//            {
//                MoveCamera(-cameraRotation.Up);
//            }
//        }

//        private void UpdateViewMatrix()
//        {
//            cameraRotation.Forward.Normalize();
//            cameraRotation.Up.Normalize();
//            cameraRotation.Right.Normalize();

//            cameraRotation *= Matrix.CreateFromAxisAngle(cameraRotation.Right, pitch);
//            cameraRotation *= Matrix.CreateFromAxisAngle(cameraRotation.Up, yaw);
//            cameraRotation *= Matrix.CreateFromAxisAngle(cameraRotation.Forward, roll);

//            yaw = 0.0f;
//            pitch = 0.0f;
//            roll = 0.0f;

//            target = LocalPosition + cameraRotation.Forward;

//            View = Matrix.CreateLookAt(LocalPosition, target, cameraRotation.Up);

//        }

//        private void MoveCamera(Vector3 addedVector)
//        {
//            LocalPosition += speed * addedVector;
//        }
//    }
//}
