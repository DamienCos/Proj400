using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Blocker
{
    
    public class LevelData
    {
        public string LevelName { get; set; }
        public CharPos character { get; set; }
        public List<Object3D_Data> GameObject3D { get; set; }
    }

    public class Object3D_Data
    {
        
        public int id { get; set; }
        public string model_Path{ get; set; }
        public float PositionX { get; set; }
        public float PositionY { get; set; }
        public float PositionZ { get; set; }
        public float RotationX { get; set; }
        public float RotationY { get; set; }
        public float RotationZ { get; set; }
        public float RotationW { get; set; }
    }

    public class CharPos
    {
        public int id { get; set; }
        public string modelPath { get; set; }
        public float PositionX { get; set; }
        public float PositionY { get; set; }
        public float PositionZ { get; set; }
        public float RotationX { get; set; }
        public float RotationY { get; set; }
        public float RotationZ { get; set; }
        public float RotationW { get; set; }
    }
}
