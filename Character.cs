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
        private bool walkingRight = true;
        public bool WalkingRight { set { walkingRight = value; } get { return walkingRight; } }
        public bool WalkingLeft { set { walkingLeft = value; } get { return walkingLeft; } }

        private bool isJumping = false;
        private bool isDead = false;


        private int health = 1;
        private float deathTimer = 0f;
        private const float deathDelay = 1f;

        public bool IsDead { set { isDead = value; } get { return isDead; } }
        public int Health { set { health = value; } get { return health; } }

        private Animation walk;
        private Animation walkUp;
        private Animation walkRight;
        private Animation walkUpRight;
        private Animation idleLeft;
        private Animation idleUpLeft;
        private Animation idleRight;
        private Animation idleUpRight;
        private Animation jumpLeft;
        private Animation jumpRight;
        private Animation jumpUpLeft;
        private Animation jumpUpRight;

        private Animation died;

        private Animation currentAnimation;

        private Character player;
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

        public void ResetMomentum()
        {
            controller.VelocityX = 0;
            controller.VelocityY = 0;
            controller.IsLookingLeft = false;
            controller.IsLookingRight = true;
        }

        public void Update(Character player)
        {
            if (player.Health > 0)
            {
                controller.Update(player);
                isJumping = controller.IsJumping;
                deathTimer = 0f;
                isDead = false;
                died.Restart();

                if (Engine.KeyPress(Engine.KEY_LEFT) && !Engine.KeyPress(Engine.KEY_RIGHT))
                {
                walkingLeft = true;
                walkingRight = false;
                }

            if (Engine.KeyPress(Engine.KEY_RIGHT) && Engine.KeyPress(Engine.KEY_LEFT))
            {
                walkingLeft = false;
                walkingRight = true;
            }

            if (isJumping)
            {
                if (walkingLeft)
                {
                    currentAnimation = Engine.KeyPress(Engine.KEY_UP) ? jumpUpLeft : jumpLeft;
                }
                if (walkingRight)
                {
                    currentAnimation = Engine.KeyPress(Engine.KEY_UP) ? jumpUpRight : jumpRight;
                }
            }
            else
            {
                if (Engine.KeyPress(Engine.KEY_LEFT) && !Engine.KeyPress(Engine.KEY_RIGHT))
                {
                    if (Engine.KeyPress(Engine.KEY_UP))
                    {
                        currentAnimation = walkUp;
                    }
                    else
                    {
                        currentAnimation = walk;
                    }
                    currentAnimation.Update();
                    walkingLeft = true;
                    walkingRight = false;
                }
                else if (Engine.KeyPress(Engine.KEY_RIGHT) && !Engine.KeyPress(Engine.KEY_LEFT))
                {
                    if (Engine.KeyPress(Engine.KEY_UP))
                    {
                        currentAnimation = walkUpRight;
                    }
                    else
                    {
                        currentAnimation = walkRight;
                    }
                    currentAnimation.Update();
                    walkingLeft = false;
                    walkingRight = true;
                }
                else
                {
                    if (walkingLeft)
                    {
                        currentAnimation = Engine.KeyPress(Engine.KEY_UP) ? idleUpLeft : idleLeft;
                    }
                    else if (walkingRight)
                    {
                        currentAnimation = Engine.KeyPress(Engine.KEY_UP) ? idleUpRight : idleRight;
                    }
                }
            }
            }
            else
            {
                deathTimer += Time.DeltaTime;
                    if (deathTimer >= deathDelay)
                    {
                        isDead = true;
                        currentAnimation = died;
                        currentAnimation.Update();
                    }
            }
        }


        public void Render(Character player)
        {
            if (!isDead)
            {
                Engine.Draw(currentAnimation.CurrentFrame, transform.Position.x, transform.Position.y);
            }
            else
            {
                Engine.Draw(currentAnimation.CurrentFrame, transform.Position.x - 30, transform.Position.y - 50);
            }
        }


        private void CreateAnimations()
        {

            // Movimiento

            List<IntPtr> walkTextures = new List<IntPtr>();
            for (int i = 0; i < 4; i++)
            {
                IntPtr frame = Engine.LoadImage($"assets/player/walk/walkLeft/{i}.png");
                walkTextures.Add(frame);
            }
            walk = new Animation("Walk", walkTextures, 0.1f, true);

            List<IntPtr> walkRightTextures = new List<IntPtr>();
            for (int i = 0; i < 4; i++)
            {
                IntPtr frame = Engine.LoadImage($"assets/player/walk/walkRight/{i}.png");
                walkRightTextures.Add(frame);
            }
            walkRight = new Animation("WalkRight", walkRightTextures, 0.1f, true);

            List<IntPtr> walkUpTextures = new List<IntPtr>();
            for (int i = 0; i < 4; i++)
            {
                IntPtr frame = Engine.LoadImage($"assets/player/walk/walkUpLeft/{i}.png");
                walkUpTextures.Add(frame);
            }
            walkUp = new Animation("WalkUp", walkUpTextures, 0.1f, true);

            List<IntPtr> walkUpRightTextures = new List<IntPtr>();
            for (int i = 0; i < 4; i++)
            {
                IntPtr frame = Engine.LoadImage($"assets/player/walk/walkUpRight/{i}.png");
                walkUpRightTextures.Add(frame);
            }
            walkUpRight = new Animation("WalkUpRight", walkUpRightTextures, 0.1f, true);

            // Idle

            IntPtr idleLeftTexture = Engine.LoadImage("assets/player/idle/idleLeft/0.png");
            idleLeft = new Animation("IdleLeft", new List<IntPtr> { idleLeftTexture }, 1.0f, false);

            IntPtr idleRightTexture = Engine.LoadImage("assets/player/idle/idleRight/0.png");
            idleRight = new Animation("IdleRight", new List<IntPtr> { idleRightTexture }, 1.0f, false);

            List<IntPtr> idleUpLeftTextures = new List<IntPtr>();
            IntPtr idleUpLeftTexture = Engine.LoadImage("assets/player/idle/idleUpLeft/0.png");
            idleUpLeftTextures.Add(idleUpLeftTexture);
            idleUpLeft = new Animation("IdleUpLeft", idleUpLeftTextures, 1.0f, false);

            List<IntPtr> idleUpRightTextures = new List<IntPtr>();
            IntPtr idleUpRightTexture = Engine.LoadImage("assets/player/idle/idleUpRight/0.png");
            idleUpRightTextures.Add(idleUpRightTexture);
            idleUpRight = new Animation("IdleUpRight", idleUpRightTextures, 1.0f, false);

            // Salto

            List<IntPtr> jumpLeftTextures = new List<IntPtr>();
            for (int i = 0; i < 1; i++)
            {
                IntPtr frame = Engine.LoadImage($"assets/player/jump/left/{i}.png");
                jumpLeftTextures.Add(frame);
            }
            jumpLeft = new Animation("JumpLeft", jumpLeftTextures, 1.0f, false);

            List<IntPtr> jumpRightTextures = new List<IntPtr>();
            for (int i = 0; i < 1; i++)
            {
                IntPtr frame = Engine.LoadImage($"assets/player/jump/right/{i}.png");
                jumpRightTextures.Add(frame);
            }
            jumpRight = new Animation("JumpRight", jumpRightTextures, 1.0f, false);

            List<IntPtr> jumpUpLeftTextures = new List<IntPtr>();
            IntPtr jumpUpLeftTexture = Engine.LoadImage("assets/player/jump/upLeft/0.png");
            jumpUpLeftTextures.Add(jumpUpLeftTexture);
            jumpUpLeft = new Animation("JumpUpLeft", jumpUpLeftTextures, 1.0f, false);

            List<IntPtr> jumpUpRightTextures = new List<IntPtr>();
            IntPtr jumpUpRightTexture = Engine.LoadImage("assets/player/jump/UpRight/0.png");
            jumpUpRightTextures.Add(jumpUpRightTexture);
            jumpUpRight = new Animation("JumpUpRight", jumpUpRightTextures, 1.0f, false);

            // Salto

            List<IntPtr> boomTextures = new List<IntPtr>();
            for (int i = 0; i < 18; i++)
            {
                IntPtr frame = Engine.LoadImage($"assets/explosion/{i}.png");
                boomTextures.Add(frame);
            }
            died = new Animation("Boom", boomTextures, 0.05f, false);

        }
    }
}
