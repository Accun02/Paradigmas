using System;

namespace MyGame
{
    internal class Bullet
    {
        public const int BulletHeight = 16;
        public const int BulletWidth = 16;

        private float bulletVel = 750;
        private float acceleration = 200;

        private Transform transform;
        private IntPtr image = Engine.LoadImage("assets/bala.png");

        // Nueva variable para almacenar la dirección de la bala
        private Vector2 direction;

        public Bullet(int x, int y, Vector2 dir)
        {
            transform = new Transform(new Vector2(x, y));
            direction = dir;
        }

        public Bullet(Vector2 position, Vector2 dir)
        {
            transform = new Transform(position);
            direction = dir;
        }

        public void Render()
        {
            Engine.Draw(image, (int)transform.Position.x, (int)transform.Position.y);
        }

        public void Update()
        {
            bulletVel += acceleration * Time.DeltaTime;

            // Actualizar la posición basada en la dirección
            transform.Translate(new Vector2(direction.x * bulletVel * Time.DeltaTime, direction.y * bulletVel * Time.DeltaTime));

            // Eliminar la bala si sale de la pantalla
            if (transform.Position.y <= 0 - BulletHeight ||
                transform.Position.x >= Program.ScreenWidth ||
                transform.Position.x <= 0 - BulletWidth)
            {
                Program.BulletList.Remove(this);
            }
        }
    }
}
