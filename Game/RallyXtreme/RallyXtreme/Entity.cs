using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace RallyXtreme
{
    public struct gameEntity
    {
        public char type;
        public ushort x;
        public ushort y;
        public bool active;
        public Vector2 pos;
        
    }

    class Entity
    {
        


        public static gameEntity createFlagEntity(gamegrid grid, ushort x, ushort y)
        {
            gameEntity newFlag = new gameEntity();

            newFlag.type = 'f';
            newFlag.x = x;
            newFlag.y = x;
            newFlag.pos = new Vector2((grid.pixelSize * x), (grid.pixelSize * y));
            newFlag.active = true;
            return newFlag;
        }

        public static gamegrid deactivateEntity(ushort x, ushort y, gamegrid grid)
        {
            grid.entities[y][x].active = false;
            return grid;
        }

        public static uint flagsRemaining(gamegrid g)
        {
            uint count = 0;
            int i = 0;
            while (i < g.ySize)
            {
                int e = 0;
                while (e < g.xSize)
                {
                    if (g.entities[i][e].type == 'f' && g.entities[i][e].active == true)
                    {
                        count++;
                    }
                    e++;
                }
                i++;
            }
            Console.WriteLine($"#ENTITY# Found {count} flags remaining");

            return count;
        }
    }
}
