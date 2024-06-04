using System;

namespace MyGame
{
    internal class BackgroundManager
    {
        static IntPtr image = Engine.LoadImage("assets/background.png");
        static IntPtr fade = Engine.LoadImage("assets/fade.png");

        public static void Render()
        {
            Engine.Draw(image, 0, 0);
        }
        public static void Fade()
        {
            Engine.Draw(fade, 0, 0);
        }

    }
}
