using System;

namespace MyGame
{
    internal class FontManager
    {
        static IntPtr font1 = Engine.LoadFont("assets/Cave-Story.ttf", 35);

        public static void Render(Enemy enemy)
        {
            if (enemy.Health > 50)
            {
                Engine.DrawText("Enemigo: " + enemy.Health + "/100", GameManager.Instance.LevelController.ScreenWidth / 2 - 100, 20, 255, 255, 255, font1);
            }
            else
            {
                Engine.DrawText("Enemigo: " + enemy.Health + "/100", GameManager.Instance.LevelController.ScreenWidth / 2 - 100, 20, 255, 0, 0, font1);
            }
        }

    }
}
