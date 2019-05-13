using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RallyXtreme
{
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
    }
}
