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
    }

    class AI
    {
        public static enemyChar setLocation(enemyChar e, ushort x, ushort y)
        {
            e.gridX = x;
            e.gridY = y;
            e.pos = new Vector2(x * e.gridPixelSize, y * e.gridPixelSize);

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
