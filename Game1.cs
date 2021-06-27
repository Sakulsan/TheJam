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
        int fadethrough = 1;

        public Map[,] currentZone = new Map[3,3];
        public Point zoneCoordinates = new Point(0, 1);

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
            currentZone[0, 1].entities.Add(new LeaveTile(5,0,placeHolderTile,false,0,0, this));


            //Entity e = entities.Find(test => test.x == 2 && test.y == 2);
            //    if (e != null) e.collision = true;



            // TODO: use this.Content to load your game content here
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            foreach (Entity e in currentZone[zoneCoordinates.X, zoneCoordinates.Y].entities) e.Update(gameTime, currentZone[zoneCoordinates.X,zoneCoordinates.Y].entities);

            if(fadethrough != 0)fadethrough += gameTime.ElapsedGameTime.Milliseconds / 255;
            if (fadethrough > 255) fadethrough = 0;
            // TODO: Add your update logic here

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);



            _spriteBatch.Begin(samplerState: SamplerState.PointClamp);
            //if (true)
            //{
            //    //_spriteBatch.DrawString(arial, "Heyo", Vector2.Zero, Color.Black, 0f, Vector2.Zero, 5, SpriteEffects.None, 0.8f);
            //    //_spriteBatch.Draw(entities[1].sprite, new Rectangle(0,0,64,64), new Rectangle(0, 0, entities[1].sprite.Width, entities[1].sprite.Height), Color.White, rotation: 0f, origin: Vector2.Zero, effects: SpriteEffects.None, layerDepth: 0);
            //
            //    //
            //}
            


            foreach (Entity e in currentZone[zoneCoordinates.X,zoneCoordinates.Y].entities) e.Draw(_spriteBatch);
            if (fadethrough != 0) _spriteBatch.Draw(pixel, new Vector2(0, 0), new Rectangle(0, 0, 1, 1), new Color(0, 0, 0, fadethrough / 300), 0f, Vector2.Zero, 600, SpriteEffects.None, 0.6f);

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
            for (int i = 0; i < 3; i++)
            {
                for (int j = 0; j < 3; j++)
                {
                    Map generating = new Map(new List<Entity>(), Content.Load<Texture2D>("Placeholder"));
                    generating.entities.Add(new Player(1, 1, Content.Load<Texture2D>("John"), this));
                    generating.entities.Add(new TouchEntity(4, 0, 5, true, Content.Load<Texture2D>("Button"), TouchEntity.Effects.Textbox, "Hello world", this));

                    v[i, j] = generating;
                }
            }
            return v;
        }
    }
}
