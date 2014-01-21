
using Microsoft.Xna.Framework.Content;
namespace AndroidTest
{

    public class LevelData
    {
        [ContentSerializer]
        public string LevelName { get; set; }
        public int LevelNo { get; set; }
        public float CharX { get; set; }
        public float CharY { get; set; }
        public float CharZ { get; set; }
    }
}
