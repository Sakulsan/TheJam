using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;
using System.Xml;
using Microsoft.Xna.Framework.Audio;

namespace TheJam
{
    public class Game1 : Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;
        public Texture2D pixel;
        public List<Entity> entities = new List<Entity>();
        public SpriteFont arial;

        public SoundEffect zone1Track;
        public int millisplaying;

        public bool texting;

        public int fadeGaol = 0;
        public int fadethrough = 0;

        public int scale = 128;
        public int offset = (1920 - 128*8) / 2;

        //Cutscenes
        bool cutsceneMode;
        int milliPause;
        Texture2D background;

        KeyboardState oldState;

        public string debug = "";

        public Map[,] currentZone = new Map[3,3];
        public Point zoneCoordinates = new Point(1, 1);

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

            arial = Content.Load<SpriteFont>(@"Arial");
            pixel = Content.Load<Texture2D>("Black");
            background = Content.Load<Texture2D>(@"ui\textbubble\text bubble fancy");
            zone1Track = Content.Load<SoundEffect>(@"music\mainmusic1");
            zone1Track.Play();

            oldState = Keyboard.GetState();

            currentZone = LoadZone1();

            Texture2D placeHolderTile = Content.Load<Texture2D>("Placeholder");
            //currentZone[0, 1].entities.Add(new LeaveTile(5,0,placeHolderTile,false,0,0,, this));


            //Entity e = entities.Find(test => test.x == 2 && test.y == 2);
            //    if (e != null) e.collision = true;


