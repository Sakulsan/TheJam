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
        public List<Entity> entities = new List<Entity>();
        public SpriteFont arial;
        public bool texting;

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

            Texture2D placeHolderTile = Content.Load<Texture2D>("Placeholder");

            //generates a makeshift map
            for (int i = 0; i < 16; i++)
            {
                for (int j = 0; j < 16; j++)
                {
                    entities.Add(new Tile(i,j, placeHolderTile,false,false));
                }
            }
            entities.Add(new Player(1, 1, Content.Load<Texture2D>("John")));
            entities.Add(new Entity(4, 0, 5, true, true, Content.Load<Texture2D>("Button")));

            Entity e = entities.Find(test => test is Tile && test.x == 2 && test.y == 2);
                if (e != null) ((Tile)e).collision = true;

                


            // TODO: use this.Content to load your game content here
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            foreach (Entity e in entities) e.Update(gameTime, entities);

            // TODO: Add your update logic here

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            _spriteBatch.Begin(samplerState: SamplerState.PointClamp);
            if(texting)
            foreach (Entity e in entities) e.Draw(_spriteBatch);

            _spriteBatch.DrawString(arial, "Heyo", Vector2.Zero, Color.Black);

            _spriteBatch.End();



            // TODO: Add your drawing code here

            base.Draw(gameTime);
        }

        public void textUpdate(GameTime gt)
        {

        }
    }
}
