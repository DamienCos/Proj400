﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;

namespace Blocker
{
    public class GameSprite : GameObject2D
    {
        private readonly string _assetFile;
        private Texture2D _texture;

        public float Depth { get; set; }
        public Color Color { get; set; }
        public SpriteEffects Effect { get; set; }
        public Rectangle? DrawRect { get; set; }

        public int Width { get { return _texture.Width; } }
        public int Height { get { return _texture.Height; } }

        public GameSprite(string assetFile)
        {
            _assetFile = assetFile;
            Color = Color.White;
            Effect = SpriteEffects.None;
        }

        public override void LoadContent(ContentManager contentManager)
        {
            base.LoadContent(contentManager);

            _texture = contentManager.Load<Texture2D>(_assetFile);
        }

        public override void Draw(RenderContext renderContext)
        {
            if (CanDraw)
            {
                renderContext.SpriteBatch.Draw(_texture, WorldPosition, DrawRect, Color,
                                               MathHelper.ToRadians(WorldRotation), Vector2.Zero, WorldScale, Effect,
                                               Depth);
                base.Draw(renderContext);
            }
        }
    }
}
