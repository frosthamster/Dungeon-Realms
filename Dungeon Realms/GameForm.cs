using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace Dungeon_Realms
{
    sealed class GameForm : Form
    {
        private readonly List<Level> levels = new List<Level>();
        private int currentLevelIndex;
        private Level CurrentLevel => levels[currentLevelIndex];
        public const int BoxSize = 48;
        private static readonly Color FloorColor = Color.FromArgb(179, 136, 162);

        public GameForm()
        {
            FormBorderStyle = FormBorderStyle.FixedToolWindow;
            DoubleBuffered = true;

            foreach (var level in LevelGenerator.GetLevels())
            {
                levels.Add(level);
                SubscribeEvents(level);
            }

            Paint += (s, args) =>
            {
                var g = args.Graphics;
                g.InterpolationMode = InterpolationMode.NearestNeighbor;
                g.PageUnit = GraphicsUnit.Pixel;
                g.FillRectangle(new SolidBrush(FloorColor), ClientRectangle);

                for (var i = 0; i < LevelGenerator.Height; i++)
                    for (var j = 0; j < LevelGenerator.Width; j++)
                    {
                        var block = CurrentLevel.Map[i, j]?.Texture;
                        var floor = CurrentLevel.Map[i, j]?.Floor?.Texture;
                        var destination = new Rectangle(j * BoxSize, i * BoxSize, BoxSize + 1, BoxSize + 1);
                        if (floor != null) g.DrawImage(floor, destination);
                        if (block != null) g.DrawImage(block, destination);
                    }
            };

            KeyDown += (sender, args) =>
            {
                var code = args.KeyCode;
                if (code == Keys.A || code == Keys.Left)
                    CurrentLevel.Hero.TryMove(Direction.Left);
                if (code == Keys.D || code == Keys.Right)
                    CurrentLevel.Hero.TryMove(Direction.Right);
                if (code == Keys.W || code == Keys.Up)
                    CurrentLevel.Hero.TryMove(Direction.Up);
                if (code == Keys.S || code == Keys.Down)
                    CurrentLevel.Hero.TryMove(Direction.Down);
                if (code == Keys.F12)
                    SwitchToNextLevel();
                if (code == Keys.F5)
                    RestoreCurrentLevel();
                Invalidate();
            };
        }

        private void RestoreCurrentLevel()
        {
            var newLevel = LevelGenerator.GetLevel(currentLevelIndex);
            levels[currentLevelIndex] = newLevel;
            SubscribeEvents(newLevel);
        }

        private void SwitchToNextLevel()
        {
            if (levels.Count - 1 > currentLevelIndex)
                currentLevelIndex++;
        }

        private void SubscribeEvents(Level level)
        {
            foreach (var gameObject in level.Map)
            {
                if (gameObject is MovableGameObject movableObject)
                    movableObject.CouldNotPassThroughHatch += hatch =>
                    {
                        hatch.Texture = Textures.WrongHatch;
                        hatch.Out.Texture = Textures.WrongHatch;
                        var timer = new CountDownTimer(500,
                            () =>
                            {
                                hatch.Texture = Textures.OpenedHatch;
                                hatch.Out.Texture = Textures.OpenedHatch;
                                Invalidate();
                            });
                        timer.Start();
                    };
            }

            level.LevelFinished += gemsEarded => SwitchToNextLevel();

            level.Hero.Moved += (obj, direction) =>
            {
                if (direction != level.Hero.Orientation &&
                (direction == Direction.Right || direction == Direction.Left))
                    level.Hero.Rotate();
            };
        }
    }
}