using System.Drawing;
using Dungeon_Realms.Properties;

namespace Dungeon_Realms
{
    static class Textures
    {
        public static Bitmap HeroRight { get; } = new Bitmap(Resources.hero_1);
        public static Bitmap HeroLeft { get; } = new Bitmap(Resources.hero_2);
        public static Bitmap Princess { get; } = new Bitmap(Resources.cup);
        public static Bitmap Floor { get; } = new Bitmap(Resources.t_1_7);
        public static Bitmap ClosedHatch { get; } = new Bitmap(Resources.t_9_16);
        public static Bitmap OpenedHatch { get; } = new Bitmap(Resources.t_6_6);
        public static Bitmap WrongHatch { get; } = new Bitmap(Resources.wrong_tp);
        public static Bitmap PressurePlate { get; } = new Bitmap(Resources.t_10_8);
        public static Bitmap Box { get; } = new Bitmap(Resources.t_6_12);
    }
}
