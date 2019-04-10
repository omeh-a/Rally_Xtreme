using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

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

        public static string getMap()
        {
            string directory = "debug";
            // stub
            return directory;
        }

        public static string getPlayer()
        {
            string directory = "debug";
            // stub
            return directory;
        }

        public static string getAi()
        {
            string directory = "debug";
            // stub
            return directory;
        }

        public static int getDifficulty()
        {
            int difficulty = 0;
            // stub
            return difficulty;
        }

    }
}
