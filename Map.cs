using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace TheJam
{
    public class Map
    {
        public List<Entity> entities;
        public Texture2D background;

        public Map(List<Entity> entities, Texture2D background)
        {
            this.entities = entities;
            this.background = background;
        }
    }
}
