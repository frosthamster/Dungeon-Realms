using System.Drawing;

namespace Dungeon_Realms
{
    class Wall : GameObject
    {
        public Wall(GameObject[,] map, Point location, Bitmap texture) 
            : base(map, location,false, texture)
        {
        }
    }
}
