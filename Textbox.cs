using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;

namespace TheJam
{
    public class Textbox
    {
        const int cursorSpeed = 20;
        public int milliMove;
        public int charCursor;
        public int pageNumber;
        public List<string> says = new List<string>();
        public SoundEffect[] talksfxs;
        public List<string> pages = new List<string>();
        public SpriteFont selectedFont;
        public int interactionCount = 0;
        public Game1 game;
        public string output = "";
        public bool cutoff;
        public SoundEffectInstance sfx;
        KeyboardState oldState = Keyboard.GetState();
        int pretime = 0;

        public Textbox(List<string> says, bool cutoff, SoundEffect[] talksfxs, SpriteFont selectedFont,Game1 game)
        {
            this.game = game;
            this.says = says;
            this.talksfxs = talksfxs;
            this.cutoff = cutoff;
            this.selectedFont = selectedFont;
            
            //if (interactionCount > talks.Length) upcomingBoxes = talks[talks.Length - 1];
            //else upcomingBoxes = talks[interactionCount - 1];
            pageNumber = 0;
            milliMove = 0;
            charCursor = 0;
            this.selectedFont = selectedFont;
        }

        public void newTalk(GameTime gameTime)
        {
            pageNumber = 0;
            milliMove = 0;
            charCursor = 0;
            game.cutsceneMode = true;
            if(interactionCount < says.Count - 1)
            sfx = talksfxs[0].CreateInstance();
            if (sfx == null) sfx = game.placeHolderSounds[0].CreateInstance();
                sfx.IsLooped = false;
            sfx.Volume = 1f;
            sfx.Play();
            interactionCount++;
            pretime = gameTime.ElapsedGameTime.Milliseconds;
        }

        public void boxUpdate(GameTime gameTime)
        {
            string[] pages = says[interactionCount].Split('|');
            string tmp = pages[pageNumber];

            if (sfx == null && talksfxs[0] != null) { sfx = talksfxs[0].CreateInstance();
                sfx.Play();
            }
            int time;
            if (pretime > 30) time = pretime - gameTime.ElapsedGameTime.Milliseconds;
            else time = gameTime.ElapsedGameTime.Milliseconds;
            if (milliMove < cursorSpeed * tmp.Length) milliMove += time;
            else if (cutoff) sfx.Stop();
            charCursor = milliMove / cursorSpeed;
            KeyboardState newstate = Keyboard.GetState();
            if (newstate.IsKeyDown(Keys.E) && oldState.IsKeyUp(Keys.E))
            {
                if (milliMove < cursorSpeed * tmp.Length)
                {
                    //Normal update
                    milliMove = cursorSpeed * tmp.Length;

                }
                else if (pageNumber == pages.Length - 1)
                {
                    game.cutsceneMode = false;
                    if(sfx != null)sfx.Stop();
                }
                else
                {
                    pageNumber++;
                    charCursor = 0;
                    milliMove = 0;
                    if (talksfxs.Length > pageNumber)
                    {
                        sfx.Stop();
                        sfx = talksfxs[pageNumber].CreateInstance();
                        sfx.Play();
                    }
                    else
                    {
                        if(sfx != null)
                        sfx.Stop();
                        sfx = talksfxs[0].CreateInstance();
                        sfx.Play();
                    }

                }
            }

            oldState = newstate;
            output = tmp.Substring(0,charCursor);
        }
    }
}
