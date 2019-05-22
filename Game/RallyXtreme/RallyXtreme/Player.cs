using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace RallyXtreme
{
    class Player
    {
        
        public bool updatePosition(int direction)
        {
            bool posState = false;
            if (direction == Const.north || direction == Const.south || 
                direction == Const.east || direction == Const.west)
            {
                posState = true;
            }
            


            return posState;
        }
        public struct playerChar
        {
            static string name;
            static int speed;
            static int pixelXY;
            static int ability;
        }


        public static playerChar createPlayer(string playerDirectory)
        {
            /* This function takes the directory given by the cache file for
             * the map and generates a 2d array representing the borders and
             * walls of the array. Walls are represented with an 'X'. This 
             * function is a simple precursor to populateGrid() - see that
             * function for full detail.
             * 
             * This function also creates the grid struct that will be used.
             * It feeds directly into populateGrid.
             * 
             * The map folder should contain 3 files:
             * 1. bg.png - the image for the background
             * 2. mapdata.rxtm - information about the map
             * 3. hitbox.rxhb - collision grid
             * 
             * The mapdata file is formatted as follows:
             * 1. grid x dimensions - this is the number of non-boundary 
             *    squares horizontally.
             *   
             * 2. grid y dimension - the number of non-boundary squares
             *    vertically.
             *    
             * 3. grid pixel count - used to stretch the map image over
             *    the generated image.
             */

            playerChar newPlayer = new playerChar();
            string directory = playerDirectory;
            // note that mapDirectory is called from Game1.cs

            Console.WriteLine($"#PLAYER# Preparing to generate from {directory}...");


            string line;
            int i = 0;



            // ############################################################
            // Reading from mapdata
            // ############################################################
            string xSizeString = "";
            string ySizeString = "";
            string pixelString = "";
            // these variables must be parsed to ints, as you can only read
            // strings or chars from a text file.

            Console.WriteLine("#GRID# attempting to read mapdata.rxtm");
            try
            {
                using (StreamReader r = new StreamReader(directory + "/mapdata.rxtm"))
                {
                    while ((line = r.ReadLine()) != null)
                    {
                        // Console.Write("#CACHEREAD# " + line + '\n');
                        i++;
                        if (i == 1)
                        {
                            xSizeString = line;
                        }
                        if (i == 2)
                        {
                            ySizeString = line;
                        }
                        if (i == 3)
                        {
                            pixelString = line;
                        }
                    }
                }
            }
            // Shows an error message if something happens
            catch (Exception er)
            {
                System.Console.WriteLine($"### GRID READ EXCEPTION -> {er}");
            }

            // The following loop attempts to convert the retrieved strings into ints
            i = 0;
            while (i < 3)
            {
                try
                {
                    if (i == 1)
                        newGrid.xSize = Int32.Parse(xSizeString);
                    else if (i == 2)
                        newGrid.ySize = Int32.Parse(ySizeString);
                    else if (i == 3)
                        newGrid.pixelSize = Int32.Parse(pixelString);
                }
                catch (FormatException)
                {
                    if (i == 1)
                        Console.WriteLine($"Unable to parse '{xSizeString}'");
                    else if (i == 2)
                        Console.WriteLine($"Unable to parse '{ySizeString}'");
                    else if (i == 3)
                        Console.WriteLine($"Unable to parse '{pixelString}'");
                }
                i++;
            }

            System.Console.WriteLine($"#GRID# Mapdata read complete, beginning hitbox read from {directory}/hitbox.rxhb");
            // ############################################################
            // Reading from hitbox
            // ############################################################


            // note: xSize and ySize do not include the boundary area around the outside
            //       of the grid and the collisions map only includes horizontal borders.
            newGrid.collisions = new char[newGrid.xSize][];

            /* 
             * Inside of the hitbox file, symbols mean as follows:
             * $ = horizontal border, use this on the extreme right
             *     of each row
             * # = free space
             * 0 = wall
             */
            i = 0;
            try
            {
                using (StreamReader r = new StreamReader(directory + "/hitbox.rxhb"))
                {
                    while ((line = r.ReadLine()) != null)
                    {
                        // Console.Write("#CACHEREAD# " + line + '\n');

                        newGrid.collisions[i] = line.ToCharArray();
                        i++;
                    }
                }
            }
            // Shows an error message if something happens
            catch (Exception er)
            {
                System.Console.WriteLine($"### GRID READ EXCEPTION -> {er}");
            }

            debugWriteGridCollisionWrite(newGrid);


            return newGrid;
        }
    }
}
