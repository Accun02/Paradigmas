using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyGame
{
  public class EnemyMovement
    {
        private Vector2 position;
         Transform transform;
        private int enemySpeed = 5;
        public EnemyMovement ( Transform transform)
        {
            this.transform = transform;
        }

        public void movement()
        { while (transform.Position.x < 450)
            {
                transform.Translate(new Vector2(-1, 0), enemySpeed);

            }

            while (transform.Position.x > 700)
            {
                transform.Translate(new Vector2(1, 0), enemySpeed);

            }


        }
    }
}
