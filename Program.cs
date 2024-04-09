

using System;
using System.Collections.Generic;
using System.Data;
using System.Threading;
using Tao.Sdl;

namespace MyGame
{

    class Program
    {

        private static Escenas escenas = new Escenas();

        static void Main(string[] args)
        {
            Engine.Initialize();
     
            while (true)
            {

                if (escenas.numPantalla == 0)
                {
                    Update();

                    Engine.Clear();

                    escenas.Render();


                Engine.Show();

                    Sdl.SDL_Delay(20);
                }

                if (escenas.numPantalla == 1)
                {
                    Update();

                    Engine.Clear();

                    escenas.Render();


                    Engine.Show();

                    Sdl.SDL_Delay(20);
                }
            }
        }

        private static void Update()
        {
            if (Engine.KeyPress(Engine.KEY_LEFT)) {  }

            if (Engine.KeyPress(Engine.KEY_RIGHT)) {  }

            if (Engine.KeyPress(Engine.KEY_UP)) {
                escenas.numPantalla = 1;
            }

            if (Engine.KeyPress(Engine.KEY_DOWN)) {
                escenas.numPantalla = 0;
            }

            if (Engine.KeyPress(Engine.KEY_ESC)) { }




        }

    }

}
