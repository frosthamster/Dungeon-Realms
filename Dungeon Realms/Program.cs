using System;
using System.Drawing;
using System.Windows.Forms;

namespace Dungeon_Realms
{
    static class Program
    {
        [STAThread]
        public static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new GameForm{ ClientSize = new Size(GameForm.BoxSize * LevelGenerator.Width,
                GameForm.BoxSize * LevelGenerator.Height) });
        }
    }
}
