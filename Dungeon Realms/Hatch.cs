using System;
using System.Drawing;

namespace Dungeon_Realms
{
    public class Hatch : GameObject
    {
        private Hatch @out;
        public Hatch Out
        {
            get => @out;
            set
            {
                if(IsOpened != value.IsOpened)
                    throw new ArgumentException();
                @out = value;
                @out.@out = this;
            }
        }

        public event Action Opened;
        public event Action Closed;

        public bool IsOpened { get; private set; } = true;

        public Hatch(GameObject[,] map, Point location) 
            : base(map, location, false, Textures.OpenedHatch)
        {
        }

        public Hatch(GameObject[,] map, Point location, Bitmap texture)
            : base(map, location, false, texture)
        {
        }

        public GameObject GetDestination(Direction direction, bool canMoveObjects)
        {
            var destination = Out.GetIncident(direction);
            if (destination == null || destination is Wall)
                return null;
            
            if (destination is Hatch hatch)
                return hatch.GetDestination(direction, canMoveObjects);
            if (destination is Box && canMoveObjects || !(destination is Box))
                return destination;

            return null;
        }

        public void Open()
        {
            Texture = Textures.OpenedHatch;
            Opened?.Invoke();
            IsOpened = true;
            Map[Location.X, Location.Y] = this;
            if(!Out.IsOpened)
                Out.Open();
        }

        public void Close()
        {
            Texture = Textures.ClosedHatch;
            Closed?.Invoke();
            IsOpened = false;
            Map[Location.X, Location.Y] = this;
            if (Out.IsOpened)
                Out.Close();
        }
    }
}
