using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
//using BEPUphysics.Entities;


namespace Blocker
{   
    //Class for static models
    //public class GameModels
    //{
    //    public Vector3 Position { get; set; }
    //    public Vector3 Rotation { get; set; }
    //    public Vector3 Scale    { get; set; }

    //    public  Model Model     {get; private set;}
    //    private Matrix[] modelTransforms;

    //    private GraphicsDevice graphicsDevice;
    //   // Entity entity;


    //    public GameModels(Model Model, Vector3 Position, 
    //                     Vector3 Scale, GraphicsDevice graphicsDevice)
    //    {
    //        this.Model = Model;


    //        modelTransforms = new Matrix[Model.Bones.Count];
    //        Model.CopyAbsoluteBoneTransformsTo(modelTransforms);

    //        this.Position = Position;
    //        this.Scale = Scale;
    //        this.Rotation = Vector3.Zero;
    //        this.graphicsDevice = graphicsDevice;
    //    }
    //    public GameModels(Model Model, Vector3 Position, Vector3 Rotation,
    //                      Vector3 Scale, GraphicsDevice graphicsDevice)
    //    {
    //        this.Model = Model;
    //        modelTransforms = new Matrix[Model.Bones.Count];
    //        Model.CopyAbsoluteBoneTransformsTo(modelTransforms);

    //        this.Position = Position;
    //        this.Scale = Scale;
    //        this.Rotation = Rotation;
    //        this.graphicsDevice = graphicsDevice;
    //    }

    //    //public GameModels(Model Model, Vector3 Position, Vector3 Rotation,
    //    //                  Vector3 Scale,Entity entity, GraphicsDevice graphicsDevice)
    //    //{
    //    //    this.Model = Model;
    //    //    this.entity = entity;

    //    //    modelTransforms = new Matrix[Model.Bones.Count];
    //    //    Model.CopyAbsoluteBoneTransformsTo(modelTransforms);

    //    //    this.Position = Position;
    //    //    this.Scale = Scale;
    //    //    this.Rotation = Rotation;
    //    //    this.graphicsDevice = graphicsDevice;
    //    //}

    //    public void Update(Vector3 pos, Vector3 rot)
    //    {
    //        Position = pos;
    //        Rotation = rot;
    //    }

    //    public void Draw(Matrix View, Matrix Projection)
    //    {
    //        Matrix baseWorld = Matrix.CreateScale(Scale) *
    //            Matrix.CreateFromYawPitchRoll(Rotation.X , Rotation.Y, Rotation.Z) *
    //            Matrix.CreateTranslation(Position);

    //        foreach (ModelMesh mesh in Model.Meshes)
    //        {
    //            Matrix localWorld = modelTransforms[mesh.ParentBone.Index] * baseWorld;

    //            foreach (ModelMeshPart meshPart in mesh.MeshParts)
    //            {
    //                BasicEffect effect = (BasicEffect)meshPart.Effect;

    //                effect.World = localWorld;
    //                effect.View = View;
    //                effect.Projection = Projection;

    //                effect.EnableDefaultLighting();
    //            }

    //            mesh.Draw();
    //        }
    //    }

    //    public void Draw(Matrix world,Matrix View, Matrix Projection)
    //    {
    //        Matrix baseWorld = world;

    //        foreach (ModelMesh mesh in Model.Meshes)
    //        {
    //            Matrix localWorld = modelTransforms[mesh.ParentBone.Index] * baseWorld;

    //            foreach (ModelMeshPart meshPart in mesh.MeshParts)
    //            {
    //                BasicEffect effect = (BasicEffect)meshPart.Effect;

    //                effect.World = localWorld;
    //                effect.View = View;
    //                effect.Projection = Projection;

    //                effect.EnableDefaultLighting();
    //            }

    //            mesh.Draw();
    //        }
    //    }
    //}
}
