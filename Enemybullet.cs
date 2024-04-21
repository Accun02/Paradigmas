using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyGame
{
    internal class EnemyBullet
    {
        Transform transform;
        Character character;
        IntPtr enemybullet = Engine.LoadImage("assets/bala.png");
        private float bulletVel = 6;
        public EnemyBullet(int x, int y)
        {
            transform = new Transform(new Vector2(x, y));
        }

        public EnemyBullet(Vector2 position)
        {
            transform = new Transform(position);
        }

        public void BulletRender()
        {
            Engine.Draw(enemybullet, transform.Position.x, transform.Position.y);
        }

        public void position()
        {
            transform.Translate(new Vector2(transform.Position.x * Time.DeltaTime,transform.Position.y * Time.DeltaTime));
        }
    }
}
