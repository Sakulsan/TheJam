using System;
using System.Collections.Generic;
using System.Text;

namespace TheJam
{
    class MoveSequence
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
            moving = false;
        }
    }
}
