using System.IO;
using System.Linq;
using System.Xml.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System.IO.IsolatedStorage;

namespace AndroidTest
{
    public class TestLevel:GameScene
    {
        private Character character;
        private GameModel ground;

        public TestLevel() : base("Test") { }

        public Vector3 pos { get; set; }
        //public LevelData thisLevel { get; set; }
        Stream stream;
        XDocument doc;

        public override void Initialize()
        {
            LoadLevel();
           
            ground = new GameModel(@"Models/Cube");

            pos = new Vector3(thisLevel.CharX, thisLevel.CharY, thisLevel.CharZ);
            ground.LocalPosition = pos;

            AddSceneObject(ground);

            //Initialize The Player
            character = new Character();
            AddSceneObject(character);

            var cam = new BaseCamera();
            cam.Translate(0, 0, 20);
                       
            AddSceneObject(cam);
            SceneManager.RenderContext.Camera = cam;
        
            character.Reset(pos);
            SceneManager.tempChar = character;
            base.Initialize();
        }

        public override void Update(RenderContext renderContext)
        {   // save level current state if back button pressed
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
            {
                SaveLevel(SceneManager.tempChar.WorldPosition);
                //SceneManager.SerializeState();
                SceneManager.ActiveScene.Deactivated();
            }
            
            base.Update(renderContext);
        }


      
    } 
}
