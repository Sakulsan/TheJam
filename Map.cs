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
        public int frameLength;
        public int currentFrame = 0;
        public int millisLastFrame = 0;        
        //Song bgMusic;

        public Map(List<Entity> entities, Texture2D background)
        {
            this.entities = entities;
            this.background = background;
            frameLength = 0;
        }
        public Map(List<Entity> entities, Texture2D background, int frameLength)
        {
            this.entities = entities;
            this.background = background;
            this.frameLength = frameLength; 
        }
    }
}
