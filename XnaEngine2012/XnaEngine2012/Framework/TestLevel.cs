using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Xml;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;


namespace AndroidTest
{
    public class TestLevel:GameScene
    {
        private GameSprite background;
        private Character character;
        private GameModel ground;
        private ChaseCam cam;

        public TestLevel() : base("Test") { }

        Vector3 pos = new Vector3(0, 0, 0);/////temp

        public override void Initialize()
        {
            //background = new GameSprite(@"Textures/BackGround");
            //background.DrawInFrontOf3D = false;
            //AddSceneObject(background);

            ground = new GameModel(@"Models/Cube");
            ground.LocalPosition = new Vector3(100, 120, 0);
            
            AddSceneObject(ground);

            //Initialize The Hero
            character = new Character();
            AddSceneObject(character);

            var cam = new BaseCamera();
            cam.Translate(0, 0, 20);
           
            //cam =  new ChaseCam(new Vector3(0, 20, 80), new Vector3(0, 10, 0),
            //    new Vector3(0, 0, 0), 800, 480); // change width and height only temp
            //cam.PositionOffset = new Vector3(0, 20, 80);
            //cam.TargetOffset = new Vector3(0, 10, 0);
            

            AddSceneObject(cam);
            SceneManager.RenderContext.Camera = cam;

          

            character.Reset(pos);
            SceneManager.tempChar = character;
            base.Initialize();
        }
    }

}
