using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Tao.Sdl;
using static System.Net.Mime.MediaTypeNames;

namespace MyGame
{
    class Program
    {
        static public int GroundHeight = 584;        // De arriba a abajo
        static public int ScreenWidth = 1280;       // De izquierda a derecha
        static float delayFrame = 60f;             // FPS
        static public bool targetFrame = false;

        static public Character player = new Character(new Vector2(ScreenWidth / 2 - Character.PlayerWidth / 2, 584 - Character.PlayerHeight));
        static public Enemy enemy = new Enemy(new Vector2(ScreenWidth / 2 - Enemy.EnemyWidth / 2, 100));

        static public List<Bullet> BulletList = new List<Bullet>();
        static public List<Teleport> TeleportList = new List<Teleport>();
        static public List <EnemyBullet> enemyBullets = new List<EnemyBullet>();

        static Stopwatch stopwatch = new Stopwatch();

        static void Main(string[] args)
        {
            Engine.Initialize(1280, 720);
            Time.Initialize();
            stopwatch.Start();

            int frameCount = 0;
            float elapsedTime = 0f;

            while (true)
            {
                float startTime = (float)stopwatch.Elapsed.TotalSeconds;
                float targetFrameTime = 1f / delayFrame;

                GameManager.Instance.Update(enemy, player);
                GameManager.Instance.Render();

                frameCount++;
                elapsedTime += Time.DeltaTime;

                if (elapsedTime >= 1.0f)
                {
                    AdjustDelayFrame(frameCount);
                    frameCount = 0;
                    elapsedTime = 0f;
                }

                float endTime = (float)stopwatch.Elapsed.TotalSeconds;
                float frameTime = endTime - startTime;
                float delayTime = targetFrameTime - frameTime;

                if (delayTime > 0)
                {
                    int delayMilliseconds = (int)(delayTime * 1000);
                    Sdl.SDL_Delay(delayMilliseconds);
                }
            }
        }

        private static void AdjustDelayFrame(int frameCount)
        {
            if (!targetFrame)
            {
                float diff = 60 - frameCount;
                float factor = Math.Sign(diff) * Math.Min(10, Math.Abs(diff) / 10f);
                delayFrame += factor;
            }
        }

        public static void Render()
        {
            Engine.Clear();

            if (player.Health > 0)
                BackgroundManager.Render();

            player.Render(player);

            if (player.Health > 0)
            {
                enemy.Render();

                foreach (Bullet bullet in BulletList)
                {
                bullet.Render();
                }
                foreach (EnemyBullet enemyBullet in enemyBullets)
                {
                enemyBullet.Render();
                }
                foreach (Teleport teleport in TeleportList)
                {
                    teleport.Render();
                }

                FontManager.Render(enemy);

                //BackgroundManager.Fade(); //Reduce demasiado el performance.
            }
            Engine.Show();
        }

        public static void Update()
        {
            player.Update(player);

            if (player.Health > 0)
            {
                enemy.Update(Time.DeltaTime);
                enemy.CheckCollision(player);

                foreach (Bullet bullet in BulletList.ToList())
                {
                    bullet.Update();
                    bullet.CheckCollisions(enemy);
                }

                foreach (EnemyBullet enemyBullet in enemyBullets.ToList())
                {
                    enemyBullet.Update();
                    enemyBullet.CheckCollisions(player);
                }

                foreach (Teleport teleport in TeleportList.ToList())
                {
                    teleport.Update();
                }
            }
        }

        public static void Restart()
        {
            //Clear Scene
            TeleportList.Clear();
            BulletList.Clear();
            enemyBullets.Clear();

            //Reset Player
            player.transform.Position = new Vector2(ScreenWidth / 2 - Character.PlayerWidth / 2, 584 - Character.PlayerHeight);
            player.ResetMomentum();

            player.Health = player.MaxHealth;
            player.IsDead = false;

            //Reset Enemy
            enemy.ResetTransform(new Vector2(ScreenWidth / 2 - Enemy.EnemyWidth / 2, 100));
            enemy.ResetAttacks();
            enemy.Health = enemy.MaxHealth;

            //Reset Input
            GameManager.Instance.ZKeyReleased = false;
        }
    }
}
