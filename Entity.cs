using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace TheJam
{
    public class Entity
    {
        public int x;
        public int y;
        public int depth;
        public Rectangle drawPosition;
        public Texture2D sprite;
        public bool collision;
        public Game1 game;
        

        public Entity(int x, int y, int depth, bool collision, Texture2D sprite, Game1 game)
        {
            this.x = x;
            this.y = y;
            this.depth = depth;
            this.collision = collision;
            this.sprite = sprite;
            this.game = game;
            drawPosition = new Rectangle(0, 0, 64, 64);
        }

        public virtual void Update(GameTime gt, List<Entity> enties)
        {
            drawPosition = new Rectangle(x * 64, y * 64, 64, 64);
        }

        public void Draw(SpriteBatch sb)
        {
            sb.Draw(sprite, drawPosition , new Rectangle(0,0,sprite.Width,sprite.Height) , Color.White, rotation: 0f, origin: Vector2.Zero, effects: SpriteEffects.None,layerDepth: (depth + 5) / 10);
    }
    }

    public class TouchEntity : Entity
    {
        public enum Effects
        {
            Textbox,
            Teleport,
            Pickup
        }

        public Effects toucheffect;
        public string data;
        public TouchEntity(int x, int y, int depth, bool collision, Texture2D sprite, Effects toucheffect, string data, Game1 gayme) : base(x, y, depth, collision, sprite, gayme)
        {
            this.toucheffect = toucheffect;
            this.data = data;
        }

        public void Touch()
        {
            switch (toucheffect)
            {
                case Effects.Textbox:
                    game.texting = true;
                    break;
                case Effects.Teleport:
                    break;
                case Effects.Pickup:
                    break;
                default:
                    break;
            }
        }
    }

}
