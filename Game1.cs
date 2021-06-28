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
        public Texture2D normalPig;
        public Texture2D crawlPig;
        public Texture2D ice;

        public List<Entity> entities = new List<Entity>();
        public Entity Joe;
        public bool JoeHasApeared;
        public int blockCountDown = 0;

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

        public bool stolen;
        public List<string> achievments = new List<string>();


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
            //_graphics.GraphicsDevice.Viewport.Y = 10;
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

            boxes.Add(("cliff", new Textbox(new List<string>(new[] { "Thats seems unsteady..." }), Content.Load<SpriteFont>(@"Fonts\Arial"), this)));
            boxes.Add(("ship", new Textbox(new List<string>(new[]
            { "The ship has seen better days|A lot better days", "Like wow, this is sooo bad", "This is worse then that time I made a game in 48hours", "Maybe I should look for those batteries" }), Content.Load<SpriteFont>(@"Fonts\Arial"), this)));
            boxes.Add(("Joe", new Textbox(new List<string>(new[] { ReadTextFile(@"dialooog\joe mama.txt") }), Content.Load<SpriteFont>(@"Fonts\ComicSans"), this)));
            boxes.Add(("BatteryBox1", new Textbox(new List<string>(ReadTextFile(@"dialooog\batteri text\tom batteri station.txt").Split('&')), Content.Load<SpriteFont>(@"Fonts\Arial"), this)));
            boxes.Add(("crobo", new Textbox(new List<string>(new[] { "HAT|I Want HAT" }), Content.Load<SpriteFont>(@"Fonts\Arial"), this)));
            boxes.Add(("frusenbat", new Textbox(new List<string>(ReadTextFile(@"dialooog\batteri text\Batteri i is.txt").Split('&')), Content.Load<SpriteFont>(@"Fonts\Arial"), this)));
            //boxes.Add(("bat", new Textbox(new List<string>(ReadTextFile(@"dialooog\batteri text\batteri upp plockad.txt").Split('&')), Content.Load<SpriteFont>(@"Fonts\Arial"), this)));
            boxes.Add(("drogo", new Textbox(new List<string>(new[] { "Dragon noises" }), Content.Load<SpriteFont>(@"Fonts\Arial"), this)));

            
            normalPig = Content.Load<Texture2D>(@"Main pig\Standing still");
            crawlPig = Content.Load<Texture2D>(@"Main pig\Crawling (1)");
            nothing = Content.Load<Texture2D>("Nothing");
            ice = Content.Load<Texture2D>(@"Andra ents\isblock");

            pixel = Content.Load<Texture2D>("Black");
            background = Content.Load<Texture2D>(@"ui\textbubble\text bubble fancy");
            //zone1Track = Content.Load<SoundEffect>(@"music\mainmusic1");
            //zone1Track.Play();

            oldState = Keyboard.GetState();

            zoneCoordinates = new myPoint(startX, startY);
            World = LoadZone1();

            Texture2D placeHolderTile = Content.Load<Texture2D>("john");


            // TODO: use this.Content to load your game content here
        }

        protected override void Update(GameTime gameTime)
        {



            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();
            Joe = new Entity(0, 0, 0, false, nothing, this);
            Joe.Update(gameTime, entities);
            Map current = World[zoneCoordinates.X, zoneCoordinates.Y];


            if (Keyboard.GetState().IsKeyDown(Keys.J))
            {
                current.entities.Add(Joe);
                cutsceneMode = true;
                blockCountDown = 1000;
                currentBox = boxes.Find(test => test.Item1 == "Joe").Item2;
            }

            if (blockCountDown < 0) blockCountDown = 0;
            if (blockCountDown != 0) blockCountDown -= gameTime.ElapsedGameTime.Milliseconds;
            //if (blockCountDown < 500 || blockCountDown > 450) Joe.sprite = Content.Load();


            if (cutsceneMode)
            {

                if (blockCountDown == 0)
                {
                    currentBox.boxUpdate(gameTime);
                }
                music.Volume = 0.5f;
            }
            else
            {
                if (music != null)
                    music.Volume = 1f;

                //Map current = World[zoneCoordinates.X, zoneCoordinates.Y];
                if (cutsceneMode)
                {
                    currentBox.boxUpdate(gameTime);
                }
                else
                {

                    foreach (Entity e in current.entities.FindAll(test => !(test is Player))) e.Update(gameTime, current.entities);
                    Entity player = current.entities.Find(test => test is Player);
                    if (player != null) player.Update(gameTime, current.entities);
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

                if (blockCountDown == 0) _spriteBatch.Draw(pixel, new Vector2(0, 0), new Rectangle(0, 0, 1, 1), new Color(0, 0, 0, 230), 0f, Vector2.Zero, 1920, SpriteEffects.None, 0.6f);


                Joe.Draw(_spriteBatch);
                if (blockCountDown == 0)
                {
                    _spriteBatch.Draw(background, new Vector2(offset + 12, 680), new Rectangle(0, 0, 100, 32), Color.White, rotation: 0, Vector2.Zero, scale: 10f, SpriteEffects.None, layerDepth: 0.5f);

                    _spriteBatch.DrawString(currentBox.selectedFont, currentBox.output, new Vector2(offset + 56, 800), Color.White);
                }
            }
            //if(currentBox != null)_spriteBatch.DrawString(fonts[0].Item2, currentBox.charCursor.ToString() + currentBox.interactionCount.ToString() + currentBox.milliMove.ToString(), Vector2.Zero, Color.Black);

            for (int i = 0; i < achievments.Count; i++)
            {

                _spriteBatch.DrawString(fonts[0].Item2, achievments[i], new Vector2(0,i*32), Color.White);
                }

/*
                _spriteBatch.Draw(pixel, new Vector2(0, 0), new Rectangle(0, 0, 1, 1), new Color(0, 0, 0, 230), 0f, Vector2.Zero, 1920, SpriteEffects.None, 0.6f);
                _spriteBatch.Draw(background, new Vector2(offset + 12, 680),new Rectangle(0,0,100,32),Color.White, rotation: 0, Vector2.Zero, scale: 10f,SpriteEffects.None,layerDepth: 0.5f);
                
                _spriteBatch.DrawString(currentBox.selectedFont, currentBox.output, new Vector2(offset + 56, 800), Color.White);
            }
            if(currentBox != null)_spriteBatch.DrawString(fonts[0].Item2, currentBox.charCursor.ToString() + currentBox.interactionCount.ToString() + currentBox.milliMove.ToString(), Vector2.Zero, Color.Black);
            */

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
                        generating = new Map(new List<Entity>(), placeholder, Content.Load<SoundEffect>(@"ljud\music\rymdskepp\rocket2"), Map.WallType.Free, this);
                    }
                    else if (i == 0 && j == 0) generating = new Map(new List<Entity>(), placeholder, Content.Load<SoundEffect>(@"ljud\music\is_värld\bossfight_pussel"), Map.WallType.Free, this);
                    else if (i == 3 && j == 2) generating = new Map(new List<Entity>(), placeholder, Content.Load<SoundEffect>(@"ljud\music\sand_värld\Tempel"), Map.WallType.Free, this);

                    else generating = new Map(new List<Entity>(),placeholder, 0, Content.Load < SoundEffect > (@"ljud\music\is_värld\mainmusic1"),Map.WallType.Free,this);
                    generating.frameLength = 1000 / 4;


                    v[i, j] = generating;
                }
            }

            v[3, 2].background = Content.Load<Texture2D>(@"Estetiska\tempe bg 3,2");
            v[3, 2].entities.Add(new LeaveTile(3, 7, nothing, false, 3, 1, 3, 3, true, this));
            v[3, 2].entities.Add(new LeaveTile(4, 7, nothing, false, 3, 1, 3, 3, true, this));


            v[4, 2].background = Content.Load<Texture2D>(@"Estetiska\igo bg 4,2");
            v[4, 2].entities.Add(new LeaveTile(3, 7, nothing, false, 4, 0, 3, 5, true, this));
            v[4, 2].entities.Add(new LeaveTile(4, 7, nothing, false, 4, 0, 3, 5, true, this));
            v[4, 2].entities.Add(new TouchEntity(false, 1, 3, framerate: 6, 0, true,
                 Content.Load<Texture2D>(@"Andra karaktärer\penguin"), TouchEntity.Effects.Textbox, "Penguin", this));
            v[4, 2].entities.Add(new TouchEntity(false, 2, 3, framerate: 6, 0, true,
                Content.Load<Texture2D>(@"Andra karaktärer\Mother"), TouchEntity.Effects.Textbox, "Penguin", this));
            v[4, 2].entities.Add(new TouchEntity(false, 1, 4, framerate: 6, 0, true,
                            Content.Load<Texture2D>(@"Andra karaktärer\Child  (1)"), TouchEntity.Effects.Textbox, "Penguin", this));



            v[4, 0].background = Content.Load<Texture2D>(@"Estetiska\öken bg 1,0");
            v[4, 0].entities.Add(new LeaveTile(3, 4, nothing, false, 4, 2, 3, 6, true, this));
            for (int i = 0; i < 8; i++) v[4, 0].entities.Add(new Entity(7, i, 0, true, nothing, this));
            for (int i = 0; i < 8; i++) v[4, 0].entities.Add(new Entity(i, 0, 0, true, nothing, this));

            v[0,3].background = Content.Load<Texture2D>(@"Estetiska\skepp bg  0,3");
            v[0,3].entities.Add(   new LeaveTile( 2, 7, nothing, false, 1, 0, 3, 3, true, this));
            v[0,3].entities.Add( new TouchEntity(false, 1, 2, 1, true, Content.Load<Texture2D>(@"Andra ents\batterihållare tom"), TouchEntity.Effects.Textbox, "BatteryBox1", this));
            v[0, 3].entities.Add(new TouchEntity(false, 3, 2,4, 1, true, Content.Load<Texture2D>(@"Andra karaktärer\karen"), TouchEntity.Effects.Textbox, "karen", this));

            for (int i = 0; i < 8; i++) v[0, 3].entities.Add(new Entity(0, i, 0, true, nothing, this));
            for (int i = 0; i < 8; i++) v[0, 3].entities.Add(new Entity(i, 2, 0, true, nothing, this));
            for (int i = 0; i < 8; i++) v[0, 3].entities.Add(new Entity(7, i, 0, true, nothing, this));
            for (int i = 0; i < 8; i++) if(i != 2)v[0, 3].entities.Add(new Entity(i, 7, 0, true, nothing, this));


            v[4,1].background = Content.Load<Texture2D>(@"Estetiska\öken bg 1,1"); 
            v[4, 1].entities.Add(new TouchEntity(false, 3, 4, framerate: 4, 0, true,
                 Content.Load<Texture2D>(@"Andra karaktärer\Drogo"), TouchEntity.Effects.Pickup, "fire^drogo", this));
            v[4, 1].entities.Add(new TouchEntity(true, 3, 4, framerate: 4, 0, true,
                 Content.Load<Texture2D>(@"Andra karaktärer\Drogo"), TouchEntity.Effects.Textbox, "drogo", this));


            v[3, 1].background = Content.Load<Texture2D>(@"Estetiska\öken bg 0,1");
            v[3, 1].entities.Add(new LeaveTile(3, 2, nothing, false, 3, 2, 4, 6, true, this));


            v[1, 2].background = Content.Load<Texture2D>(@"Estetiska\snövärld bg 1,2"); 
            for (int i = 0; i < 8; i++) v[1, 2].entities.Add(new TouchEntity(false, i, 5, 0, true, nothing, TouchEntity.Effects.Textbox, "ocean", this));


            v[0, 0].background = Content.Load<Texture2D>(@"Estetiska\snövärld bg 0,0");
            v[2, 0].background = Content.Load<Texture2D>(@"Estetiska\snövärld bg 2,0");
            for (int i = 3; i < 8; i++) v[2, 0].entities.Add(new TouchEntity(false, -1, i, 0, true, nothing, TouchEntity.Effects.Textbox, "cliff", this));
            for (int i = 0; i < 8; i++) v[2, 0].entities.Add(new TouchEntity(false, i, 0, 0, true, nothing, TouchEntity.Effects.Textbox, "ocean", this));
            for (int i = 0; i < 8; i++) v[2, 0].entities.Add(new TouchEntity(false, i, -1, 0, true, nothing, TouchEntity.Effects.Textbox, "ocean", this));



            for (int i = 0; i < 8; i++) v[0, 2].entities.Add(new TouchEntity(false, i, 5, 0, true, nothing, TouchEntity.Effects.Textbox, "ocean", this));
            for (int i = 0; i < 8; i++) v[0, 2].entities.Add(new TouchEntity(false, 3, i, 0, true, nothing, TouchEntity.Effects.Textbox, "ocean", this));


            v[1, 0].background = Content.Load<Texture2D>(@"Estetiska\snövärld bg 1,0");
            for (int i = 0; i < 8; i++)
                v[1, 0].entities.Add(new TouchEntity(false, 0, i, 1, true, nothing, TouchEntity.Effects.Textbox, "cliff", this));
            for (int i = 3; i < 8; i++)
                v[1, 0].entities.Add(new TouchEntity(false, 7, i, 1, true, nothing, TouchEntity.Effects.Textbox, "cliff", this));
            v[1, 0].entities.Add(new TouchEntity(false, 7, -1, 1, true, nothing, TouchEntity.Effects.Textbox, "cliff", this));
            v[1, 0].entities.Add(new TouchEntity(false, 6, -1, 1, true, nothing, TouchEntity.Effects.Textbox, "cliff", this));
            v[1, 0].entities.Add(new TouchEntity(false, 5, 0, 1, true, nothing, TouchEntity.Effects.Textbox, "cliff", this));
            v[1, 0].entities.Add(new TouchEntity(false, 5, 1, 1, true, nothing, TouchEntity.Effects.Textbox, "cliff", this));
            v[1, 0].entities.Add(new Entity(1, 2, 1, true, nothing, this));
            v[1, 0].entities.Add(new TouchEntity(false, 2, 2, 1, true, nothing,TouchEntity.Effects.Textbox,"ship", this));
            v[1, 0].entities.Add(new TouchEntity(false, 4, 2, 1, true, nothing, TouchEntity.Effects.Textbox, "ship", this));
            v[1, 0].entities.Add(new TouchEntity(false, 5, 2, 1, true, nothing, TouchEntity.Effects.Textbox, "ship", this));
            v[1, 0].entities.Add(new Entity(6, 2, 1, true, nothing, this));
            v[1, 0].entities.Add(new LeaveTile(3, 2, nothing, false, 0, 3, 2, 6, true, this));

            v[2, 1].background = Content.Load<Texture2D>(@"Estetiska\snövärld bg 2,1");
            for (int i = 0; i < 8; i++) v[2, 1].entities.Add(new TouchEntity(false, 4, i, 0, true, nothing, TouchEntity.Effects.Textbox, "ocean", this));

            v[0, 1].background = Content.Load<Texture2D>(@"Estetiska\snövärld bg 0,1");
            for (int i = 0; i < 8; i++)v[0, 1].entities.Add(new TouchEntity(false, 3,i,0,true,nothing,TouchEntity.Effects.Textbox,"ocean",this));
            
            v[1, 1].background = Content.Load<Texture2D>(@"Estetiska\snövärld bg 1,1");
            v[0, 2].background = Content.Load<Texture2D>(@"Estetiska\snövärld bg 0,2");
            v[2, 2].background = Content.Load<Texture2D>(@"Estetiska\snövärld bg 2,2");
            for (int i = 0; i < 8; i++) v[2, 2].entities.Add(new TouchEntity(false, 4, i, 0, true, nothing, TouchEntity.Effects.Textbox, "ocean", this));
            for (int i = 0; i < 8; i++) v[2, 2].entities.Add(new TouchEntity(false, i, 5, 0, true, nothing, TouchEntity.Effects.Textbox, "ocean", this));


            v[1, 1].entities.Add(new TouchEntity(false, 2, 6, framerate: 6, 0, true,
                Content.Load<Texture2D>(@"Andra karaktärer\penguin"), TouchEntity.Effects.Textbox, "Penguin", this));

            v[2, 0].entities.Add(new TouchEntity(false, 1, 2, framerate: 6, 0, true,
                Content.Load<Texture2D>(@"Andra karaktärer\crobo"), TouchEntity.Effects.Textbox, "crobo", this));
            v[2, 0].entities.Add(new TouchEntity(false, 6, 2, framerate: 6, 0, false,
                Content.Load<Texture2D>(@"Andra ents\isblock"), TouchEntity.Effects.Textbox, "crobo", this));
            v[2, 0].entities.Add(new TouchEntity(false, 7, 2, framerate: 5, 0, false,
                Content.Load<Texture2D>(@"Andra ents\isblock"), TouchEntity.Effects.Textbox, "crobo", this));

            v[startX, startY].entities.Add(new Player(3, 3, 6, normalPig, this));

            v[0, 1].entities.Add(new TouchEntity(false, 2,2,0,true, Content.Load<Texture2D>(@"Andra ents\isblock"),
                TouchEntity.Effects.Pickup,"fire",this));

/*
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
            
            v[startX, startY].entities.Add(new Player(3, 3, 6, Content.Load<Texture2D>(@"Main pig\Standing still"), this));

*/
            v[1, 0].entities.Add(new TouchEntity(false, 4, 5, 0, true, Content.Load<Texture2D>(@"Andra ents\batteri i is"),
                TouchEntity.Effects.Textbox, "frusenbat", this));
            v[0, 2].entities.Add(new TouchEntity(false, 5, 2, 0, true, Content.Load<Texture2D>(@"Andra ents\batteri"),
                TouchEntity.Effects.Pickup, "battery", this));

            v[1,2].entities.Add(new TouchEntity(false, 4, 3, 0, true, Content.Load<Texture2D>(@"Andra ents\topphatt i is"),
                TouchEntity.Effects.Lock, "fire^Top-Hat", this));
            v[1, 2].entities.Add(new TouchEntity(true, 4, 3, 0, true, Content.Load<Texture2D>(@"Andra ents\topphatt"),
                TouchEntity.Effects.Pickup, "Top-Hat", this));

            return v;
        }
    }
}
