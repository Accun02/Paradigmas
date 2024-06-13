using System;
namespace MyGame
{
    internal static class StaticImage
    {
        static IntPtr image = Engine.LoadImage("assets/background.png");

        public static void RenderBG()
        {
            Engine.Draw(image, 0, 0);
        }
    }
}