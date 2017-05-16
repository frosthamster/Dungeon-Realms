using System.Drawing;

namespace Dungeon_Realms
{
    class Crystal : GameObject
    {
        public Crystal(GameObject[,] map, Point location) :
            base(map, location, false)
        {
        }

        public Crystal(GameObject[,] map, Point location, Bitmap texture) 
            : base(map, location, false, texture)
        {
        }
    }
}
