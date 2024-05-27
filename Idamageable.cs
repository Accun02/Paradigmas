using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyGame
{
    public interface Idamageable
    {

        int Health { get; set; }

        

        void TakeDamage(int damage);



    }
}
