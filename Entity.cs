using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace TheJam
{
    public class Entity
    {
        public int x;
        public int y;
        decimal frameLength;
        int currentFrame = 0;
        int millisLastFrame = 0;
        public int depth;
        public Rectangle drawPosition;
        public Texture2D sprite;
        public bool collision;
        public Game1 game;
        

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

        public void Draw(SpriteBatch sb)
        {
            sb.Draw(sprite, drawPosition , new Rectangle(0,currentFrame * 16,16,16) , Color.White, rotation: 0f, origin: Vector2.Zero, effects: SpriteEffects.None,layerDepth: (depth + 5) / 10);
    }
    }

    public class TouchEntity : Entity
    {
        public enum Effects
        {
            Textbox,
            Teleport,
            Pickup
        }

        public Effects toucheffect;
        public string data;
        public int interactionCount = 0;
        public TouchEntity(int x, int y, int depth, bool collision, Texture2D sprite, Effects toucheffect, string data, Game1 gayme) : base(x, y, depth, collision, sprite, gayme)
        {
            this.toucheffect = toucheffect;
            this.data = data;
        }

        public TouchEntity(int x, int y, int framerate, int depth, bool collision, Texture2D sprite, Effects toucheffect, string data, Game1 gayme) : base(x, y, framerate,depth, collision, sprite, gayme)
        {
            this.toucheffect = toucheffect;
            this.data = data;
        }

        public void Touch()
        {
            interactionCount++;
            switch (toucheffect)
            {
                case Effects.Textbox:
                    //string font = data.Substring(0, data.IndexOf('*'));
                    //List<string> talks = new List<string>(data.Substring(data.IndexOf('*') + 1).Split('&'));
                    game.currentBox = game.boxes.Find(test => test.Item1 == data).Item2;
                    game.currentBox.newTalk();
                    break;
                case Effects.Teleport:
                    break;
                case Effects.Pickup:
                    break;
                default:
                    break;
            }
        }
    }

}
