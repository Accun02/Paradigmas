using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Tao.Sdl;

namespace MyGame
{
    class Program
    {
        static float delayFrame = 60f;             // FPS
        static public bool targetFrame = false;

        static Stopwatch stopwatch = new Stopwatch();

        static void Main(string[] args)
        {
            Engine.Initialize(1280, 720);
            GameManager.Instance.Initialize();
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
    }
}
