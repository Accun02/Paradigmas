using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyGame
{
    public abstract class GameObject
    {
        protected Transform transform;
        protected Animation currentAnimation;
        protected IntPtr image;

        public GameObject()
        {
            transform = new Transform(new Vector2());
        }

        public void Render()
        {
            Engine.Draw(image, transform.Position.x, transform.Position.y);
        }

        public virtual void Update()
        {
            currentAnimation.Update();
        }
    }
}
