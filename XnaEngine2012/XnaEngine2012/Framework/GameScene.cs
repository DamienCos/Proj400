using System.IO;
using System.Linq;
using System.Xml.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System.IO.IsolatedStorage;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Content;


namespace AndroidTest
{
    public abstract class GameScene
    {
        public string SceneName { get; private set; }
        public List<GameObject2D> SceneObjects2D { get; private set; }
        public List<GameObject3D> SceneObjects3D { get; private set; }
        Stream stream;
        XDocument doc;
        public LevelData thisLevel { get; set; }

        public bool IsSerializable
        {
            get { return isSerializable; }
            protected set { isSerializable = value; }
        }

        bool isSerializable = true;

        public GameScene(string name)
        {
            SceneName = name;
            SceneObjects2D = new List<GameObject2D>();
            SceneObjects3D = new List<GameObject3D>();
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

        public virtual void Activated() { }
        public virtual void Deactivated() { }

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
//using (StreamReader sr = new StreamReader(Game.Activity.Assets.Open("Content/TestData.xml"))

        public void LoadLevel()
        {
#if WINDOWS_PHONE
            IsolatedStorageFile storage = IsolatedStorageFile.GetUserStoreForApplication();
#else
            IsolatedStorageFile storage = IsolatedStorageFile.GetUserStoreForDomain();
#endif            
            using (storage)
            {
                XDocument document;
                if (storage.FileExists("TestData.xml"))
                {
                    using (var stream1 = storage.OpenFile("TestData.xml", FileMode.Open))
                    {
                        document = XDocument.Load(stream1);
                    }

                    var data = (from query in document.Descendants("Levels")
                                select new LevelData()
                                {
                                    LevelName = (string)query.Element("Name"),
                                    CharX = (float)query.Element("CharX"),
                                    CharY = (float)query.Element("CharY"),
                                    CharZ = (float)query.Element("CharZ")
                                });

                    foreach (LevelData l in data)
                    {
                        thisLevel = l;
                    }
                }
                else
                {   // if first time use, use default settings from content to seed level settings
                    stream = TitleContainer.OpenStream("Content/TestData.xml");
                    doc = XDocument.Load(stream);
                    var data = (from query in doc.Descendants("Levels")
                                select new LevelData()
                                {
                                    LevelName = (string)query.Element("Name"),
                                    CharX = (float)query.Element("CharX"),
                                    CharY = (float)query.Element("CharY"),
                                    CharZ = (float)query.Element("CharZ")
                                });

                    foreach (LevelData l in data)
                    {
                        thisLevel = l;
                    }
                }
            }
        }

        public void SaveLevel(Vector3 pos)
        {
#if WINDOWS_PHONE
            IsolatedStorageFile storage = IsolatedStorageFile.GetUserStoreForApplication();
#else
            IsolatedStorageFile storage = IsolatedStorageFile.GetUserStoreForDomain();
#endif
            
            using (storage)
            {
                XDocument document;
                XElement levelRootNode = null;
                // Check if there is a file to  write to
                if (storage.FileExists("TestData.xml"))
                {
                    using (var stream = storage.OpenFile("TestData.xml", FileMode.Open))
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
                                   new XElement("LevelName", "Test"),
                                   new XElement("CharX", pos.X),
                                   new XElement("CharY", pos.Y),
                                   new XElement("CharZ", pos.Z));
                    document.Add(levelRootNode);
                }
                else 
                {   //If file exists, clear it and re-write new data
                    document.RemoveNodes();
                    //adds updated data to isolated storage
                    levelRootNode = new XElement("Levels",
                                    new XElement("LevelName", "Test"),
                                    new XElement("CharX", pos.X),
                                    new XElement("CharY", pos.Y),
                                    new XElement("CharZ", pos.Z));
                    document.Add(levelRootNode);
                }
                using (Stream stream = storage.CreateFile("TestData.xml"))
                {
                    document.Save(stream);
                }
            }
        }

        /// <summary>
        /// Tells the scene to serialize its state into the given stream.
        /// </summary>
        public virtual void Serialize(Stream stream) { }

        /// <summary>
        /// Tells the scene to deserialize its state from the given stream.
        /// </summary>
        public virtual void Deserialize(Stream stream) { }

    }
}
