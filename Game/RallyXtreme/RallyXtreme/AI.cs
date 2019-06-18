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


        public static enemyChar stepFoward(ushort direction, enemyChar e, gamegrid g)
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
            while (i < 3)
            {
                try
                {
                    if (i == 1)
                        newEnemy.speed = Int32.Parse(speedValue);
                    else if (i == 2)
                        newEnemy.pixelXY = Int32.Parse(xySize);
                    else if (i == 3)
                        newEnemy.ability = Int32.Parse(ability);
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

            newEnemy.modelDirectory = $"{directory}";

            System.Console.WriteLine($"#ENEMY# Read complete");

            return newEnemy;
        }
    }
}