            // TODO: use this.Content to load your game content here
        }

        protected override void Update(GameTime gameTime)
        {
            

            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            Map current = currentZone[zoneCoordinates.X, zoneCoordinates.Y];
            if (cutsceneMode) {

                KeyboardState newstate = Keyboard.GetState();
                if (newstate.IsKeyDown(Keys.E) && oldState.IsKeyUp(Keys.E)) cutsceneMode = false;
                oldState = newstate;

            }
            else
            {
                foreach (Entity e in current.entities) e.Update(gameTime, currentZone[zoneCoordinates.X, zoneCoordinates.Y].entities);

                if (Keyboard.GetState().IsKeyDown(Keys.C)) cutsceneMode = true;

                if (Keyboard.GetState().IsKeyDown(Keys.F)) fadeGaol = 255;
                if (fadeGaol - 17 > fadethrough) fadethrough += gameTime.ElapsedGameTime.Milliseconds;
                else if (fadeGaol + 17 < fadethrough) fadethrough -= gameTime.ElapsedGameTime.Milliseconds;
                else if (fadethrough != 0) fadeGaol = 0;
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
                millisplaying += gameTime.ElapsedGameTime.Milliseconds;
                if (millisplaying > 54000)
                {
                    zone1Track.Play();
                    millisplaying = 0;
                }
            }
            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.DimGray);



            _spriteBatch.Begin(samplerState: SamplerState.PointClamp);

            _spriteBatch.Draw(currentZone[zoneCoordinates.X, zoneCoordinates.Y].background, new Rectangle(offset, 0, scale * 8, scale * 8)
                , new Rectangle(0, currentZone[zoneCoordinates.X, zoneCoordinates.Y].currentFrame * 128,128,128), Color.White);
            //if (true)
            //{
            //    //_spriteBatch.DrawString(arial, "Heyo", Vector2.Zero, Color.Black, 0f, Vector2.Zero, 5, SpriteEffects.None, 0.8f);
            //    //_spriteBatch.Draw(entities[1].sprite, new Rectangle(0,0,64,64), new Rectangle(0, 0, entities[1].sprite.Width, entities[1].sprite.Height), Color.White, rotation: 0f, origin: Vector2.Zero, effects: SpriteEffects.None, layerDepth: 0);
            //
            //    //
            //}


            

            foreach (Entity e in currentZone[zoneCoordinates.X,zoneCoordinates.Y].entities) e.Draw(_spriteBatch);
            //_spriteBatch.DrawString(arial, $"{fadethrough} : {fadeGaol}", Vector2.Zero, Color.Black);
            _spriteBatch.DrawString(arial, debug, Vector2.Zero, Color.Black);
            if (fadethrough != 0) _spriteBatch.Draw(pixel, new Vector2(0, 0), new Rectangle(0, 0, 1, 1), new Color(0, 0, 0, fadethrough), 0f, Vector2.Zero, 1920, SpriteEffects.None, 0.6f);

            if (cutsceneMode)
            {
                _spriteBatch.Draw(pixel, new Vector2(0, 0), new Rectangle(0, 0, 1, 1), new Color(0, 0, 0, 200), 0f, Vector2.Zero, 1920, SpriteEffects.None, 0.6f);
                _spriteBatch.Draw(background, new Vector2(offset + 12, 500),new Rectangle(0,0,100,50),Color.White, rotation: 0, Vector2.Zero, scale: 10f,SpriteEffects.None,layerDepth: 0.5f);
                _spriteBatch.DrawString(arial, "pls show", new Vector2(offset + 16, 800), Color.White);
            }

            _spriteBatch.End();



            // TODO: Add your drawing code here

            base.Draw(gameTime);
        }

        public void textUpdate(GameTime gt)
        {

        }

        public Map[,] LoadZone1()
        {
            Map[,] v = new Map[3, 3];
            //generates a makeshift map
            Texture2D nothing = Content.Load<Texture2D>("Nothing");
            for (int i = 0; i < 3; i++)
            {
                for (int j = 0; j < 3; j++)
                {
                    Map generating = new Map(new List<Entity>(), Content.Load<Texture2D>("Placeholder"));
                    generating.entities.Add(new Player(1, 1, 6,Content.Load<Texture2D>(@"Main pig\Standing still"), this));
                    //generating.entities.Add(new TouchEntity(4, 0, 5, true, Content.Load<Texture2D>("Button"), TouchEntity.Effects.Textbox, "Hello world", this));
                    
                    for (int u = 0; u < 9; u++) generating.entities.Add(new Entity(-1, u, 0, true, nothing, this));
                    for (int u = 0; u < 8; u++) generating.entities.Add(new Entity(u, -1, 0, true, nothing, this));
                    for (int u = 0; u < 9; u++) generating.entities.Add(new Entity(8, u, 0, true, nothing, this));
                    for (int u = 0; u < 8; u++) generating.entities.Add(new Entity(u, 8, 0, true, nothing, this));


                    v[i, j] = generating;
                }
            }
            v[1, 2].background = Content.Load<Texture2D>(@"Estetiska\snövärld bg 1,2");
            v[1, 2].entities.Add(new LeaveTile(4, 0, Content.Load<Texture2D>("Placeholder"), false, 1, 1, 4, 7, this));
            v[1, 2].frameLength = 1000 / 4;

            v[2, 1].background = Content.Load<Texture2D>(@"Estetiska\snövärld bg 2,1");
            v[2, 1].entities.Add(new LeaveTile(0, 2, Content.Load<Texture2D>("Placeholder"), false, 1, 1, 7, 2, this));
            v[2, 1].frameLength = 1000 / 4;

            v[0, 1].background = Content.Load<Texture2D>(@"Estetiska\snövärld bg 0,1");
            v[0, 1].entities.Add(new LeaveTile(7, 2, Content.Load<Texture2D>("Placeholder"), false, 1, 1, 1, 6, this));
            v[0, 1].frameLength = 1000 / 4;

            v[1, 1].background = Content.Load<Texture2D>(@"Estetiska\snövärld bg 1,1");
            v[1, 1].entities.Add(new LeaveTile(4, 7, Content.Load<Texture2D>("Placeholder"), false, 1, 2,4,0, this));
            v[1, 1].entities.Add(new LeaveTile(7, 2, Content.Load<Texture2D>("Placeholder"), false, 2, 1, 0, 2, this));
            v[1, 1].entities.Add(new LeaveTile(0, 6, Content.Load<Texture2D>("Placeholder"), false, 0, 1, 7, 2, this));

            return v;
        }
    }
}
