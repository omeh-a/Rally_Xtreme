using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RallyXtreme
{
    class Player
    {
        public int xPos = Game1.gameMap.xSize / 2;
        public int yPos = Game1.gameMap.ySize / 2;
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
    }
}
