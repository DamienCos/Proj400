using System.Collections.Generic;
using System.IO;
using System.IO.IsolatedStorage;
using System.Linq;
using System.Xml.Linq;
using System.Xml.Serialization;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using BEPUphysics;
using BEPUphysics.DataStructures;
using BEPUphysics.BroadPhaseEntries;
using BEPUphysics.MathExtensions;
using BEPUphysics.CollisionShapes.ConvexShapes;
using Microsoft.Xna.Framework.Graphics;
using BEPUphysics.Paths;
using BEPUphysics.Entities.Prefabs;
using BEPUphysics.Entities;
using BEPUphysics.Paths.PathFollowing;
using System;

namespace Blocker
{
    public class TestLevel : GameScene
    {
        public Character character { get; set; }
        public GameModel model { get; set; }

        public List<GameObject3D> gameObj3D = new List<GameObject3D>();
        public List<Object3D_Data> levelData = new List<Object3D_Data>();
        public LevelData level { get; set; }


        public TestLevel() : base("Test","Ground") { }

        public override void Initialize()
        {
            #region Load level from storage
            int id = 1; // the player character will always have an id of 1
            SceneManager.LoadLevel();
            level = SceneManager.LoadLevel();
            Space = new BEPUphysics.Space();
            Space.ForceUpdater.Gravity = new Vector3(0, -19.81f, 0f);
            if (level.LevelName == this.SceneName)
            {
                character = new Character();
                character.id = id;
                character.modelPath = level.character.modelPath;
                character.charInput = new CharacterControllerInput(Space,character);
                character.LocalPosition = new Vector3(level.character.PositionX, level.character.PositionY, level.character.PositionZ);
                character.LocalRotation = new Quaternion(level.character.RotationX, level.character.RotationY, level.character.RotationZ, level.character.RotationW);
                AddSceneObject(character);
                character.charInput.Activate();
                SceneManager.c = character;
                foreach (Object3D_Data g in level.GameObject3D)
                {
                    if (g.GetType() == typeof(ChaseCamera))
                    {
                        continue;
                    }
                    else if (g.id == 0)
                    {
                        model = new GameModel(g.model_Path);
                        model.LocalPosition = new Vector3(g.PositionX, g.PositionY, g.PositionZ);
                        model.LocalRotation = new Quaternion(g.RotationX, g.RotationY, g.RotationZ, g.RotationW);
                        AddSceneObject(model);
                    }
                    else
                    {
                        id++;
                        model = new GameModel(g.model_Path);
                        model.id = id;
                        model.LocalPosition = new Vector3(g.PositionX, g.PositionY, g.PositionZ);
                        model.LocalRotation = new Quaternion(g.RotationX, g.RotationY, g.RotationZ, g.RotationW);
                        AddSceneObject(model);
                    }
                }
            } 
            #endregion

            Entity movingEntity = new Box(new Vector3(0, 0, 0), 3000, 1, 3000);      
            Space.Add(movingEntity);

            #region Camera Setup
            camera = new ChaseCamera(new Vector3(0, 200, -400), new Vector3(0, 80, 0),
                new Vector3(0, 0, 0), SceneManager.RenderContext.GraphicsDevice);
          
            AddSceneObject(camera);
  
            #endregion

            SceneManager.RenderContext.Camera = camera;
            SceneManager.thisLevel = level;

            base.Initialize();
        }

        public override void Update(RenderContext renderContext)
        {
            Space.Update();
            // Move the camera to the new model's position and orientation
            ((ChaseCamera)camera).Move(character.LocalPosition, QuaternionToEuler(character.LocalRotation));
            // Update the camera
            camera.Update(renderContext);     
            base.Update(renderContext);           
        }
      
       
        

    }
}
