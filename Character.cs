using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyGame
{
    public class Character
    {
        public const float PlayerWidth = 64;
        public const float PlayerHeight = 64;

        private bool walkingLeft = false;
        private bool walkingRight = false;

        private Animation walk;
        private Animation walkRight;
        private Animation idleLeft;
        private Animation idleRight;
        private Animation currentAnimation;
        public Transform transform
        {
            get; 
            set;
        }

        private CharacterController controller;
        private IntPtr image;

        public Character(Vector2 position)
        {
            transform = new Transform(position);
            controller = new CharacterController(transform);
            image = Engine.LoadImage("assets/player.png");

            CreateAnimations();
            currentAnimation = idleRight;
        }

        public void Update()
        {
            controller.Update();

            if (Engine.KeyPress(Engine.KEY_LEFT))
            {
                currentAnimation = walk;
                currentAnimation.Update();
                walkingLeft = true;
                walkingRight = false;
            }
            else if (Engine.KeyPress(Engine.KEY_RIGHT))
            {
                currentAnimation = walkRight;
                currentAnimation.Update();
                walkingLeft = false;
                walkingRight = true;
            }
            else
            {
                if (walkingLeft)
                    currentAnimation = idleLeft;
                else if (walkingRight)
                    currentAnimation = idleRight;
                else
                    currentAnimation.Update();
            }
        }

        public void Render()
        {
            Engine.Draw(currentAnimation.CurrentFrame, transform.Position.x, transform.Position.y);
        }


        private void CreateAnimations()
        {
            List<IntPtr> walkTextures = new List<IntPtr>();
            for (int i = 0; i < 4; i++)
            {
                IntPtr frame = Engine.LoadImage($"assets/player/walk/{i}.png");
                walkTextures.Add(frame);
            }
            walk = new Animation("Walk", walkTextures, 0.1f, true);

            List<IntPtr> walkRightTextures = new List<IntPtr>();
            for (int i = 0; i < 4; i++)
            {
                IntPtr frame = Engine.LoadImage($"assets/player/walkright/{i}.png");
                walkRightTextures.Add(frame);
            }
            walkRight = new Animation("WalkRight", walkRightTextures, 0.1f, true);

            IntPtr idleLeftTexture = Engine.LoadImage("assets/player/idleleft/0.png");
            idleLeft = new Animation("IdleLeft", new List<IntPtr> { idleLeftTexture }, 1.0f, false);

            IntPtr idleRightTexture = Engine.LoadImage("assets/player/idleright/0.png");
            idleRight = new Animation("IdleRight", new List<IntPtr> { idleRightTexture }, 1.0f, false);
        }
    }
}
