using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;

namespace TheJam
{
    public class MoveSequence
    {
        public Entity e;
        public (int,int)[] position;
        public int speed;
        public int millis;
        public bool moving;
        

        public MoveSequence(Entity e, (int, int)[] position, int speed)
        {
            this.e = e;
            this.position = position;
            this.speed = speed;
            millis = speed;
            moving = true;
        }

        public void Update(GameTime gt, Game1 g)
        {
            millis += gt.ElapsedGameTime.Milliseconds;
            int pos = millis / speed;
            if (pos > position.Length)
            {
                moving = false;
                g.Joe.deactivated = true;
            }
            else
            {
                g.Joe.x = position[pos - 1].Item1;
                g.Joe.y = position[pos - 1].Item2;
            }
        }
    }
}
