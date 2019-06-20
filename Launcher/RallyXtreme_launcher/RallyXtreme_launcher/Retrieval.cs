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
        public static int mapNum = 0;
        public static int playerNum = 0;
        public static int enemyNum = 0;

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
            Console.WriteLine($"#FILE SEARCH# Attempting to read maps from {directory}\\Content\\Maps");

            string[] directories = Directory.GetDirectories($"{directory}\\Content\\Maps");
            int i = 0;

            while (i < directories.Length)
            {
                found[i].type = 'm';
                found[i].directory = directories[i];
                found[i].typeDesc = getDesc(directories[i]);
                Console.WriteLine($"#FILE SEARCH# Found {found[i].directory}");
                i++;
                mapNum++;
                
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

            while (i < directories.Length)
            {
                found[i].type = 'p';
                found[i].directory = directories[i];
                found[i].typeDesc = getDesc(directories[i]);
                i++;
                playerNum++;
            }




            return found;
        }
        public static description[] getEnemies()
        {
            description[] found = new description[255];
            string directory = Path.GetDirectoryName(Process.GetCurrentProcess().MainModule.FileName);
            Console.WriteLine($"#FILE SEARCH# Attempting to read cache from {directory}");

            string[] directories = Directory.GetDirectories($"{directory}\\Content\\Enemies");
            int i = 0;

            while (i < directories.Length)
            {
                found[i].type = 'm';
                found[i].directory = directories[i];
                found[i].typeDesc = getDesc(directories[i]);
                i++;
                enemyNum++;
            }




            return found;
        }
        public static string[] getDesc(string directory)
        {
            string[] result = new string[2];


            directory = directory + "\\description.txt";
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
                        Console.WriteLine($"#GETDESC# read: {line}");
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
