using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Diagnostics;

namespace RallyXtreme
{


    public static class CacheLoad
    {
        /* 
         * 
         * This class is responsible for the reading of and
         * management of the cache file which is written to by
         * either the launcher or the main menu.
         * 
         * Data is limited to the directories for customisable 
         * content as well as the difficulty value.
         *
         */
        public static bool cacheCheck()
        {
            bool cacheValid = true;
            Debug.Assert(File.Exists(getLocation()) == true);
            return cacheValid;
        }
        public static string getMap()
        {
            // #1
            Debug.Assert(CacheLoad.cacheCheck() == true);
            string directory = get(1);
            System.Console.WriteLine($"#CACHEREAD# found map as {directory}");
            return directory;
        }

        public static string getPlayer()
        {
            // #2
            Debug.Assert(CacheLoad.cacheCheck() == true);
            string directory = get(2);
            System.Console.WriteLine($"#CACHEREAD# found player as {directory}");
            return directory;
        }

        public static string getAi()
        {
            // #3
            Debug.Assert(CacheLoad.cacheCheck() == true);
            string directory = get(3);
            System.Console.WriteLine($"#CACHEREAD# found AI as {directory}");
            return directory;
        }

        public static int getDifficulty()
        {
            // #4
            Debug.Assert(CacheLoad.cacheCheck() == true);
            int difficulty = 0;
            // stub
            return difficulty;
        }

        public static int getResolutionX()
        {
            // 5
            Debug.Assert(CacheLoad.cacheCheck() == true);
            string resolutionString = get(5);
            int resolution = 0;
            // The following loop attempts to convert the retrieved strings into 
            try
            {
                resolution = Int32.Parse(resolutionString);
            }
            catch (FormatException)
            {
                Console.WriteLine($"Unable to parse '{resolutionString}' for resolutionX");
            }
                
            
            System.Console.WriteLine($"#CACHEREAD# found xRes as {resolution}");
            return resolution;
        }

        public static int getResolutionY()
        {
            // 6
            Debug.Assert(CacheLoad.cacheCheck() == true);
            string resolutionString = get(6);
            int resolution = 0;
            // The following loop attempts to convert the retrieved strings into 
            try
            {
                resolution = Int32.Parse(resolutionString);
            }
            catch (FormatException)
            {
                Console.WriteLine($"Unable to parse '{resolutionString}' for resolutionX");
            }


            System.Console.WriteLine($"#CACHEREAD# found yRes as {resolution}");
            return resolution;
        }

        private static string get(int type)
        {
            string result = "";
            string directory = getLocation();
            Console.WriteLine($"#CACHEREAD# Attempting to read cache from {directory}");


            string line;
            int i = 0;


            // finding data
            try
            {
                using (StreamReader r = new StreamReader(directory))
                {
                    while ((line = r.ReadLine()) != null)
                    {
                        // Console.Write("#CACHEREAD# " + line + '\n');
                        i++;
                        if (i == 1 && type == 1)
                        {
                            result = line;
                        }
                        if (i == 2 && type == 2)
                        {
                            result = line;
                        }
                        if (i == 3 && type == 3)
                        {
                            result = line;
                        }
                        if (i == 4 && type == 4)
                        {
                            result = line;
                        }
                        if (i == 5 && type == 5)
                        {
                            result = line;
                        }
                        if (i == 6 && type == 6)
                        {
                            result = line;
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

        private static string getLocation()
        {
            string location;
            string folder = Path.GetDirectoryName(Process.GetCurrentProcess().MainModule.FileName);
            string filter = "*.rxtcache";
            //System.Console.WriteLine($"#CACHEREAD# Process file at {folder}");
            string[] files = new string[100];
            files[0] = "NONE";
            files = Directory.GetFiles(folder, filter);
            //System.Console.WriteLine($"#CACHEREAD# Found {Directory.GetFiles(folder, filter)[0]}");
            if (files[0] == "NONE")
            {
                location = "";
                System.Console.WriteLine("#CACHEREAD $CACHE NOT FOUND&");
            }
            else
            {
                location = files[0];
            }
            Debug.Assert(location != "");
            //System.Console.WriteLine($"#CACHEREAD# Found {location}");
            return location;
        }

    }
}
// "C:\\Users\\matth\\Documents\\GitHub\\Rally_Xtreme\\Game\\RallyXtreme\\RallyXtreme\\bin\\DesktopGL\\AnyCPU\\Debug";