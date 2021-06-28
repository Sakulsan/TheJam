using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;
using System.Xml;
using Microsoft.Xna.Framework.Audio;
using System.IO;

namespace TheJam
{
    public class Game1 : Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;
        public Texture2D pixel;
        public List<Entity> entities = new List<Entity>();
        public SpriteFont arial;
        public List<(string, SpriteFont)> fonts = new List<(string, SpriteFont)>();
        public List<(string, Textbox)> boxes = new List<(string, Textbox)>();

        public SoundEffect currentMusic;
        public SoundEffectInstance music;
        SoundBank soundEffects;
        public Textbox currentBox;
        public bool cutsceneMode = false;

        public const int startX = 1;
        public const int startY = 0;

        public bool texting;

        public int moveFadeGaol = 0;
        public int moveFadethrough = 0;


        public int soundFadeGaol = 0;
        public int soundFadethrough = 0;

        public int scale = 128;
        public int offset = (1920 - 128*8) / 2;

        //Cutscenes

        public Texture2D nothing;

        Texture2D background;

        KeyboardState oldState;

        public string debug = "";

        public Map[,] World = new Map[5,5];
        public myPoint zoneCoordinates;

        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        protected override void Initialize()
        {
            // TODO: Add your initialization logic here
            IsMouseVisible = false;

            _graphics.PreferredBackBufferHeight = 1080;
            _graphics.PreferredBackBufferWidth = 1920;
            //_graphics.IsFullScreen = true;
            _graphics.ApplyChanges();

            

            Window.AllowUserResizing = true;
            base.Initialize();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            fonts.Add(("Arial", Content.Load<SpriteFont>(@"Fonts\Arial")));
            fonts.Add(("littletrobulegirl", Content.Load<SpriteFont>(@"Fonts\Penguin")));
            fonts.Add(("YellowMagician", Content.Load<SpriteFont>(@"Fonts\YellowMagician")));
            fonts.Add(("comic", Content.Load<SpriteFont>(@"Fonts\ComicSans")));
            fonts.Add(("bearpaw", Content.Load<SpriteFont>(@"Fonts\BEARPAW")));

            boxes.Add(("Penguin", new Textbox(new List<string>(ReadTextFile(@"dialooog\pinguin.txt").Split('&')), Content.Load<SpriteFont>(@"Fonts\Penguin"), this)));
            boxes.Add(("ocean", new Textbox(new List<string>(ReadTextFile(@"dialooog\ocean.txt").Split('&')), Content.Load<SpriteFont>(@"Fonts\BEARPAW"), this)));
            boxes.Add(("cliff", new Textbox(new List<string>(new[]{"cliff"}), Content.Load<SpriteFont>(@"Fonts\Arial"), this)));


            nothing = Content.Load<Texture2D>("Nothing");

            pixel = Content.Load<Texture2D>("Black");
            background = Content.Load<Texture2D>(@"ui\textbubble\text bubble fancy");
            //zone1Track = Content.Load<SoundEffect>(@"music\mainmusic1");
            //zone1Track.Play();

            oldState = Keyboard.GetState();

            zoneCoordinates = new myPoint(startX, startY);
            World = LoadZone1();

            Texture2D placeHolderTile = Content.Load<Texture2D>("john");
            //currentZone[0, 1].entities.Add(new LeaveTile(5,0,placeHolderTile,false,0,0,, this));


            //Entity e = entities.Find(test => test.x == 2 && test.y == 2);
            //    if (e != null) e.collision = true;


            // TODO: use this.Content to load your game content here
        }

