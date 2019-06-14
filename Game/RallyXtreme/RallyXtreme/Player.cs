using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Audio;


namespace RallyXtreme
{
    public struct playerChar
    {
        public string name;
        public int pixelXY;
        public int ability;
        public string modelDirectory;
        public Vector2 pos;
        public int gridX;
        public int gridY;
        public ushort direction;
        public ushort prevDirection;
        public bool alive;
        public int lives;
        public int gridPixelSize;
        public uint score;
        public int speed;
        public float fuel;
    }

    class Player
    {
        
        public static playerChar updatePos(ushort desiredDirection, playerChar player, gamegrid grid)
        {
            /* This function checks for walls and obstructions before issuing the instruction for the car to move.
             * NOTE! -> Coordinates are in reverse order -> (y, x)
             */

            // The below if/else statement checks for flags to give score and checks for enemies to kill the player.
            if (Grid.returnEntityType(player.gridX, player.gridY, grid) == 'f' && Grid.returnEntityState(player.gridX, player.gridY, grid) == true)
            {
                player.score += 100;
                Console.WriteLine($"#GRID# Flag collected at x{player.gridX}, y{player.gridY}");
                grid = Entity.deactivateEntity((ushort)player.gridX, (ushort)player.gridY, grid);
                Game1.sfx[1].CreateInstance().Play();
            }

            if (Grid.returnEntityType(player.gridX, player.gridY, grid) == 'b')
            {
                // This kills the player if they encounter a boulder
                player = Player.kill(player);
            } else
            {
                if (desiredDirection == 4)
                    desiredDirection = player.prevDirection;

                if (desiredDirection == 0 && player.gridY > 0)
                {
                    // Turning north
                    if ((grid.collisions[player.gridY - 1][player.gridX] != '$') && (grid.collisions[player.gridY - 1][player.gridX] != '0'))
                    {
                        player = stepForward(desiredDirection, player, grid);
                        player.prevDirection = player.direction;
                    }
                    else
                    {
                        player = stepForwardAuto(desiredDirection, player, grid);
                        Console.WriteLine("AutostepNorth");
                    }
                }
                else if (desiredDirection == 2 && player.gridY < grid.ySize)
                {
                    // Turning south
                    if ((grid.collisions[player.gridY + 1][player.gridX] != '$') && (grid.collisions[player.gridY + 1][player.gridX] != '0'))
                    {
                        player = stepForward(desiredDirection, player, grid);
                        player.prevDirection = player.direction;
                    }
                    else
                    {
                        player = stepForwardAuto(desiredDirection, player, grid);
                        Console.WriteLine("AutostepSouth");
                    }
                }
                else if (desiredDirection == 1 && player.gridX < grid.xSize)
                {
                    // Turning east
                    if ((grid.collisions[player.gridY][player.gridX + 1] != '$') && (grid.collisions[player.gridY][player.gridX + 1] != '0'))
                    {
                        player = stepForward(desiredDirection, player, grid);
                        player.prevDirection = player.direction;

                    }
                    else
                    {
                        player = stepForwardAuto(desiredDirection, player, grid);
                        Console.WriteLine($"AutostepEast");
                    }
                }
                else if (desiredDirection == 3 && player.gridX > 0)
                {
                    // Turning west
                    if ((grid.collisions[player.gridY][player.gridX - 1] != '$') && (grid.collisions[player.gridY][player.gridX - 1] != '0'))
                    {
                        player = stepForward(desiredDirection, player, grid);
                        player.prevDirection = player.direction;
                    }
                    else
                    {
                        player = stepForwardAuto(desiredDirection, player, grid);
                        Console.WriteLine($"AutostepWest");
                    }
                }
            }

            

            
            return player;
        }

        public static playerChar stepForward(ushort direction, playerChar player, gamegrid grid)
        {
            // This function moves the player from one square to another, unless the the square
            // it is ordered to move to is a border tile ($)
            if (direction == 1 && player.gridX < 14)
            {
                player.gridX += 1;
                player.direction = 1;
            } else if (direction == 3 && player.gridX > 1)
            {
                player.gridX -= 1;
                player.direction = 3;
            } else if (direction == 0 && player.gridY > 1)
            {
                player.gridY -= 1;
                player.direction = 0;
            } else if (direction == 2 && player.gridY < 13)
            {
                player.gridY += 1;
                player.direction = 2;
            }

            player.fuel -= 1f;

            player.pos = new Vector2(((player.gridX * player.gridPixelSize) + player.gridPixelSize / 2),
                ((player.gridY * player.gridPixelSize)) + player.gridPixelSize / 2);

            return player;
        }

        public static char checkDir(ushort direction, playerChar player, gamegrid grid)
        {
            // This function returns the contents of a square in a direction relative to player

            Console.WriteLine($"#PLAYER# Checking dir{direction}");
            char contents = ' ';
            if (direction == 0)
            {
                contents = grid.collisions[player.gridY - 1][player.gridX];
            }
            else if (direction == 1)
            {
                contents = grid.collisions[player.gridY][player.gridX + 1];
            }
            else if (direction == 2)
            {
                contents = grid.collisions[player.gridY + 1][player.gridX];
            }
            else if (direction == 3)
            {
                contents = grid.collisions[player.gridY][player.gridX - 1];
            }
            else
            {
                contents = 'x';
                Console.WriteLine($"#PLAYER# Checkdir was given an invalid direction -> {direction}");
            }
            return contents;
        }

        public static void animate(playerChar player, ushort prevX, ushort prevY) 
        {

        }

