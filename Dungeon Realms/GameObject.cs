using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace Dungeon_Realms
{
    public class GameObject
    {
        protected readonly GameObject[,] Map;
        public GameObject Floor { get; protected set; }
        public bool IsEnemy { get; }
        public Point Location { get; protected set; }
        public Bitmap Texture { get; set; }

        public GameObject(GameObject[,] map, Point location, bool isEnemy, Bitmap texture)
        {
            Map = map;
            Location = location;
            IsEnemy = isEnemy;
            Texture = texture;
        }

        public GameObject(GameObject[,] map, Point location, bool isEnemy)
            : this(map, location, isEnemy, Textures.Floor)
        {
        }

        protected virtual void RestoreDefault() { }

        private IEnumerable<GameObject> GetIncidents()
        {
            return Enumerable
                .Range(-1, 3)
                .SelectMany(e => Enumerable.Range(-1, 3).Select(f => new { dx = e, dy = f }))
                .Where(e => e.dx * e.dx + e.dy * e.dy == 1)
                .Select(e => (Location.X + e.dx, Location.Y + e.dy))
                .Where(IsExist)
                .Select(e => Map[e.Item1, e.Item2]);
        }

        protected void Swap(GameObject to)
        {
            Map[Location.X, Location.Y] = Floor ?? new Floor(Map, Location);
            Floor?.RestoreDefault();
            Floor = to;
            Map[to.Location.X, to.Location.Y] = this;
            Location = to.Location;
        }

        private (int dx, int dy) GetOffset(Direction direction)
        {
            var dx = 0;
            var dy = 0;
            if (direction == Direction.Right)
                dy = 1;
            if (direction == Direction.Left)
                dy = -1;
            if (direction == Direction.Up)
                dx = -1;
            if (direction == Direction.Down)
                dx = 1;
            return (dx, dy);
        }

        public GameObject GetIncident(Direction direction)
        {
            (var dx, var dy) = GetOffset(direction);
            return GetIncidents()
                .FirstOrDefault(e => e != null && e.Location.X - Location.X == dx && e.Location.Y - Location.Y == dy);
        }

        private bool IsExist((int, int) location)
        {
            (int x, int y) = location;
            return x >= 0 && y >= 0 && y < Map.GetLength(1) && x < Map.GetLength(0);
        }

        public override string ToString()
        {
            return base.ToString() + Location;
        }
    }
}
