using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyGame
{
    public interface IDamageable
    {
        int Health { get; set; }
        void TakeDamage(int damage);
    }
}
