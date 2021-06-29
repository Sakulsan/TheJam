using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Audio;

namespace TheJam
{
    public class Entity
    {
        public int x;
        public int y;
        public decimal frameLength;
        int currentFrame = 0;
        int millisLastFrame = 0;
        public int depth;
        public Rectangle drawPosition;
        public Texture2D sprite;
        public bool collision;
        public Game1 game;
        public bool deactivated = false;



        public Entity(int x, int y, int depth, bool collision, Texture2D sprite, Game1 game)
        {
            this.x = x;
            this.y = y;
            this.depth = depth;
            this.collision = collision;
            this.sprite = sprite;
            this.game = game;
            drawPosition = new Rectangle(x * game.scale, y * game.scale, game.scale, game.scale);
            frameLength = 0;
        }

        public Entity(int x, int y, int framerate, int depth,  bool collision, Texture2D sprite, Game1 game)
        {
            this.x = x;
            this.y = y;
            this.depth = depth;
            this.collision = collision;
            this.sprite = sprite;
            this.game = game;
            this.frameLength = 1000 / ((decimal)framerate);
            drawPosition = new Rectangle(x * game.scale, y * game.scale, game.scale, game.scale);
        }

        public virtual void Update(GameTime gt, List<Entity> enties)
        {
            if (/*!deactivated*/true) { 
                if (frameLength != 0)
            {
                millisLastFrame += (int)gt.ElapsedGameTime.TotalMilliseconds;

                //int movedFrames = (int)Math.Floor(((decimal)millisLastFrame) / (decimal)frameLength);
                if (millisLastFrame > frameLength)
                {
                    currentFrame++;
                    int frameCount = sprite.Height / 16;
                    currentFrame %= frameCount;
                    millisLastFrame = 0;
                }
            }
            drawPosition = new Rectangle(x * game.scale + game.offset, y * game.scale, game.scale, game.scale);
            }
            
        }

        public void Draw(SpriteBatch sb)
        {
            if(!deactivated)sb.Draw(sprite, drawPosition , new Rectangle(0,currentFrame * 16,16,16) , Color.White, rotation: 0f, origin: Vector2.Zero, effects: SpriteEffects.None,layerDepth: (depth + 5) / 10);
            //else sb.Draw(game.pixel, drawPosition, new Rectangle(0, currentFrame * 16, 16, 16), Color.White, rotation: 0f, origin: Vector2.Zero, effects: SpriteEffects.None, layerDepth: (depth + 5) / 10);

        }
    }

    public class TouchEntity : Entity
    {
        public enum Effects
        {
            Textbox,
            Teleport,
            Pickup,
            trade,
            Lock
        }

        public Effects toucheffect;
        public string data;
        public int interactionCount = 0;
        public TouchEntity(bool deactivated,int x, int y, int depth, bool collision, Texture2D sprite, Effects toucheffect, string data, Game1 gayme) : base(x, y, depth, collision, sprite, gayme)
        {
            this.toucheffect = toucheffect;
            this.data = data;
            this.deactivated = deactivated;
        }

        public TouchEntity(bool deactivated, int x, int y, int framerate, int depth, bool collision, Texture2D sprite, Effects toucheffect, string data, Game1 gayme) : base(x, y, framerate,depth, collision, sprite, gayme)
        {
            this.toucheffect = toucheffect;
            this.data = data;
            this.deactivated = deactivated;
        }

        public void Touch(GameTime gt, Entity arv)
        {
            interactionCount++;
            switch (toucheffect)
            {
                case Effects.Textbox:
                    //string font = data.Substring(0, data.IndexOf('*'));
                    //List<string> talks = new List<string>(data.Substring(data.IndexOf('*') + 1).Split('&'));
                    game.currentBox = game.boxes.Find(test => test.Item1 == data).Item2;
                    game.currentBox.newTalk(gt);
                    break;
                case Effects.Teleport:
                    break;
                case Effects.trade:
                    { 
                    string[] s = data.Split('^');

                    if (game.achievments.Contains(s[0]))
                    {
                        game.achievments.Add(s[1]);
                        game.achievments.Remove(s[0]);

                        game.currentBox = new Textbox(new List<string> { s[3] }, false, game.placeHolderSounds, game.fonts[0].Item2, game);
                        game.cutsceneMode = true;
                            deactivated = true;
                           if(arv != null) arv.deactivated = false;
                        }
                    else
                    {
                        game.currentBox = new Textbox(new List<string>(new[] { s[2] }), false, game.placeHolderSounds, game.fonts[0].Item2, game);
                        game.cutsceneMode = true;
                    }
            }
                    break;
                case Effects.Pickup:
                    { 
                    string[] s;
                    
                        s = data.Split('^');
                    game.achievments.Add(s[0]);
                    game.currentBox = new Textbox(new List<string>(new[] { "You have picked up: " + s[0] } ),false, game.placeHolderSounds, game.fonts[0].Item2,game);
                    deactivated = true;
                    game.cutsceneMode = true;
                    if(s.Length > 1)
                        {
                            string testo = "";
                            for (int i = 1; i < s.Length; i++)
                            {
                                testo += s[i];
                            }

                            for (int i = 0; i < game.World.GetLength(0); i++)
                            {
                                for (int j = 0; j < game.World.GetLength(1); j++)
                                {
                                    if (game.World[i, j].entities.Exists(test => test is TouchEntity && ((TouchEntity)test).data == testo))
                                    {
                                        Entity replacement = game.World[i, j].entities.Find(test => test is TouchEntity && ((TouchEntity)test).data == testo);
                                        replacement.deactivated = false;
                                    };
                                }
                            }
                        }
                    }
                    break;
                case Effects.Lock:
                    {
                        string[] s = data.Split('^');


                        if (game.achievments.Contains(s[0])) { 
                        game.achievments.Remove(s[0]);
                        game.currentBox = new Textbox(new List<string>(new[] { "You have used: " + s[0] }),false, game.placeHolderSounds, game.fonts[0].Item2, game);
                        deactivated = true;
                        game.cutsceneMode = true;
                        if (s.Length > 2)
                        {
                                string testo = "";
                                for (int i = 2; i < s.Length; i++)
                                {
                                    testo += s[i];
                                    if (i != s.Length - 1) testo += "^";
                                }


                                for (int i = 0; i < game.World.GetLength(0); i++)
                                {
                                    for (int j = 0; j < game.World.GetLength(1); j++)
                                    {
                                        if (game.World[i, j].entities.Exists(test => test is TouchEntity && ((TouchEntity)test).data == testo))
                                        {
                                            Entity replacement = game.World[i, j].entities.Find(test => test is TouchEntity && ((TouchEntity)test).data == testo);
                                            replacement.deactivated = false;
                                        };
                                    }
                                }
                        }
                        }
                        else
                        {
                            game.currentBox = new Textbox(new List<string>(new[] { s[1] }),false,game.placeHolderSounds, game.fonts[0].Item2, game);
                            game.cutsceneMode = true;
                        }
                    }
                    break;
                default:
                    break;
            }
        }
    }


    public class Volatile : TouchEntity
    {
        public Volatile(bool deactivated, int x, int y, int depth, bool collision, Texture2D sprite, Effects toucheffect, string data, Game1 gayme) : base(deactivated, x, y, depth, collision, sprite, toucheffect, data, gayme)
        {
        }

        public Volatile(bool deactivated, int x, int y, int framerate, int depth, bool collision, Texture2D sprite, Effects toucheffect, string data, Game1 gayme) : base(deactivated, x, y, framerate, depth, collision, sprite, toucheffect, data, gayme)
        {
        }
    }
}
