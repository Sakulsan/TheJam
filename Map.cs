using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Audio;

namespace TheJam
{
    public class Map
    {
        public List<Entity> entities;
        public Texture2D background;
        //Song bgMusic;

        public Map(List<Entity> entities, Texture2D background)
        {
            this.entities = entities;
            this.background = background;
        }
    }
}