        public static playerChar stepForwardAuto(ushort failedDirection, playerChar player, gamegrid grid)
        {
            /* This function is responsible for reorienting the player if they try make
             * an invalid turn. It will try and put the player back on the path they were
             * last on, or else try and find a suitable direction
             * 
             */
            ushort dir = 4;

                
                if (failedDirection == 0)
                {
                    if (checkDir(1, player, grid) != '$' && checkDir(1, player, grid) != '0')
                    {
                        dir = 1;
                    }
                    else if (checkDir(3, player, grid) != '$' && checkDir(3, player, grid) != '0')
                    {
                        dir = 3;
                    }
                    else
                        dir = 2;
                }
                else if (failedDirection == 1)
                {
                    if (checkDir(0, player, grid) != '$' && checkDir(0, player, grid) != '0')
                    {
                        dir = 0;
                    }
                    else if (checkDir(2, player, grid) != '$' && checkDir(2, player, grid) != '0')
                    {
                        dir = 2;
                    }
                    //else
                      //  dir = 3;
                }
                else if (failedDirection == 2)
                {
                    if (checkDir(1, player, grid) != '$' && checkDir(1, player, grid) != '0')
                    {
                        dir = 1;
                    }
                    else if (checkDir(3, player, grid) != '$' && checkDir(3, player, grid) != '0')
                    {
                        dir = 3;
                    }
                    //else
                      //  dir = 0;
                }
                else if (failedDirection == 3)
                {
                    if (checkDir(0, player, grid) != '$' && checkDir(0, player, grid) != '0')
                    {
                        dir = 0;
                    }
                    else if (checkDir(2, player, grid) != '$' && checkDir(2, player, grid) != '0')
                    {
                        dir = 2;
                    }
                    //else
                      //  dir = 1;
                }
            //}
            player = Player.stepForward(dir, player, grid);
            //player.prevDirection = dir;


            return player;



        }

        public static playerChar kill(playerChar player)
        {
            //stub
            Console.Out.WriteLine("#PLAYER# Player has been killed !");
            player.alive = false;
            player.lives = player.lives - 1;
            return player;
        }

        public static void giveLife(playerChar player)
        {
            player.lives += 1;
            Console.Out.WriteLine($"#PLAYER# Life added -> total = {player.lives}");
        }



        public static float reportRotation(playerChar player)
        {
            // this function converts the integer direction into radians and returns it
            float rtn = 0f;
            if (player.direction == 0)
                rtn = 0f;
            else if (player.direction == 1)
                rtn = (float) Math.PI / 2f;
            else if (player.direction == 2)
                rtn = (float)Math.PI;
            else if (player.direction == 3)
                rtn = (float) Math.PI + ((float) Math.PI / 2f);
            return rtn;
        }


        public static playerChar createPlayer(string playerDirectory, gamegrid grid)
        {
            /* This function takes in a directory from the cache read and then
             * attempts to retrieve variables from its datafile in preparation
             * for use by other functions
             */

            playerChar newPlayer = new playerChar();
            string directory = playerDirectory;
            // note that playerDirectory is called from Game1.cs

            Console.WriteLine($"#PLAYER# Preparing to generate from {directory}...");


            string line;
            int i = 0;


            newPlayer.lives = 3;
            newPlayer.alive = true;
            newPlayer.direction = 0;
            newPlayer.gridPixelSize = grid.pixelSize;
            newPlayer.score = 0;
            newPlayer.fuel = 100f;

            
            newPlayer.gridX = grid.playerStart[0];
            newPlayer.gridY = grid.playerStart[1];

            // ############################################################
            // Reading from mapdata
            // ############################################################
            string speedValue = "";
            string xySize = "";
            string ability = "";
            // these variables must be parsed to ints, as you can only read
            // strings or chars from a text file.

            Console.WriteLine("#PLAYER# attempting to read charinfo.rtxci");
            try
            {
                using (StreamReader r = new StreamReader(directory + "/charinfo.rtxci"))
                {
                    while ((line = r.ReadLine()) != null)
                    {
                        if (line[0] != '#')
                        {
                            i++;
                            if (i == 1)
                            {
                                newPlayer.name = line;
                            }
                            if (i == 2)
                            {
                                speedValue = line;
                            }
                            if (i == 3)
                            {
                                xySize = line;
                            }
                            if (i == 4)
                            {
                                ability = line;
                            }
                            // System.Console.WriteLine(line);
                        }
                        
                    }
                }
            }
            // Shows an error message if something happens
            catch (Exception er)
            {
                System.Console.WriteLine($"### PLAYER READ EXCEPTION -> {er}");
            }

            // The following loop attempts to convert the retrieved strings into ints
            i = 0;
            while (i < 3)
            {
                try
                {
                    if (i == 1)
                        newPlayer.speed = Int32.Parse(speedValue);
                    else if (i == 2)
                        newPlayer.pixelXY = Int32.Parse(xySize);
                    else if (i == 3)
                        newPlayer.ability = Int32.Parse(ability);
                }
                catch (FormatException)
                {
                    if (i == 1)
                        Console.WriteLine($"Unable to parse '{speedValue}'");
                    else if (i == 2)
                        Console.WriteLine($"Unable to parse '{xySize}'");
                    else if (i == 3)
                        Console.WriteLine($"Unable to parse '{ability}'");
                }
                i++;
            }

            newPlayer.modelDirectory = $"{directory}";

            System.Console.WriteLine($"#PLAYER# Read complete");
           
            return newPlayer;
        }
    }
}
