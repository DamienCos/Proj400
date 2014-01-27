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

namespace AndroidTest
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

            if (level.LevelName == this.SceneName)
            {
                character = new Character();
                character.id = id;
                character.modelPath = level.character.modelPath;
                character.charInput = new CharacterControllerInput(Space,character);
                character.LocalPosition = new Vector3(level.character.PositionX, level.character.PositionY, level.character.PositionZ);
                character.LocalRotation = new Quaternion(level.character.RotationX, level.character.RotationY, level.character.RotationZ, level.character.RotationW);
                AddSceneObject(character);
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
                        // model.LocalRotation = new Quaternion(g.RotationX, g.RotationY, g.RotationZ, g.RotationW);
                        AddSceneObject(model);
                    }
                }
            } 
            #endregion

            //var playgroundModel = new GameModel(this.levelModelName);
            //var levelModel = SceneManager.MainGame.Content.Load<Model>(this.levelModelName);
            ////This is a little convenience method used to extract vertices and indices from a model.
            ////It doesn't do anything special; any approach that gets valid vertices and indices will work.
            //TriangleMesh.GetVerticesAndIndicesFromModel(levelModel, out staticTriangleVertices, out staticTriangleIndices);
            //var staticMesh = new StaticMesh(staticTriangleVertices, staticTriangleIndices, new AffineTransform(new Vector3(.01f, .01f, .01f), Quaternion.Identity, new Vector3(0, 0, 0)));
            //staticMesh.Sidedness = TriangleSidedness.Counterclockwise;

            //Space.Add(staticMesh);
            //AddSceneObject(playgroundModel); // may have to change this

            var cam = new ChaseCamera();//new BaseCamera();
            cam.Translate(0, 0, 400);
            AddSceneObject(cam);

            SceneManager.RenderContext.Camera = cam;
            SceneManager.thisLevel = level;

            base.Initialize();
        }

        public override void Update(RenderContext renderContext)
        {
            base.Update(renderContext);
        }



    }
}
