using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Diagnostics;

namespace RallyXtreme
{
    /// <summary>
    /// This is the main type for your game.
    /// </summary>


    

    public class Game1 : Game
    {
        
        public const int north = 0, east = 1, west = 2, south = 3;
        public string mapDirectory = CacheLoad.getMap();
        public string playerDirectory = CacheLoad.getPlayer();
        public string aiDirectory = CacheLoad.getAi();
        public int difficulty = CacheLoad.getDifficulty();
        // public static MapLoad.map gameMap = new MapLoad.map();
        public gamegrid mainGrid = grid.generateGrid(CacheLoad.getMap());
        public playerChar player0 = new playerChar();
        public enemyChar enemy0 = AI.createEnemy(CacheLoad.getAi());
        Texture2D car;
        Texture2D background;
        Vector2 carPosition;
        float carRotation;
        float carSpeed;
        SpriteFont font;
        int score = 0;
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        public int tickLimit = 10000000;
        double accumulator = 0f;
        ushort nextDirection = 4;

        public Game1()
        {



            if (mapDirectory == "debug")
            {
                //gameMap = MapLoad.debugLoad(true);
                // loads a hardloaded map
                // bool input designates whether the game will run normally
                // or as debug mode with no victory/end state
            }
            else
            {
                //gameMap = MapLoad.loadMap(mapDirectory);
            }
            graphics = new GraphicsDeviceManager(this);
            graphics.PreferredBackBufferWidth = CacheLoad.getResolutionX();
            graphics.PreferredBackBufferHeight = CacheLoad.getResolutionY();

            Content.RootDirectory = "Content";
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            // TODO: Add your initialization logic here.
            // Called after the constructor, used for calling
            // non-graphical stuff.v5\


            player0 = Player.createPlayer(CacheLoad.getPlayer(), mainGrid);
            // This assert check crashes the program if the cache is not found
            Debug.Assert(CacheLoad.cacheCheck() == true);

            carPosition = player0.pos;
            carSpeed = 250f;
            carRotation = 0f;

            /*if (MapLoad.checkMap(gameMap) == false)
            {
                // add function to crash program here
            }*/
            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.

            spriteBatch = new SpriteBatch(GraphicsDevice);

            // TODO: use this.Content to load your game content here
            car = Content.Load<Texture2D>("goodcar70x70mk1");
            background = Content.Load<Texture2D>("bg");
            font = Content.Load<SpriteFont>("TestFont");
            // calls graphical stuff mainly
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// game-specific content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            // gameTime.ElapsedGameTime.Totalseconds is used here to ensure consistent timings between activations because each update
            // frame is not neccesarily the same length.
            
            

            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();
            var kstate = Keyboard.GetState();



            if (kstate.IsKeyDown(Keys.W) || kstate.IsKeyDown(Keys.Up))
            {
                nextDirection = 0;
            }
            if (kstate.IsKeyDown(Keys.S) || kstate.IsKeyDown(Keys.Down))
            {
                nextDirection = 2;
            }
            if (kstate.IsKeyDown(Keys.A) || kstate.IsKeyDown(Keys.Left))
            {
                nextDirection = 3;
            }
            if (kstate.IsKeyDown(Keys.D) || kstate.IsKeyDown(Keys.Right))
            {
                nextDirection = 1;
            }

            if ((accumulator) > 0.3)
            {
                player0 = Player.updatePos(nextDirection, player0, mainGrid);
                score++;
                Console.WriteLine($"#GAME# Movement tick -> Player = ({player0.gridX},{player0.gridY})");
                accumulator = 0;
            }
            carRotation = Player.reportRotation(player0);
            carPosition = player0.pos;

            // This code makes sure that the car cannot leave the screen by comparing the location of the car to the size of the screen
            //carPosition.X = Math.Min(Math.Max(car.Width/2, carPosition.X), graphics.PreferredBackBufferWidth - car.Width/2);
            //carPosition.Y = Math.Min(Math.Max(car.Height/2, carPosition.Y), graphics.PreferredBackBufferHeight - car.Height/2);

            //    N
            //  # 0 # 
            //W 3 # 1 E
            //  # 2 #
            //    S

            accumulator += (double) gameTime.ElapsedGameTime.TotalSeconds;
            
            base.Update(gameTime);
            // TODO: Add your update logic here
            // Called every tick to update game state
            // Checks collisions
            // Getting inputs
            // 

            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {

            GraphicsDevice.Clear(Color.CornflowerBlue);

            // TODO: Add your drawing code here
            spriteBatch.Begin();
            spriteBatch.Draw(background, new Vector2(0, 0), Color.White);
            spriteBatch.Draw(car, carPosition, new Rectangle(0,0,70,70), Color.White, carRotation, new Vector2(35,35), 1.0f, SpriteEffects.None, 1);
            spriteBatch.DrawString(font, $"SCORE = {score}", new Vector2(50, 50), Color.Black);
            spriteBatch.End();

            base.Draw(gameTime);
            // like update method, but for graphics only
        }

        /* protected void carMovement(int direction)
         {
             if (direction == 0) ;
                 carPosition.Y -= carSpeed * (float)gameTime.ElapsedGameTime.TotalSeconds;

             if (kstate.IsKeyDown(Keys.S) || kstate.IsKeyDown(Keys.Down))
                 carPosition.Y += carSpeed * (float)gameTime.ElapsedGameTime.TotalSeconds;

             if (kstate.IsKeyDown(Keys.A) || kstate.IsKeyDown(Keys.Left))
                 carPosition.X -= carSpeed * (float)gameTime.ElapsedGameTime.TotalSeconds;

             if (kstate.IsKeyDown(Keys.D) || kstate.IsKeyDown(Keys.Right))
                 carPosition.X += carSpeed * (float)gameTime.ElapsedGameTime.TotalSeconds;

         }*/
    }
}
