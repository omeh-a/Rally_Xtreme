using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Diagnostics;


namespace RallyXtreme_launcher
{
    public class Retrieval
    {
        public static description playerDesc;
        public static description enemyDesc;
        public static description mapDesc;
        public static description[] maps;
        public static description[] players;
        public static description[] enemies;


        public static void selectPlayer(int direction)
        {

        }
        public static void selectEnemy(int direction)
        {

        }

        public static description[] getMaps()
        {
            description[] found = new description[255];
            string directory = Path.GetDirectoryName(Process.GetCurrentProcess().MainModule.FileName);
            Console.WriteLine($"#FILE SEARCH# Attempting to read cache from {directory}");

            string[] directories = Directory.GetDirectories($"{directory}\\Content\\Maps");
            int i = 0;

            while (i > directories.Length)
            {
                found[i].type = 'm';
                found[i].directory = directories[i];
                found[i].typeDesc = getDesc(directories[i]);
                i++;
            }


            return found;
        }
        public static description[] getPlayers()
        {
            description[] found = new description[255];
            string directory = Path.GetDirectoryName(Process.GetCurrentProcess().MainModule.FileName);
            Console.WriteLine($"#FILE SEARCH# Attempting to read cache from {directory}");

            string[] directories = Directory.GetDirectories($"{directory}\\Content\\Characters");
            int i = 0;

            while (i > directories.Length)
            {
                found[i].type = 'p';
                found[i].directory = directories[i];
                found[i].typeDesc = getDesc(directories[i]);
                i++;
            }




            return found;
        }
        public static description[] getEnemies()
        {
            description[] found = new description[255];
            string directory = Path.GetDirectoryName(Process.GetCurrentProcess().MainModule.FileName);
            Console.WriteLine($"#FILE SEARCH# Attempting to read cache from {directory}");

            string[] directories = Directory.GetDirectories($"{directory}\\Content\\Maps");
            int i = 0;

            while (i > directories.Length)
            {
                found[i].type = 'm';
                found[i].directory = directories[i];
                found[i].typeDesc = getDesc(directories[i]);
                i++;
            }




            return found;
        }
        private static string[] getDesc(string directory)
        {
            string[] result = new string[2];
            


            string line;
            int i = 0;


            // finding data
            try
            {
                using (StreamReader r = new StreamReader(directory))
                {
                    while ((line = r.ReadLine()) != null)
                    {
                        // Console.Write("#FILE SEARCH# " + line + '\n');
                        i++;
                        if (i == 1)
                        {
                            result[0] = line;
                        }
                        if (i == 2)
                        {
                            result[1] = line;
                        }
                    }
                }
            }

            // Shows an error message if something happens
            catch (Exception er)
            {
                System.Console.WriteLine($"### CACHE READ EXCEPTION -> {er}");
            }



            return result;
        }


    }
}
