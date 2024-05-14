using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyGame
{
    internal class EnemyBullet : Projectile
    {
        Vector2 direction;

        private int GroundHeight = Program.GroundHeight;
        private int ScreenWidth = Program.ScreenWidth;

        public const float BulletHeight = 24;
        public const float BulletWidth = 24;

        private float bulletVel = 10;
        private float acceleration = 2000;

        public EnemyBullet(Vector2 position, Vector2 playerPosition, Vector2 offset, string imagePath)
        {
            Vector2 adjustedPosition = new Vector2(position.x + offset.x, position.y + offset.y);
            transform = new Transform(adjustedPosition);
            direction = new Vector2(playerPosition.x + (Character.PlayerWidth / 2) - adjustedPosition.x, playerPosition.y + (Character.PlayerHeight / 2) - adjustedPosition.y);
            float length = (float)Math.Sqrt(direction.x * direction.x + direction.y * direction.y);
            direction.x /= length;
            direction.y /= length;
            image = Engine.LoadImage(imagePath);
        }

        public override void Update()
        {
            bulletVel += acceleration * Time.DeltaTime;

            transform.Translate(direction, bulletVel * Time.DeltaTime);
        }

        public void CheckCollisions(Character player)
        {
            float bulletLeft = transform.Position.x;
            float bulletRight = transform.Position.x + BulletWidth;
            float bulletTop = transform.Position.y;
            float bulletBottom = transform.Position.y + BulletHeight;

            float playerLeft = player.transform.Position.x;
            float playerRight = player.transform.Position.x + Character.PlayerWidth;
            float playerTop = player.transform.Position.y;
            float playerBottom = player.transform.Position.y + Character.PlayerHeight;

            if (bulletRight >= playerLeft && bulletLeft <= playerRight && bulletBottom >= playerTop && bulletTop <= playerBottom)
            {
                Program.enemyBullets.Remove(this);
                Console.WriteLine(player.Health);
                player.Health -= 1;
            }

            if (transform.Position.y >= GroundHeight - BulletHeight / 2 || transform.Position.x >= Program.ScreenWidth || transform.Position.x <= 0 - BulletWidth)
            {
                Program.enemyBullets.Remove(this);
            }
        }
    }
}
