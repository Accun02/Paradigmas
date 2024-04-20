using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyGame
{
   public class Enemyattackselect
    {

        private int enemyAttack;
        public int EnemyAttack => enemyAttack;
        Random rnd;
        private float attackTime;
        private bool canAttack = true;
        private float pause = 0;
        teleport  teleport = new teleport();
        Enemybullet enemybullet;
        
        private void Attacklist()
        {
            
            while (canAttack) {
                attackTime = Time.DeltaTime;
                if (attackTime > 5)
                {
                   

                    rnd = new Random();

             

                    switch (rnd.Next(1, 2)) // elegir ataque
                    {
                        case 1:
                            //teleport
                            teleport.randompos();
                            canAttack = false;
                            attackTime = 0;
                            break;
                        case 2:
                            //ivoke bullet
                            canAttack = false;
                           // Invoke(enemybullet);
                            enemybullet.Movement();
                            attackTime = 0;
                            break;
                    }
                    if (canAttack == false) // tiempo entre ataque para que no se vuelva loco el juego.
                    {
                        pause = Time.DeltaTime;

                        if (pause > 5) 
                        {
                            canAttack = true;
                            pause = 0;
                        }
                    }
                }
             
                }
                
            }
        }
    }



