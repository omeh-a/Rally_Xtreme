﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace RallyXtreme
{
    public struct playerChar
    {
        public string name;
        public int speed;
        public int pixelXY;
        public int ability;
        public string modelDirectory;
    }

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
        


        public static playerChar createPlayer(string playerDirectory)
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
