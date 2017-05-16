using System.Drawing;

namespace Dungeon_Realms
{
    public class PressurePlate : GameObject
    {
        public Hatch Target { get; set; } 
        public PressurePlate(GameObject[,] map, Point location, Bitmap texture) 
            : base(map, location, false, texture)
        {
        }

        public PressurePlate(GameObject[,] map, Point location)
            : base(map, location, false, Textures.PressurePlate)
        {
        }

        public void Activate()
        {
            Target.Open();
        }

        public void Deactivate()
        {
            Target.Close();
        }

        protected override void RestoreDefault()
        {
            Deactivate();
        }
    }
}
