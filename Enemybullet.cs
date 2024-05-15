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

        private string idlePath = "assets/enemyBullet/bullet.png";
        private string destroyPath;
        private bool destroyed = false;

        public const float BulletHeight = 24;
        public const float BulletWidth = 24;

        private float bulletVel = 10;
        private float acceleration = 2000;

        private Animation destroy;
        private Animation idle;

        public EnemyBullet(Vector2 position, Vector2 playerPosition, Vector2 offset)
        {
            Vector2 adjustedPosition = new Vector2(position.x + offset.x, position.y + offset.y);
            transform = new Transform(adjustedPosition);
            direction = new Vector2(playerPosition.x + (Character.PlayerWidth / 2) - adjustedPosition.x, playerPosition.y + (Character.PlayerHeight / 2) - adjustedPosition.y);
            float length = (float)Math.Sqrt(direction.x * direction.x + direction.y * direction.y);
            direction.x /= length;
            direction.y /= length;

            CreateAnimations();
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

            if (bulletRight >= playerLeft && bulletLeft <= playerRight && bulletBottom >= playerTop && bulletTop <= playerBottom && !destroyed)
            {
                Console.WriteLine(player.Health);
                player.Health -= 1;

                currentAnimation = destroy;
                currentAnimation.Update();
                destroyed = true;
                bulletVel = 0;
                acceleration = 0;
            }

            if (transform.Position.y >= GroundHeight - BulletHeight / 2 || transform.Position.x >= Program.ScreenWidth || transform.Position.x <= 0 - BulletWidth)
            {
                currentAnimation = destroy;
                currentAnimation.Update();
                destroyed = true;
                bulletVel = 0;
                acceleration = 0;
    }
            else
            {
                currentAnimation = idle;
            }
        }

        private void CreateAnimations()
        {
            IntPtr idleTexture = Engine.LoadImage(idlePath);
            idle = new Animation("Idle", new List<IntPtr> { idleTexture }, 1.0f, false);

            List<IntPtr> destroyTextures = new List<IntPtr>();
            for (int i = 0; i < 3; i++)
            {
                IntPtr frame = Engine.LoadImage($"assets/enemyBullet/destroy/{i}.png");
                destroyTextures.Add(frame);
            }
            destroy = new Animation("destroy", destroyTextures, 0.035f, false);
        }
    }
}
