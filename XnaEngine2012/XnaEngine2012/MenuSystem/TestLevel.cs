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

namespace Blocker
{
    public class TestLevel : GameScene
    {
        public Character character { get; set; }
        public GameModel model { get; set; }

        public List<GameObject3D> gameObj3D = new List<GameObject3D>();
        public List<Object3D_Data> levelData = new List<Object3D_Data>();
        public LevelData level { get; set; }

        bool cameraSpringEnabled = false;

        public TestLevel() : base("Test","Ground") { }

        public override void Initialize()
        {
            #region Load level from storage
            int id = 1; // the player character will always have an id of 1
            SceneManager.LoadLevel();
            level = SceneManager.LoadLevel();

            if (level.LevelName == this.SceneName)
            {
                character = new Character();
                character.id = id;
                character.modelPath = level.character.modelPath;
                character.charInput = new CharacterControllerInput(Space,character);
                character.LocalPosition = new Vector3(level.character.PositionX, level.character.PositionY, level.character.PositionZ);
                character.LocalRotation = new Quaternion(level.character.RotationX, level.character.RotationY, level.character.RotationZ, level.character.RotationW);
                AddSceneObject(character);
                character.charInput.Activate(); SceneManager.c = character;
                foreach (Object3D_Data g in level.GameObject3D)
                {
                    if (g.GetType() == typeof(ChaseCamera))
                    {
                        continue;
                    }
                    else
                    {
                        id++;
                        model = new GameModel(g.model_Path);
                        model.id = id;
                        model.LocalPosition = new Vector3(g.PositionX, g.PositionY, g.PositionZ);
                        //model.LocalRotation = new Quaternion(g.RotationX, g.RotationY, g.RotationZ, g.RotationW);
                        AddSceneObject(model);
                    }
                }
            } 
            #endregion

            #region Camera Setup
            camera = new ChaseCamera();
            // Set the camera offsets
            camera.DesiredPositionOffset = new Vector3(0.0f, 200.0f, -400.0f);
            camera.LookAtOffset = new Vector3(0.0f, 80.0f, 00.0f);
            // Set camera perspective
            camera.NearPlaneDistance = 1.0f;
            camera.FarPlaneDistance = 10000.0f;
            UpdateCameraChaseTarget();
            camera.Reset();
            AddSceneObject(camera); 
            #endregion

            SceneManager.RenderContext.Camera = camera;
            SceneManager.thisLevel = level;

            base.Initialize();
        }

        public override void Update(RenderContext renderContext)
        {
            UpdateCameraChaseTarget();

            // The chase camera's update behavior is the springs, but we can
            // use the Reset method to have a locked, spring-less camera
            if (cameraSpringEnabled)
                camera.Update(renderContext);
            else
                camera.Reset();

            base.Update(renderContext);
        }

        /// <summary>
        /// Update the values to be chased by the camera
        /// </summary>
        private void UpdateCameraChaseTarget()
        {
            camera.ChasePosition = character.LocalPosition;
            camera.ChaseDirection = character.Direction;
            camera.Up = character.Up;
        }

    }
}
