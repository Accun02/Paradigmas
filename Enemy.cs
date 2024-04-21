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
        private teleport teleport;
        private Enemybullet enemybullet;
        private int healt;
        public int Healt => healt;
        private IntPtr image = Engine.LoadImage("assets/untitled.png");
<<<<<<< Updated upstream
     
       
   public Enemy (Vector2 position)
=======
        
        public Enemy (Vector2 position)
>>>>>>> Stashed changes
        {
            transform = new Transform(position);
            enemyMovement = new EnemyMovement(transform); 
            
        }
        public void Render()
        {
            Engine.Draw(image, transform.Position.x, transform.Position.y);
        }
<<<<<<< Updated upstream
=======

        private void launchbullet()
        {
            Enemybullet enemybullet = new Enemybullet(transform.Position.x +6,transform.Position.y);
        }
>>>>>>> Stashed changes
    }
}
