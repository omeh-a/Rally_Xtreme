using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using Microsoft.Xna.Framework.Audio;

namespace RallyXtreme
{
    public struct enemyChar
    {
        public string name;
        public int speed;
        public int pixelXY;
        public int ability;
        public string modelDirectory;
        public int variation;
        public Vector2 pos;
        public int gridX;
        public int gridY;
        public ushort direction;
        public ushort prevDirection;
        public int gridPixelSize;
        public bool active;
        public ushort searchMode;
    }

    class AI
    {
        public static enemyChar updatePos(enemyChar e, playerChar p, gamegrid g)
        {
            if (e.searchMode == 1)
            {
                e = pathFindRandom(e, g);
            } else if (e.searchMode == 2)
            {
                e = pathFindSectorial(e, p, g);
            }
            return e;
        }

        public static enemyChar pathFindRandom(enemyChar e, gamegrid g)
        {
            // This is called when enemy type == 1
            // The AI will randomly wander through the map

            Random rn = new Random();

            if (e.direction == 4)
                e.direction = (ushort)rn.Next(0, 3);
            if (checkDirAI(0, e, g) != '0' && checkDirAI(0, e, g) != '$' && e.direction != 0 && rn.Next(0,7) == 2)
            {
                e.direction = 0;
            }
            if (checkDirAI(2, e, g) != '0' && checkDirAI(2, e, g) != '$' && e.direction != 2 && rn.Next(0, 7) == 2)
            {
                e.direction = 2;
            }
            if (checkDirAI(1, e, g) != '0' && checkDirAI(1, e, g) != '$' && e.direction != 1 && rn.Next(0, 7) == 1)
            {
                e.direction = 2;
            }


            if (checkDirAI(e.direction, e, g) != '$' && checkDirAI(e.direction, e, g) != '0')
                e = stepFowardAI(e.direction, e, g);
            else
            {
                e = stepForwardAutoAI(e.direction, e, g);
            }
                
                
            return e;
        }

        public static enemyChar pathFindSectorial(enemyChar e, playerChar p, gamegrid g)
        {


            return e;
        }

        public static float reportRotationAI(enemyChar e)
        {
            // this function converts the integer direction into radians and returns it
            float rtn = 0f;
            if (e.direction == 0)
                rtn = 0f;
            else if (e.direction == 1)
                rtn = (float)Math.PI / 2f;
            else if (e.direction == 2)
                rtn = (float)Math.PI;
            else if (e.direction == 3)
                rtn = (float)Math.PI + ((float)Math.PI / 2f);
            return rtn;
        }

        public static char checkDirAI(ushort direction, enemyChar e, gamegrid grid)
        {
            // This function returns the contents of a square in a direction relative to e

            //Console.WriteLine($"#e# Checking dir{direction}");
            char contents = ' ';
            if (direction == 0)
            {
                contents = grid.collisions[e.gridY - 1][e.gridX];
            }
            else if (direction == 1)
            {
                contents = grid.collisions[e.gridY][e.gridX + 1];
            }
            else if (direction == 2)
            {
                contents = grid.collisions[e.gridY + 1][e.gridX];
            }
            else if (direction == 3)
            {
                contents = grid.collisions[e.gridY][e.gridX - 1];
            }
            else
            {
                contents = 'x';
                Console.WriteLine($"#e# checkDirAI was given an invalid direction -> {direction}");
            }
            return contents;
        }

        public static enemyChar stepForwardAutoAI (ushort failedDirection, enemyChar e, gamegrid grid)
        {
            /* This function is responsible for reorienting the enemy if they try make
             * an invalid turn. It will try and put the enemy back on the path they were
             * last on, or else try and find a suitable direction
             * 
             */
            ushort dir = 4;


            if (failedDirection == 0)
            {
                if (checkDirAI(1, e, grid) != '$' && checkDirAI(1, e, grid) != '0')
                {
                    dir = 1;
                }
                else if (checkDirAI(3, e, grid) != '$' && checkDirAI(3, e, grid) != '0')
                {
                    dir = 3;
                }
                else
                    dir = 2;
            }
            else if (failedDirection == 1)
            {
                if (checkDirAI(0, e, grid) != '$' && checkDirAI(0, e, grid) != '0')
                {
                    dir = 0;
                }
                else if (checkDirAI(2, e, grid) != '$' && checkDirAI(2, e, grid) != '0')
                {
                    dir = 2;
                }
                else
                  dir = 3;
            }
            else if (failedDirection == 2)
            {
                if (checkDirAI(1, e, grid) != '$' && checkDirAI(1, e, grid) != '0')
                {
                    dir = 1;
                }
                else if (checkDirAI(3, e, grid) != '$' && checkDirAI(3, e, grid) != '0')
                {
                    dir = 3;
                }
                else
                  dir = 0;
            }
            else if (failedDirection == 3)
            {
                if (checkDirAI(0, e, grid) != '$' && checkDirAI(0, e, grid) != '0')
                {
                    dir = 0;
                }
                else if (checkDirAI(2, e, grid) != '$' && checkDirAI(2, e, grid) != '0')
                {
                    dir = 2;
                }
                else
                  dir = 1;
            }
            //}
            e = stepFowardAI(dir, e, grid);
            //e.prevDirection = dir;


            return e;
        }

  
        
