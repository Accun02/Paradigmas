using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.AccessControl;
using System.Text;
using System.Threading.Tasks;
namespace MyGame
{
    public class EnemyMovement
    {
        private Transform transform;
        private Random rnd = new Random();
        public EnemyMovement(Transform transform)
        {
            this.transform = transform;
        }
        public void Teleport()
        {
            Console.WriteLine(transform.Position.x + " / " + transform.Position.y);

            float newX = rnd.Next(200, 1000);
            float newY = rnd.Next(100, 300);

            transform.Position = new Vector2(newX, newY);
        }
    }
}