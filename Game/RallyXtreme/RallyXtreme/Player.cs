﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;


namespace RallyXtreme
{
    public struct playerChar
    {
        public string name;
        public int speed;
        public int pixelXY;
        public int ability;
        public string modelDirectory;
        public Vector2 pos;
        public int gridX;
        public int gridY;
        public ushort direction;
        public bool alive;
        public int lives;
        public int gridPixelSize;
    }

    class Player
    {
        
        public static playerChar updatePos(ushort desiredDirection, playerChar player)
        {


            player = stepFoward(desiredDirection, player);

            return player;
        }

        public static playerChar stepFoward(ushort direction, playerChar player)
        {

            if (direction == 1 && player.gridX < 14)
            {
                player.gridX += 1;
                player.direction = 1;
            }

            if (direction == 3 && player.gridX > 1)
            {
                player.gridX -= 1;
                player.direction = 3;
            }

            if (direction == 0 && player.gridY > 1)
            {
                player.gridY -= 1;
                player.direction = 0;
            }

            if (direction == 2 && player.gridY < 13)
            {
                player.gridY += 1;
                player.direction = 2;
            }



            player.pos = new Vector2(((player.gridX * player.gridPixelSize) + player.gridPixelSize / 2),
                ((player.gridY * player.gridPixelSize)) + player.gridPixelSize / 2);

            return player;
        }

        public static void kill(playerChar player)
        {
            //stub
            Console.Out.WriteLine("#PLAYER# Player has been killed !");
            player.alive = false;
            player.lives = player.lives - 1;
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
