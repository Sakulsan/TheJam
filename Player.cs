using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Audio;

namespace TheJam
{
    class Player : Entity
    {
        int goalX;
        int goalY;
        int millismoved;

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
            if (!game.cutsceneMode)
            {
                TouchEntity ice = (TouchEntity)enties.Find(test => (test.sprite == game.ice || test.sprite == game.glow) && test.x == x && test.y == y && !test.deactivated);
            if (ice != null)
            {
                if (ice.data == "true")
                {
                    if (sprite != game.crawlPig)
                    {
                        sprite = game.crawlPig;
                        millismoved = 0;
                    }
                }
                else
                {
                    //Drown
                    sprite = game.drownPig;
                    frameLength = 250;


                    ice.deactivated = true;
                    game.World[0, 0].entities.Add(new Entity(ice.x, ice.y,0, true, game.nothing, game));
                    game.currentBox = new Textbox(new List<string> { "You have drowned", "You wake up inside the cloning vat of the ship" },false, game.placeHolderSounds, game.fonts[0].Item2, game);

                        game.sfx = game.Content.Load<SoundEffect>(@"ljud\ljudeffekter\drowning1").CreateInstance();
                        game.sfx.Play();

                    millismoved = 0;
                    game.cutsceneMode = true;
                    game.blockCountDown = 1800;
                        game.achievments.Add("Drowned");
                }
            }
            else sprite = game.normalPig;
            
                KeyboardState newState = Keyboard.GetState();
            if ((newState.IsKeyDown(Keys.Left) || newState.IsKeyDown(Keys.A)) && !(oldState.IsKeyDown(Keys.Left) || oldState.IsKeyDown(Keys.A)))
                if (!enties.Exists(test => test.collision && !test.deactivated && test.x == x - 1 && test.y == y)) x--;
            if ((newState.IsKeyDown(Keys.Up) || newState.IsKeyDown(Keys.W)) && !(oldState.IsKeyDown(Keys.Up) || oldState.IsKeyDown(Keys.W)))
                if (!enties.Exists(test => test.collision && !test.deactivated && test.x == x && test.y == y - 1)) y--;
            if ((newState.IsKeyDown(Keys.Right) || newState.IsKeyDown(Keys.D)) && !(oldState.IsKeyDown(Keys.Right) || oldState.IsKeyDown(Keys.D)))
                if (!enties.Exists(test => test.collision && !test.deactivated && test.x == x + 1 && test.y == y)) x++;
            if ((newState.IsKeyDown(Keys.Down) || newState.IsKeyDown(Keys.S)) && !(oldState.IsKeyDown(Keys.Down) || oldState.IsKeyDown(Keys.S)))
                if (!enties.Exists(test => test.collision && !test.deactivated && test.x == x && test.y == y + 1)) y++;
            
                Entity touched = enties.Find(test => test is TouchEntity && !(((TouchEntity)test).data == "true" || ((TouchEntity)test).data == "fake") && !((TouchEntity)test).deactivated &&
                ((test.x == x - 1 && test.y == y) ||
                (test.x == x + 1 && test.y == y) ||
                (test.x == x && test.y == y - 1) ||
                (test.x == x && test.y == y + 1)));

                if (touched != null)
                {
                    game.hud["E"].deactivated = false;
                    if (newState.IsKeyDown(Keys.E) && !(oldState.IsKeyDown(Keys.E))) {
                        Entity arv;
                        switch (touched.depth)
                        {
                            case 8:
                                arv = game.World[2, 0].entities.Find(x => x is TouchEntity && ((TouchEntity)x).data == "crobo");
                                break;
                            default:
                                arv = null;
                                break;
                        }
                        ((TouchEntity)touched).Touch(gt,arv);
                    }
                }
                else game.hud["E"].deactivated = true;
            
            



            oldState = newState;
            }
            else
                if(game.hud.ContainsKey("E")) game.hud["E"].deactivated = true;
            base.Update(gt, enties);
            if (goalX != x || goalY != y)
            {
                int xOffset = x - goalX;
                int yOffset = y - goalY;
                drawPosition = new Rectangle(x * game.scale + game.offset, y * game.scale, game.scale, game.scale);
            }
        }
    }
}
