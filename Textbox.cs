using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework;

namespace TheJam
{
    public class Textbox
    {
        const int cursorSpeed = 20;
        public int milliMove;
        public int charCursor;
        public int pageNumber;
        public List<string> says = new List<string>();
        public List<string> pages = new List<string>();
        public SpriteFont selectedFont;
        public int interactionCount = 0;
        public Game1 game;
        public string output = "";
        KeyboardState oldState = Keyboard.GetState();

        public Textbox(List<string> says, SpriteFont selectedFont, Game1 game)
        {
            this.game = game;
            this.says = says;
            this.selectedFont = selectedFont;
            
            //if (interactionCount > talks.Length) upcomingBoxes = talks[talks.Length - 1];
            //else upcomingBoxes = talks[interactionCount - 1];
            pageNumber = 0;
            milliMove = 0;
            charCursor = 0;
            this.selectedFont = selectedFont;
        }

        public void newTalk()
        {
            pageNumber = 0;
            milliMove = 0;
            charCursor = 0;
            game.cutsceneMode = true;
            if(interactionCount < says.Count - 1)
            interactionCount++;
        }

        public void boxUpdate(GameTime gameTime)
        {
            
            

            //if (says[interactionCount].Contains('|'))
            //{
            string[] pages = says[interactionCount].Split('|');
            string tmp = pages[pageNumber];
            //}
            //else
            //{
            //    pages = new[] { says[0] };
            //    tmp = says[0];
            //}
            

            if (milliMove < cursorSpeed * tmp.Length) milliMove += gameTime.ElapsedGameTime.Milliseconds;
            charCursor = milliMove / cursorSpeed;
            KeyboardState newstate = Keyboard.GetState();
            if (newstate.IsKeyDown(Keys.Enter) && oldState.IsKeyUp(Keys.Enter))
            {
                if (milliMove < cursorSpeed * tmp.Length)
                {
                    //Normal update
                    milliMove = cursorSpeed * tmp.Length;

                }
                else if (pageNumber == pages.Length - 1) game.cutsceneMode = false;
                else
                {
                    pageNumber++;
                    charCursor = 0;
                    milliMove = 0;
                }
            }

            oldState = newstate;
            output = tmp.Substring(0,charCursor);
        }
    }
}
