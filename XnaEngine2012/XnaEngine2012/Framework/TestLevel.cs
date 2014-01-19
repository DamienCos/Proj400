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

        Vector3 pos;/////temp
        Level thisLevel;
        Stream stream;
        XDocument doc;

        public override void Initialize()
        {
            LoadLevel();
            ground = new GameModel(@"Models/Cube");

            pos = new Vector3(thisLevel.CharX, thisLevel.CharY, thisLevel.CharZ);
            ground.LocalPosition = pos;
            
            AddSceneObject(ground);

            //Initialize The Hero
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
            }
            base.Update(renderContext);
        }

        public void LoadLevel()
        {
            using (var storage = IsolatedStorageFile.GetUserStoreForApplication())
            {
                XDocument document;
                if (storage.FileExists("Level1Update.xml"))
                {
                    using (var stream1 = storage.OpenFile("Level1Update.xml", FileMode.Open))
                    {
                        document = XDocument.Load(stream1);
                    }
                    var data = (from query in document.Descendants("Levels")
                                select new Level()
                                {
                                    LevelNo = (int)query.Element("LevelNo"),
                                    LevelName = (string)query.Element("Name"),
                                    CharX = (float)query.Element("CharX"),
                                    CharY = (float)query.Element("CharY"),
                                    CharZ = (float)query.Element("CharZ")
                                });

                    foreach (Level l in data)
                    {
                        thisLevel = l;
                    }                   
                }
                else
                {   // if first time use, use default settings from content to seed level settings
                    stream = TitleContainer.OpenStream("Content\\Level1.xml");
                    doc = XDocument.Load(stream);
                    var data = (from query in doc.Descendants("Levels")
                                select new Level()
                                {
                                    LevelNo = (int)query.Element("LevelNo"),
                                    LevelName = (string)query.Element("Name"),
                                    CharX = (float)query.Element("CharX"),
                                    CharY = (float)query.Element("CharY"),
                                    CharZ = (float)query.Element("CharZ")
                                });

                    foreach (Level l in data)
                    {
                        thisLevel = l;
                    }
                }              
            }
        }

        public void SaveLevel(Vector3 pos)
        {
            using (var storage = IsolatedStorageFile.GetUserStoreForApplication())
            {
                XDocument document;
                XElement levelRootNode = null;
                // Check if there is a file to  write to
                if (storage.FileExists("Level1Update.xml"))
                {
                    using (var stream = storage.OpenFile("Level1Update.xml", FileMode.Open))
                    {
                        document = XDocument.Load(stream);
                    }
                    levelRootNode = document.Descendants("Levels").FirstOrDefault();
                }
                else
                {
                    document = new XDocument();
                }
                // If new file add data
                if (levelRootNode == null)
                {
                    levelRootNode = new XElement("Levels",
                                    new XElement("LevelNo", 0),
                                    new XElement("Name", "ABC"),
                                    new XElement("CharX", 0),
                                    new XElement("CharY", 0),
                                    new XElement("CharZ", 0));
                    document.Add(levelRootNode);
                }
                else // If file exists, clear it and re-write new data
                {    //clears document in isolated storage
                    document.RemoveNodes(); 
                     //adds updated data to isolated storage
                    levelRootNode = new XElement("Levels",         
                                    new XElement("LevelNo", 1),
                                    new XElement("Name", "Test"),
                                    new XElement("CharX", pos.X),
                                    new XElement("CharY", pos.Y),
                                    new XElement("CharZ", pos.Z));
                    document.Add(levelRootNode);
                }

            
                using (Stream stream = storage.CreateFile("Level1Update.xml"))
                {
                    document.Save(stream);
                }
            }
        }
    }

    public class Level
    {
        public string LevelName { get; set; }
        public int LevelNo { get; set; }
        public float CharX { get; set; }
        public float CharY { get; set; }
        public float CharZ { get; set; }
    }
  
}
