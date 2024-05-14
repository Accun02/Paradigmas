using System;
using System.Diagnostics;

namespace MyGame
{
    public class CharacterController
    {
        // Jugador
        private const float PlayerWidth = Character.PlayerWidth;
        private const float PlayerHeight = Character.PlayerHeight;

        // Escenario
        private int GroundHeight = Program.GroundHeight;
        private int ScreenWidth = Program.ScreenWidth;

        // Movimiento
        private float Acceleration = 3000;
        private float Deceleration = 2000;

        private float velocityX = 0;
        private float velocityY = 0;

        private float MaxSpeed = 450;

        private const float JumpSpeed = 700;
        private const float Gravity = 1600f;

        private bool canMoveLeft = true;
        private bool canMoveRight = true;
        private bool isLookingLeft = false;
        private bool isLookingRight = true;

        public float VelocityX { set { velocityX = value; } get { return velocityX; } }
        public float VelocityY { set { velocityY = value; } get { return velocityY; } }
        public bool IsLookingLeft { set { isLookingLeft = value; } get { return isLookingLeft; } }
        public bool IsLookingRight { set { isLookingRight = value; } get { return isLookingRight; } }

        // Salto doble
        private bool isJumping = false;

        public bool IsJumping
        {
            get { return isJumping; }
            set { isJumping = value; }
        }

        private int jumpCounter = 0;
        private float actualCoolDown = 0f;
        private float dobleJumpCoolDown = 0.25f;

        //Cooldown disparo
        private float shootCooldown = 0.15f;
        private float shootTime;

        private Transform transform;

        public CharacterController(Transform transform)
        {
            this.transform = transform;
        }

        public void Update(Character player)
        {
            GetInputs();
            HorizontalMovement();
            ConstraintArea();

            if (actualCoolDown > 0f)
            {
                actualCoolDown -= Time.DeltaTime;
            }

            if (shootCooldown > 0.25f)
            {
                shootCooldown -= Time.DeltaTime;
            }

            if (shootTime > 0)
            {
                shootTime -= Time.DeltaTime;
            }

            // Debug
            //Console.WriteLine(zKeyReleased);
        }

        private void GetInputs()
        {
            bool pressingLeft = Engine.KeyPress(Engine.KEY_LEFT) && canMoveLeft;
            bool pressingRight = Engine.KeyPress(Engine.KEY_RIGHT) && canMoveRight;

            if (pressingLeft && !pressingRight)
            {
                velocityX = Math.Max(velocityX - Acceleration * Time.DeltaTime, -MaxSpeed);
                isLookingLeft = true;
                isLookingRight = false;
            }
            else if (pressingRight && !pressingLeft)
            {
                velocityX = Math.Min(velocityX + Acceleration * Time.DeltaTime, MaxSpeed);
                isLookingLeft = false;
                isLookingRight = true;
            }
            else
            {
                if (velocityX > 0)
                {
                    velocityX = Math.Max(velocityX - Deceleration * Time.DeltaTime, 0);
                }
                else if (velocityX < 0)
                {
                    velocityX = Math.Min(velocityX + Deceleration * Time.DeltaTime, 0);
                }
            }

            if (!Engine.KeyPress(Engine.KEY_Z))  // Registra "KeyUp"
            {
                GameManager.Instance.ZKeyReleased = true;
            }

            if (Engine.KeyPress(Engine.KEY_Z) && actualCoolDown <= 0 && jumpCounter < 2 && GameManager.Instance.ZKeyReleased) // Salto
            {
                Jump();
                jumpCounter++;
                actualCoolDown = dobleJumpCoolDown;
                GameManager.Instance.ZKeyReleased = false;
            }

            if (Engine.KeyPress(Engine.KEY_X) && Engine.KeyPress(Engine.KEY_UP) && shootTime <= 0)
            {
                ShootVertical();
                shootTime = shootCooldown;
            }

            if (Engine.KeyPress(Engine.KEY_X) && !Engine.KeyPress(Engine.KEY_UP) && shootTime <= 0)
            {
                ShootHorizontal();
                shootTime = shootCooldown;
            }
        }

        private void HorizontalMovement()
        {
            transform.Translate(new Vector2(velocityX * Time.DeltaTime, 0));
        }

        private void Jump()
        {
            velocityY = -JumpSpeed;
            isJumping = true;
        }

        private void ConstraintArea()
        {
            velocityY += Gravity * Time.DeltaTime;
            float playerY = transform.Position.y + velocityY * Time.DeltaTime;

            if (playerY > (GroundHeight - PlayerHeight))
            {
                transform.Translate(new Vector2(0, (GroundHeight - PlayerHeight) - transform.Position.y));
                isJumping = false;
                jumpCounter = 0;
                velocityY = 0;
                actualCoolDown = 0;
            }
            else
            {
                transform.Translate(new Vector2(0, velocityY * Time.DeltaTime));
            }

            if (transform.Position.x < 0)
            {
                transform.Translate(new Vector2(-transform.Position.x, 0));
            }
            else if (transform.Position.x > ScreenWidth - PlayerWidth)
            {
                transform.Translate(new Vector2(ScreenWidth - PlayerWidth - transform.Position.x, 0));
            }

            if (transform.Position.y < 0)
            {
                transform.Translate(new Vector2(0, -transform.Position.y));
            }
            else if (transform.Position.y > GroundHeight - PlayerHeight)
            {
                transform.Translate(new Vector2(0, (GroundHeight - PlayerHeight) - transform.Position.y));
                jumpCounter = 0;
                velocityY = 0;
                actualCoolDown = 0;
            }

            if (transform.Position.x <= 0)
            {
                transform.Position = new Vector2(0, transform.Position.y);
                velocityX = 0;
                canMoveLeft = false;
            }
            else if (transform.Position.x >= ScreenWidth - PlayerWidth)
            {
                transform.Position = new Vector2((ScreenWidth - PlayerWidth), transform.Position.y);
                velocityX = 0;
                canMoveRight = false;
            }
            else
            {
                canMoveLeft = true;
                canMoveRight = true;
            }
        }

        public void ShootVertical()
        {
            Program.BulletList.Add(new Bullet((int)transform.Position.x + ((int)Character.PlayerWidth / 2) - Bullet.BulletWidth / 2, (int)transform.Position.y, new Vector2(0, -1), "assets/bala.png"));
        }

        public void ShootHorizontal()
        {
            if (isLookingRight)
            {
                Program.BulletList.Add(new Bullet((int)transform.Position.x + ((int)Character.PlayerWidth / 2) - Bullet.BulletWidth / 2, (int)transform.Position.y + (int)Character.PlayerHeight / 2, new Vector2(1, 0), "assets/balaH.png"));
            }
            else if (isLookingLeft)
            {
                Program.BulletList.Add(new Bullet((int)transform.Position.x + ((int)Character.PlayerWidth / 2) - Bullet.BulletWidth / 2, (int)transform.Position.y + (int)Character.PlayerHeight / 2, new Vector2(-1, 0), "assets/balaH.png"));
            }
        }
    }
}
