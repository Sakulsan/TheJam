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
        public Texture2D drownPig;
        public Texture2D ice;
        public Texture2D glow;

        public int bootOut = 200;

        public SoundEffectInstance sfx;
        public SoundEffect[] placeHolderSounds;

        public List<Entity> entities = new List<Entity>();
        public Dictionary<string, Entity> hud = new Dictionary<string, Entity>();

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
        public MoveSequence moving = null;
        public bool drowned = false;
        public bool Menu = true;

        public const int startX = 0;
        public const int startY = 3;

        public bool texting;

        public bool stolen;
        public List<string> achievments = new List<string>();


        public int moveFadeGaol = 0;
        public int moveFadethrough = 0;


        public int soundFadeGaol = 0;
        public int soundFadethrough = 0;

        public int scale = 128;
        public int offset = (1920 - 128 * 8) / 2;

        //Cutscenes

        public Texture2D nothing;

        Texture2D background;

        KeyboardState oldState;

        public string debug = "";

        public Map[,] World = new Map[5, 5];
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

            for (int i = 0; i < 6; i++)
            {
                achievments.Add("");
            }
            placeHolderSounds = new SoundEffect[] { Content.Load<SoundEffect>(@"ljud\ljudeffekter\joe\silence") };


            boxes.Add(("Penguin", new Textbox(new List<string>(ReadTextFile(@"dialooog\pinguin.txt").Split('&')), true, new[] { Content.Load<SoundEffect>(@"ljud\ljudeffekter\penguin\mainpenguin1") }, Content.Load<SpriteFont>(@"Fonts\Penguin"), this)));
            boxes.Add(("Mother", new Textbox(new List<string>(ReadTextFile(@"dialooog\pinguin.txt").Split('&')), true, new[] { Content.Load<SoundEffect>(@"ljud\ljudeffekter\penguin\mainpenguin2") }, Content.Load<SpriteFont>(@"Fonts\Penguin"), this)));
            boxes.Add(("Child", new Textbox(new List<string>(ReadTextFile(@"dialooog\pinguin.txt").Split('&')), true, new[] { Content.Load<SoundEffect>(@"ljud\ljudeffekter\penguin\babypenguin") }, Content.Load<SpriteFont>(@"Fonts\Penguin"), this)));
            boxes.Add(("ocean", new Textbox(new List<string>(ReadTextFile(@"dialooog\Pingvin_babu.txt").Split('&')), true, placeHolderSounds, Content.Load<SpriteFont>(@"Fonts\BEARPAW"), this)));
            boxes.Add(("Sarox", new Textbox(new List<string>(ReadTextFile(@"dialooog\Sarox.txt").Split('&')), true, new[] { Content.Load<SoundEffect>(@"ljud\ljudeffekter\dator") }, Content.Load<SpriteFont>(@"Fonts\Arial"), this)));
            boxes.Add(("bigD", new Textbox(new List<string>(ReadTextFile(@"dialooog\drake_stor.txt").Split('&')), true, new[] { Content.Load<SoundEffect>(@"ljud\ljudeffekter\dragon\dragon1") }, Content.Load<SpriteFont>(@"Fonts\Arial"), this)));
            
            boxes.Add(("cliff", new Textbox(new List<string>(new[] { "Thats seems unsteady..." }), true, placeHolderSounds, Content.Load<SpriteFont>(@"Fonts\Penguin"), this)));
            boxes.Add(("ship", new Textbox(new List<string>(new[]
            { "The ship has seen better days|A lot better days", "Like wow, this is sooo bad", "This is worse then that time I made a game in 48hours", "Maybe I should look for those batteries" }), true, placeHolderSounds, Content.Load<SpriteFont>(@"Fonts\Arial"), this)));
            boxes.Add(("Joe", new Textbox(new List<string>(new[] { ReadTextFile(@"dialooog\joe mama.txt") }), false, placeHolderSounds, Content.Load<SpriteFont>(@"Fonts\ComicSans"), this)));
            // 
            //


            boxes.Add(("BatteryBox1", new Textbox(new List<string>(ReadTextFile(@"dialooog\batteri text\tom batteri station.txt").Split('&')), true, placeHolderSounds, Content.Load<SpriteFont>(@"Fonts\Arial"), this)));
            boxes.Add(("crobo", new Textbox(new List<string>(new[] { "HAT!\nHAT!\nHAT!|HAT!HAT!HAT!\nHAT!HAT!HAT!\nHAT!HAT!HAT!" }), true, new[] { Content.Load<SoundEffect>(@"ljud\ljudeffekter\crab2") }, Content.Load<SpriteFont>(@"Fonts\Arial"), this)));
            boxes.Add(("frusenbat", new Textbox(new List<string>(ReadTextFile(@"dialooog\batteri text\Batteri i is.txt").Split('&')), false, placeHolderSounds, Content.Load<SpriteFont>(@"Fonts\Arial"), this)));
            //boxes.Add(("bat", new Textbox(new List<string>(ReadTextFile(@"dialooog\batteri text\batteri upp plockad.txt").Split('&')), Content.Load<SpriteFont>(@"Fonts\Arial"), this)));
            boxes.Add(("drogo", new Textbox(new List<string>(ReadTextFile(@"dialooog\liten_drake.txt").Split('&')), true, new[] { Content.Load<SoundEffect>(@"ljud\ljudeffekter\dragon\dragon2") }, Content.Load<SpriteFont>(@"Fonts\Arial"), this)));


            normalPig = Content.Load<Texture2D>(@"Main pig\Standing still");
            crawlPig = Content.Load<Texture2D>(@"Main pig\Crawling (1)");
            drownPig = Content.Load<Texture2D>(@"Main pig\Drowning");
            nothing = Content.Load<Texture2D>("Nothing");
            ice = Content.Load<Texture2D>(@"Andra ents\isblock");
            glow = Content.Load<Texture2D>(@"Andra ents\guldplatta");

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

            if (!Menu) {
                Map current = World[zoneCoordinates.X, zoneCoordinates.Y];

                if (blockCountDown < 0) blockCountDown = 0;
                if (blockCountDown != 0) blockCountDown -= gameTime.ElapsedGameTime.Milliseconds;

                Joe.Update(gameTime, entities);

                Entity player = current.entities.Find(test => test is Player);
                if (cutsceneMode)
                {
                    music.Volume = 0.3f;
                    if (sfx != null) sfx.Volume = 1f;

                    if (moving == null)
                    {

                        if (blockCountDown == 0)
                        {
                            currentBox.boxUpdate(gameTime);
                        }
                        else
                        {
                            foreach (Entity e in current.entities.FindAll(test => !(test is Player))) e.Update(gameTime, current.entities);
                            player.Update(gameTime, current.entities);
                        }
                    }
                    else
                    {
                        moving.Update(gameTime, this);
                    }
                }
                else
                {
                    if (zoneCoordinates.X == 0 && zoneCoordinates.Y == 0 && !achievments.Contains("Joe1") && !achievments.Contains("Slapped"))
                    {
                        Joe.deactivated = false;
                        World[0, 0].entities.Add(Joe);
                        sfx = Content.Load<SoundEffect>(@"ljud\ljudeffekter\joe\joe ice").CreateInstance();
                        sfx.Volume = 1f;
                        sfx.Play();
                        moving = new MoveSequence(Joe, new[]{

                        (1, 1),
                        (1, 1),
                        (1, 1),
                        (1, 0),
              (2, 0),
              (3, 0),
              (4, 0),
              (5, 0),
              (5, 1),
              (5, 2),
              (4, 2),
              (3, 2),
              (3, 3),
              (3, 4),
              (2, 4),
              (1, 4),
              (1, 5),
              (1, 6),
              (2, 6),
              (3, 6),
              (4, 6),
              (4, 7),
                    (5, 7)}, 640);
                        cutsceneMode = true;
                        currentBox = new Textbox(new List<string> { "*slap*" }, false, placeHolderSounds, fonts[0].Item2, this);
                        achievments.Add("Joe1");
                    }


                    if (achievments.Contains("Joe1") && !cutsceneMode)
                    {
                        moveFadeGaol = 255;
                        moveFadethrough = 255;
                        achievments.Remove("Joe1");
                        achievments.Add("Slapped");
                    }

                    if (achievments.Contains("JoeFavour"))
                    {
                        Joe.deactivated = false;
                        World[0, 0].entities.Add(Joe);
                        sfx = Content.Load<SoundEffect>(@"ljud\ljudeffekter\joe\joe ice").CreateInstance();
                        sfx.Play();
                        moving = new MoveSequence(Joe, new[]{
              (3, 6),
              (3, 6),
              (3, 6),
              (4, 6),
              (5, 6),
              (5, 5),
              (5, 4),
              (6, 4),
              (7, 4),
              (7, 3),
              (7, 2),
              (6, 2),
              (5, 2),
              (4, 2),
              (4, 3),
              (3, 3),
              (3, 4),
              (2, 4)}, 400);
                        cutsceneMode = true;
                        currentBox = new Textbox(new List<string> { "You'r turn" }, false, placeHolderSounds, fonts[0].Item2, this);
                        achievments.Remove("JoeFavour");
                    }

                    if (achievments.Contains("Drowned"))
                    {

                        moveFadeGaol = 255;
                        moveFadethrough = 50;


                        World[zoneCoordinates.X, zoneCoordinates.Y].entities.Remove(player);
                        foreach (Entity e in hud.Values)
                            World[zoneCoordinates.X, zoneCoordinates.Y].entities.Remove(e);

                        zoneCoordinates = new myPoint(0, 3);

                        World[zoneCoordinates.X, zoneCoordinates.Y].entities.Add(player);
                        foreach (Entity e in hud.Values)
                            World[zoneCoordinates.X, zoneCoordinates.Y].entities.Add(e);



                        current = World[zoneCoordinates.X, zoneCoordinates.Y];
                    }

                    if (achievments.Contains("Menu-boot"))
                    {
                        Menu = true;
                        achievments = new List<string>();
                        for (int i = 0; i < 5; i++)
                        {
                            achievments.Add("");
                        }
                        World = LoadZone1();
                        bootOut = 200;
                    }

                    if (achievments.Contains("Battery3")) {
                        cutsceneMode = true;
                        moveFadeGaol = 255;
                        currentBox = new Textbox(new List<string> { "You left the planet" }, false, placeHolderSounds, fonts[0].Item2, this);
                        achievments.Add("Menu-boot");
                        hud = new Dictionary<string, Entity>();
                    }

                    if (Keyboard.GetState().IsKeyDown(Keys.B)) achievments.Add("Bridge");
                    if (Keyboard.GetState().IsKeyDown(Keys.C)) achievments.Add("Claw");
                    if (achievments.Contains("Bridge"))
                    {
                        World[2, 0].entities.Find(x => x.collision == true && x.x == 5 && x.y == 2).deactivated = true;
                        World[1, 1].entities.Find(x => x is TouchEntity && ((TouchEntity)x).data == "Penguin").deactivated = true;
                        World[1, 0].entities.Find(x => x is TouchEntity && ((TouchEntity)x).data == "frusenbat").deactivated = true;
                        World[1, 0].entities.Find(x => x is TouchEntity && ((TouchEntity)x).data == "frusenbat").deactivated = true;
                        World[2, 0].entities.FindAll(x => x is TouchEntity && ((TouchEntity)x).data == "true").ForEach(x => x.deactivated = false);
                        World[3, 0].entities.FindAll(x => x is TouchEntity && ((TouchEntity)x).data == "true").ForEach(x => x.deactivated = false);
                        achievments.Remove("Bridge");
                        World[1, 1].background = Content.Load<Texture2D>(@"Estetiska\stoen bg 1,1");
                        World[1, 0].background = Content.Load<Texture2D>(@"Estetiska\stoen bg 1,0");
                        World[2, 0].background = Content.Load<Texture2D>(@"Estetiska\stoen bg 2,0");

                        Joe.deactivated = false;
                        Joe = new TouchEntity(false, 2, 6, 3, 0, true, Content.Load<Texture2D>(@"Andra karaktärer\joe ner"), TouchEntity.Effects.trade,
                            "Juice^JoeFavour^Thanks for that|I will show you the way^I am really thirsty", this);
                        Joe.deactivated = false;
                        World[3, 2].entities.Add(Joe);

                    }

                    if (music != null)
                        music.Volume = 1f;

                    //Map current = World[zoneCoordinates.X, zoneCoordinates.Y];

                    foreach (Entity e in current.entities.FindAll(test => !(test is Player))) e.Update(gameTime, current.entities);

                    if (player != null) player.Update(gameTime, current.entities);
                    current.Update(gameTime);

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
                    if (!cutsceneMode && achievments.Contains("Drowned"))
                    {
                        achievments.Remove("Drowned");
                        player.x = 3;
                        player.y = 4;
                    };
                }

                if (moving != null && moving.moving == false) moving = null;
            }
            else
            {
                if (bootOut >= 0) bootOut -= gameTime.ElapsedGameTime.Milliseconds;
                if (Keyboard.GetState().GetPressedKeys().Length > 0 && bootOut < 0)
                {
                    Menu = false;
                    zoneCoordinates = new myPoint(startX, startY);
                }

            }
            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            if(Menu)GraphicsDevice.Clear(Color.Turquoise);
            else GraphicsDevice.Clear(Color.DimGray);

            _spriteBatch.Begin(samplerState: SamplerState.PointClamp);
            if (!Menu)
            {
                _spriteBatch.Draw(World[zoneCoordinates.X, zoneCoordinates.Y].background, new Rectangle(offset, 0, scale * 8, scale * 8)
                , new Rectangle(0, World[zoneCoordinates.X, zoneCoordinates.Y].currentFrame * 128, 128, 128), Color.White);


                foreach (Entity e in World[zoneCoordinates.X, zoneCoordinates.Y].entities) e.Draw(_spriteBatch);
                //_spriteBatch.DrawString(arial, $"{fadethrough} : {fadeGaol}", Vector2.Zero, Color.Black);
                _spriteBatch.DrawString(fonts[0].Item2, debug, Vector2.Zero, Color.Black);
                if (moveFadethrough != 0) _spriteBatch.Draw(pixel, new Vector2(0, 0), new Rectangle(0, 0, 1, 1), new Color(0, 0, 0, moveFadethrough), 0f, Vector2.Zero, 1920, SpriteEffects.None, 0.6f);

                if (World[zoneCoordinates.X, zoneCoordinates.Y].entities.Contains(Joe)) Joe.Draw(_spriteBatch);
                //CutsceneMODE!
                if (cutsceneMode)
                {

                    if (moving == null)
                    {

                        if (blockCountDown == 0)
                        {
                            _spriteBatch.Draw(pixel, new Vector2(0, 0), new Rectangle(0, 0, 1, 1), new Color(0, 0, 0, 230), 0f, Vector2.Zero, 1920, SpriteEffects.None, 0.6f);

                            _spriteBatch.Draw(background, new Vector2(offset + 12, 680), new Rectangle(0, 0, 100, 32), Color.White, rotation: 0, Vector2.Zero, scale: 10f, SpriteEffects.None, layerDepth: 0.5f);

                            _spriteBatch.DrawString(currentBox.selectedFont, currentBox.output, new Vector2(offset + 56, 800), Color.White);
                        }
                    }
                    else
                    {

                    }
                }
                //if(currentBox != null)_spriteBatch.DrawString(fonts[0].Item2, currentBox.charCursor.ToString() + currentBox.interactionCount.ToString() + currentBox.milliMove.ToString(), Vector2.Zero, Color.Black);

                //for (int i = 0; i < achievments.Count; i++)
                //{
                //
                //    _spriteBatch.DrawString(fonts[0].Item2, achievments[i], new Vector2(0, i * 32), Color.White);
                //}
            }
            else
            {
                _spriteBatch.DrawString(fonts.Find(x => x.Item1 == "comic").Item2, "Tanha", new Vector2(offset + 64, 50), Color.Black,0,Vector2.Zero,8,SpriteEffects.None,0.5f);
                _spriteBatch.DrawString(fonts[0].Item2, "Press any key to start", new Vector2(offset + 360, 50), Color.Black);
                _spriteBatch.DrawString(fonts[0].Item2, "" +
                    "Roller:\n\n" +
"Lukas Vida Valentin Jacobson Hakola: game designer, \nspaghettiprogrammer, location provider and technical support.\n\n" +
"Astrid Kronflall: Game designer, enviroment design and misc \nart.\n\n" +
"Maya Uhlin Hellerstedt: game designer, enviroment design, \ndesigner of JOE and quest objects.\n\n" +
"Hiwa Otto Holst:game designer, character design, animation, \ncook and dialogue writer.\n\n" +
"Neddie boi Bergsrom: game designer, background music creator, \nsound effect recorder / editor, voice actor and dialogue writer.\n\n" +
"John Love Sandholm: game designer, background music creator, \nsound effect recorder / editor, voice actor and dialogue writer.\n\nFaranak Jasemynejad: Translator.", new Vector2(offset + 140, 450), Color.Black);
            }
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
                    else if (i >= 3) generating = new Map(new List<Entity>(), placeholder, Content.Load<SoundEffect>(@"ljud\music\sand_värld\sand"), Map.WallType.Free, this);

                    else generating = new Map(new List<Entity>(), placeholder, 0, Content.Load<SoundEffect>(@"ljud\music\is_värld\mainmusic1"), Map.WallType.Free, this);
                    generating.frameLength = 1000 / 4;


                    v[i, j] = generating;
                }
            }



            v[4, 2].background = Content.Load<Texture2D>(@"Estetiska\igo bg 4,2");
            v[4, 2].entities.Add(new LeaveTile(3, 7, nothing, false, 4, 0, 3, 5, true, this));
            v[4, 2].entities.Add(new LeaveTile(4, 7, nothing, false, 4, 0, 3, 5, true, this));
            v[4, 2].entities.Add(new TouchEntity(false, 1, 3, framerate: 6, 0, true,
                 Content.Load<Texture2D>(@"Andra karaktärer\Strong"), TouchEntity.Effects.Textbox, "Penguin", this));
            v[4, 2].entities.Add(new TouchEntity(false, 2, 3, framerate: 6, 0, true,
                Content.Load<Texture2D>(@"Andra karaktärer\Mother"), TouchEntity.Effects.Textbox, "Mother", this));
            v[4, 2].entities.Add(new TouchEntity(false, 1, 4, framerate: 6, 0, true,
                            Content.Load<Texture2D>(@"Andra karaktärer\Child  (1)"), TouchEntity.Effects.Textbox, "Child", this));
            for (int i = 0; i < 8; i++) v[4, 2].entities.Add(new TouchEntity(false, 8, i, 0, true, nothing, TouchEntity.Effects.Textbox, "cliff", this));
            for (int i = 0; i < 8; i++) v[4, 2].entities.Add(new TouchEntity(false, i, 8, 0, true, nothing, TouchEntity.Effects.Textbox, "ocean", this));
            for (int i = 0; i < 8; i++) v[4, 2].entities.Add(new TouchEntity(false, 0, i, 0, true, nothing, TouchEntity.Effects.Textbox, "cliff", this));
            for (int i = 0; i < 8; i++) v[4, 2].entities.Add(new TouchEntity(false, i, 1, 0, true, nothing, TouchEntity.Effects.Textbox, "ocean", this));
            v[4, 2].entities.Add(new TouchEntity(false, 7, 7, 0, true, nothing, TouchEntity.Effects.Textbox, "cliff", this));
            v[4, 2].entities.Add(new TouchEntity(false, 6, 7, 0, true, nothing, TouchEntity.Effects.Textbox, "cliff", this));
            v[4, 2].entities.Add(new TouchEntity(false, 1, 7, 0, true, nothing, TouchEntity.Effects.Textbox, "cliff", this));
            v[4, 2].entities.Add(new TouchEntity(false, 2, 7, 0, true, nothing, TouchEntity.Effects.Textbox, "cliff", this));
            v[4, 2].entities.Add(new TouchEntity(false, 5, 7, 0, true, nothing, TouchEntity.Effects.Textbox, "cliff", this));
            v[4, 2].entities.Add(new TouchEntity(false, 6, 6, 0, true, nothing, TouchEntity.Effects.Textbox, "cliff", this));
            v[4, 2].entities.Add(new TouchEntity(false, 7, 6, 0, true, nothing, TouchEntity.Effects.Textbox, "cliff", this));
            v[4, 2].entities.Add(new TouchEntity(false, 7, 5, 0, true, nothing, TouchEntity.Effects.Textbox, "cliff", this));
            v[4, 2].entities.Add(new TouchEntity(false, 1, 6, 0, true, nothing, TouchEntity.Effects.Textbox, "cliff", this));
            v[4, 2].entities.Add(new TouchEntity(false, 5, 4, 0, true, Content.Load<Texture2D>(@"Andra ents\batteri"), TouchEntity.Effects.trade, "^Battery3^^I have stolen the battery", this));


            v[4, 0].entities.Add(new TouchEntity(false, 3, 4, 0, true, nothing, TouchEntity.Effects.trade, "key^^It is locked|I will need a key^You have unlocked the Igloo", this));



            v[3, 0].background = Content.Load<Texture2D>(@"Estetiska\öken bg 0,0");

            for (int i = 3; i < 8; i++) v[3, 0].entities.Add(new TouchEntity(false, 0, i, 0, true, nothing, TouchEntity.Effects.Textbox, "ocean", this));
            for (int i = 0; i < 6; i++) v[3, 0].entities.Add(new TouchEntity(false, i, 8, 0, true, nothing, TouchEntity.Effects.Textbox, "ocean", this));
            for (int i = 0; i < 8; i++) v[3, 0].entities.Add(new TouchEntity(false, i, 0, 0, true, nothing, TouchEntity.Effects.Textbox, "ocean", this));
            v[3, 0].entities.Add(new TouchEntity(false, 0, 1, 0, true, nothing, TouchEntity.Effects.Textbox, "ocean", this));

            v[4, 0].background = Content.Load<Texture2D>(@"Estetiska\öken bg 1,0");
            v[4, 0].entities.Add(new LeaveTile(3, 4, nothing, false, 4, 2, 3, 6, true, this));
            for (int i = 0; i < 8; i++) v[4, 0].entities.Add(new TouchEntity(false, 7, i, 0, true, nothing, TouchEntity.Effects.Textbox, "ocean", this));
            for (int i = 0; i < 8; i++) v[4, 0].entities.Add(new TouchEntity(false, i, 0, 0, true, nothing, TouchEntity.Effects.Textbox, "ocean", this));
            v[4, 0].entities.Add(new TouchEntity(false, 2, 4, 0, true, nothing, TouchEntity.Effects.Textbox, "ocean", this));
            v[4, 0].entities.Add(new TouchEntity(false, 4, 4, 0, true, nothing, TouchEntity.Effects.Textbox, "ocean", this));
            v[4, 0].entities.Add(new TouchEntity(false, 2, 3, 0, true, nothing, TouchEntity.Effects.Textbox, "ocean", this));
            v[4, 0].entities.Add(new TouchEntity(false, 4, 3, 0, true, nothing, TouchEntity.Effects.Textbox, "ocean", this));
            v[4, 0].entities.Add(new TouchEntity(false, 2, 2, 0, true, nothing, TouchEntity.Effects.Textbox, "ocean", this));
            v[4, 0].entities.Add(new TouchEntity(false, 4, 2, 0, true, nothing, TouchEntity.Effects.Textbox, "ocean", this));
            v[4, 0].entities.Add(new TouchEntity(false, 2, 1, 0, true, nothing, TouchEntity.Effects.Textbox, "ocean", this));
            v[4, 0].entities.Add(new TouchEntity(false, 4, 1, 0, true, nothing, TouchEntity.Effects.Textbox, "ocean", this));
            v[4, 0].entities.Add(new TouchEntity(false, 5, 4, 0, true, nothing, TouchEntity.Effects.Textbox, "ocean", this));
            v[4, 0].entities.Add(new TouchEntity(false, 5, 3, 0, true, nothing, TouchEntity.Effects.Textbox, "ocean", this));
            v[4, 0].entities.Add(new TouchEntity(false, 5, 2, 0, true, nothing, TouchEntity.Effects.Textbox, "ocean", this));
            v[4, 0].entities.Add(new TouchEntity(false, 5, 1, 0, true, nothing, TouchEntity.Effects.Textbox, "ocean", this));


            v[0, 3].background = Content.Load<Texture2D>(@"Estetiska\skepp bg  0,3");
            v[0, 3].entities.Add(new LeaveTile(2, 7, nothing, false, 1, 0, 3, 3, true, this));
            v[0, 3].entities.Add(new TouchEntity(false, 1, 2, 1, true, Content.Load<Texture2D>(@"Andra ents\batterihållare tom"), TouchEntity.Effects.Lock, "battery^battery^battery^doneBats", this));
            v[0, 3].entities.Add(new TouchEntity(true, 1, 2, 1, true, Content.Load<Texture2D>(@"Andra ents\hatterihållare 1 batteri"), TouchEntity.Effects.Lock, "battery^battery^doneBats", this));
            v[0, 3].entities.Add(new TouchEntity(true, 1, 2, 1, true, Content.Load<Texture2D>(@"Andra ents\batterihållare 2 batterier"), TouchEntity.Effects.Lock, "battery^doneBats", this));
            v[0, 3].entities.Add(new TouchEntity(true, 1, 2, 1, true, Content.Load<Texture2D>(@"Andra ents\batterihållare full"), TouchEntity.Effects.Textbox, "doneBats", this));
            v[0, 3].entities.Add(new TouchEntity(false, 3, 2, 4, 1, true, Content.Load<Texture2D>(@"Andra karaktärer\karen"), TouchEntity.Effects.Textbox, "Sarox", this));
            v[0, 3].entities.Add(new TouchEntity(false, 2, 2, 4, 1, true, Content.Load<Texture2D>(@"mixer"), TouchEntity.Effects.trade, "Fruit^Juice^This is a nice mixer^*vrrrrrrrrr*|This is some nice juice", this));

            for (int i = 0; i < 8; i++) v[0, 3].entities.Add(new Entity(0, i, 0, true, nothing, this));
            for (int i = 0; i < 8; i++) v[0, 3].entities.Add(new Entity(i, 2, 0, true, nothing, this));
            for (int i = 0; i < 8; i++) v[0, 3].entities.Add(new Entity(7, i, 0, true, nothing, this));
            for (int i = 0; i < 8; i++) if (i != 2) v[0, 3].entities.Add(new Entity(i, 7, 0, true, nothing, this));



            v[4, 1].entities.Add(new TouchEntity(false, 2, 3, framerate: 4, 0, true,
                 Content.Load<Texture2D>(@"Andra karaktärer\top left"), TouchEntity.Effects.Textbox, "bigD", this));
            v[4, 1].entities.Add(new TouchEntity(false, 3, 3, framerate: 4, 0, true,
                 Content.Load<Texture2D>(@"Andra karaktärer\top right"), TouchEntity.Effects.Textbox, "bigD", this));
            v[4, 1].entities.Add(new TouchEntity(false, 2, 4, framerate: 4, 0, true,
                 Content.Load<Texture2D>(@"Andra karaktärer\bottom left"), TouchEntity.Effects.Textbox, "bigD", this));
            v[4, 1].entities.Add(new TouchEntity(false, 3, 4, framerate: 4, 0, true,
                 Content.Load<Texture2D>(@"Andra karaktärer\bottom right"), TouchEntity.Effects.Textbox, "bigD", this));


            v[4, 1].background = Content.Load<Texture2D>(@"Estetiska\öken bg 1,1");
            v[3, 1].entities.Add(new TouchEntity(false, 4, 6, framerate: 4, 0, true,
                 Content.Load<Texture2D>(@"Andra karaktärer\Drogo"), TouchEntity.Effects.trade, "^fire^^Here take my fire|Its hot idfk", this));
            v[3, 1].entities.Add(new TouchEntity(false, 4, 6, framerate: 4, 0, true,
                 Content.Load<Texture2D>(@"Andra karaktärer\Drogo"), TouchEntity.Effects.Textbox, "drogo", this));
            //v[4, 1].entities.Add(new TouchEntity(true, 3, 4, framerate: 4, 0, true,
            //     Content.Load<Texture2D>(@"Andra karaktärer\Drogo"), TouchEntity.Effects.Textbox, "drogo", this));

            for (int i = 0; i < 8; i++) v[4, 1].entities.Add(new TouchEntity(false, 7, i, 0, true, nothing, TouchEntity.Effects.Textbox, "cliff", this));
            for (int i = 0; i < 8; i++) v[4, 1].entities.Add(new TouchEntity(false, i, 7, 0, true, nothing, TouchEntity.Effects.Textbox, "ocean", this));
            v[4, 1].entities.Add(new TouchEntity(false, 6, 6, 0, true, nothing, TouchEntity.Effects.Textbox, "ocean", this));
            v[4, 1].entities.Add(new TouchEntity(false, 5, 6, 0, true, nothing, TouchEntity.Effects.Textbox, "ocean", this));



            v[3, 1].background = Content.Load<Texture2D>(@"Estetiska\öken bg 0,1");
            v[3, 1].entities.Add(new LeaveTile(3, 2, nothing, false, 3, 2, 4, 6, true, this));
            for (int i = 0; i < 8; i++) v[3, 1].entities.Add(new TouchEntity(false, 0, i, 0, true, nothing, TouchEntity.Effects.Textbox, "ocean", this));
            for (int i = 0; i < 8; i++) v[3, 1].entities.Add(new TouchEntity(false, i, 7, 0, true, nothing, TouchEntity.Effects.Textbox, "ocean", this));
            v[3, 1].entities.Add(new TouchEntity(false, 2, 6, 0, true, nothing, TouchEntity.Effects.Textbox, "ocean", this));
            v[3, 1].entities.Add(new TouchEntity(false, 1, 6, 0, true, nothing, TouchEntity.Effects.Textbox, "ocean", this));
            v[3, 1].entities.Add(new TouchEntity(false, 2, 3, 0, true, nothing, TouchEntity.Effects.Textbox, "ocean", this));
            v[3, 1].entities.Add(new TouchEntity(false, 1, 3, 0, true, nothing, TouchEntity.Effects.Textbox, "ocean", this));
            v[3, 1].entities.Add(new TouchEntity(false, 4, 3, 0, true, nothing, TouchEntity.Effects.Textbox, "ocean", this));
            v[3, 1].entities.Add(new TouchEntity(false, 5, 3, 0, true, nothing, TouchEntity.Effects.Textbox, "ocean", this));
            v[3, 1].entities.Add(new TouchEntity(false, 4, 2, 0, true, nothing, TouchEntity.Effects.Textbox, "ocean", this));
            v[3, 1].entities.Add(new TouchEntity(false, 5, 2, 0, true, nothing, TouchEntity.Effects.Textbox, "ocean", this));
            v[3, 1].entities.Add(new TouchEntity(false, 4, 1, 0, true, nothing, TouchEntity.Effects.Textbox, "ocean", this));
            v[3, 1].entities.Add(new TouchEntity(false, 5, 1, 0, true, nothing, TouchEntity.Effects.Textbox, "ocean", this));
            v[3, 1].entities.Add(new TouchEntity(false, 4, 0, 0, true, nothing, TouchEntity.Effects.Textbox, "ocean", this));
            v[3, 1].entities.Add(new TouchEntity(false, 5, 0, 0, true, nothing, TouchEntity.Effects.Textbox, "ocean", this));


            v[1, 2].background = Content.Load<Texture2D>(@"Estetiska\snövärld bg 1,2");
            for (int i = 0; i < 8; i++) v[1, 2].entities.Add(new TouchEntity(false, i, 5, 0, true, nothing, TouchEntity.Effects.Textbox, "ocean", this));

            //Puzzel 1
            {
                v[0, 0].background = Content.Load<Texture2D>(@"Estetiska\snövärld bg 0,0");
                TouchEntity[,] puzzleEnts = new TouchEntity[8, 8];
                for (int i = 0; i < 7; i++)
                {
                    for (int j = 0; j < 7; j++)
                    {
                        puzzleEnts[i, j] = new TouchEntity(false, i, j, framerate: 6, 0, false,
                        Content.Load<Texture2D>(@"Andra ents\isblock"), TouchEntity.Effects.Textbox, "fake", this);
                        v[0, 0].entities.Add(puzzleEnts[i, j]);
                    }
                }
            
                v[0, 0].entities.Remove(puzzleEnts[1, 1]);
                v[0, 0].entities.Remove(puzzleEnts[1, 2]);



                v[0, 0].entities.Remove(puzzleEnts[0, 0]);
                v[0, 0].entities.Remove(puzzleEnts[0, 2]);
                v[0, 0].entities.Remove(puzzleEnts[0, 5]);
                v[0, 0].entities.Remove(puzzleEnts[3, 5]);
                v[0, 0].entities.Remove(puzzleEnts[3, 5]);
                v[0, 0].entities.Remove(puzzleEnts[2, 3]);
                v[0, 0].entities.Remove(puzzleEnts[6, 0]);
                v[0, 0].entities.Remove(puzzleEnts[4, 1]);
                puzzleEnts[1, 0].data = "true";
                puzzleEnts[2, 0].data = "true";
                puzzleEnts[3, 0].data = "true";
                puzzleEnts[4, 0].data = "true";
                puzzleEnts[5, 0].data = "true";
                puzzleEnts[5, 1].data = "true";
                puzzleEnts[5, 2].data = "true";
                puzzleEnts[4, 2].data = "true";
                puzzleEnts[3, 2].data = "true";
                puzzleEnts[3, 3].data = "true";
                puzzleEnts[3, 4].data = "true";
                puzzleEnts[2, 4].data = "true";
                puzzleEnts[1, 4].data = "true";
                puzzleEnts[1, 5].data = "true";
                puzzleEnts[1, 6].data = "true";
                puzzleEnts[2, 6].data = "true";
                puzzleEnts[3, 6].data = "true";
                puzzleEnts[4, 6].data = "true";

                v[0, 0].entities.Add(new TouchEntity(false, 1, 2, 0, true, Content.Load<Texture2D>(@"Andra ents\batteri"),
                TouchEntity.Effects.trade, "^Bridge^^You have collected one of the 3 batteries", this));
                for (int i = 0; i < 8; i++) v[0, 0].entities.Add(new Entity(8, i, 0, true, nothing, this));
                for (int i = 0; i < 8; i++) v[0, 0].entities.Add(new Entity(-1, i, 0, true, nothing, this));
                for (int i = 0; i < 8; i++) v[0, 0].entities.Add(new Entity(i, -1, 0, true, nothing, this));
                for (int i = 0; i < 4; i++) v[0, 0].entities.Add(new Entity(i, 7, 0, true, nothing, this));
                v[0, 0].entities.Add(new Entity(7, 0, 0, true, nothing, this));
                v[0, 0].entities.Add(new Entity(7, 1, 0, true, nothing, this));

            }


            //Puzzel 2
            {
                v[3, 2].background = Content.Load<Texture2D>(@"Estetiska\tempe bg 3,2");
                v[3, 2].entities.Add(new LeaveTile(3, 7, nothing, false, 3, 1, 3, 3, true, this));
                v[3, 2].entities.Add(new LeaveTile(4, 7, nothing, false, 3, 1, 3, 3, true, this));


                TouchEntity[,] puzzleEnts = new TouchEntity[8, 8];
                for (int i = 0; i < 8; i++)
                {
                    for (int j = 2; j < 6; j++)
                    {
                        puzzleEnts[i, j] = new TouchEntity(false, i, j, framerate: 6, 0, false,
                        glow, TouchEntity.Effects.Textbox, "fake", this);
                        v[3, 2].entities.Add(puzzleEnts[i, j]);
                    }
                }

                v[3, 2].entities.Remove(puzzleEnts[1, 2]);
                v[3, 2].entities.Remove(puzzleEnts[1, 3]);
                v[3, 2].entities.Remove(puzzleEnts[0, 2]);
                v[3, 2].entities.Remove(puzzleEnts[0, 3]);
                v[3, 2].entities.Remove(puzzleEnts[1, 4]);
                v[3, 2].entities.Remove(puzzleEnts[0, 4]);
                v[3, 2].entities.Remove(puzzleEnts[1, 5]);
                v[3, 2].entities.Remove(puzzleEnts[0, 5]);

                puzzleEnts[5, 5].data = "true";
                puzzleEnts[5, 4].data = "true";
                puzzleEnts[6, 4].data = "true";
                puzzleEnts[7, 4].data = "true";
                puzzleEnts[7, 3].data = "true";
                puzzleEnts[7, 2].data = "true";
                puzzleEnts[6, 2].data = "true";
                puzzleEnts[5, 2].data = "true";
                puzzleEnts[4, 2].data = "true";
                puzzleEnts[4, 3].data = "true";
                puzzleEnts[3, 3].data = "true";
                puzzleEnts[3, 4].data = "true";
                puzzleEnts[2, 4].data = "true";


                v[3, 2].entities.Add(new TouchEntity(false, 1, 2, 0, true, Content.Load<Texture2D>(@"john"),
                TouchEntity.Effects.trade, "^key^^You can enter the igloo now|Cause why tf not|Dude we are running out of time", this));
                for (int i = 0; i < 8; i++) v[3, 2].entities.Add(new Entity(-1, i, 0, true, nothing, this));
                for (int i = 0; i < 8; i++) v[3, 2].entities.Add(new Entity(i, -1, 0, true, nothing, this));
                for (int i = 0; i < 8; i++) v[3, 2].entities.Add(new Entity(8, i, 0, true, nothing, this));
                for (int i = 0; i < 8; i++) v[3, 2].entities.Add(new Entity(i, 8, 0, true, nothing, this));
                v[0, 0].entities.Add(new Entity(7, 0, 0, true, nothing, this));
                v[0, 0].entities.Add(new Entity(7, 1, 0, true, nothing, this));

            }




            v[3, 0].entities.Add(new TouchEntity(false, 2, 5, 0, true, Content.Load<Texture2D>(@"Andra ents\kaktusfrukt"), TouchEntity.Effects.trade,
    "Claw^Fruit^That is a cactus^*snip*snip*", this));

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
            v[1, 0].entities.Add(new TouchEntity(false, 2, 2, 1, true, nothing, TouchEntity.Effects.Textbox, "ship", this));
            v[1, 0].entities.Add(new TouchEntity(false, 4, 2, 1, true, nothing, TouchEntity.Effects.Textbox, "ship", this));
            v[1, 0].entities.Add(new TouchEntity(false, 5, 2, 1, true, nothing, TouchEntity.Effects.Textbox, "ship", this));
            v[1, 0].entities.Add(new Entity(6, 2, 1, true, nothing, this));
            v[1, 0].entities.Add(new LeaveTile(3, 2, nothing, false, 0, 3, 2, 6, true, this));

            v[2, 1].background = Content.Load<Texture2D>(@"Estetiska\snövärld bg 2,1");
            for (int i = 0; i < 8; i++) v[2, 1].entities.Add(new TouchEntity(false, 4, i, 0, true, nothing, TouchEntity.Effects.Textbox, "ocean", this));

            v[0, 1].background = Content.Load<Texture2D>(@"Estetiska\snövärld bg 0,1");
            for (int i = 0; i < 8; i++) v[0, 1].entities.Add(new TouchEntity(false, 3, i, 0, true, nothing, TouchEntity.Effects.Textbox, "ocean", this));

            v[1, 1].background = Content.Load<Texture2D>(@"Estetiska\snövärld bg 1,1");
            v[0, 2].background = Content.Load<Texture2D>(@"Estetiska\snövärld bg 0,2");
            v[2, 2].background = Content.Load<Texture2D>(@"Estetiska\snövärld bg 2,2");
            for (int i = 0; i < 8; i++) v[2, 2].entities.Add(new TouchEntity(false, 4, i, 0, true, nothing, TouchEntity.Effects.Textbox, "ocean", this));
            for (int i = 0; i < 8; i++) v[2, 2].entities.Add(new TouchEntity(false, i, 5, 0, true, nothing, TouchEntity.Effects.Textbox, "ocean", this));


            v[1, 1].entities.Add(new TouchEntity(false, 2, 6, framerate: 6, 0, true,
                Content.Load<Texture2D>(@"Andra karaktärer\penguin"), TouchEntity.Effects.Textbox, "Penguin", this));

            v[2, 0].entities.Add(new TouchEntity(false, 1, 2, framerate: 6, 8, true,
                Content.Load<Texture2D>(@"Andra karaktärer\crobo"), TouchEntity.Effects.trade, "Top-Hat^Claw^HAT|I Want HAT^HAT!!!^Here take claw, cus I ahve HAT!", this));
            v[2, 0].entities.Add(new TouchEntity(true, 1, 2, framerate: 6, 0, true,
                Content.Load<Texture2D>(@"Andra ents\Crobo Hat"), TouchEntity.Effects.Textbox, "crobo", this));
            v[2, 0].entities.Add(new TouchEntity(true, 6, 2, framerate: 6, 0, false,
                Content.Load<Texture2D>(@"Andra ents\isblock"), TouchEntity.Effects.Textbox, "true", this));
            v[2, 0].entities.Add(new TouchEntity(true, 7, 2, framerate: 5, 0, false,
                Content.Load<Texture2D>(@"Andra ents\isblock"), TouchEntity.Effects.Textbox, "true", this));
            v[3, 0].entities.Add(new TouchEntity(true, 0, 2, framerate: 5, 0, false,
                Content.Load<Texture2D>(@"Andra ents\isblock"), TouchEntity.Effects.Textbox, "true", this));


            for (int i = 3; i < 8; i++) v[2, 0].entities.Add(new Entity(5, i, 0, true, nothing, this));
            for (int i = 2; i < 8; i++) v[2, 0].entities.Add(new Entity(i, 0, 0, true, nothing, this));
            v[2, 0].entities.Add(new Entity(5, 1, 0, true, nothing, this));
            v[2, 0].entities.Add(new Entity(4, 6, 0, true, nothing, this));
            v[2, 0].entities.Add(new Entity(4, 7, 0, true, nothing, this));
            v[2, 0].entities.Add(new Entity(6, 3, 0, true, nothing, this));
            v[2, 0].entities.Add(new Entity(7, 3, 0, true, nothing, this));
            v[2, 0].entities.Add(new Entity(6, 1, 0, true, nothing, this));
            v[2, 0].entities.Add(new Entity(7, 1, 0, true, nothing, this));
            v[2, 0].entities.Add(new Entity(5, 2, 0, true, nothing, this));

            v[startX, startY].entities.Add(new Player(3, 3, 6, normalPig, this));

            Joe = new TouchEntity(true, 1, 1, 6, 0, true, Content.Load<Texture2D>(@"Andra karaktärer\joe ner"), TouchEntity.Effects.Pickup, "Advice", this);

            v[1, 0].entities.Add(new TouchEntity(false, 4, 5, 0, true, Content.Load<Texture2D>(@"Andra ents\batteri i is"),
                TouchEntity.Effects.Textbox, "frusenbat", this));
            v[0, 2].entities.Add(new TouchEntity(false, 5, 2, 0, true, Content.Load<Texture2D>(@"Andra ents\batteri"),
                TouchEntity.Effects.Pickup, "battery", this));

            

            v[1, 2].entities.Add(new TouchEntity(false, 4, 3, 0, true, Content.Load<Texture2D>(@"Andra ents\topphatt i is"),
                TouchEntity.Effects.Lock, "fire^This seems too cold to acces^Top-Hat", this));
            v[1, 2].entities.Add(new TouchEntity(true, 4, 3, 0, true, Content.Load<Texture2D>(@"Andra ents\topphatt"),
                TouchEntity.Effects.Pickup, "Top-Hat", this));

            {
                Entity interactable = new Entity(0, 0, 0, false, Content.Load<Texture2D>(@"ui\ui\interact_maybe"), this);
                interactable.deactivated = true;
                hud.Add("E", interactable);
                v[startX, startY].entities.Add(interactable);
            }
            return v;
        }
    }
}
