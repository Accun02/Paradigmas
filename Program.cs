using System;
using System.Collections.Generic;
using System.Diagnostics;
using Tao.Sdl;

namespace MyGame
{
    class Program
    {
        static float delayFrame = 60f;
        static public bool targetFrame = false;

        static IntPtr image = Engine.LoadImage("assets/fondo.png");

        static public Character player = new Character(new Vector2(608, 688));
        static public List<Enemy> EnemyList = new List<Enemy>();
        static public List<Bullet> BulletList = new List<Bullet>();

        static Stopwatch stopwatch = new Stopwatch();

        static void Main(string[] args)
        {
            Engine.Initialize(1280, 720);
            CreateEnemies();
            Time.Initialize();
            stopwatch.Start();

            int frameCount = 0;
            float elapsedTime = 0f;

            while (true)
            {
                float startTime = (float)stopwatch.Elapsed.TotalSeconds;
                float targetFrameTime = 1f / delayFrame;

                GameManager.Instance.Update();
                GameManager.Instance.Render();

                frameCount++;
                elapsedTime += Time.DeltaTime;

                if (elapsedTime >= 1.0f)
                {
                    //Console.WriteLine("FPS: " + frameCount + " / SPEED: " + delayFrame);
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

        public static void Render()
        {
            Engine.Clear();
            Engine.Draw(image, 0, 0);
            player.Render();
            foreach (Enemy enemy in EnemyList)
            {
                enemy.Render();
            }

            foreach (Bullet bullet in BulletList)
            {
                bullet.Render();
            }

            Engine.Show();
        }

        public static void Update()
        {
            player.Update();

            foreach (Enemy enemy in EnemyList)
            {
                enemy.Update();
            }

            foreach (Bullet bullet in BulletList)
            {
                bullet.Update();
            }
        }

        private static void CreateEnemies()
        {
            EnemyList.Add(new Enemy(new Vector2(0, 0), new Vector2()));
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
    }
}
