﻿using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace Dungeon_Realms
{
    static class LevelGenerator
    {
        public const int Width = 25;
        public const int Height = 17;
        private static readonly Dictionary<int, Bitmap> textures = new Dictionary<int, Bitmap>();

        static LevelGenerator()
        {
            for (var i = 1; i <= 6; i++)
                textures[i] = (Bitmap)Image.FromFile($"resources\\t_1_{i}.png");
            for (var i = 8; i <= 19; i++)
                textures[i] = (Bitmap)Image.FromFile($"resources\\t_1_{i - 1}.png");
            for (var i = 20; i <= 34; i++)
                textures[i] = (Bitmap)Image.FromFile($"resources\\t_2_{i - 19}.png");
            for (var i = 35; i <= 36; i++)
                textures[i] = (Bitmap)Image.FromFile($"resources\\t_2_{i - 18}.png");
            for (var i = 37; i <= 54; i++)
                textures[i] = (Bitmap)Image.FromFile($"resources\\t_3_{i - 36}.png");
            for (var i = 55; i <= 70; i++)
                textures[i] = (Bitmap)Image.FromFile($"resources\\t_4_{i - 54}.png");
            for (var i = 73; i <= 83; i++)
                textures[i] = (Bitmap)Image.FromFile($"resources\\t_5_{i - 72}.png");
            for (var i = 95; i <= 99; i++)
                textures[i] = (Bitmap)Image.FromFile($"resources\\t_6_{i - 88}.png");
            for (var i = 108; i <= 113; i++)
                textures[i] = (Bitmap)Image.FromFile($"resources\\t_7_{i - 102}.png");
            for (var i = 84; i <= 86; i++)
                textures[i] = (Bitmap)Image.FromFile($"resources\\t_5_{i - 72}.png");
            for (var i = 73; i <= 86; i++)
                textures[i] = (Bitmap)Image.FromFile($"resources\\t_5_{i - 72}.png");
            for (var i = 100; i <= 102; i++)
                textures[i] = (Bitmap)Image.FromFile($"resources\\t_6_{i - 88}.png");
            for (var i = 116; i <= 118; i++)
                textures[i] = (Bitmap)Image.FromFile($"resources\\t_7_{i - 102}.png");
            for (var i = 130; i <= 134; i++)
                textures[i] = (Bitmap)Image.FromFile($"resources\\t_8_{i - 118}.png");
            for (var i = 144; i <= 150; i++)
                textures[i] = (Bitmap)Image.FromFile($"resources\\t_9_{i - 134}.png");
            for (var i = 151; i <= 158; i++)
                textures[i] = (Bitmap)Image.FromFile($"resources\\t_10_{i - 150}.png");
            for (var i = 159; i <= 166; i++)
                textures[i] = (Bitmap)Image.FromFile($"resources\\t_11_{i - 158}.png");
            textures[128] = (Bitmap)Image.FromFile("resources\\t_8_10.png");
            textures[129] = (Bitmap)Image.FromFile("resources\\t_8_11.png");
            textures[103] = (Bitmap)Image.FromFile("resources\\t_7_1.png");
            textures[104] = (Bitmap)Image.FromFile("resources\\t_7_2.png");
            textures[114] = (Bitmap)Image.FromFile("resources\\t_7_12.png");
            textures[115] = (Bitmap)Image.FromFile("resources\\t_7_13.png");
            textures[89] = (Bitmap)Image.FromFile("resources\\t_6_1.png");
            textures[90] = (Bitmap)Image.FromFile("resources\\t_6_2.png");
            textures[94] = (Bitmap)Image.FromFile("resources\\t_6_6.png");
            textures[202] = (Bitmap)Image.FromFile("resources\\cup.png");
            textures[200] = (Bitmap)Image.FromFile("resources\\hero_1.png");
            textures[201] = (Bitmap)Image.FromFile("resources\\hero_2.png");
            textures[0] = (Bitmap)Image.FromFile("resources\\t_zero.png");
        }

        public static Level GetLevel(int index)
        {
            return GetLevels(GetMaps().Skip(index)).FirstOrDefault();
        }

        public static IEnumerable<Level> GetLevels()
        {
            return GetLevels(GetMaps());
        }

        private static IEnumerable<Level> GetLevels(IEnumerable<Data> levelsData)
        {
            foreach (var data in levelsData)
            {
                var boxes = new HashSet<int> { 100, 15, 16, 17, 32, 33, 34, 50, 51, 52, 101, 102, 116 };
                var hatches = new HashSet<int> { 78, 150 };
                var floors = new HashSet<int> {8, 9,  10,  11, 12,13, 14,25,  26, 27, 28, 29,  30,
                    31 , 43 ,44 , 45, 46, 47 ,48, 49 , 61 ,62 , 63 ,64 , 65, 66 ,67, 71 ,72, 88,87, 166, 150};
                var plateCode = 158;

                var map = new GameObject[Height, Width];
                for (var i = 0; i < map.GetLength(0); i++)
                    for (var j = 0; j < map.GetLength(1); j++)
                    {
                        var code = data.Matrix[i, j];
                        if (boxes.Contains(code))
                            map[i, j] = new Box(map, new Point(i, j), textures[code]);
                        else if (hatches.Contains(code))
                            map[i, j] = new Hatch(map, new Point(i, j));
                        else if (floors.Contains(code))
                            map[i, j] = new Floor(map, new Point(i, j), textures[code]);
                        else if (code == plateCode)
                            map[i, j] = new PressurePlate(map, new Point(i, j), textures[code]);
                        else
                            map[i, j] = new Wall(map, new Point(i, j), textures[code]);
                    }

                for (var i = 0; i < data.Hatches.Length; i++)
                    for (var j = 1; j < data.Hatches[i].Length; j++)
                    {
                        var inHatchLocation = data.Hatches[i][0];
                        var inHatch = (Hatch)map[inHatchLocation.X, inHatchLocation.Y];
                        var outHatchLocation = data.Hatches[i][j];
                        var outHatch = (Hatch)map[outHatchLocation.X, outHatchLocation.Y];
                        inHatch.Out = outHatch;
                        outHatch.Out = inHatch;
                    }

                for (var i = 0; i < data.PressurePlates.Length; i++)
                    for (var j = 1; j < data.PressurePlates[i].Length; j++)
                    {
                        var platLocation = data.PressurePlates[i][0];
                        var plate = (PressurePlate)map[platLocation.X, platLocation.Y];
                        var outHatchLocation = data.PressurePlates[i][j];
                        var outHatch = (Hatch)map[outHatchLocation.X, outHatchLocation.Y];
                        plate.Target = outHatch;
                        outHatch.Close();
                    }

                var hero = new Hero(map, data.Start);
                var finish = new Finish(map, data.Finish);
                map[data.Finish.X, data.Finish.Y] = finish;
                map[data.Start.X, data.Start.Y] = hero;
                yield return new Level(map, hero, data.Finish);
            }
        }

        internal class Data
        {
            public int[,] Matrix { get; set; }
            public Point Start { get; set; }
            public Point Finish { get; set; }
            public Point[][] PressurePlates { get; set; }
            public Point[][] Hatches { get; set; }
        }

        private static IEnumerable<Data> GetMaps()
        {
            yield return new Data
            {
                Matrix = new[,]
                {
                    {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
                    {0, 0, 0, 0, 0, 6, 0, 0, 0, 6, 0, 0, 0, 0, 6, 0, 0, 0, 6, 0, 0, 0, 0, 0, 0},
                    {0, 0, 0, 0, 4, 24, 56, 38, 38, 24, 5, 0, 0, 4, 24, 38, 38, 55, 24, 5, 0, 0, 0, 0, 0},
                    {0, 0, 0, 40, 41, 8, 8, 8, 8, 62, 58, 59, 40, 41, 61, 8, 15, 32, 16, 58, 59, 0, 0, 0, 0},
                    {0, 0, 0, 0, 21, 8, 64, 8, 8, 8, 37, 38, 55, 39, 8, 8, 8, 16, 52, 74, 0, 0, 0, 0, 0},
                    {0, 0, 0, 0, 21, 8, 8, 8, 8, 8, 8, 33, 8, 8, 8, 8, 8, 8, 33, 20, 0, 0, 0, 0, 0},
                    {0, 0, 0, 0, 103, 8, 8, 8, 8, 8, 1, 77, 2, 3, 8, 8, 8, 8, 8, 20, 0, 0, 0, 0, 0},
                    {0, 0, 0, 40, 41, 62, 8, 8, 8, 8, 58, 59, 40, 41, 8, 8, 100, 8, 62, 58, 59, 0, 0, 0, 0},
                    {0, 0, 0, 0, 22, 42, 3, 8, 1, 42, 23, 0, 0, 22, 42, 3, 8, 1, 42, 23, 0, 0, 0, 0, 0},
                    {0, 0, 0, 0, 0, 60, 21, 50, 20, 60, 0, 0, 0, 0, 60, 21, 8, 20, 60, 0, 0, 0, 0, 0, 0},
                    {0, 0, 0, 0, 0, 4, 39, 52, 37, 5, 6, 0, 0, 0, 6, 21, 8, 20, 0, 0, 0, 0, 0, 0, 0},
                    {0, 0, 0, 0, 40, 41, 63, 8, 8, 37, 24, 38, 38, 55, 24, 39, 8, 104, 0, 0, 0, 0, 0, 0, 0},
                    {0, 0, 0, 0, 0, 73, 8, 8, 8, 8, 8, 61, 8, 8, 8, 8, 64, 58, 59, 0, 0, 0, 0, 0, 0},
                    {0, 0, 0, 0, 40, 41, 8, 8, 8, 1, 42, 2, 75, 42, 2, 2, 42, 23, 0, 0, 0, 0, 0, 0, 0},
                    {0, 0, 0, 0, 0, 22, 42, 2, 42, 23, 60, 0, 0, 60, 0, 0, 60, 0, 0, 0, 0, 0, 0, 0, 0},
                    {0, 0, 0, 0, 0, 0, 60, 0, 60, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
                    {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0}
                },
                Start = new Point(5, 7),
                Finish = new Point(12, 7),
                PressurePlates = new Point[0][],
                Hatches = new Point[0][]
            };

            yield return new Data
            {
                Matrix = new[,]
                {
                    {0, 0, 0, 0, 0, 6, 0, 0, 0, 6, 0, 0, 0, 6, 0, 0, 0, 6, 0, 0, 0, 6, 0, 0, 0},
                    {0, 4, 38, 38, 38, 24, 38, 38, 38, 24, 38, 38, 38, 24, 56, 38, 38, 24, 55, 38, 55, 24, 57, 5, 0},
                    {0, 21, 8, 8, 8, 8, 8, 64, 8, 8, 8, 8, 8, 65, 8, 8, 8, 8, 8, 8, 9, 8, 67, 20, 0},
                    {0, 21, 15, 79, 80, 80, 80, 81, 80, 80, 80, 81, 80, 80, 80, 81, 80, 80, 80, 81, 82, 8, 100, 74, 0},
                    {0, 73, 100, 8, 8, 16, 8, 8, 100, 100, 63, 8, 8, 8, 8, 8, 8, 31, 8, 8, 8, 8, 32, 20, 0},
                    {40, 41, 8, 17, 8, 8, 100, 8, 8, 8, 100, 8, 95, 82, 32, 8, 34, 83, 8, 95, 80, 82, 8, 58, 59},
                    {0, 21, 8, 79, 96, 69, 70, 79, 82, 8, 83, 8, 113, 8, 8, 8, 8, 129, 8, 113, 29, 8, 30, 20, 0},
                    {0, 73, 8, 8, 129, 85, 86, 8, 100, 8, 113, 8, 99, 8, 83, 8, 51, 100, 8, 113, 8, 79, 80, 156, 0},
                    {0, 21, 8, 8, 8, 8, 8, 100, 8, 100, 113, 27, 113, 8, 113, 52, 50, 8, 8, 113, 8, 8, 8, 74, 0},
                    {40, 41, 8, 79, 80, 81, 80, 80, 82, 8, 99, 8, 113, 8, 113, 50, 8, 28, 8, 99, 8, 108, 8, 58, 59},
                    {0, 21, 8, 8, 25, 8, 8, 8, 8, 8, 113, 8, 113, 8, 99, 8, 11, 83, 8, 111, 82, 8, 13, 20, 0},
                    {0, 103, 8, 8, 8, 14, 8, 8, 26, 8, 113, 62, 99, 8, 113, 8, 8, 113, 8, 113, 8, 8, 8, 20, 0},
                    {0, 21, 8, 108, 8, 79, 80, 80, 80, 80, 110, 8, 113, 8, 113, 100, 100, 99, 8, 99, 12, 8, 79, 156, 0},
                    {40, 41, 8, 50, 8, 52, 8, 8, 8, 8, 8, 8, 113, 8, 129, 8, 8, 113, 8, 109, 82, 8, 8, 58, 59},
                    {0, 22, 2, 42, 75, 2, 42, 75, 2, 3, 8, 8, 113, 10, 8, 8, 8, 113, 8, 8, 8, 61, 8, 20, 0},
                    {0, 0, 0, 60, 0, 0, 60, 0, 0, 22, 42, 2, 164, 42, 76, 77, 42, 164, 2, 77, 42, 2, 2, 23, 0},
                    {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 60, 0, 0, 60, 0, 0, 60, 0, 0, 0, 60, 0, 0, 0, 0}
                },
                Start = new Point(8, 8),
                Finish = new Point(2, 2),
                PressurePlates = new Point[0][],
                Hatches = new Point[0][]
            };

            yield return new Data
            {
                Matrix = new[,]
                {
                    {0,0,6,0,0,0,6,0,0,0,6,0,0,0,6,0,0,0,6,0,0,0,6,0,0},
                    {0,4,24,38,55,38,24,38,38,57,24,56,38,38,24,38,38,55,24,38,57,38,24,5,0},
                    {40,41,8,8,100,8,50,32,32,8,8,61,8,33,8,8,64,8,51,8,34,8,8,58,59},
                    {0,73,8,8,100,8,8,17,50,8,33,16,8,34,62,8,8,62,8,100,8,8,100,20,0},
                    {0,21,100,100,100,8,15,8,16,62,51,50,8,8,51,51,17,16,32,8,61,100,8,104,0},
                    {0,21,8,8,100,8,33,8,33,8,50,51,8,34,64,8,17,8,8,62,100,8,100,20,0},
                    {40,41,8,16,8,34,8,17,100,51,16,8,15,8,100,100,8,8,8,32,8,16,8,58,59},
                    {0,21,64,17,15,16,15,8,100,8,8,17,61,100,100,8,33,34,50,8,15,8,15,74,0},
                    {0,21,8,8,50,63,17,8,100,17,15,63,8,8,100,8,32,33,8,15,8,50,51,20,0},
                    {0,103,100,100,61,34,15,50,100,8,100,100,100,8,8,8,34,8,17,17,15,50,8,20,0},
                    {40,41,8,100,8,16,33,8,51,52,8,100,100,16,8,16,8,32,32,50,63,8,8,58,59},
                    {0,21,8,8,100,8,8,33,8,33,8,100,100,8,50,8,100,8,16,33,100,100,100,20,0},
                    {0,73,64,8,8,51,16,33,33,8,16,62,8,16,61,100,100,17,15,8,64,100,8,104,0},
                    {0,21,8,8,17,8,8,100,100,8,32,8,32,8,32,8,100,8,8,8,100,61,8,74,0},
                    {40,41,8,50,8,17,8,8,100,8,17,17,8,16,8,51,8,8,100,8,100,8,8,58,59},
                    {0,22,42,2,2,75,42,2,2,76,42,76,2,2,42,2,2,2,42,77,2,2,42,23,0},
                    {0,0,60,0,0,0,60,0,0,0,60,0,0,0,60,0,0,0,60,0,0,0,60,0,0}
                },
                Start = new Point(2, 2),
                Finish = new Point(14, 22),
                PressurePlates = new Point[0][],
                Hatches = new Point[0][]
            };

            yield return new Data
            {
                Matrix = new[,]
                {
                    {0,0,6,0,0,0,6,0,0,0,6,0,0,0,6,0,0,0,6,0,0,0,6,0,0},
                    {0,4,24,38,55,38,24,56,38,38,24,38,38,57,24,38,38,57,24,56,38,38,24,5,0},
                    {40,41,8,101,8,61,8,8,8,101,8,8,8,8,61,8,8,8,8,8,61,8,63,58,59},
                    {0,22,114,82,8,79,80,80,97,81,80,80,80,81,80,80,80,81,80,80,80,96,8,74,0},
                    {0,40,41,8,8,61,8,8,109,80,81,80,80,96,15,16,16,17,16,149,149,113,8,104,0},
                    {0,0,21,8,50,52,8,61,8,32,8,8,8,113,17,95,80,80,96,35,36,113,61,20,0},
                    {0,0,21,8,83,8,79,80,97,82,69,70,8,113,15,113,8,8,109,80,80,110,8,58,59},
                    {0,0,103,8,113,16,63,8,113,8,85,86,8,99,15,113,100,8,61,8,8,62,8,20,0},
                    {0,40,41,8,113,8,83,8,129,8,69,70,8,113,17,99,8,95,80,81,80,96,8,20,0},
                    {0,0,103,8,99,8,113,8,62,8,85,86,8,113,15,113,8,113,62,8,101,113,8,104,0},
                    {0,0,21,62,113,8,113,8,8,95,81,96,8,113,16,113,8,129,8,8,8,99,8,58,59},
                    {0,0,21,8,113,8,109,80,80,110,8,129,8,109,80,110,8,8,8,69,70,113,8,20,0},
                    {0,40,41,8,113,8,61,8,8,8,15,8,8,8,8,8,62,8,8,85,86,113,8,74,0},
                    {0,4,39,8,109,80,80,81,80,82,8,79,80,80,80,81,80,80,80,81,80,110,8,20,0},
                    {40,41,8,8,8,61,8,8,8,8,61,8,8,8,8,61,8,8,8,8,8,8,101,58,59},
                    {0,22,42,75,2,2,42,2,75,2,42,2,2,2,42,77,2,2,42,2,2,76,42,23,0},
                    {0,0,60,0,0,0,60,0,0,0,60,0,0,0,60,0,0,0,60,0,0,0,60,0,0}
                },
                Start = new Point(7, 22),
                Finish = new Point(7, 9),
                PressurePlates = new Point[0][],
                Hatches = new Point[0][]
            };

            yield return new Data
            {
                Matrix = new[,]
                {
                    {0,0,0,0,0,0,0,0,6,0,0,6,0,0,0,0,0,0,0,0,0,0,0,0,0},
                    {0,0,0,0,0,0,0,4,24,38,38,24,5,0,0,0,0,0,0,0,0,0,0,0,0},
                    {0,0,0,0,0,0,40,41,8,8,8,78,58,59,0,0,0,0,0,0,0,0,0,0,0},
                    {0,0,0,0,0,0,0,21,8,8,8,62,20,0,0,0,6,0,0,0,6,0,0,0,0},
                    {0,0,0,0,0,0,0,22,3,100,1,2,23,0,0,4,24,38,38,38,24,5,0,0,0},
                    {0,0,0,0,0,0,0,0,21,8,20,0,0,0,40,41,8,8,8,8,61,58,59,0,0},
                    {0,0,0,0,0,0,0,4,39,8,20,0,0,0,0,157,80,82,52,108,8,20,0,0,0},
                    {0,0,0,0,0,0,40,41,78,8,58,59,0,0,0,21,64,8,8,8,8,20,0,0,0},
                    {0,0,0,0,0,0,0,22,3,8,20,0,0,0,0,21,8,79,80,80,80,156,0,0,0},
                    {0,0,0,0,0,0,0,0,21,100,20,0,0,0,0,21,34,16,8,8,61,20,0,0,0},
                    {0,0,0,0,0,0,0,4,39,8,37,5,0,0,0,21,8,32,8,79,80,156,0,0,0},
                    {0,0,0,0,0,0,40,41,63,8,8,58,59,0,40,41,8,15,8,8,78,58,59,0,0},
                    {0,0,0,0,0,0,0,21,8,78,8,20,0,0,0,22,42,2,2,2,42,23,0,0,0},
                    {0,0,0,0,0,0,40,41,8,8,64,58,59,0,0,0,60,0,0,0,60,0,0,0,0},
                    {0,0,0,0,0,0,0,22,42,2,42,23,0,0,0,0,0,0,0,0,0,0,0,0,0},
                    {0,0,0,0,0,0,0,0,60,0,60,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
                    {0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0}
                },
                Start = new Point(5, 16),
                Finish = new Point(2, 8),
                PressurePlates = new Point[0][],
                Hatches = new[]
                {
                   new []{ new Point(2,11), new Point(7,8)},
                    new []{ new Point(12,9), new Point(11,20)}
                }
            };

            yield return new Data
            {
                Matrix = new[,]
                {
                    {0,0,6,0,0,0,6,0,0,0,6,0,0,0,6,0,0,0,6,0,0,0,6,0,0},
                    {0,4,24,38,38,55,24,56,38,38,24,38,55,38,24,57,38,56,24,38,38,56,24,5,0},
                    {40,41,78,8,8,8,8,8,8,8,8,8,8,63,83,61,8,8,8,8,8,8,8,58,59},
                    {0,73,8,8,108,69,70,79,80,81,80,82,8,8,113,8,8,8,8,64,8,8,8,74,0},
                    {0,21,8,8,62,85,86,61,8,8,69,70,8,95,112,82,51,8,8,8,8,8,8,20,0},
                    {0,21,8,83,8,8,8,8,83,8,85,86,78,113,100,8,8,79,81,82,78,95,80,156,0},
                    {40,41,32,111,80,81,80,97,110,8,69,70,8,113,100,8,8,8,8,8,8,113,61,58,59},
                    {0,103,34,129,8,8,62,129,8,8,85,86,8,109,81,80,81,80,96,63,8,113,8,74,0},
                    {0,21,8,8,8,8,8,100,78,8,95,82,51,69,70,8,8,8,109,96,8,129,8,20,0},
                    {0,73,8,78,79,80,81,80,97,80,110,64,8,85,86,8,50,8,62,113,8,8,8,20,0},
                    {40,41,8,78,8,8,8,100,113,8,8,8,8,8,8,8,8,8,8,113,8,62,8,58,59},
                    {0,157,80,81,96,8,95,81,98,8,83,100,100,95,80,80,81,80,80,98,8,8,8,104,0},
                    {0,21,69,70,129,8,113,62,113,8,109,80,80,98,61,8,8,8,8,113,63,8,8,20,0},
                    {0,103,85,86,8,8,129,8,113,8,8,33,64,109,80,80,81,80,80,112,82,8,8,20,0},
                    {40,41,8,8,8,78,8,8,113,78,15,8,8,8,8,8,8,8,8,62,8,8,8,58,59},
                    {0,22,42,2,2,75,42,2,164,2,42,76,2,77,42,2,2,2,42,77,2,2,42,23,0},
                    {0,0,60,0,0,0,60,0,0,0,60,0,0,0,60,0,0,0,60,0,0,0,60,0,0}
                },
                Start = new Point(2, 22),
                Finish = new Point(14, 2),
                PressurePlates = new Point[0][],
                Hatches = new[]
                {
                    new []{ new Point(5,12), new Point(5,20)},
                    new []{ new Point(2,2), new Point(9,3)},
                    new []{ new Point(8,8), new Point(10,3)},
                    new []{ new Point(14,5), new Point(14,9)}
                }
            };

            yield return new Data
            {
                Matrix = new[,]
                {
                    {0,0,6,0,0,0,6,0,0,0,6,0,0,0,6,0,0,0,6,0,0,0,6,0,0},
                    {0,4,24,165,38,56,24,38,38,55,24,38,165,57,24,38,165,57,24,165,38,55,24,5,0},
                    {40,41,61,113,78,8,8,62,50,52,50,61,129,8,8,78,99,100,78,99,64,8,78,58,59},
                    {0,73,8,99,8,8,108,8,8,8,8,8,50,78,79,81,110,8,8,113,8,108,62,74,0},
                    {0,21,8,109,96,8,78,95,80,81,80,96,8,108,62,8,8,8,34,113,78,8,8,20,0},
                    {0,103,8,50,109,96,8,113,78,8,63,113,78,8,8,79,80,96,8,111,81,82,8,20,0},
                    {40,41,8,8,61,113,8,111,81,82,8,109,82,8,83,8,78,99,8,99,78,63,8,58,59},
                    {0,21,8,8,8,113,8,113,78,8,8,78,100,8,129,8,78,113,8,109,81,96,8,104,0},
                    {0,21,8,8,8,99,8,109,80,82,8,95,96,8,78,95,80,110,8,8,63,113,64,20,0},
                    {0,103,63,8,8,113,8,8,78,64,78,99,113,33,8,129,8,8,79,96,8,113,8,74,0},
                    {40,41,79,96,8,113,8,95,81,80,81,110,99,8,8,8,16,78,61,113,8,99,8,58,59},
                    {0,73,78,113,8,99,8,109,80,81,80,80,110,8,79,82,78,83,8,99,8,113,8,74,0},
                    {0,21,61,99,8,113,62,8,8,8,8,64,8,8,8,8,62,113,78,113,8,113,8,20,0},
                    {0,21,8,113,8,109,80,80,81,80,80,81,80,80,81,80,80,112,80,110,8,129,8,104,0},
                    {40,41,78,113,63,8,8,8,8,8,8,62,8,8,8,8,8,8,8,8,8,8,61,58,59},
                    {0,22,42,164,75,2,42,76,2,2,42,77,2,75,42,2,2,2,42,76,2,2,42,23,0},
                    {0,0,60,0,0,0,60,0,0,0,60,0,0,0,60,0,0,0,60,0,0,0,60,0,0}
                },
                Start = new Point(14, 20),
                Finish = new Point(7, 10),
                PressurePlates = new Point[0][],
                Hatches = new[]
                {
                    new []{ new Point(6,20), new Point(6,16)},
                    new []{ new Point(2,4), new Point(12,18)},
                    new []{ new Point(4,6), new Point(4,20)},
                    new []{ new Point(2,22), new Point(5,8)},
                    new []{ new Point(7,8), new Point(2,15)},
                    new []{ new Point(9,8), new Point(7,16)},
                    new []{ new Point(9,10), new Point(7,11)},
                    new []{ new Point(2,18), new Point(14,2)},
                    new []{ new Point(11,16), new Point(11,2)},
                    new []{ new Point(8,14), new Point(10,17)},
                    new []{ new Point(3,13), new Point(5,12)}
                }
            };

            yield return new Data
            {
                Matrix = new[,]
                {
                    {0,0,6,0,0,0,6,0,0,0,6,0,0,0,6,0,0,0,6,0,0,0,6,0,0},
                    {0,4,24,38,55,38,24,38,55,38,24,165,55,38,24,38,55,38,24,38,55,165,24,5,0},
                    {40,41,78,8,8,8,25,32,32,32,8,113,29,8,8,8,8,25,83,29,8,129,78,58,59},
                    {0,21,8,95,81,96,8,8,8,8,8,113,8,95,80,80,96,16,129,51,8,8,8,20,0},
                    {0,103,8,109,81,110,8,95,81,96,78,99,8,129,32,32,113,8,8,8,95,81,80,156,0},
                    {0,21,31,8,8,8,43,113,78,109,81,110,8,8,8,8,113,8,8,8,109,81,80,156,0},
                    {40,41,79,80,81,80,97,110,8,78,78,34,8,79,96,8,129,33,83,15,8,8,8,58,59},
                    {0,21,8,8,8,78,113,8,16,79,82,8,50,8,99,31,8,43,99,31,8,8,78,20,0},
                    {0,103,32,78,32,8,99,78,8,8,16,8,108,8,111,80,81,80,128,80,81,80,80,156,0},
                    {0,21,8,8,8,78,113,8,16,79,82,8,50,8,99,29,8,25,99,29,8,8,78,20,0},
                    {40,41,79,80,81,80,112,96,8,78,78,34,8,79,110,8,83,33,129,15,8,8,8,58,59},
                    {0,21,29,8,8,8,25,113,78,95,81,96,8,8,8,8,113,8,8,8,95,81,80,156,0},
                    {0,103,8,95,81,96,8,109,81,110,78,99,8,83,32,32,113,8,8,8,109,81,80,156,0},
                    {0,21,8,109,81,110,8,8,8,8,8,113,8,109,80,80,110,16,83,51,8,8,8,20,0},
                    {40,41,78,8,8,8,43,32,32,32,8,113,31,8,8,8,8,43,129,31,8,83,78,58,59},
                    {0,22,42,2,75,2,42,2,75,2,42,164,75,2,42,2,75,2,42,2,75,164,42,23,0},
                    {0,0,60,0,0,0,60,0,0,0,60,0,0,0,60,0,0,0,60,0,0,0,60,0,0}
                },
                Start = new Point(8, 13),
                Finish = new Point(8, 11),
                PressurePlates = new Point[0][],
                Hatches = new[]
                {
                    new []{ new Point(6,9), new Point(10,10)},
                    new []{ new Point(6,10), new Point(10,9)},
                    new []{ new Point(5,8), new Point(11,8)},
                    new []{ new Point(7,22), new Point(9,22)},
                    new []{ new Point(2,22), new Point(4,10)},
                    new []{ new Point(14,22), new Point(12,10)},
                    new []{ new Point(9,5), new Point(2,2)},
                    new []{ new Point(7,5), new Point(14,2)},
                    new []{ new Point(8,3), new Point(8,7)}
                }
            };

            yield return new Data
            {
                Matrix = new[,]
                {
                    {0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
                    {0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
                    {0,0,6,0,6,0,0,0,6,0,6,0,0,0,6,0,6,0,0,0,6,0,6,0,0},
                    {0,4,24,55,24,5,0,4,24,56,24,5,0,4,24,57,24,5,0,4,24,55,24,5,0},
                    {40,41,62,8,61,58,59,21,8,78,8,20,40,41,63,150,8,58,59,103,63,150,8,58,59},
                    {0,73,8,8,8,20,40,41,8,8,8,58,59,103,8,8,61,20,40,41,8,8,8,20,0},
                    {0,21,8,8,8,74,0,73,15,8,15,74,0,21,33,16,33,74,0,73,8,79,81,156,0},
                    {0,21,8,8,8,20,0,21,16,8,16,20,0,21,8,8,8,20,0,21,8,8,61,74,0},
                    {0,21,63,158,8,20,0,21,33,8,33,20,0,73,52,34,52,20,0,157,81,82,8,20,0},
                    {0,103,8,8,8,104,0,21,8,8,63,104,0,21,8,8,8,20,0,21,64,8,8,20,0},
                    {0,21,8,33,8,20,40,41,52,33,52,58,59,21,8,158,8,104,40,41,8,79,81,156,0},
                    {40,41,8,8,8,58,59,103,61,8,8,20,40,41,64,8,8,58,59,21,8,8,8,58,59},
                    {0,22,3,150,1,23,0,22,3,8,1,23,0,22,3,150,1,23,0,22,3,78,1,23,0},
                    {0,0,22,42,23,0,0,0,22,42,23,0,0,0,22,42,23,0,0,0,22,42,23,0,0},
                    {0,0,0,60,0,0,0,0,0,60,0,0,0,0,0,60,0,0,0,0,0,60,0,0,0},
                    {0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
                    {0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0}
                },
                Start = new Point(6, 3),
                Finish = new Point(12, 9),
                PressurePlates = new[]
                {
                    new []{ new Point(8,3), new Point(12,3), new Point(4,15)},
                    new []{ new Point(10,15), new Point(12,15), new Point(4,21)}
                },
                Hatches = new[]
                {
                    new []{ new Point(12,3), new Point(4,15)},
                    new []{ new Point(12,15), new Point(4,21)},
                    new []{ new Point(12,21), new Point(4,9)},
                }
            };

            yield return new Data
            {
                Matrix = new[,]
                {
                    {0,0,6,0,0,0,6,0,0,0,6,0,0,0,6,0,0,0,6,0,0,0,6,0,0},
                    {0,4,24,55,38,56,24,38,57,38,24,38,165,56,24,38,165,38,24,55,38,56,24,5,0},
                    {40,41,50,51,52,8,8,100,8,8,8,8,113,61,8,8,113,63,8,8,8,100,150,58,59},
                    {0,73,52,52,61,8,8,100,79,81,82,8,113,8,8,8,113,8,8,8,8,8,62,74,0},
                    {0,21,50,8,8,8,8,78,8,50,78,8,99,16,15,16,99,8,8,8,8,8,8,20,0},
                    {0,21,8,51,8,8,8,100,79,81,82,8,113,8,8,8,113,8,8,64,8,8,8,104,0},
                    {40,41,64,52,50,8,8,100,8,8,8,8,113,32,8,32,113,8,8,8,8,8,8,58,59},
                    {0,103,50,52,50,8,8,100,8,8,32,33,99,8,52,63,99,62,8,8,78,8,158,20,0},
                    {0,157,80,97,81,80,80,81,80,80,81,80,98,8,8,8,111,80,81,80,80,81,80,156,0},
                    {0,21,158,99,61,8,8,8,8,8,8,100,99,8,8,8,99,100,100,8,8,8,62,104,0},
                    {40,41,8,109,80,80,82,8,8,8,8,8,113,150,100,8,113,100,8,8,8,8,8,58,59},
                    {0,73,8,8,8,8,8,8,78,8,8,8,113,8,8,61,113,8,8,8,8,8,8,20,0},
                    {0,21,8,79,80,80,82,8,8,8,8,100,111,80,81,80,110,8,8,8,8,8,8,74,0},
                    {0,21,8,8,8,8,8,8,8,8,100,100,99,32,62,8,8,8,8,62,95,82,8,20,0},
                    {40,41,8,8,8,8,61,8,100,100,100,64,113,16,8,8,8,8,8,52,99,150,150,58,59},
                    {0,22,42,75,2,2,42,76,2,2,42,2,164,77,42,75,2,75,42,2,164,2,42,23,0},
                    {0,0,60,0,0,0,60,0,0,0,60,0,0,0,60,0,0,0,60,0,0,0,60,0,0}
                },
                Start = new Point(14, 2),
                Finish = new Point(2, 14),
                PressurePlates = new[]
                {
                    new []{ new Point(9,2), new Point(10,13), new Point(14,22)},
                    new []{ new Point(7,22), new Point(2,22), new Point(14,21)}
                },
                Hatches = new[]
                {
                    new []{ new Point(14,21), new Point(2,22)},
                    new []{ new Point(11,8), new Point(4,7)},
                    new []{ new Point(4,10), new Point(7,20)},
                    new []{ new Point(10,13), new Point(14,22)}
                }
            };

            yield return new Data
            {
                Matrix = new[,]
                {
                    {0,0,6,0,0,0,6,0,0,0,6,0,0,0,6,0,0,0,6,0,0,0,6,0,0},
                    {0,4,24,165,38,55,24,38,38,38,24,38,56,38,24,57,38,38,24,55,38,56,24,5,0},
                    {40,41,150,109,96,50,8,8,158,8,8,8,8,8,8,8,158,8,8,8,8,8,61,58,59},
                    {0,73,8,61,113,61,8,8,83,8,79,82,61,79,82,8,95,80,81,80,82,8,79,156,0},
                    {0,21,8,32,111,80,81,80,98,150,8,8,8,8,8,150,113,62,8,8,8,8,8,20,0},
                    {0,21,8,8,113,62,8,8,109,80,81,96,8,95,81,80,98,8,8,8,8,17,83,74,0},
                    {40,41,8,33,99,8,108,8,8,8,150,113,8,113,62,8,99,8,8,64,108,8,99,58,59},
                    {0,103,8,8,113,8,8,8,8,8,8,113,8,113,8,8,109,80,81,82,8,8,129,104,0},
                    {0,21,8,8,111,82,8,79,82,8,8,99,8,99,8,8,8,8,63,8,8,8,150,20,0},
                    {0,21,64,8,113,8,15,8,8,8,63,113,8,113,150,8,79,80,97,81,80,81,80,156,0},
                    {40,41,108,8,109,82,16,79,97,80,81,110,34,109,81,82,150,63,113,32,32,32,32,58,59},
                    {0,21,61,8,8,8,8,64,113,63,158,8,8,8,158,8,83,8,99,8,32,8,50,20,0},
                    {0,73,8,8,8,8,83,8,113,8,108,8,8,8,108,8,113,8,113,8,33,16,8,74,0},
                    {0,21,8,79,82,8,129,8,113,8,8,8,8,8,8,61,113,8,129,8,15,8,8,20,0},
                    {40,41,150,8,8,8,8,8,109,80,81,80,81,80,81,80,110,64,8,8,34,8,32,58,59},
                    {0,22,42,2,2,75,42,76,2,2,42,77,2,2,42,2,75,2,42,76,2,2,42,23,0},
                    {0,0,60,0,0,0,60,0,0,0,60,0,0,0,60,0,0,0,60,0,0,0,60,0,0}
                },
                Start = new Point(13, 12),
                Finish = new Point(13, 22),
                PressurePlates = new[]
                {
                    new []{ new Point(11,10), new Point(4,9), new Point(6,10)},
                    new []{ new Point(11,14), new Point(14,2), new Point(4,15)},
                    new []{ new Point(2,8), new Point(8,22), new Point(10,16)},
                    new []{ new Point(2,16), new Point(2,2), new Point(9,14)}
                },
                Hatches = new[]
                {
                    new []{ new Point(4,9), new Point(6,10)},
                    new []{ new Point(14,2), new Point(4,15)},
                    new []{ new Point(8,22), new Point(10,16)},
                    new []{ new Point(2,2), new Point(9,14)}
                }
            };
        }
    }
}
