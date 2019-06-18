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
        public gameEntity[][] entities;
        public int[,] turnData;
        public int xSize;
        public int ySize;
        public int pixelSize;
        public string name;
        public int[] playerStart;
        public int[][] enemystart;
        public uint enemyCount;
        public byte[] roadColour;
        public byte[] wallColour;
        public byte[] borderColour;
    }



    class Grid
    {
        public static char returnEntityType(int x, int y, gamegrid grid)
        {
            // This function returns the contents of the entity array
            // if the grid refers to an empty space or a border tile
            // a * will be returned instead.
            char entity = grid.entities[y][x].type;
            if (entity == '$' || entity == '#')
                entity = '*';
            return entity;
        }

        public static bool returnEntityState(int x, int y, gamegrid grid)
        {
            // This function returns the Activity state of an entity in
            // the array.
            bool active= grid.entities[y][x].active;
            
            return active;
        }

        public static gamegrid populateFlags(gamegrid grid0)
        {
            uint flagNumber = (uint) grid0.xSize;
            int f = 0;
            

            while (f < flagNumber)
            {
                
                int v = 1;
                Random r = new Random();
                while (v <= grid0.ySize-1)
                {
                    int h = 0;
                    while (h <= grid0.xSize)
                    {
                        if (r.Next(0, 5) == 2 && grid0.collisions[v][h] == '#')
                        {
                            grid0.entities[v][h] = Entity.createFlagEntity(grid0, (ushort)h, (ushort)v);
                            f++;
                        }
                        h++;
                    }
                    v++;
                    System.Console.WriteLine($"#GRID# Generated {v} line of flags");
                }
                    
            }


           return grid0;
        }


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
            newGrid.roadColour = new byte[3];
            newGrid.wallColour = new byte[3];
            newGrid.borderColour = new byte[3];

            // ############################################################
            // Reading from mapdata
            // ############################################################
            string xSizeString = "";
            string ySizeString = "";
            string pixelString = "";
            string enemyCountString = "";
            string[] roadColourString = new string[3];
            string[] wallColourString = new string[3];
            string[] borderColourString = new string[3];
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
                            xSizeString = line;
                        
                        if (i == 2)
                            ySizeString = line;
                        
                        if (i == 3)
                            pixelString = line;
                            
                        if (i == 4)
                            newGrid.name = line;
                        
                        if (i == 5)
                            enemyCountString = line;

                        if (i == 6)
                            roadColourString[0] = line;

                        if (i == 7)
                            roadColourString[1] = line;

                        if (i == 8)
                            roadColourString[2] = line;

                        if (i == 9)
                            wallColourString[0] = line;

                        if (i == 10)
                            wallColourString[1] = line;

                        if (i == 11)
                            wallColourString[2] = line;

                        if (i == 12)
                            borderColourString[0] = line;

                        if (i == 13)
                            borderColourString[1] = line;

                        if (i == 14)
                            borderColourString[2] = line;
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
            while (i <= 13)
            {
                try
                {
                    if (i == 1)
                        newGrid.xSize = Int32.Parse(xSizeString);
                    else if (i == 2)
                        newGrid.ySize = Int32.Parse(ySizeString);
                    else if (i == 3)
                        newGrid.pixelSize = Int32.Parse(pixelString);
                    else if (i == 4)
                        newGrid.enemyCount = (uint)Int32.Parse(enemyCountString);
                    else if (i == 5)
                        newGrid.roadColour[0] = (byte)Int32.Parse(roadColourString[0]);
                    else if (i == 6)
                        newGrid.roadColour[1] = (byte)Int32.Parse(roadColourString[1]);
                    else if (i == 7)
                        newGrid.roadColour[2] = (byte)Int32.Parse(roadColourString[2]);
                    else if (i == 8)
                        newGrid.wallColour[0] = (byte)Int32.Parse(wallColourString[0]);
                    else if (i == 9)
                        newGrid.wallColour[1] = (byte)Int32.Parse(wallColourString[1]);
                    else if (i == 10)
                        newGrid.wallColour[2] = (byte)Int32.Parse(wallColourString[2]);
                    else if (i == 11)
                        newGrid.borderColour[0] = (byte)Int32.Parse(borderColourString[0]);
                    else if (i == 12)
                        newGrid.borderColour[1] = (byte)Int32.Parse(borderColourString[1]);
                    else if (i == 13)
                        newGrid.borderColour[2] = (byte)Int32.Parse(borderColourString[2]);
                }
                catch (FormatException er)
                {
                    Console.WriteLine($"Failed to parse -> {er}");
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


            newGrid.entities = new gameEntity[newGrid.ySize][];
            i = 0;

            while (i < newGrid.ySize)
            {
                newGrid.entities[i] = new gameEntity[newGrid.xSize];
                i++;
            }
            return newGrid;
        }


        
        
    }
}
