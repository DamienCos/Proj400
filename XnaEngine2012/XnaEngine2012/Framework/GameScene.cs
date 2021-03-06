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
        public ChaseCamera camera;

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

       

    }
}
