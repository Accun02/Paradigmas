using System;
namespace MyGame
{
    internal static class StaticImage
    {
        static IntPtr image = Engine.LoadImage("assets/background.png");
        static IntPtr fade = Engine.LoadImage("assets/fade.png");

        public static void RenderBG()
        {
            Engine.Draw(image, 0, 0);
        }
        public static void RenderFade()
        {
            Engine.Draw(fade, 0, 0);
        }
    }
}