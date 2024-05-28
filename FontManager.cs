using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyGame
{
    internal class FontManager
    {
        static IntPtr font1 = Engine.LoadFont("assets/Cave-Story.ttf", 35);

        public static void Render(Enemy enemy)
        {
            if (enemy.Health > 50)
            {
                Engine.DrawText("Enemy: " + enemy.Health + "/100", GameManager.Instance.LevelController.ScreenWidth / 2 - 85, 20, 255, 255, 255, font1);
            }
            else if (enemy.Health <= 25)
            {
                Engine.DrawText("Enemy: " + enemy.Health + "/100", GameManager.Instance.LevelController.ScreenWidth / 2 - 85, 20, 255, 0, 0, font1);
            }
            else
            {
                Engine.DrawText("Enemy: " + enemy.Health + "/100", GameManager.Instance.LevelController.ScreenWidth / 2 - 85, 20, 255, 100, 100, font1);
            }
        }

    }
}
