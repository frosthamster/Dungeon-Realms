using System;
using System.Drawing;

namespace Dungeon_Realms
{
     public class Hero : MovableGameObject
    {
        public event Action CrystalEarned;
        public bool IsDead { get; private set; }
        public int CrystalsCount { get; private set; }
        public Direction Orientation { get; private set; } = Direction.Right;
        public event Action Finish;

        public Hero(GameObject[,] map, Point location)
            : base(map, location, false, Textures.HeroRight, true)
        {
        }

        public Hero(GameObject[,] map, Point location, Bitmap texture)
            : base(map, location, false, texture, true)
        {
        }

        public void Rotate()
        {
            if (Orientation == Direction.Right)
            {
                Orientation = Direction.Left;
                Texture = Textures.HeroLeft;
            }
            else
            {
                Orientation = Direction.Right;
                Texture = Textures.HeroRight;
            }
        }

        public override bool TryMove(Direction direction)
        {
            var moved = base.TryMove(direction);

            if (!moved)
            {
                var destination = GetIncident(direction);
                if (destination == null)
                    return false;
                if (destination.IsEnemy)
                {
                    IsDead = true;
                    return false;
                }
                if (destination is Crystal crystal)
                {
                    CrystalEarned?.Invoke();
                    CrystalsCount++;
                    Map[crystal.Location.X, crystal.Location.Y] = new Floor(Map, crystal.Location);
                    Swap(Map[crystal.Location.X, crystal.Location.Y]);
                }
                if (destination is Box box)
                {
                    if (box.TryMove(direction))
                    {
                        Swap(GetIncident(direction));
                    }
                    else
                        return false;
                }
                if (destination is Finish)
                    Finish?.Invoke();
            }
            return moved;
        }
    }
}