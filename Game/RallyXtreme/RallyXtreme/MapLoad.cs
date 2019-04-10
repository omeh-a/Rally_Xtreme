using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace RallyXtreme
{
    public static class MapLoad
    {
        public struct map
        {
            public ushort gridSize;
            public ushort xSize;
            public ushort ySize;
        }
        

        public static map loadMap(string directory)
        {
            map foundMap = new map();
            if (File.Exists(directory))
            {
                // deserialise XML file and retrieve map data
            }
            // stub function


            return foundMap;
        }


        public static map debugLoad(bool debugState)
        {
            // loads a hardcoded version of env_1314
            map foundMap = new map();
            // loadstate is included for testing sake

            // stub function

            return foundMap;
        }

        public static bool checkMap(map toCheck)
        {
            bool state = true;
            // stub function
            return state;
        }
    }
}
