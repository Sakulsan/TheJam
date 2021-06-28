using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace TheJam
{
    public class myPoint
    {
        public int X;
        public int Y;

        public myPoint(int x, int y)
        {
            this.X = x;
            this.Y = y;
        }
    }

    class LeaveTile : Entity
    {
        int leaveX;
        int leaveY;
        int spawnX;
        int spawnY;
        public bool permanent;
        public LeaveTile(int x, int y, Texture2D sprite, bool collision, int leaveX, int leaveY, int spawnX,int spawnY, bool permanent,Game1 game) : base(x, y, -10, collision,  sprite, game)
        {
            this.leaveY = leaveY;
            this.leaveX = leaveX;
            this.spawnX = spawnX;
            this.spawnY = spawnY;
            this.permanent = permanent;
        }          

        public override void Update(GameTime gt, List<Entity> enties)
        {
            Entity player = game.World[game.zoneCoordinates.X, game.zoneCoordinates.Y].entities.Find(test => test is Player);
            if (player != null && player.x == x && player.y == y)
            {
                game.moveFadeGaol = 255;
                game.moveFadethrough = 200;
                game.soundFadeGaol = 500;
                game.World[game.zoneCoordinates.X, game.zoneCoordinates.Y].entities.Remove(player);
                game.zoneCoordinates = new myPoint(leaveX, leaveY);
                game.World[game.zoneCoordinates.X, game.zoneCoordinates.Y].entities.Add(player);
                player.x = spawnX;
                player.y = spawnY;
            }

            base.Update(gt, enties);
        }
    }

}
