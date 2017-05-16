using System.Drawing;

namespace Dungeon_Realms
{
    public class Box : MovableGameObject
    {
        public Box(GameObject[,] map, Point location, Bitmap texture) 
            : base(map, location, false, texture, false)
        {
        }

        public Box(GameObject[,] map, Point location)
            : base(map, location, false, Textures.Box, false)
        {
        }
    }
}
