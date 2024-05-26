using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyGame
{
    public class EnemyController
    {
        private Transform transform;
        public int speed = 1;

        public EnemyController(Transform transform, Vector2 speed)
        {
            this.transform = transform;
        }

        public void MoveEnemy() 
        {
            transform.Translate(new Vector2(1,0), speed);
        }
    }
}
