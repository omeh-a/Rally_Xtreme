using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;


namespace RallyXtreme
{
    public struct gamegrid
    {
        public char[][] collisions;
        public char[][] entities;
        public int[,] turnData;
        public int xSize;
        public int ySize;
        public int pixelSize;
        public string name;
        public int[] playerStart;
        public int[][] enemystart;
    }



    class grid
    {

        public static gamegrid generateGrid(string mapDirectory)
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

            gamegrid newGrid = new gamegrid();
            string directory = mapDirectory;
            // note that mapDirectory is called from Game1.cs

            Console.WriteLine($"#GRID# Preparing to generate from {directory}...");


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
                            System.Console.WriteLine($"#GRID# Pixel size = {line}");
                        }
                        if (i == 4)
                        {
                            newGrid.name = line;
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
            while (i <= 3)
            {
                try
                {
                    if (i == 1)
                        newGrid.xSize = Int32.Parse(xSizeString);
                    else if (i == 2)
                        newGrid.ySize = Int32.Parse(ySizeString);
                    else if (i == 3)
                    {
                        newGrid.pixelSize = Int32.Parse(pixelString);
                        System.Console.WriteLine($"#GRID# Parsed {pixelString} -> {newGrid.pixelSize}");
                    }
                    

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
             * $ = border of map
             * # = free space
             * 0 = wall
             * s = player spawn (also free space)
             * e = enemy spawn (also free space)
             */
            i = 0;
            int o = 0;
            int e = 0;
            try
            {
                using (StreamReader r = new StreamReader(directory + "/hitbox.rxhb"))
                {
                    while ((line = r.ReadLine()) != null)
                    {
                        /* The below lines look for an player/enemy start position and
                         * store them in int arrays.
                         */
                        while (o < line.Length)
                        {
                            if (line[o] == 's')
                            {
                                newGrid.playerStart = new int[2];
                                newGrid.playerStart[0] = o;
                                newGrid.playerStart[1] = i;
                                System.Console.WriteLine($"#GRID# player start @{o},{i}");
                            }
                            if (line[o] == 'e')
                            {
                                newGrid.enemystart = new int[10][];
                                newGrid.enemystart[e] = new int[2];
                                newGrid.enemystart[e][0] = o;
                                newGrid.enemystart[e][1] = i;
                                System.Console.WriteLine($"#GRID# enemy #{e} start @{o},{i}");
                            }
                            o++;
                        }
                        o = 0;

                        // the below simply stores the hitbox
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
            newGrid.entities = newGrid.collisions;

            return newGrid;
        }
        public static void populateGrid(int[][] grid)
        {
            /* This function takes in the generated grid and generates a
             * complimentary grid of turn information.
             * 
             * The map is stored as a 2D array in memory, with a series of 
             * indicators used to describe information regarding corners pre
             * -generated to ensure minimal slowdown in game.
             * 
             * The game's map is stored as two 2d arrays - one carrying the
             * collision information consisting of only wall data and the 
             * other containing only the pathing markers.
             * 
             * The markers are as follows:
             * 
             * X = Wall
             * 
             * T[xy] = a teleport square - these can be used to allow the player
             *         to be sent to the other side of the map instead of just 
             *         hitting a dead end, as seen in games like PacMan. 
             *         X and Y are numeric values representing where the player
             *         should be sent. These values must be defined by the mapdata
             *         file, as there is simply no way to automate this.
             * 
             * 0 = A dead end. The player can only go backwards in these. Ideally
             *     these should not be used as they are unpleasant to the player.
             * 
             * 1 = A square where there is no possible turn. The player may only
             *     proceed forward or backwards. No additional data is needed as
             *     the game knows there is only one possible command in this square
             *     (backwards).
             *     
             * 2[d1d2] = A square where a turn is mandatory. The two possible 
             *             directions are stored in d1 and d2 as a compass 
             *              direction (N, E, S, W). Formatted as "2NE" or 2"SW".
             *
             * 3[d1] = A square which forms a T-intersect. Since all directions
             *         but one are turnable, only the invalid one is stored as d1.
             *         
             * 4 = A square forming a 4 way intersection. A turn may be made in
             *     any direction so no additional data is required.
             */





        }

        public static void debugWriteGridCollisionWrite(gamegrid toTest)
        {
            string line = string.Empty;

            int i = 0;

            System.Console.WriteLine("HITBOX");
            while (i < toTest.xSize)
            {
                System.Console.WriteLine(toTest.collisions[i]);

                i++;
            }

        }
    }
}
