using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyGame
{
    public class Enemyattackselect
    {
        Transform transform;
        private int enemyAttack;
        Random rnd;
        private float attackTime = 0;
        private bool canAttack = true;
        private float pause = 0;
        EnemyMovement enemyMovement;

        public Enemyattackselect (Vector2 position)
        {

            transform = new Transform( position);
          
        }


      public void Update()
      {
           timers();
            selecttion();

        }
        private void timers()
        {
            if (canAttack)
            {
                attackTime = Time.DeltaTime;

            }

            if (canAttack == false ) 
            
            {
                pause = Time.DeltaTime;
            }
              
            

        }


        private void selecttion()
        {
          
            if (attackTime >= 5 && canAttack)
            {
                enemyAttack = rnd.Next(1, 2);

            }

            switch (enemyAttack)
            {
                case 1:
                    Program.enemyBullets.Add(new EnemyBullet((int)transform.Position.x, (int)transform.Position.y));
                    canAttack = false;
                    attackTime = 0;
                    break;

                case 2:
                    enemyMovement.teleport();
                    canAttack = false;
                    attackTime = 0;
                    break;
            }

            if (pause > 5)
            {
                canAttack = true;
                pause = 0;
            }
        }

    }
}

  