        protected override void Update(GameTime gameTime)
        {
            

            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            Map current = World[zoneCoordinates.X, zoneCoordinates.Y];
            if (cutsceneMode) {
                currentBox.boxUpdate(gameTime);
            }
            else
            {
                foreach (Entity e in current.entities.FindAll(test => !(test is Player))) e.Update(gameTime, current.entities);
                Entity player = current.entities.Find(test => test is Player);
                if(player != null) player.Update(gameTime, current.entities);
                current.Update(gameTime);
                //if (Keyboard.GetState().IsKeyDown(Keys.C))
                //{
                //    //currentSay = "This is a placeHolder";
                //    cutsceneMode = true;
                //    charCursor = 0;
                //    milliMove = 0;
                //}

                if (moveFadeGaol - 17 > moveFadethrough) moveFadethrough += gameTime.ElapsedGameTime.Milliseconds;
                else if (moveFadeGaol + 17 < moveFadethrough) moveFadethrough -= gameTime.ElapsedGameTime.Milliseconds;
                else if (moveFadethrough != 0) moveFadeGaol = 0;
                // TODO: Add your update logic here


                //if (soundFadeGaol - 17 > soundFadethrough)
                //{
                //    soundFadethrough += gameTime.ElapsedGameTime.Milliseconds;
                //
                //}
                //else if (soundFadeGaol + 17 < soundFadethrough) soundFadethrough -= gameTime.ElapsedGameTime.Milliseconds;
                //else if (soundFadethrough != 0) soundFadeGaol = 0;
                //music.Pause();
                //music.Volume = (500 - soundFadethrough) / 500;
                //music.Resume();

                //BG anim

                if (current.frameLength != 0)
                {
                    current.millisLastFrame += (int)gameTime.ElapsedGameTime.TotalMilliseconds;

                    //int movedFrames = (int)Math.Floor(((decimal)millisLastFrame) / (decimal)frameLength);
                    if (current.millisLastFrame > current.frameLength)
                    {
                        current.currentFrame++;
                        int frameCount = current.background.Height / 128;
                        current.currentFrame %= frameCount;
                        current.millisLastFrame = 0;
                    }
                }
            }
            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.DimGray);



            _spriteBatch.Begin(samplerState: SamplerState.PointClamp);

            _spriteBatch.Draw(World[zoneCoordinates.X, zoneCoordinates.Y].background, new Rectangle(offset, 0, scale * 8, scale * 8)
                , new Rectangle(0, World[zoneCoordinates.X, zoneCoordinates.Y].currentFrame * 128,128,128), Color.White);
           

            foreach (Entity e in World[zoneCoordinates.X,zoneCoordinates.Y].entities) e.Draw(_spriteBatch);
            //_spriteBatch.DrawString(arial, $"{fadethrough} : {fadeGaol}", Vector2.Zero, Color.Black);
            _spriteBatch.DrawString(fonts[0].Item2, debug, Vector2.Zero, Color.Black);
            if (moveFadethrough != 0) _spriteBatch.Draw(pixel, new Vector2(0, 0), new Rectangle(0, 0, 1, 1), new Color(0, 0, 0, moveFadethrough), 0f, Vector2.Zero, 1920, SpriteEffects.None, 0.6f);

            //CutsceneMODE!
            if (cutsceneMode)
            {
                _spriteBatch.Draw(pixel, new Vector2(0, 0), new Rectangle(0, 0, 1, 1), new Color(0, 0, 0, 230), 0f, Vector2.Zero, 1920, SpriteEffects.None, 0.6f);
                _spriteBatch.Draw(background, new Vector2(offset + 12, 680),new Rectangle(0,0,100,32),Color.White, rotation: 0, Vector2.Zero, scale: 10f,SpriteEffects.None,layerDepth: 0.5f);
                
                _spriteBatch.DrawString(currentBox.selectedFont, currentBox.output, new Vector2(offset + 56, 800), Color.White);
            }
            if(currentBox != null)_spriteBatch.DrawString(fonts[0].Item2, currentBox.charCursor.ToString() + currentBox.interactionCount.ToString() + currentBox.milliMove.ToString(), Vector2.Zero, Color.Black);

