using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyGame
{
    internal class Bullet
    {
        private int posX = 0;
        private int posY = 0;
        IntPtr image = Engine.LoadImage("assets/bullet.png");

        public Bullet(int x, int y)
        {
            posX = x;
            posY = y;
        }

        public Bullet()
        {

        }

        public void Render()
        {
            Engine.Draw(image, posX, posY);
        }

        public void Update()
        {
            posY -= 10;
        }
    }
}
