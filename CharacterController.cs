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

        private const float JumpSpeed = 600;
        private const float Gravity = 1300f;

        private bool canMoveLeft = true;
        private bool canMoveRight = true;

        // Salto doble
        private int jumpCounter = 0;
        private float actualCoolDown = 0f;
        private float dobleJumpCoolDown = 0.25f;
        private bool spaceReleased = true;

        //Cooldown disparo
        private float shootCooldown = 0.1f;
        private float shootTime;
        private int shootCount = 0;

        private Transform transform;

        public CharacterController(Transform transform)
        {
            this.transform = transform;
        }

        public void Update()
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
            //Console.WriteLine(transform.Position.x);
        }

        private void GetInputs()
        {
            if (Engine.KeyPress(Engine.KEY_LEFT) && canMoveLeft)  // Izquierda
            {
                velocityX = Math.Max(velocityX - Acceleration * Time.DeltaTime, -MaxSpeed);
            }
            else if (Engine.KeyPress(Engine.KEY_RIGHT) && canMoveRight)  // Derecha
            {
                velocityX = Math.Min(velocityX + Acceleration * Time.DeltaTime, MaxSpeed);
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
                spaceReleased = true;
            }

            if (Engine.KeyPress(Engine.KEY_Z) && actualCoolDown <= 0 && jumpCounter < 2 && spaceReleased) // Salto
            {
                Jump();
                jumpCounter++;
                actualCoolDown = dobleJumpCoolDown;
                spaceReleased = false;
            }

            if (Engine.KeyPress(Engine.KEY_X) && Engine.KeyPress(Engine.KEY_UP) && shootTime <= 0)
            {
                ShootUp();
                shootTime = shootCooldown;
            }

            if (Engine.KeyPress(Engine.KEY_X) && Engine.KeyPress(Engine.KEY_RIGHT) && shootTime <= 0)
            {
                ShootRight();
                shootTime = shootCooldown;
            }

            if (Engine.KeyPress(Engine.KEY_X) && Engine.KeyPress(Engine.KEY_LEFT) && shootTime <= 0)
            {
                ShootLeft();
                shootTime = shootCooldown;
            }

            if (Engine.KeyPress(Engine.KEY_ESC)) // Escape
            {
                Environment.Exit(0);
            }

            if (Engine.KeyPress(Engine.KEY_Q)) // Run
            {
                MaxSpeed = 950f;
            }
            else
            {
                MaxSpeed = 450f;
            }
        }


        private void HorizontalMovement()
        {
            transform.Translate(new Vector2(velocityX * Time.DeltaTime, 0));
        }

        private void Jump()
        {
            velocityY = -JumpSpeed;
        }

        private void ConstraintArea()
        {
            velocityY += Gravity * Time.DeltaTime;
            float playerY = transform.Position.y + velocityY * Time.DeltaTime;

            if (playerY > (GroundHeight - PlayerHeight))
            {
                transform.Translate(new Vector2(0, (GroundHeight - PlayerHeight) - transform.Position.y));
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


        public void ShootUp()
        {
            Program.BulletList.Add(new Bullet((int)transform.Position.x + ((int)Character.PlayerWidth / 2) - Bullet.BulletWidth / 2, (int)transform.Position.y, new Vector2(0, -1)));
        }

        public void ShootRight()
        {
            Program.BulletList.Add(new Bullet((int)transform.Position.x + ((int)Character.PlayerWidth / 2) - Bullet.BulletWidth / 2, (int)transform.Position.y, new Vector2(1, 0)));
        }

        public void ShootLeft()
        {
            Program.BulletList.Add(new Bullet((int)transform.Position.x + ((int)Character.PlayerWidth / 2) - Bullet.BulletWidth / 2, (int)transform.Position.y, new Vector2(-1, 0)));
        }
    }
}