using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyGame
{
    internal class BackgroundManager
    {
        static IntPtr image = Engine.LoadImage("assets/fondo.png");
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