            //_spriteBatch.DrawString(fonts[0].Item2, World[zoneCoordinates.X,zoneCoordinates.Y].entities.Find(test => test is Player).x.ToString() + World[zoneCoordinates.X, zoneCoordinates.Y].entities.Find(test => test is Player).y.ToString(), Vector2.Zero, Color.Black);
            _spriteBatch.End();


            // TODO: Add your drawing code here

            base.Draw(gameTime);
        }

        public string ReadTextFile(string path)
        {
            string value;
            using (var stream = TitleContainer.OpenStream(Path.Combine(Content.RootDirectory, path)))
            {
                using (var reader = new StreamReader(stream))
                {
                    value = reader.ReadToEnd();
                }
            }
            return value;
        }

        public Map[,] LoadZone1()
        {
            Map[,] v = new Map[5, 5];
            //generates a makeshift map
            Texture2D placeholder = Content.Load<Texture2D>("john");
            for (int i = 0; i < 5; i++)
            {
                for (int j = 0; j < 5; j++)
                {
                    Map generating;
                    if (i == 0 && j == 3)
                    {
                        generating = new Map(new List<Entity>(), placeholder, Content.Load<SoundEffect>(@"music\rocket2"), Map.WallType.Free, this);
                    }
                        else if (i == 0 && j == 0) generating = new Map(new List<Entity>(), placeholder, Content.Load<SoundEffect>(@"music\bossfight_pussel"), Map.WallType.Free, this);
                    else generating = new Map(new List<Entity>(),placeholder, 0, Content.Load < SoundEffect > (@"music\mainmusic1"),Map.WallType.Free,this);
                    generating.frameLength = 1000 / 4;


                    v[i, j] = generating;
                }
            }

            v[0,3].background = Content.Load<Texture2D>(@"Estetiska\skepp bg  0,3");
            v[0,3].entities.Add(new LeaveTile(2, 7, nothing, false, 1, 0, 3, 3, true, this));
            v[3,0].background = Content.Load<Texture2D>(@"Estetiska\öken bg 1,1");

            v[1, 2].background = Content.Load<Texture2D>(@"Estetiska\snövärld bg 1,2");

            v[0, 0].background = Content.Load<Texture2D>(@"Estetiska\snövärld bg 0,0");
            v[2, 0].background = Content.Load<Texture2D>(@"Estetiska\snövärld bg 2,0");
            v[1, 0].background = Content.Load<Texture2D>(@"Estetiska\snövärld bg 1,0");
            v[1, 0].entities.Add(new LeaveTile(3, 2, nothing, false, 0, 3, 2, 6, true, this));
            v[2, 1].background = Content.Load<Texture2D>(@"Estetiska\snövärld bg 2,1");
            v[0, 1].background = Content.Load<Texture2D>(@"Estetiska\snövärld bg 0,1");
            for (int i = 0; i < 8; i++)
            {
                v[0, 1].entities.Add(new TouchEntity(3,i,0,true,nothing,TouchEntity.Effects.Textbox,"ocean",this));
            }
            v[1, 1].background = Content.Load<Texture2D>(@"Estetiska\snövärld bg 1,1");
            v[0, 2].background = Content.Load<Texture2D>(@"Estetiska\snövärld bg 0,2");
            v[2, 2].background = Content.Load<Texture2D>(@"Estetiska\snövärld bg 2,2");
            v[startX, startY].entities.Add(new TouchEntity(1, 1, framerate: 6, 0, true,
                Content.Load<Texture2D>(@"Andra karaktärer\penguin"), TouchEntity.Effects.Textbox, "Penguin", this));

            v[2, 0].entities.Add(new TouchEntity(1, 1, framerate: 6, 0, true,
                Content.Load<Texture2D>(@"Andra karaktärer\crobo"), TouchEntity.Effects.Textbox, "crobo", this));

            v[startX, startY].entities.Add(new Player(3, 3, 6, Content.Load<Texture2D>(@"Main pig\Standing still"), this));




            return v;
        }
    }
}
