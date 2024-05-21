using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyGame
{
    public class EnemyBullet : Projectile
    {
        Vector2 direction;

        private int GroundHeight = GameManager.Instance.LevelController.GroundHeight;
        private int ScreenWidth = GameManager.Instance.LevelController.ScreenWidth;

        private string idlePath = "assets/enemyBullet/bullet.png";
        private bool destroyed = false;
        private float coolDown = 0.3f;

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

            if (transform.Position.y >= GroundHeight - BulletHeight || transform.Position.x >= GameManager.Instance.LevelController.ScreenWidth || transform.Position.x <= 0 - BulletWidth)
            {
                currentAnimation = destroy;
                currentAnimation.Update();
                destroyed = true;
                bulletVel = 0;
                acceleration = 0;

                coolDown -= Time.DeltaTime;
                if (coolDown <= 0)
                    GameManager.Instance.LevelController.enemyBullets.Remove(this);
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
            for (int i = 0; i < 7; i++)
            {
                IntPtr frame = Engine.LoadImage($"assets/enemyBullet/destroy/{i}.png");
                destroyTextures.Add(frame);
            }
            destroy = new Animation("destroy", destroyTextures, 0.030f, false);
        }
    }
}
