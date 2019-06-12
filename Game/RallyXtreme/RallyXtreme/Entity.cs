using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RallyXtreme
{
    public struct gameEntity
    {
        public char type;
        public ushort x;
        public ushort y;
        public bool active;
    }

    class Entity
    {
        


        public static gameEntity createFlagEntity(gamegrid grid, ushort x, ushort y)
        {
            gameEntity newFlag = new gameEntity();

            newFlag.type = 'f';
            newFlag.x = x;
            newFlag.y = x;

            return newFlag;
        }

        public static gameEntity deactivateEntity(gameEntity entity)
        {
            entity.active = false;
            return entity;
        }
    }
}
