using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace Blocker
{
    public class RenderContext
    {
        public SpriteBatch SpriteBatch { get; set; }
        public GraphicsDevice GraphicsDevice { get; set; }
        public GameTime GameTime { get; set; }
        public Camera Camera { get; set; }
        public InputManager Input { get; set; }
        public Character Hero { get; set; }
    }
}
