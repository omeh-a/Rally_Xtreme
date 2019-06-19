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
        Texture2D car;
        Texture2D background;
        Texture2D flag;
        Texture2D enemyCar;
        Texture2D logo;
        Texture2D fuelBar;
        Texture2D fuelBg;
        Texture2D explosion;
        Texture2D wall;
        Texture2D border;
        Texture2D gameOver;
        Texture2D playerLose;
        Song bgMusic;
        public static List<SoundEffect> sfx;
        public static double gameTimer = -3f;
        
        Vector2 carPosition;
        float carRotation;
        float carSpeed;
        SpriteFont font;
        SpriteFont cdFont;
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        public int tickLimit = 10000000;
        double accumulator = 0f;
        ushort nextDirection = 4;
        double tickTime = 0.25f;
        int uiPixelOffset = 300;
        int xRes = CacheLoad.getResolutionX();
        int yRes = CacheLoad.getResolutionY();
        bool explode = false;
        bool playMusic = true;
        bool muteNextFrame = false;
        bool isPaused;
        bool pauseNextFrame;
        enemyChar e0, e1, e2, e3;
        int explosionHappened;
        bool gameRunning = true;

        public Game1()
        {
            // This list will hold all sound effects
            sfx = new List<SoundEffect>();

            // This instantiates the graphics process, declaring a new window with xRes x yRes pixels size.
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

            // Instantiating player and enemies
            player0 = Player.createPlayer(CacheLoad.getPlayer(), mainGrid);
            e0 = (AI.createEnemy(aiDirectory, 0, mainGrid));
            e1 = (AI.createEnemy(aiDirectory, 1, mainGrid));
            e2 = (AI.createEnemy(aiDirectory, 2, mainGrid));
            e3 = (AI.createEnemy(aiDirectory, 3, mainGrid));

            // This assert check crashes the program if the cache is not found
            Debug.Assert(CacheLoad.cacheCheck() == true);

            uint enemyCount = mainGrid.enemyCount;

            isPaused = false;
            explosionHappened = 0;
            // The following statements activate the correct number of enemies for the map, putting them 
            // in spawn locations marked in the hitbox.rxhb file.
            if (enemyCount > 0)
            {
                
                e0 = AI.setLocation(e0, (ushort)mainGrid.enemystart[0][0], (ushort)mainGrid.enemystart[0][1]);
                e0 = AI.activate(e0);
            }
                
            if (enemyCount > 1)
            {
                e1 = AI.setLocation(e1, (ushort)mainGrid.enemystart[1][0], (ushort)mainGrid.enemystart[1][1]);
                e1 = AI.activate(e1);
            }
                
            if (enemyCount > 2)
            {
                e2 = AI.setLocation(e2, (ushort)mainGrid.enemystart[2][0], (ushort)mainGrid.enemystart[2][1]);
                e2 = AI.activate(e2);
            }
                
            if (enemyCount > 3)
            {
                e3 = AI.setLocation(e3, (ushort)mainGrid.enemystart[3][0], (ushort)mainGrid.enemystart[3][1]);
                e3 = AI.activate(e3);
            }
               


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
            
            background = Content.Load<Texture2D>("Internal /dirtyRoadBordered");
            wall = Content.Load<Texture2D>("Internal /wallBordered");
            border = Content.Load<Texture2D>("Internal /borderblock");
            font = Content.Load<SpriteFont>("TestFont");
            cdFont = Content.Load<SpriteFont>("countIn");
            flag = Content.Load<Texture2D>("flag_normal");
            logo = Content.Load<Texture2D>("logo");
            fuelBar = Content.Load<Texture2D>("fuelbar");
            fuelBg = Content.Load<Texture2D>("fuelbg");
            explosion = Content.Load<Texture2D>("carexplosion");
            enemyCar = Content.Load<Texture2D>("Enemies /classic /model0");
            gameOver = Content.Load<Texture2D>("gameover");
            playerLose = Content.Load<Texture2D>("playerlose");
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

            // Mute button
            if (kstate.IsKeyDown(Keys.M))
            {
                muteNextFrame = true;
            }

            // Suicide key
            if (kstate.IsKeyDown(Keys.K))
            {
                player0 = Player.kill(player0);
            }

            // Pause key
            if (kstate.IsKeyDown(Keys.P))
            {
                if (isPaused == true)
                    isPaused = false;
                else if (isPaused == false)
                    pauseNextFrame = true;
            }

            if (gameTimer > 0 && gameRunning == true)
            {
                // Checking if all flags are collected ot end game 
                if (Entity.flagsRemaining(mainGrid) <= 0)
                    gameRunning = false;



                // Detecting the player's desired movement
                if (kstate.IsKeyDown(Keys.W) || kstate.IsKeyDown(Keys.Up))
                {
                    nextDirection = 0;
                }
                else if (kstate.IsKeyDown(Keys.S) || kstate.IsKeyDown(Keys.Down))
                {
                    nextDirection = 2;
                }
                else if (kstate.IsKeyDown(Keys.A) || kstate.IsKeyDown(Keys.Left))
                {
                    nextDirection = 3;
                }
                else if (kstate.IsKeyDown(Keys.D) || kstate.IsKeyDown(Keys.Right))
                {
                    nextDirection = 1;
                }



                if ((accumulator) > tickTime)
                {
                    if (player0.fuel > 0 && player0.alive == true)
                    {
                        player0 = Player.updatePos(nextDirection, player0, mainGrid);
                        if (e0.active == true)
                            e0 = AI.updatePos(e0, player0, mainGrid);
                        AI.distanceToPlayer(e0, player0, mainGrid);
                        if (e1.active == true)
                            e1 = AI.updatePos(e1, player0, mainGrid);
                        if (e2.active == true)
                            e2 = AI.updatePos(e2, player0, mainGrid);
                        if (e3.active == true)
                            e3 = AI.updatePos(e3, player0, mainGrid);

                        if (Player.checkEnemyPos(player0, e0, e1, e2, e3) == false)
                            player0 = Player.kill(player0);

                        if (pauseNextFrame == true)
                            isPaused = true;
                        if (muteNextFrame == true)
                            if (playMusic == true)
                            {
                                MediaPlayer.IsMuted = true;
                                playMusic = false;
                            }
                                
                            else if (playMusic == false)
                            {
                                MediaPlayer.IsMuted = false;
                                playMusic = true;
                            }
                                

                        nextDirection = player0.direction;
                        Console.WriteLine($"#GAME# Movement tick -> Player = ({player0.gridX},{player0.gridY})");

                    }
                    else
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
            }

            if (gameRunning == false)
            {
                MediaPlayer.IsMuted = true;
                if (player0.fuel > 0 && accumulator > 0.2f)
                {
                    player0.fuel -= 1;
                    player0.score += 10;
                    accumulator = 0;
                    Console.WriteLine("#GAME OVER# -> converting fuel to score!");
                    sfx[1].Play();
                    
                }

            }

            if (isPaused == false)
            {
                accumulator += (double)gameTime.ElapsedGameTime.TotalSeconds;
                if (player0.alive == true && gameRunning == true)
                    gameTimer += (double)gameTime.ElapsedGameTime.TotalSeconds;
            }
            
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
            //spriteBatch.Draw(background, new Vector2(0, 0), Color.White);

            y = 0;
            while (y <= mainGrid.ySize)
            {
                ushort x = 0;
                while (x <= mainGrid.xSize)
                {

                    if (mainGrid.collisions[y][x] == '#' || mainGrid.collisions[y][x] == 'e' || mainGrid.collisions[y][x] == 's' || mainGrid.collisions[y][x] == 'n')
                    {
                        spriteBatch.Draw(background, new Vector2((x * mainGrid.pixelSize), (y * mainGrid.pixelSize)), new Rectangle(0, 0, mainGrid.pixelSize, mainGrid.pixelSize), 
                            new Color(mainGrid.roadColour[0], mainGrid.roadColour[1], mainGrid.roadColour[2], (byte)255), 0f,
                            new Vector2(0, 0), 1f, SpriteEffects.None, 1);
                    }
                    else if (mainGrid.collisions[y][x] == '$')
                    {
                        spriteBatch.Draw(border, new Vector2((x * mainGrid.pixelSize), (y * mainGrid.pixelSize)), new Rectangle(0, 0, mainGrid.pixelSize, mainGrid.pixelSize),
                            new Color(mainGrid.borderColour[0], mainGrid.borderColour[1], mainGrid.borderColour[2], (byte)255), 0f,
                            new Vector2(0, 0), 1f, SpriteEffects.None, 1);
                    }
                    else if (mainGrid.collisions[y][x] == '0')
                    {
                        spriteBatch.Draw(wall, new Vector2((x * mainGrid.pixelSize), (y * mainGrid.pixelSize)), new Rectangle(0, 0, mainGrid.pixelSize, mainGrid.pixelSize),
                            new Color(mainGrid.wallColour[0], mainGrid.wallColour[1], mainGrid.wallColour[2], (byte)255), 0f,
                            new Vector2(0, 0), 1f, SpriteEffects.None, 1);
                    }

                    x++;
                }
                y++;

            }


            // ### ENTITY DRAW ###
            y = 0;
            while (y < mainGrid.ySize)
            {
                ushort x = 0;
                while (x < mainGrid.xSize)
                {

                    if (mainGrid.collisions[y][x] == '#' && Grid.returnEntityState(x, y, mainGrid) == true)
                    {
                        spriteBatch.Draw(flag, mainGrid.entities[y][x].pos, new Rectangle(0, 0, mainGrid.pixelSize, mainGrid.pixelSize), Color.White, 0f,
                            new Vector2(0, 0), 1f, SpriteEffects.None, 1);

                    }

                    x++;
                }
                y++;

            }

            // ### ENEMY DRAW ###
            if (e0.active == true && gameTimer > 0)
                spriteBatch.Draw(enemyCar, e0.pos, new Rectangle(0, 0, 70, 70), Color.White, AI.reportRotationAI(e0), new Vector2(35, 35), 1.0f, SpriteEffects.None, 1);
            if (e1.active == true && gameTimer > 0)
                spriteBatch.Draw(enemyCar, e1.pos, new Rectangle(0, 0, 70, 70), Color.White, AI.reportRotationAI(e1), new Vector2(35, 35), 1.0f, SpriteEffects.None, 1);
            if (e2.active == true && gameTimer > 0)
                spriteBatch.Draw(enemyCar, e2.pos, new Rectangle(0, 0, 70, 70), Color.White, AI.reportRotationAI(e2), new Vector2(35, 35), 1.0f, SpriteEffects.None, 1);
            if (e3.active == true && gameTimer > 0)
                spriteBatch.Draw(enemyCar, e3.pos, new Rectangle(0, 0, 70, 70), Color.White, AI.reportRotationAI(e3), new Vector2(35, 35), 1.0f, SpriteEffects.None, 1);

            // ### PLAYER DRAW ###
            if (player0.alive == true && gameTimer > 0)
            {
                spriteBatch.Draw(car, carPosition, new Rectangle(0, 0, 70, 70), Color.White, carRotation, new Vector2(35, 35), 1.0f, SpriteEffects.None, 1);
            } else if (player0.alive == false && explosionHappened < 40)
            {
                // This draws an explosion when the player dies
                spriteBatch.Draw(explosion, carPosition, new Rectangle(0, 0, 70, 70), new Color(255,255,255, (int)(255*(accumulator/tickTime))), carRotation, new Vector2(35, 35), 1.0f, SpriteEffects.None, 1);
                explosionHappened += 1;
            } else if (player0.alive == false && explosionHappened >= 40)
            {
                spriteBatch.Draw(playerLose, new Vector2((xRes/4), (yRes/2)- 150), Color.White);
            }

            // ### UI DRAW ###
            // Score counter
            spriteBatch.DrawString(font, $"Score = {player0.score}", new Vector2((float)xRes + uiPixelOffset/4, 90), Color.White);

            // Timer
            spriteBatch.DrawString(cdFont, $"Time: {Math.Round(gameTimer, 2)} s", new Vector2((float)xRes + (uiPixelOffset / 5), 900), Color.White);

            // Logo
            spriteBatch.Draw(logo, new Vector2((float)xRes, 15f), Color.White);

            // Fuel Gauge Label
            spriteBatch.DrawString(font, $"Fuel: {player0.fuel}", new Vector2((float)xRes + uiPixelOffset / 4, 180), Color.NavajoWhite);

            // Fuel Gauge Background
            spriteBatch.Draw(fuelBg, new Vector2((float)xRes, 200f), Color.White);

            // Fuel Gauge
            spriteBatch.Draw(fuelBar, new Vector2((float)xRes, 200f), new Rectangle(0, 0, 3*((int)player0.fuel), 70), Color.White);

            // Keys display
            spriteBatch.DrawString(font, $"       Mute = 'm'", new Vector2(((float)xRes + uiPixelOffset / 4), 300), Color.White);
            spriteBatch.DrawString(font, $"Kill player = 'k'", new Vector2(((float)xRes + uiPixelOffset / 4), 320), Color.White);

            // Count in
            if (gameTimer < 0)
            {
                spriteBatch.Draw(fuelBg, new Vector2(((xRes + uiPixelOffset) / 2) - 230, (yRes / 2) - 15), Color.White);
                spriteBatch.DrawString(cdFont, $"Start in... {Math.Round(Math.Abs(gameTimer))}", new Vector2(((xRes + uiPixelOffset)/2)-200, yRes/2), Color.White);
                
            }

            // Game over screen
            if (gameRunning == false)
            {
                spriteBatch.Draw(gameOver, new Vector2((xRes / 4), (yRes / 2) -150), Color.White);
            }


            spriteBatch.End();

            base.Draw(gameTime);
            // like update method, but for graphics only
        }

        public static void resetGame(gamegrid g, playerChar p, enemyChar e0, enemyChar e1, enemyChar e2, enemyChar e3)
        {

        }


        
    }
}
