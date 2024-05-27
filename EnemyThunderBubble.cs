using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace MyGame
{
    public class EnemyThunderBubble : Projectile
    {
        private readonly int GroundHeight = GameManager.Instance.LevelController.GroundHeight;
        private readonly int ScreenWidth = GameManager.Instance.LevelController.ScreenWidth;

        private string idlePath = "assets/enemyBullet/thunder.png";

        private bool playerDetected;
        private bool shootThunder;

        private float detectionCoolDown = 0.15f;
        private float timeSinceDetection;

        public const float BulletHeight = 48;
        public const float BulletWidth = 48;

        private float waveFrequency = 6f;
        private float waveAmplitude = 3f;
        private float waveTime = 0;

        private Animation spawn;
        private Animation wait;

        public EnemyThunderBubble(Vector2 position, Vector2 playerPosition, Vector2 offset)
        {
            Vector2 adjustedPosition = new Vector2(position.x + offset.x, position.y + offset.y);
            transform = new Transform(adjustedPosition);
            direction = new Vector2(playerPosition.x + (Character.PlayerWidth / 2) - adjustedPosition.x, 0);
            float length = (float)Math.Sqrt(direction.x * direction.x + direction.y * direction.y);
            direction.x /= length;

            bulletVel = 150;
            acceleration = 100;
            timeSinceDetection = 0;

            CreateAnimations();
        }

        public override void Update()
        {
            if (playerDetected)
            {
                currentAnimation = wait;
                currentAnimation.Update();

                timeSinceDetection += Time.DeltaTime;
                if (timeSinceDetection >= detectionCoolDown)
                {
                    shootThunder = true;
                    transform.Position = new Vector2(transform.Position.x, GroundHeight - BulletHeight);
                    bulletVel = 0;
                    acceleration = 0;
                }
            }
            else
            {
                currentAnimation = spawn;
                currentAnimation.Update();

                bulletVel += acceleration * Time.DeltaTime;
                transform.Translate(direction, bulletVel * Time.DeltaTime);

                waveTime += Time.DeltaTime;
                float waveOffset = (float)Math.Sin(waveTime * waveFrequency) * waveAmplitude;
                transform.Position = new Vector2(transform.Position.x, transform.Position.y + waveOffset);
            }
        }

        public void CheckPositions(Character player)
        {
            Vector2 playerPosition = player.Transform.Position;
            Vector2 bulletPosition = transform.Position;

            if (bulletPosition.x + BulletWidth > playerPosition.x && bulletPosition.x < playerPosition.x + Character.PlayerWidth &&
                bulletPosition.y + BulletHeight > playerPosition.y && bulletPosition.y < playerPosition.y + Character.PlayerHeight && !shootThunder)
            {
                player.Health -= 1;
                playerDetected = true;
                timeSinceDetection = 0;
                GameManager.Instance.LevelController.thunderattacks.Remove(this);
                return;
            }
            else if (Math.Abs(playerPosition.x - bulletPosition.x) <= Character.PlayerWidth / 2 && !playerDetected)
            {
                playerDetected = true;
                timeSinceDetection = 0;
            }

            if (transform.Position.y >= GroundHeight - BulletHeight || transform.Position.x >= ScreenWidth || transform.Position.x <= 0 - BulletWidth)
            {
                destroyed = true;
                bulletVel = 0;
                acceleration = 0;
                GameManager.Instance.LevelController.thunderattacks.Remove(this);
            }
        }

        private void CreateAnimations()
        {
            List<string> destroyPaths = new List<string>();
            for (int i = 0; i < 3; i++)
            {
                destroyPaths.Add($"assets/enemyBullet/destroy/{i}.png");
            }
            List<IntPtr> spawn = new List<IntPtr>();
            for (int i = 0; i < 5; i++)
            {
                IntPtr frame = Engine.LoadImage($"assets/enemyBullet/thunderSpawn/{i}.png");
                spawn.Add(frame);
            }
            this.spawn = new Animation("Spawn", spawn, 0.075f, false);

            List<IntPtr> wait = new List<IntPtr>();
            for (int i = 0; i < 2; i++)
            {
                IntPtr frame = Engine.LoadImage($"assets/enemyBullet/thunderWait/{i}.png");
                wait.Add(frame);
            }
            this.wait = new Animation("Wait", wait, 0.05f, true);

            CreateAnimations(idlePath, destroyPaths, 0.1f);
        }
    }
}
