using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyGame
{
    public class Enemy
    {
        private Transform transform;
        private EnemyController controller;

        public IntPtr image = Engine.LoadImage("assets/enemy.png");


        public Enemy(Vector2 posicion, Vector2 speed)
        {
            transform = new Transform(posicion);
            controller = new EnemyController(transform, speed);
        }
        public Enemy() 
        {
            transform = new Transform(new Vector2(200,200));
            controller = new EnemyController(transform, new Vector2(2,2));
        }

        public void Update() 
        {
            controller.MoverEnemigo();
        }

        public void Render() 
        {
            Engine.Draw(image,transform.Position.x, transform.Position.y);
        }
    }
}
