﻿using System;
using System.Collections.Generic;

namespace MyGame
{
    public class Bullet : Projectile
    {
        public event Action<Bullet> OnDestroy;

        private int BulletHeight;
        private int BulletWidth;

        int offsetX;
        int offsetY;

        private bool isVertical;
        private string idlePath;

        private static Random random = new Random();

        public Bullet(int x, int y, Vector2 dir, string imagePath, bool isHorizontal) : base(new Vector2(x, y), dir)
        {
            Initialize(x, y, dir, imagePath, isHorizontal);
        }

        public void Initialize(int x, int y, Vector2 dir, string imagePath, bool isHorizontal)
        {
            currentAnimation = idle;
            destroyed = false;
            transform = new Transform(new Vector2(x, y));
            direction = dir;
            bulletVel = 1500;
            acceleration = 100;
            coolDown = 0.3f;
            idlePath = imagePath;
            this.isVertical = !isHorizontal;
            CreateAnimations();
        }

        public override void Update()
        {
            if (isVertical)
            {
                BulletHeight = 60;
                BulletWidth = 10;
            }
            else
            {
                BulletHeight = 10;
                BulletWidth = 60;
            }

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
                {
                    enemy.TakeDamage(1);
                    ParticleOffset();
                }

                bulletVel = 0;
                acceleration = 0;
                destroyed = true;
                currentAnimation = destroy;
                currentAnimation.Update();

                coolDown -= Time.DeltaTime;
                if (coolDown <= 0)
                    DestroyBullet();
            }

            if (transform.Position.y <= 0 - BulletHeight ||
                transform.Position.x >= GameManager.Instance.LevelController.ScreenWidth ||
                transform.Position.x <= 0 - BulletWidth)
            {
                DestroyBullet();
            }
        }

        private void ParticleOffset()
        {
            if (isVertical)
            {
                offsetX = random.Next(-15, 10);
                offsetY = random.Next(-5, 10);
            }
            else
            {
                offsetX = 25;
                offsetY = random.Next(-20, 20);
            }

            transform.Position = new Vector2(transform.Position.x + offsetX, transform.Position.y + offsetY);
        }

        private void DestroyBullet()
        {
            GameManager.Instance.LevelController.BulletList.Remove(this);
            OnDestroy?.Invoke(this);
        }

        private void CreateAnimations()
        {
            List<string> destroyPaths = new List<string>();
            for (int i = 0; i < 5; i++)
            {
                destroyPaths.Add($"assets/bullet/destroy/{i}.png");
            }
            CreateAnimations(idlePath, destroyPaths, 0.1f);
        }
    }
}
