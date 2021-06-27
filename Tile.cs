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
        int spawnX;
        int spawnY;
        public LeaveTile(int x, int y, Texture2D sprite, bool collision, int leaveX, int leaveY, int spawnX,int spawnY,Game1 game) : base(x, y, -10, collision,  sprite, game)
        {
            this.leaveY = leaveY;
            this.leaveX = leaveX;
            this.spawnX = spawnX;
            this.spawnY = spawnY;
        }          

        public override void Update(GameTime gt, List<Entity> enties)
        {
            Entity player = game.currentZone[game.zoneCoordinates.X, game.zoneCoordinates.Y].entities.Find(test => test is Player);
            if (player.x == x && player.y == y)
            {
                game.fadeGaol = 255;
                game.fadethrough = 200;
                game.zoneCoordinates = new Point(leaveX, leaveY);
                player = game.currentZone[game.zoneCoordinates.X, game.zoneCoordinates.Y].entities.Find(test => test is Player);
                player.x = spawnX;
                player.y = spawnY;
            }

            base.Update(gt, enties);
        }
    }

}
