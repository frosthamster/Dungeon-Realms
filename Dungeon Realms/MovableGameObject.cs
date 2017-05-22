using System;
using System.Drawing;

namespace Dungeon_Realms
{
    public class MovableGameObject : GameObject
    {
        public event Action<GameObject, Direction> Moved;
        public event Action<Hatch> CouldNotPassThroughHatch;
        private readonly bool canMoveObjects;

        protected MovableGameObject(GameObject[,] map, Point location, bool isEnemy, bool canMoveObjects)
            :
            base(map, location, isEnemy)
        {
            this.canMoveObjects = canMoveObjects;
        }

        protected MovableGameObject(GameObject[,] map, Point location, bool isEnemy, Bitmap texture, bool canMoveObjects) :
            base(map, location, isEnemy, texture)
        {
            this.canMoveObjects = canMoveObjects;
        }

        public virtual bool TryMove(Direction direction)
        {
            Moved?.Invoke(this, direction);
            var destination = GetIncident(direction);
            if (destination == null)
                return false;

            if (destination is Wall || destination is Finish)
                return false;

            if (destination is Hatch hatch)
            {
                if (!hatch.IsOpened)
                {
                    Swap(destination);
                    return true;
                }
                destination = hatch.GetDestination(direction, canMoveObjects);
                if (destination != null)
                {
                    if (destination is Box box)
                    {
                        if (box.TryMove(direction))
                            Swap(hatch.GetDestination(direction, canMoveObjects));
                        else
                            CouldNotPassThroughHatch?.Invoke(hatch);
                    }
                    else
                        Swap(destination);
                    return true;
                }
                CouldNotPassThroughHatch?.Invoke(hatch);
            }
            if (destination is PressurePlate plate)
            {
                plate.Activate();
                Swap(plate);
                return true;
            }
            if (destination is Floor floor)
            {
                Swap(floor);
                return true;
            }
            return false;
        }
    }
}
