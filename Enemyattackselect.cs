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
        private float attackTimer = 0;
        private float pauseTimer = 0;
        private bool canAttack = true;
        EnemyMovement enemyMovement;

        public Enemyattackselect(Vector2 position, EnemyMovement movement)
        {
            transform = new Transform(position);
            enemyMovement = movement;
            rnd = new Random();
        }

        public void Update()
        {
            timers();
            selection();
        }

        private void timers()
        {
            if (canAttack)
            {
                attackTimer += Time.DeltaTime;
            }
            else
            {
                pauseTimer += Time.DeltaTime;
            }
        }

        private void selection()
        {
            if (attackTimer >= 5 && canAttack)
            {
                enemyAttack = rnd.Next(1, 3);
            }

            switch (enemyAttack)
            {
                case 1:
                    Console.WriteLine("Balas");
                    Program.enemyBullets.Add(new EnemyBullet((int)transform.Position.x, (int)transform.Position.y));
                    canAttack = false;
                    attackTimer = 0;
                    break;

                case 2:
                    Console.WriteLine("Teleport");
                    enemyMovement.teleport();
                    canAttack = false;
                    attackTimer = 0;
                    break;
            }

            if (pauseTimer >= 5)
            {
                canAttack = true;
                pauseTimer = 0;
            }
        }
    }
}
