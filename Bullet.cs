using System;
using System.Collections.Generic;

namespace MyGame
{
    public class Bullet : Projectile
    {
        private int BulletHeight;
        private int BulletWidth;
        public bool isVertical;

        private string idlePath;

        public Bullet(int x, int y, Vector2 dir, string imagePath, bool isHorizontal) : base(new Vector2(x, y), dir)
        {
            bulletVel = 1500;
            acceleration = 100;
            coolDown = 0.3f;
            idlePath = imagePath;
            isVertical = !isHorizontal;
            CreateAnimations();
        }

        public override void Update()
        {
            BulletHeight = isVertical ? 10 : 40;
            BulletWidth = isVertical ? 40 : 10;

            bulletVel += acceleration * Time.DeltaTime;
            transform.Translate(new Vector2(direction.x * bulletVel * Time.DeltaTime, direction.y * bulletVel * Time.DeltaTime));
            base.Update();
        }

        public void CheckCollisions(Enemy enemy)
        {
            float bulletLeft = transform.Position.x;
            float bulletRight = transform.Position.x + BulletWidth;
            float bulletTop = transform.Position.y;
            float bulletBottom = transform.Position.y + BulletHeight;

            float enemyLeft = enemy.Transform.Position.x;
            float enemyRight = enemy.Transform.Position.x + Enemy.EnemyWidth;
            float enemyTop = enemy.Transform.Position.y;
            float enemyBottom = enemy.Transform.Position.y + Enemy.EnemyHeight;

            if (enemy.Vulnerable && bulletRight >= enemyLeft && bulletLeft <= enemyRight && bulletBottom >= enemyTop && bulletTop <= enemyBottom || destroyed)
            {
                if (!destroyed)
                    enemy.TakeDamage(1);

                bulletVel = 0;
                acceleration = 0;
                destroyed = true;
                currentAnimation = destroy;
                currentAnimation.Update();

                coolDown -= Time.DeltaTime;
                if (coolDown <= 0)
                    GameManager.Instance.LevelController.BulletList.Remove(this);
            }

            if (transform.Position.y <= 0 - BulletHeight ||
                transform.Position.x >= GameManager.Instance.LevelController.ScreenWidth ||
                transform.Position.x <= 0 - BulletWidth)
            {
                GameManager.Instance.LevelController.BulletList.Remove(this);
            }
        }

        private void CreateAnimations()
        {
            List<string> destroyPaths = new List<string>();
            for (int i = 0; i < 3; i++)
            {
                destroyPaths.Add($"assets/bullet/destroy/{i}.png");
            }
            CreateAnimations(idlePath, destroyPaths);
        }
    }
}
