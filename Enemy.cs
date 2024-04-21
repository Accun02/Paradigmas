using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace MyGame
{
    public class Enemy
    {
        private Transform transform;
        private EnemyMovement enemyMovement;
        private Enemyattackselect enemyattackselect;
        private int healt;
        public int Healt => healt;
        private IntPtr image = Engine.LoadImage("assets/untitled.png");

  

        public Enemy (Vector2 position)
        {
            transform = new Transform(position);
            enemyMovement = new EnemyMovement(transform);
            enemyattackselect = new Enemyattackselect(transform.Position);
        }
        public void Render()
        {
            Engine.Draw(image, transform.Position.x, transform.Position.y);
        }

   public void Attack()
        {

            enemyattackselect.Update();
            return;
        }
    }
}
