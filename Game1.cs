using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;
using System.Xml;

namespace TheJam
{
    public class Game1 : Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;
        public Texture2D pixel;
        public List<Entity> entities = new List<Entity>();
        public SpriteFont arial;

        public bool texting;

        public int fadeGaol = 0;
        public int fadethrough = 0;

        public int scale = 128;
        public int offset = 0;

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

            foreach (Entity e in currentZone[zoneCoordinates.X, zoneCoordinates.Y].entities) e.Update(gameTime, currentZone[zoneCoordinates.X,zoneCoordinates.Y].entities);

            if(Keyboard.GetState().IsKeyDown(Keys.F)) fadeGaol = 255;
            if (fadeGaol - 17 > fadethrough) fadethrough += gameTime.ElapsedGameTime.Milliseconds;
            else if (fadeGaol + 17 < fadethrough) fadethrough -= gameTime.ElapsedGameTime.Milliseconds;
            else if (fadethrough != 0) fadeGaol= 0;
            // TODO: Add your update logic here

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);



            _spriteBatch.Begin(samplerState: SamplerState.PointClamp);

            _spriteBatch.Draw(currentZone[zoneCoordinates.X, zoneCoordinates.Y].background, new Rectangle(0,0,scale*8,scale*8), Color.White);
            //if (true)
            //{
            //    //_spriteBatch.DrawString(arial, "Heyo", Vector2.Zero, Color.Black, 0f, Vector2.Zero, 5, SpriteEffects.None, 0.8f);
            //    //_spriteBatch.Draw(entities[1].sprite, new Rectangle(0,0,64,64), new Rectangle(0, 0, entities[1].sprite.Width, entities[1].sprite.Height), Color.White, rotation: 0f, origin: Vector2.Zero, effects: SpriteEffects.None, layerDepth: 0);
            //
            //    //
            //}


            if (texting) {
                _spriteBatch.Draw(pixel, new Vector2(32, 32), new Rectangle(0, 0, 1, 1), new Color(0, 0, 0, fadethrough / 5), 0f, Vector2.Zero, 600, SpriteEffects.None, 0.6f);
            
            }

            foreach (Entity e in currentZone[zoneCoordinates.X,zoneCoordinates.Y].entities) e.Draw(_spriteBatch);
            //_spriteBatch.DrawString(arial, $"{fadethrough} : {fadeGaol}", Vector2.Zero, Color.Black);
            _spriteBatch.DrawString(arial, debug, Vector2.Zero, Color.Black);
            if (fadethrough != 0) _spriteBatch.Draw(pixel, new Vector2(0, 0), new Rectangle(0, 0, 1, 1), new Color(0, 0, 0, fadethrough), 0f, Vector2.Zero, 1920, SpriteEffects.None, 0.6f);
            

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
                    generating.entities.Add(new Player(1, 1, 10,Content.Load<Texture2D>(@"Main pig\Forward"), this));
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
            v[1, 1].background = Content.Load<Texture2D>(@"Estetiska\snövärld bg 1,1");
            v[1, 1].entities.Add(new LeaveTile(4, 7, Content.Load<Texture2D>("Placeholder"), false, 1, 2,4,0, this));

            return v;
        }
    }
}
