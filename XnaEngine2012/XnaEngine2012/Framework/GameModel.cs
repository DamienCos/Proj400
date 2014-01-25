using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using System.Xml.Serialization;

namespace AndroidTest
{
    public class GameModel : GameObject3D
    {

        private Model _model;

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
