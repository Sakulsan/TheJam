using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace TheJam
{
    class Player : Entity
    {
        private KeyboardState oldState;
        public Player(int x, int y, Texture2D sprite, Game1 game) : base(x, y, 0,false, sprite, game)
        {
            oldState = Keyboard.GetState();
        }

        public Player(int x, int y, int framerate,Texture2D sprite, Game1 game) : base(x, y, framerate,0, false, sprite, game)
        {
            oldState = Keyboard.GetState();
        }

        public override void Update(GameTime gt, List<Entity> enties)
        {
            KeyboardState newState = Keyboard.GetState();
            if ((newState.IsKeyDown(Keys.Left) || newState.IsKeyDown(Keys.A)) && !(oldState.IsKeyDown(Keys.Left) || oldState.IsKeyDown(Keys.A)))
                if (!enties.Exists(test => test.collision && test.x == x - 1 && test.y == y)) x--;
            if ((newState.IsKeyDown(Keys.Up) ||   newState.IsKeyDown(Keys.W)) &&   !(oldState.IsKeyDown(Keys.Up)   ||oldState.IsKeyDown(Keys.W)))
                if (!enties.Exists(test => test.collision && test.x == x  && test.y == y - 1)) y--;
            if ((newState.IsKeyDown(Keys.Right) || newState.IsKeyDown(Keys.D)) && !(oldState.IsKeyDown(Keys.Right) ||  oldState.IsKeyDown(Keys.D)))
                if (!enties.Exists(test => test.collision && test.x == x + 1 && test.y == y)) x++;
            if ((newState.IsKeyDown(Keys.Down) || newState.IsKeyDown(Keys.S)) && !(oldState.IsKeyDown(Keys.Down) ||  oldState.IsKeyDown(Keys.S)))
                if (!enties.Exists(test => test.collision && test.x == x  && test.y == y + 1)) y++;
            if ((newState.IsKeyDown(Keys.E)) && !(oldState.IsKeyDown(Keys.E))) {
                Entity touched = enties.Find(test => test is TouchEntity &&
                ((test.x == x - 1 && test.y == y) ||
                (test.x == x + 1 && test.y == y) ||
                (test.x == x && test.y == y - 1) ||
                (test.x == x && test.y == y + 1)));

                if (touched != null) ((TouchEntity)touched).Touch();
                    }

            oldState = newState;
            base.Update(gt, enties);
        }
    }
}
