using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyGame
{
    internal class teleport
    {

        Transform transform;
        
        Random posx = new Random();
        Random posy = new Random();
      public void randompos()
        {
            transform.Translate(new Vector2(posx.Next(200, 400), posy.Next(400, 600)),1);
        }
    }
}
