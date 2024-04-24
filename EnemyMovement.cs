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
        private float coolDown = 0.65f;
        private bool teleported = false;
        public bool Teleported { set { teleported = value; } get { return teleported; } }
        public EnemyMovement(Transform transform)
        {
            this.transform = transform;
        }
        public void Teleport()
        {
            Program.TeleportList.Add(new Teleport((int)transform.Position.x, (int)transform.Position.y, new Vector2(1, 0), "assets/Misery/Teleport/0.png"));
            Program.TeleportList.Add(new Teleport((int)transform.Position.x, (int)transform.Position.y, new Vector2(-1, 0), "assets/Misery/Teleport/1.png"));

            float newX = rnd.Next(200, 1000);
            float newY = rnd.Next(100, 300);

            transform.Position = new Vector2(newX, newY);
        }
    }
}