        public static float distanceToPlayer(enemyChar e, playerChar p, gamegrid g)
        {

            double distance = 0f;
            double xComponent = 0f;
            double yComponent = 0f;
            if (e.gridX > p.gridX)
                xComponent = (e.gridX - p.gridX);
            else
                xComponent = (p.gridX - e.gridX);

            if (e.gridY > p.gridY)
                yComponent = (e.gridY - p.gridY);
            else
                yComponent = (p.gridY - e.gridY);

            xComponent = xComponent * xComponent;
            yComponent = yComponent * yComponent;


            distance = Math.Sqrt(xComponent + yComponent);
            Console.WriteLine($"Distance = {distance}, AI @ ({e.gridX},{e.gridY}) P @ ({p.gridX},{p.gridY})");
            return (float)distance;
        }


        public static enemyChar stepFowardAI(ushort direction, enemyChar e, gamegrid g)
        {
            if (direction == 1 && e.gridX < 14)
            {
                e.gridX += 1;
                e.direction = 1;
            }
            else if (direction == 3 && e.gridX > 1)
            {
                e.gridX -= 1;
                e.direction = 3;
            }
            else if (direction == 0 && e.gridY > 1)
            {
                e.gridY -= 1;
                e.direction = 0;
            }
            else if (direction == 2 && e.gridY < 13)
            {
                e.gridY += 1;
                e.direction = 2;
            }


            e.pos = new Vector2(((e.gridX * e.gridPixelSize) + e.gridPixelSize / 2),
                ((e.gridY * e.gridPixelSize)) + e.gridPixelSize / 2);

            

            return e;
        }


        public static enemyChar setLocation(enemyChar e, ushort x, ushort y)
        {
            e.gridX = x;
            e.gridY = y;
            
            e.pos = new Vector2((x * e.gridPixelSize) + (e.gridPixelSize/2), (y * e.gridPixelSize) + (e.gridPixelSize / 2));

            return e;
        }

        public static enemyChar activate(enemyChar e)
        {
            e.active = true;
            return e;
        }


        public static enemyChar createEnemy(string enemyDirectory, int variation, gamegrid g)
        {
            /* This function takes in a directory from the cache read and then
             * attempts to retrieve variables from its datafile in preparation
             * for use by other functions
             */

            enemyChar newEnemy = new enemyChar();
            string directory = enemyDirectory;
            newEnemy.variation = variation;
            newEnemy.gridPixelSize = g.pixelSize;
            // note that enemyDirectory is called from Game1.cs

            Console.WriteLine($"#ENEMY# Preparing to generate from {directory}...");


            string line;
            int i = 0;



            // ############################################################
            // Reading from mapdata
            // ############################################################
            string speedValue = "";
            string xySize = "";
            string ability = "";
            string typeString = "";
            // these variables must be parsed to ints, as you can only read
            // strings or chars from a text file.

            Console.WriteLine("#ENEMY# attempting to read charinfo.rtxci");
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
                                newEnemy.name = line;
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
                            if (i == 5)
                            {
                                typeString = line;
                            }
                            // System.Console.WriteLine(line);
                        }

                    }
                }
            }
            // Shows an error message if something happens
            catch (Exception er)
            {
                System.Console.WriteLine($"### ENEMY READ EXCEPTION -> {er}");
            }

            // The following loop attempts to convert the retrieved strings into ints
            i = 0;
            while (i <= 4)
            {
                try
                {
                    if (i == 1)
                        newEnemy.speed = Int32.Parse(speedValue);
                    else if (i == 2)
                        newEnemy.pixelXY = Int32.Parse(xySize);
                    else if (i == 3)
                        newEnemy.ability = Int32.Parse(ability);
                    else if (i == 4)
                        newEnemy.searchMode = (ushort)Int32.Parse(typeString);
                }
                catch (FormatException er)
                {
                    Console.WriteLine("#AI# Parse error ->" + er);
                }
                i++;
            }

            System.Console.WriteLine($"#ENEMY# Found Navigation type as {newEnemy.searchMode}");


            System.Console.WriteLine($"#ENEMY# Read complete");

            return newEnemy;
        }
    }
}
