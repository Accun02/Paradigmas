using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyGame
{
    internal class Enemybullet
    {
        // Character character 
        Transform transform;
        private int posx;
        private int posy;
        private int bulletvel = 6;

        IntPtr enmeybullet = Engine.LoadImage("assets/bullet.png");

        public Enemybullet()
        {
          
        }

public void Movement()
        {
        // transform.Translate (Vector2 ( character.Transform.position.x, character.Transform.position.y ),6)
        }
    }
}
