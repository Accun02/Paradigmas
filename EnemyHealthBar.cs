using System;

namespace MyGame
{
    internal class EnemyHealthBar
    {
        static IntPtr bossBar = Engine.LoadImage("assets/bossBar.png");
        static IntPtr skull = Engine.LoadImage("assets/skull.png");
        static IntPtr skullDying = Engine.LoadImage("assets/skullDying.png");
        static IntPtr hp = Engine.LoadImage("assets/hp.png");

        static private int positionX = 422;
        static private int positionY = 650;

        public static void RenderEnemyHP(Enemy enemy)
        {
            float hpBarCount = enemy.Health * 4.56f - 1;

            for (int i = 0; i < hpBarCount; i++)
            {
                Engine.Draw(hp, positionX + 3 + (i) + ((int)enemy.ShakeOffsetX * 2), positionY + 1);
            }

            Engine.Draw(bossBar, positionX + ((int)enemy.ShakeOffsetX * 2), positionY);

            if (enemy.Health > 50)
            {
                Engine.Draw(skull, positionX - 23 + enemy.ShakeOffsetX, positionY - 6);
            }
            else
            {
                Engine.Draw(skullDying, positionX - 23 + enemy.ShakeOffsetX, positionY - 6);
            }

            FontManager.Render(enemy, new Vector2(((positionX - 75) / 2), positionY - 24));
        }
    }
}
