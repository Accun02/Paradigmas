using System;
using System.Collections.Generic;

namespace MyGame
{
    public class EnemyBullet : Projectile
    {
        private int GroundHeight = GameManager.Instance.LevelController.GroundHeight;
        private int ScreenWidth = GameManager.Instance.LevelController.ScreenWidth;

        public const float BulletHeight = 24;
        public const float BulletWidth = 24;

        private string idlePath = "assets/enemyBullet/bullet.png";

        public EnemyBullet(Vector2 position, Vector2 playerPosition, Vector2 offset)
            : base(new Vector2(position.x + offset.x, position.y + offset.y), CalculateDirection(position, playerPosition, offset))
        {
            bulletVel = 10;
            acceleration = 2000;
            coolDown = 0.3f;

            CreateAnimations();
        }

        private static Vector2 CalculateDirection(Vector2 position, Vector2 playerPosition, Vector2 offset)
        {
            Vector2 adjustedPosition = new Vector2(position.x + offset.x, position.y + offset.y);
            Vector2 direction = new Vector2(playerPosition.x + (Character.PlayerWidth / 2) - adjustedPosition.x, playerPosition.y + (Character.PlayerHeight / 2) - adjustedPosition.y);
            float length = (float)Math.Sqrt(direction.x * direction.x + direction.y * direction.y);
            direction.x /= length;
            direction.y /= length;
            return direction;
        }

        public override void Update()
        {
            bulletVel += acceleration * Time.DeltaTime;
            transform.Translate(direction, bulletVel * Time.DeltaTime);
            base.Update();
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
                player.Health -= 1;

                currentAnimation = destroy;
                currentAnimation.Update();
                destroyed = true;
                bulletVel = 0;
                acceleration = 0;
            }

            if (transform.Position.y >= GroundHeight - BulletHeight || transform.Position.x >= ScreenWidth || transform.Position.x <= 0 - BulletWidth)
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
            List<string> destroyPaths = new List<string>();
            for (int i = 0; i < 7; i++)
            {
                destroyPaths.Add($"assets/enemyBullet/destroy/{i}.png");
            }
            CreateAnimations(idlePath, destroyPaths, 0.030f);
        }
    }
}
