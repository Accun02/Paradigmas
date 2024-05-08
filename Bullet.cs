using System;

namespace MyGame
{
    internal class Bullet : Projectile
    {
        public const int BulletHeight = 20;
        public const int BulletWidth = 20;

        private float bulletVel = 1500;
        private float acceleration = 100;

        private Vector2 direction;

        public Bullet(int x, int y, Vector2 dir, string imagePath)
        {
            transform = new Transform(new Vector2(x, y));
            direction = dir;
            image = Engine.LoadImage(imagePath);
        }

        public Bullet(Vector2 position, Vector2 dir, string imagePath)
        {
            transform = new Transform(position);
            direction = dir;
            image = Engine.LoadImage(imagePath);
        }

        public override void Update()
        {
            bulletVel += acceleration * Time.DeltaTime;
            transform.Translate(new Vector2(direction.x * bulletVel * Time.DeltaTime, direction.y * bulletVel * Time.DeltaTime));
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

            if (enemy.Vulnerable && bulletRight >= enemyLeft && bulletLeft <= enemyRight && bulletBottom >= enemyTop && bulletTop <= enemyBottom)
            {
                enemy.TakeDamage(1);
                Program.BulletList.Remove(this);
            }

            if (transform.Position.y <= 0 - BulletHeight ||
                transform.Position.x >= Program.ScreenWidth ||
                transform.Position.x <= 0 - BulletWidth)
            {
                Program.BulletList.Remove(this);
            }
        }


    }
}
