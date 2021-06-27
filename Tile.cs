using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace TheJam
{
    class LeaveTile : Entity
    {
        int leaveX;
        int leaveY;
        public LeaveTile(int x, int y, Texture2D sprite, bool collision, int leaveX, int leaveY, Game1 game) : base(x, y, -10, collision,  sprite, game)
        {
            this.leaveY = leaveY;
            this.leaveX = leaveX;
        }

        public override void Update(GameTime gt, List<Entity> enties)
        {
            Entity player = enties.Find(test => test is Player);
            if (player.x == x && player.y == y)
            {
                game.fadeGaol = 255 * 5;
                game.zoneCoordinates = new Point(leaveX, leaveY);
            }

            base.Update(gt, enties);
        }
    }

}
