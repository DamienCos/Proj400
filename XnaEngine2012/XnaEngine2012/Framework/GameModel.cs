using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using System.Xml.Serialization;
using BEPUphysics.DataStructures;
using BEPUphysics.BroadPhaseEntries;
using BEPUphysics.MathExtensions;
using BEPUphysics.CollisionShapes.ConvexShapes;

namespace AndroidTest
{
    public class GameModel : GameObject3D
    {

        public Model _model { get; set; }
        //Load in mesh data for the environment.
        Vector3[] staticTriangleVertices;
        int[] staticTriangleIndices;

        public GameModel(string assetFile)
        {
            modelPath = assetFile;
        }

        public GameModel()
        {

        }

        public override void LoadContent(ContentManager contentManager)
        {
            _model = contentManager.Load<Model>(modelPath);
            if (modelPath == "Models\\Ground")
            {////This is a little convenience method used to extract vertices and indices from a model.
                ////It doesn't do anything special; any approach that gets valid vertices and indices will work.
                TriangleMesh.GetVerticesAndIndicesFromModel(_model, out staticTriangleVertices, out staticTriangleIndices);
                var staticMesh = new StaticMesh(staticTriangleVertices, staticTriangleIndices, new AffineTransform(new Vector3(.01f, .01f, .01f), Quaternion.Identity, new Vector3(0, 0, 0)));
                staticMesh.Sidedness = TriangleSidedness.Counterclockwise;

                Scene.Space.Add(staticMesh);
            }
            base.LoadContent(contentManager);
        }

        public override void Draw(RenderContext renderContext)
        {
            var transforms = new Matrix[_model.Bones.Count];
            _model.CopyAbsoluteBoneTransformsTo(transforms);

            foreach (ModelMesh mesh in _model.Meshes)
            {
                foreach (BasicEffect effect in mesh.Effects)
                {
                    effect.EnableDefaultLighting();

                    effect.View = renderContext.Camera.View;
                    effect.Projection = renderContext.Camera.Projection;
                    effect.World = transforms[mesh.ParentBone.Index] * WorldMatrix;

                    effect.GraphicsDevice.BlendState = BlendState.AlphaBlend;
                    effect.PreferPerPixelLighting = true;
                    effect.SpecularColor = new Vector3(0.25f);
                    effect.SpecularPower = 16;
                }

                mesh.Draw();
            }
            base.Draw(renderContext);
        }
    }
}
