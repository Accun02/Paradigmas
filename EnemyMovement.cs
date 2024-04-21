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
        private Vector2 position;
        private Transform transform;
        private Random rndx = new Random();
        private Random rndy = new Random();
        private float enemySpeed = 10;

        public EnemyMovement(Transform transform)
        {
            this.transform = transform;
        }

        public void teleport()
        {
            transform.Position = new Vector2(rndx.Next(200, 400), rndy.Next(200, 600));
        }
    }

}
