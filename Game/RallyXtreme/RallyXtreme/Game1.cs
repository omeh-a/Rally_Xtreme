using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using Microsoft.Xna.Framework.Audio;
using System;
using System.Diagnostics;
using System.Collections.Generic;

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
        public gamegrid mainGrid = Grid.generateGrid(CacheLoad.getMap());
        public playerChar player0 = new playerChar();
        public enemyChar enemy0 = AI.createEnemy(CacheLoad.getAi());
        Texture2D car;
        Texture2D background;
        Texture2D flag;
        Texture2D logo;
        Texture2D fuelBar;
        Texture2D fuelBg;
        Texture2D explosion;
        Song bgMusic;
        public static List<SoundEffect> sfx;
        
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
        double tickTime = 0.4f;
        int uiPixelOffset = 300;
        int xRes = CacheLoad.getResolutionX();
        int yRes = CacheLoad.getResolutionY();
        bool explode = false;
        bool playMusic = true;

        public Game1()
        {
            // This list will hold all sound effects
            sfx = new List<SoundEffect>();



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
            graphics.PreferredBackBufferWidth = xRes + uiPixelOffset;
            graphics.PreferredBackBufferHeight = yRes;

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
            mainGrid = Grid.populateFlags(mainGrid);
            sfx.Add(Content.Load<SoundEffect>("SoundFX /bang"));
            sfx.Add(Content.Load<SoundEffect>("SoundFX /blip"));
            bgMusic = Content.Load<Song>("Music /too-cool");
            MediaPlayer.Play(bgMusic);
            MediaPlayer.IsRepeating = true;
            MediaPlayer.MediaStateChanged += MediaPlayer_MediaStateChanged;
            MediaPlayer.Volume = 0.2f;
            Console.WriteLine($"#GAME# Grid X limit = {mainGrid.xSize}, Grid Y limit = {mainGrid.ySize}");
            
            base.Initialize();
        }

        void MediaPlayer_MediaStateChanged(object sender, System.
                                           EventArgs e)
        {
            // 0.0f is silent, 1.0f is full volume
            MediaPlayer.Play(bgMusic);
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
            flag = Content.Load<Texture2D>("flag_normal");
            logo = Content.Load<Texture2D>("logo");
            fuelBar = Content.Load<Texture2D>("fuelbar");
            fuelBg = Content.Load<Texture2D>("fuelbg");
            explosion = Content.Load<Texture2D>("carexplosion");
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
            // An accumulator is used for timing, adding up the time since each previous frame in order to funtion as a timer.
    

            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();
            var kstate = Keyboard.GetState();



            if (kstate.IsKeyDown(Keys.W) || kstate.IsKeyDown(Keys.Up))
            {
                nextDirection = 0;
            } else if (kstate.IsKeyDown(Keys.S) || kstate.IsKeyDown(Keys.Down))
            {
                nextDirection = 2;
            } else if (kstate.IsKeyDown(Keys.A) || kstate.IsKeyDown(Keys.Left))
            {
                nextDirection = 3;
            } else if (kstate.IsKeyDown(Keys.D) || kstate.IsKeyDown(Keys.Right))
            {
                nextDirection = 1;
            }

            if (kstate.IsKeyDown(Keys.M))
            {
                if (playMusic == true)
                {
                    playMusic = false;
                    MediaPlayer.IsMuted = false;
                } else if (playMusic == false)
                {
                    playMusic = true;
                    MediaPlayer.IsMuted = true;
                }
                    
            }

                if ((accumulator) > tickTime)
            {
                if (player0.fuel > 0)
                {
                    player0 = Player.updatePos(nextDirection, player0, mainGrid);
                    nextDirection = player0.direction;
                    score++;
                    Console.WriteLine($"#GAME# Movement tick -> Player = ({player0.gridX},{player0.gridY}) nextDir = {nextDirection}, trueDir = {player0.direction}");
                } else
                {
                    player0.alive = false;
                    if (explode == false)
                    {
                        sfx[0].CreateInstance().Play();
                        explode = true;
                    }
                    MediaPlayer.Pause();
                    MediaPlayer.Stop();
                    MediaPlayer.IsMuted = true;
                }
                
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
            // ### CLEARSCREEN DRAW ###
            GraphicsDevice.Clear(Color.DarkGray);

            ushort y = 0;
            
            spriteBatch.Begin();

            // ### BACKGROUND DRAW ###
            spriteBatch.Draw(background, new Vector2(0, 0), Color.White);


            // ### ENTITY DRAW ###
            y = 0;
            while (y < mainGrid.ySize)
            {
                ushort x = 0;
                while (x < mainGrid.xSize)
                {

                    if (Grid.returnEntityType(x, y, mainGrid) == 'f' && Grid.returnEntityState(x, y, mainGrid) == true) 
                    {
                        spriteBatch.Draw(flag, mainGrid.entities[y][x].pos, new Rectangle(0, 0, mainGrid.pixelSize, mainGrid.pixelSize), Color.White, 0f,
                            new Vector2(0, 0), 1f, SpriteEffects.None, 1);
                        
                    }
                        
                    x++;
                }
                y++;

            }

            // ### PLAYER DRAW ###
            if (player0.alive == true)
            {
                spriteBatch.Draw(car, carPosition, new Rectangle(0, 0, 70, 70), Color.White, carRotation, new Vector2(35, 35), 1.0f, SpriteEffects.None, 1);
            } else if (player0.alive == false)
            {
                // This draws an explosion when the player dies
                spriteBatch.Draw(explosion, carPosition, new Rectangle(0, 0, 70, 70), Color.White, carRotation, new Vector2(35, 35), 1.0f, SpriteEffects.None, 1);
            }

            // ### UI DRAW ###
            // Score counter
            spriteBatch.DrawString(font, $"Score = {player0.score}", new Vector2((float)xRes + uiPixelOffset/4, 90), Color.White);

            // Logo
            spriteBatch.Draw(logo, new Vector2((float)xRes, 0f), Color.White);

            // Fuel Gauge Label
            spriteBatch.DrawString(font, $"Fuel: {player0.fuel}", new Vector2((float)xRes + uiPixelOffset / 4, 180), Color.NavajoWhite);

            // Fuel Gauge Background
            spriteBatch.Draw(fuelBg, new Vector2((float)xRes, 200f), Color.White);

            // Fuel Gauge
            spriteBatch.Draw(fuelBar, new Vector2((float)xRes, 200f), new Rectangle(0, 0, 3*((int)player0.fuel), 70), Color.White);

            spriteBatch.End();

            base.Draw(gameTime);
            // like update method, but for graphics only
        }

        
    }
}
