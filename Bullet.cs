using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyGame
{
    internal class Bullet
    {
        private Transform transform;
        private IntPtr image = Engine.LoadImage("assets/bullet.png");

        public Bullet(int x, int y)
        {
            transform = new Transform(new Vector2(x, y));
        }

        public Bullet(Vector2 position)
        {
            transform = new Transform(position);
        }

        public void Render()
        {
            Engine.Draw(image, (int)transform.Position.x, (int)transform.Position.y);
        }

        public void Update()
        {
            transform.Translate(new Vector2(0, -10));
        }
    }
}

//Hola mundo.
