using System.Collections.Generic;
using System.IO;
using System.IO.IsolatedStorage;
using System.Linq;
using System.Xml.Linq;
using System.Xml.Serialization;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using BEPUphysics;

namespace AndroidTest
{
    public class TestLevel : GameScene
    {
        public Character character { get; set; }
        public GameModel model { get; set; }

        public List<GameObject3D> gameObj3D = new List<GameObject3D>();
        public List<Object3D_Data> levelData = new List<Object3D_Data>();
        public LevelData level { get; set; }

        public TestLevel() : base("Test") { }

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
                //id++;
                character.LocalPosition = new Vector3(level.character.PositionX, level.character.PositionY, level.character.PositionZ);
                character.LocalRotation = new Quaternion(level.character.RotationX, level.character.RotationY, level.character.RotationZ, level.character.RotationW);
                AddSceneObject(character);
                foreach (Object3D_Data g in level.GameObject3D)
                {
                    if (g.GetType() == typeof(BaseCamera))
                    {
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

            Space s = new Space();
            var cam = new BaseCamera();
            cam.Translate(0, 0, 20);
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
