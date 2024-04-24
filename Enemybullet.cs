using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyGame
{
    internal class EnemyBullet
    {
        Transform transform;
        Vector2 direction;
        IntPtr enemybullet = Engine.LoadImage("assets/evilBullet.png");

        private int GroundHeight = Program.GroundHeight;
        private int ScreenWidth = Program.ScreenWidth;

        public const float BulletHeight = 24;
        public const float BulletWidth = 24;

        private float bulletVel = 10;
        private float acceleration = 2000;

        public EnemyBullet(Vector2 position, Vector2 playerPosition, Vector2 offset)
        {
            Vector2 adjustedPosition = new Vector2(position.x + offset.x, position.y + offset.y);
            transform = new Transform(adjustedPosition);
            direction = new Vector2(playerPosition.x + (Character.PlayerWidth / 2) - adjustedPosition.x, playerPosition.y + (Character.PlayerHeight / 2) - adjustedPosition.y);
            float length = (float)Math.Sqrt(direction.x * direction.x + direction.y * direction.y);
            direction.x /= length;
            direction.y /= length;
        }


        public void BulletRender()
        {
            Engine.Draw(enemybullet, transform.Position.x, transform.Position.y);
        }

        public void Update()
        {
            bulletVel += acceleration * Time.DeltaTime;

            transform.Translate(direction, bulletVel * Time.DeltaTime);
        }

        public void CheckCollisions()
        {
            if (transform.Position.y >= GroundHeight)
            {
                Program.enemyBullets.Remove(this);
            }
        }
    }
}
