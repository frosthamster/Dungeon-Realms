using System.Drawing;

namespace Dungeon_Realms
{
    class Finish : GameObject
    {
        public Finish(GameObject[,] map, Point location) 
            : base(map, location, false, Textures.Princess)
        {
        }
    }
}
