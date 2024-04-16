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
        private int healt;
        public int Healt => healt;
        private IntPtr image = Engine.LoadImage("assets/untitled.png");
     
       
   public Enemy (Vector2 position)
        {
            transform = new Transform(position);
            enemyMovement = new EnemyMovement(transform); 
            
        }
        public void Render()
        {
            Engine.Draw(image, transform.Position.x, transform.Position.y);
        }
    }
}
