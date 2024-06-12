using System;

namespace MyGame
{
    internal class FontManager
    {
        static IntPtr gameFont = Engine.LoadFont("assets/Cave-Story.ttf", 30);

        public static void Render(Enemy enemy, Vector2 position)
        {
            if (enemy.Health > 50)
            {
                Engine.DrawText("Shiori Hisoka,  Eminencia del Torneo", GameManager.Instance.LevelController.ScreenWidth / 2 - (int)position.x + (int)enemy.ShakeOffsetX, (int)position.y, 255, 255, 255, gameFont);
            }
            else
            {
                Engine.DrawText("Shiori Hisoka,  Eminencia del Torneo", GameManager.Instance.LevelController.ScreenWidth / 2 - (int)position.x + (int)enemy.ShakeOffsetX, (int)position.y, 255, 0, 0, gameFont);
            }
        }
    }
}
