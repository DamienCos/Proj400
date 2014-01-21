
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework;
using System.Collections.Generic;
namespace AndroidTest
{

    public class LevelData
    {
        [ContentSerializer]
        public string LevelName { get; set; }
       // public List<Vector3> GameObjPos { get; set; }
        public float CharX { get; set; }
        public float CharY { get; set; }
        public float CharZ { get; set; }
    }
}
