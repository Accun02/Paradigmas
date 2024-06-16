﻿using System;
using System.Collections.Generic;

namespace MyGame
{
    public class EnemyAnvil : Projectile, ICheckForCollision
    {
        private readonly int GroundHeight = GameManager.Instance.LevelController.GroundHeight;
        private string idlePath = "assets/enemyBullet/anvil.png";

        private bool bounced;

        public const float anvilHeight = 67;
        public const float anvilWidth = 184;

        private float shakeSize = 3f;
        private float anvilShakeTime = 0f;
        private float anvilShakeSpeed = 55f;

        public EnemyAnvil(Vector2 position, Vector2 offset) : base(new Vector2(position.x + offset.x, position.y + offset.y), new Vector2(0, 1))
        {
            bulletVel = 0;

            if (GameManager.Instance.HardMode)
            {
                acceleration = 1500;
            }
            else
            {
                acceleration = 1250;
            }

            coolDown = 0.4f;
            CreateAnimations();
        }

        public override void Update()
        {
            bulletVel += acceleration * Time.DeltaTime;
            transform.Translate(direction, bulletVel * Time.DeltaTime);

            if (transform.Position.y >= GroundHeight - 92 && !bounced)
            {
                CameraShake.Instance.EnemyHugeShake();
                bounced = true;
                shakeSize = 6.5f;
                anvilShakeSpeed = 60f;
                bulletVel = -bulletVel * 0.5f;
            }
        }


        public override void Render()
        {
            anvilShakeTime += Time.DeltaTime;
            float offsetX = shakeSize * (float)Math.Sin(anvilShakeSpeed * anvilShakeTime);

            Engine.Draw(currentAnimation.CurrentFrame, (transform.Position.x + offsetX) + CameraShake.Instance.value, transform.Position.y);
        }


        public void CheckPositions(Character player)
        {
            Vector2 playerPosition = player.Transform.Position;
            Vector2 bulletPosition = transform.Position;
            if (bulletPosition.x + anvilWidth > playerPosition.x && bulletPosition.x < playerPosition.x + Character.PlayerWidth &&
                bulletPosition.y + anvilHeight > playerPosition.y && bulletPosition.y < playerPosition.y + Character.PlayerHeight && player.Vulnerable)
            {
                player.TakeDamage(1);
                return;
            }

            if (transform.Position.y >= 720)
            {
                destroyed = true;
                GameManager.Instance.LevelController.AnvilList.Remove(this);
            }
        }

        private void CreateAnimations()
        {
            List<string> destroyPaths = new List<string>();
            for (int i = 0; i < 2; i++)
            {
                destroyPaths.Add($"assets/enemyBullet/thunderBolt/destroy/{i}.png");
            }
            CreateAnimations(idlePath, destroyPaths, 0.05f);
        }
    }
}
