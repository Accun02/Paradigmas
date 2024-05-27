using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
namespace MyGame
{
    public class EnemyLightningBolt : Projectile
    {
        private readonly int GroundHeight = GameManager.Instance.LevelController.GroundHeight;
        private string idlePath = "assets/enemyBullet/thunderBolt/0.png";
        private bool shootThunder;
        public const float BulletHeight = 100;
        public const float BulletWidth = 48;
        private Animation spawn;

        public EnemyLightningBolt(Vector2 position, Vector2 offset) : base(new Vector2(position.x + offset.x, position.y + offset.y), new Vector2(0, 1))
        {
            bulletVel = 550;
            acceleration = 2000;
            coolDown = 0.4f;
            CreateAnimations();
        }
        public override void Update()
        {
            if (!shootThunder)
            {
                bulletVel += acceleration * Time.DeltaTime;
                transform.Translate(direction, bulletVel * Time.DeltaTime);

                if (transform.Position.y >= GroundHeight - BulletHeight)
                {
                    shootThunder = true;
                    transform.Position = new Vector2(transform.Position.x, GroundHeight - BulletHeight);
                    bulletVel = 0;
                    acceleration = 0;
                }
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
                GameManager.Instance.LevelController.LightningList.Remove(this);
                return;
            }
            if (transform.Position.y >= GroundHeight - BulletHeight)
            {
                currentAnimation = destroy;
                currentAnimation.Update();
                destroyed = true;
                bulletVel = 0;
                acceleration = 0;
            }
            else
            {
                currentAnimation = spawn;
                currentAnimation.Update();
            }
        }
        private void CreateAnimations()
        {
            List<IntPtr> spawn = new List<IntPtr>();
            for (int i = 0; i < 2; i++)
            {
                IntPtr frame = Engine.LoadImage($"assets/enemyBullet/thunderBolt/{i}.png");
                spawn.Add(frame);
            }
            this.spawn = new Animation("Spawn", spawn, 0.095f, true);

            List<string> destroyPaths = new List<string>();
            for (int i = 0; i < 3; i++)
            {
                destroyPaths.Add($"assets/enemyBullet/thunderBolt/destroy/{i}.png");
            }
            CreateAnimations(idlePath, destroyPaths, 0.06f);
        }
    }
}