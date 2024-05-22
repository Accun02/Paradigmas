using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace MyGame
{
    public class ThunderAttack : Projectile 
    {
        Vector2 direction;
        Vector2 player;

        private string idlePath = "assets/enemyBullet/bullet.png";



        public const float BulletHeight = 24;
        public const float BulletWidth = 24;

        private float bulletVel = 100;
        private float acceleration = 3000;

        private Animation idle;

        public ThunderAttack (Vector2 position, Vector2 playerPosition, Vector2 offset)
        {
            Vector2 adjustedPosition = new Vector2(position.x + offset.x, position.y + offset.y);
            transform = new Transform(adjustedPosition);
            player = playerPosition;
            direction = new Vector2(playerPosition.x + (Character.PlayerWidth / 2) - adjustedPosition.x, 0);
            float length = (float)Math.Sqrt(direction.x * direction.x + direction.y * direction.y);
            direction.x /= length;

        }

        public override void Update()
        {
            bulletVel += acceleration * Time.DeltaTime;
            transform.Translate(direction, bulletVel * Time.DeltaTime);
        }
        public void CheckPositions(Character player)
        {
            if (transform.Position.x > player.transform.Position.x )
            {
               GameManager.Instance.LevelController.thunderattacks.Remove(this);
            }
        }
        public void Render()
        {
            Engine.Draw(idlePath, transform.Position.x, transform.Position.y);
        }
       
    }
}
