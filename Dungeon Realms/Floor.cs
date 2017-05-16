using System.Drawing;

namespace Dungeon_Realms
{
    public class Floor : GameObject
    {
        public Floor(GameObject[,] map, Point location, Bitmap texture) :
            base(map, location, false, texture)
        {
        }

        public Floor(GameObject[,] map, Point location) 
            : base(map, location, false)
        {
        }
    }
}
