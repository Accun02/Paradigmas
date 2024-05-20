using System;
using System.Collections.Generic;
using Tao.Sdl;

namespace MyGame
{
    internal class Bullet : Projectile
    {
        public const int BulletHeight = 20;
        public const int BulletWidth = 20;

        private float bulletVel = 1500;
        private float acceleration = 100;

        private string idlePath;

        private float coolDown = 0.3f;
        private bool destroyed;

        private Vector2 direction;

        private Animation destroy;
        private Animation idle;

        public Bullet(int x, int y, Vector2 dir, string imagePath)
        {
            transform = new Transform(new Vector2(x, y));
            direction = dir;
            image = Engine.LoadImage(imagePath);
            idlePath = imagePath;

            CreateAnimations();
        }

        public Bullet(Vector2 position, Vector2 dir, string imagePath)
        {
            transform = new Transform(position);
            direction = dir;
            image = Engine.LoadImage(imagePath);
            idlePath = imagePath;

            CreateAnimations();
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
                    Program.BulletList.Remove(this);
            }

            if (transform.Position.y <= 0 - BulletHeight ||
                transform.Position.x >= .ScreenWidth ||
                transform.Position.x <= 0 - BulletWidth)
            {
                Program.BulletList.Remove(this);
            }
        }

        private void CreateAnimations()
        {
            IntPtr idleTexture = Engine.LoadImage(idlePath);
            idle = new Animation("Idle", new List<IntPtr> { idleTexture }, 1.0f, false);

            List<IntPtr> destroyTextures = new List<IntPtr>();
            for (int i = 0; i < 3; i++)
            {
                IntPtr frame = Engine.LoadImage($"assets/bullet/destroy/{i}.png");
                destroyTextures.Add(frame);
            }
            destroy = new Animation("Destroy", destroyTextures, 0.035f, false);
            currentAnimation = idle;
        }
    }
}
