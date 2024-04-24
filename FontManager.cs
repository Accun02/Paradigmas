using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyGame
{
    internal class FontManager
    {
        private Enemy enemy;

        static IntPtr font1 = Engine.LoadFont("assets/Cave-Story.ttf", 35);

        public static void Render(Enemy enemy)
        {
            Engine.DrawText("Enemy: " + enemy.Health + "/100", Program.ScreenWidth / 2 - 85, 20, 255, 255, 255, font1);
        }

    }
}
