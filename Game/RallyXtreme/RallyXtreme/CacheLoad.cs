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
            string folder = /*"C:\\Users\\matth\\Documents\\GitHub\\Rally_Xtreme\\Game\\RallyXtreme\\RallyXtreme\\cache";*/ 
                Path.GetDirectoryName(Process.GetCurrentProcess().MainModule.FileName) + @"\cache\";
            string filter = "*.rxtcache";
            System.Console.WriteLine($"Process file at {folder}");
            string[] files = Directory.GetFiles(folder, filter);
            System.Console.WriteLine($"Found {files}");

            if (files[0] == null)
            {
                cacheValid = false;
            }
            
<<<<<<< HEAD
            bool cacheValid = true;
            Debug.Assert(File.Exists(getLocation()) == true);
=======
>>>>>>> parent of 54a845a... Finished cacheLoad class
            return cacheValid;
        }
        public static string getMap()
        {
            Debug.Assert(CacheLoad.cacheCheck() == true);
            string directory = "debug";
            // stub
            return directory;
        }

        public static string getPlayer()
        {
            Debug.Assert(CacheLoad.cacheCheck() == true);
            string directory = "debug";
            // stub
            return directory;
        }

        public static string getAi()
        {
            Debug.Assert(CacheLoad.cacheCheck() == true);
            string directory = "debug";
            // stub
            return directory;
        }

        public static int getDifficulty()
        {
            Debug.Assert(CacheLoad.cacheCheck() == true);
            int difficulty = 0;
            // stub
            return difficulty;
        }

    }
}
