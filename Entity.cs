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
        public bool interactable;
        

        public Entity(int x, int y, int depth, bool collision, bool interactable, Texture2D sprite)
        {
            this.x = x;
            this.y = y;
            this.depth = depth;
            this.collision = collision;
            this.interactable = interactable;
            this.sprite = sprite;
            drawPosition = new Rectangle(0, 0, 64, 64);
        }

        public virtual void Update(GameTime gt, List<Entity> enties)
        {
            drawPosition = new Rectangle(x * 64, y * 64, 64, 64);
        }

        public void Draw(SpriteBatch sb)
        {
            sb.Draw(sprite, drawPosition , Color.White);
    }
    }

    
}
