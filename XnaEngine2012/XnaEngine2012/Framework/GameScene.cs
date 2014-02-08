using System;
using System.Collections.Generic;
using System.IO;
using System.IO.IsolatedStorage;
using System.Linq;
using System.Xml.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Input;

using BEPUphysics.Entities.Prefabs;
using BEPUphysics;
using BEPUphysics.Entities;
using BEPUphysics.DataStructures;
using BEPUphysics.MathExtensions;
using BEPUphysics.CollisionShapes.ConvexShapes;
using BEPUphysics.BroadPhaseEntries;


namespace Blocker
{
    
    public abstract class GameScene
    {
        public string SceneName { get; private set; }
        public List<GameObject2D> SceneObjects2D { get; private set; }
        public List<GameObject3D> SceneObjects3D { get; private set; }

        public Space Space;
        public Camera camera;

        public GameObject3D levelModel;
        public string levelModelName { get; private set; }

        public GameScene(string name, string modelName)
        {
            SceneName = name;
            SceneObjects2D = new List<GameObject2D>();
            SceneObjects3D = new List<GameObject3D>();
            Space = new Space();
            levelModelName = modelName;
        }
    
        public override bool Equals(object obj)
        {
            if (obj is GameScene)
            {
                return SceneName.Equals((obj as GameScene).SceneName);
            }

            return false;
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        public void AddSceneObject(GameObject2D sceneObject)
        {
            if (!SceneObjects2D.Contains(sceneObject))
            {
                sceneObject.Scene = this;
                SceneObjects2D.Add(sceneObject);
            }
        }

        public void AddSceneObject(GameObject3D sceneObject)
        {
            if (!SceneObjects3D.Contains(sceneObject))
            {
                sceneObject.Scene = this;
                SceneObjects3D.Add(sceneObject);
            }
        }
     
        public void RemoveSceneObject(GameObject2D sceneObject)
        {
            if (SceneObjects2D.Remove(sceneObject))
            {
                sceneObject.Scene = null;
            }
        }

        public void RemoveSceneObject(GameObject3D sceneObject)
        {
            if (SceneObjects3D.Remove(sceneObject))
            {
                sceneObject.Scene = null;
            }
        }

        public virtual void Activated() {}
        public virtual void Deactivated() {}

        public virtual void Initialize()
        {
            SceneObjects2D.ForEach(sceneObject => sceneObject.Initialize());
            SceneObjects3D.ForEach(sceneObject => sceneObject.Initialize());
        }

        public virtual void LoadContent(ContentManager contentManager)
        {
            SceneObjects2D.ForEach(sceneObject => sceneObject.LoadContent(contentManager));
            SceneObjects3D.ForEach(sceneObject => sceneObject.LoadContent(contentManager));
          
        }

        public virtual void Update(RenderContext renderContext)
        {
            SceneObjects2D.ForEach(sceneObject => sceneObject.Update(renderContext));
            SceneObjects3D.ForEach(sceneObject => sceneObject.Update(renderContext));
            Space.Update();
        }

        public virtual void Draw2D(RenderContext renderContext, bool drawInFrontOf3D)
        {
            SceneObjects2D.ForEach(obj =>
            {
                if (obj.DrawInFrontOf3D == drawInFrontOf3D)
                    obj.Draw(renderContext);
            });
        }

        public virtual void Draw3D(RenderContext renderContext)
        {
            SceneObjects3D.ForEach(sceneObject => sceneObject.Draw(renderContext));
        }

        public static float ArcTanAngle(float X, float Y)
        {
            if (X == 0)
            {
                if (Y == 1)
                    return (float)MathHelper.PiOver2;
                else
                    return (float)-MathHelper.PiOver2;
            }
            else if (X > 0)
                return (float)Math.Atan(Y / X);
            else if (X < 0)
            {
                if (Y > 0)
                    return (float)Math.Atan(Y / X) + MathHelper.Pi;
                else
                    return (float)Math.Atan(Y / X) - MathHelper.Pi;
            }
            else
                return 0;
        }
        //returns Euler angles that point from one point to another
        public static Vector3 AngleTo(Vector3 from, Vector3 location)
        {
            Vector3 angle = new Vector3();
            Vector3 v3 = Vector3.Normalize(location - from);
            angle.X = (float)Math.Asin(v3.Y);
            angle.Y = ArcTanAngle(-v3.Z, -v3.X);
            return angle;
        }

        public static Vector3 QuaternionToEuler(Quaternion rotation)
        {
            Vector3 rotationaxes = new Vector3();

            Vector3 forward = Vector3.Transform(Vector3.Forward, rotation);
            Vector3 up = Vector3.Transform(Vector3.Up, rotation);
            rotationaxes = AngleTo(new Vector3(), forward);
            if (rotationaxes.X == MathHelper.PiOver2)
            {
                rotationaxes.Y = ArcTanAngle(up.Z, up.X);
                rotationaxes.Z = 0;
            }
            else if (rotationaxes.X == -MathHelper.PiOver2)
            {
                rotationaxes.Y = ArcTanAngle(-up.Z, -up.X);
                rotationaxes.Z = 0;
            }
            else
            {
                up = Vector3.Transform(up, Matrix.CreateRotationY(-rotationaxes.Y));
                up = Vector3.Transform(up, Matrix.CreateRotationX(-rotationaxes.X));
                rotationaxes.Z = ArcTanAngle(up.Y, -up.X);
            }
            return rotationaxes;
        }

    }
}
