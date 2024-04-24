using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace MyGame
{
    public class Enemyattackselect
    {
        private Transform transform;
        private int enemyAttack;
        public int  EnemyAttack
        {
            get { return enemyAttack; }
        }
        private Random rnd;
        private EnemyMovement enemyMovement;

        private float attackTimer = 0;
        private float pauseTimer = 0;
        private float timeBetweenAttacks = 0.25f;

        private bool canAttack = true;
        private bool isAttacking = false;

        private float EnemyWidth = Enemy.EnemyWidth;
        private float EnemyHeight = Enemy.EnemyHeight;
        private float BulletWidth = EnemyBullet.BulletWidth;
        private float BulletHeight = EnemyBullet.BulletHeight;

        public Enemyattackselect(Vector2 position, EnemyMovement enemyMovement)
        {
            transform = new Transform(position);
            this.enemyMovement = enemyMovement;
            rnd = new Random();
        }
        public void Update( Vector2 Position)
        {
            timers();
            selection(Position);
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
        private void selection(Vector2 position)
        {
            if (attackTimer >= 1 && canAttack)
            {
                enemyAttack = rnd.Next(1, 3);
                isAttacking = true;
            }
            if (isAttacking)
            {
                switch (enemyAttack)
                {
                    case 1:
                        Program.enemyBullets.Add(new EnemyBullet(position, Program.player.transform.Position, new Vector2(-BulletWidth, EnemyHeight / 2 - BulletHeight / 2)));
                        Program.enemyBullets.Add(new EnemyBullet(position, Program.player.transform.Position, new Vector2(EnemyWidth + EnemyBullet.BulletWidth, EnemyHeight / 2 - BulletHeight / 2)));

                        canAttack = false;
                        attackTimer = 0;
                        break;
                    case 2:
                        enemyMovement.Teleport();
                        canAttack = false;
                        attackTimer = 0;
                        break;
                }
                isAttacking = false;
            }
            if (pauseTimer >= timeBetweenAttacks)
            {
                canAttack = true;
                pauseTimer = 0;
            }
        }
    }
}