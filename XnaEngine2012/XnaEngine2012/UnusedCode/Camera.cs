using BEPUphysics;
using BEPUphysics.BroadPhaseEntries;
using BEPUphysics.BroadPhaseSystems;
using BEPUphysics.Entities;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using BEPUphysics.CollisionRuleManagement;
using BEPUphysics.MathExtensions;
using System;
using Microsoft.Xna.Framework.Graphics;

namespace Blocker
{
    /// <summary>
    /// Simple camera class 
    /// </summary>
    public class Camera : GameObject3D
    {
        private Matrix view { get;  set; }
        private Matrix projection { get;  set; }

        public Matrix Projection
        {
            get { return projection; }
            protected set
            {
                projection = value;
               // generateFrustum();
            }
        }

        public Matrix View
        {
            get { return view; }
            protected set
            {
                view = value;
                //generateFrustum();
            }
        }

        //public BoundingFrustum Frustum { get; private set; }

       // private void generateFrustum()
        //{
        //    Matrix viewProjection = View * Projection;
        //    Frustum = new BoundingFrustum(viewProjection);
        //}

        private void generatePerspectiveProjectionMatrix()
        {
            this.Projection = Matrix.CreatePerspectiveFieldOfView(
                MathHelper.PiOver4, 1.6f, 1f, 10000);
        }

        protected GraphicsDevice GraphicsDevice { get; set; }

        public Camera() 
        {
            generatePerspectiveProjectionMatrix();
        }

        public Camera(GraphicsDevice graphicsDevice)
        {
            this.GraphicsDevice = graphicsDevice;
            generatePerspectiveProjectionMatrix();
            //Projection = Matrix.CreatePerspectiveFieldOfView(MathHelper.PiOver4, 1.6f, 1f, 10000);//Matrix.CreateOrthographic(640, 360, 0.1f, 300);
        }

        public override void Update(RenderContext renderContext)
        {
            base.Update(renderContext);
        }

       
        //public bool BoundingVolumeIsInView(BoundingSphere sphere)
        //{
        //    return (Frustum.Contains(sphere) != ContainmentType.Disjoint);
        //}

        //public bool BoundingVolumeIsInView(BoundingBox box)
        //{
        //    return (Frustum.Contains(box) != ContainmentType.Disjoint);
        //}
            
    }
}