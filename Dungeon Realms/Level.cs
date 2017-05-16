using System;
using System.Drawing;

namespace Dungeon_Realms
{
    class Level
    {
        public event Action<string, int> ShowDialog;
        public event Action GameOver;
        public event Action<int> LevelFinished;

        public  Hero Hero { get; }
        public Point Destination { get; }
        public GameObject[,] Map { get; }

        public Level(GameObject[,] map, Hero hero, Point destination)
        {
            Map = map;
            Hero = hero;
            Destination = destination;

            hero.Moved += (obj, direction) =>
            {
                if (hero.IsDead)
                    GameOver?.Invoke();
            };

            hero.Finish += () =>
            {
                LevelFinished?.Invoke(hero.CrystalsCount);
            };
        }
    }
}
