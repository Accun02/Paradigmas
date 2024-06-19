using System;
using MyGame;

public class CharacterController
{
    // Jugador
    private const float PlayerWidth = Character.PlayerWidth;
    private const float PlayerHeight = Character.PlayerHeight;

    // Escenario
    private int GroundHeight = GameManager.Instance.LevelController.GroundHeight;
    private int ScreenWidth = GameManager.Instance.LevelController.ScreenWidth;

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

    private float footCounter = 0;

    // Salto doble
    private bool isJumping = false;
    private bool landed = false;

    private int jumpCounter = 0;
    private float actualCoolDown = 0f;
    private float dobleJumpCoolDown = 0.25f;

    // Input buffer
    private float jumpBufferTime = 0.15f;
    private float jumpBufferCounter = 0f;

    // Cooldown disparo
    private float shootCooldown = 0.15f;
    private float shootTime;

    private Transform transform;
    private CharacterWeapon weapon;

    private Sound shootSound;
    private Sound walkSound;
    private Sound walkSound2;
    private Sound emptyWalkSound;
    private Sound jumpSound;
    private Sound landSound;

    public float VelocityX { set { velocityX = value; } get { return velocityX; } }
    public float VelocityY { set { velocityY = value; } get { return velocityY; } }
    public float JumpBufferCounter { set { jumpBufferCounter = value; } get { return jumpBufferCounter; } }
    public bool IsLookingLeft { set { isLookingLeft = value; } get { return isLookingLeft; } }
    public bool IsLookingRight { set { isLookingRight = value; } get { return isLookingRight; } }
    public bool IsJumping { set { isJumping = value; } get { return isJumping; } }
    public bool Landed { set { landed = value; } get { return landed; } }
    public int JumpCounter { set { jumpCounter = value; } get { return jumpCounter; } }

    public CharacterController(Transform transform)
    {
        this.transform = transform;
        weapon = new CharacterWeapon();

        shootSound = new Sound("shoot.wav");
        walkSound = new Sound("walk.wav");
        walkSound2 = new Sound("walk2.wav");
        emptyWalkSound = new Sound("emptyWalk.wav");
        jumpSound = new Sound("jump.wav");
        landSound = new Sound("walk.wav");
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

        if (jumpBufferCounter > 0)
        {
            jumpBufferCounter -= Time.DeltaTime;
        }
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

            if (!walkSound.IsPlaying() && !isJumping)
            {
                footCounter++;

                if (footCounter == 1)
                {
                    walkSound.PlayOnce(GameManager.Instance.audioMixer.WalkChannel);
                }
                if (footCounter == 2)
                {
                    emptyWalkSound.PlayOnce(GameManager.Instance.audioMixer.WalkChannel);
                }
                if (footCounter == 3)
                {
                    walkSound2.PlayOnce(GameManager.Instance.audioMixer.WalkChannel);
                }
                if (footCounter == 4)
                {
                    emptyWalkSound.PlayOnce(GameManager.Instance.audioMixer.WalkChannel);
                    footCounter = 0;
                }
            }
        }
        else if (pressingRight && !pressingLeft)
        {
            velocityX = Math.Min(velocityX + Acceleration * Time.DeltaTime, MaxSpeed);
            isLookingLeft = false;
            isLookingRight = true;

            if (!walkSound.IsPlaying() && !isJumping)
            {
                footCounter++;

                if (footCounter == 1)
                {
                    walkSound.PlayOnce(GameManager.Instance.audioMixer.WalkChannel);
                }
                if (footCounter == 2)
                {
                    emptyWalkSound.PlayOnce(GameManager.Instance.audioMixer.WalkChannel);
                }
                if (footCounter == 3)
                {
                    walkSound2.PlayOnce(GameManager.Instance.audioMixer.WalkChannel);
                }
                if (footCounter == 4)
                {
                    emptyWalkSound.PlayOnce(GameManager.Instance.audioMixer.WalkChannel);
                    footCounter = 0;
                }
            }
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

            walkSound.Stop();
        }

        // Saltar
        if (!Engine.KeyPress(Engine.KEY_Z))
        {
            GameManager.Instance.ZKeyReleased = true;
        }

        if (Engine.KeyPress(Engine.KEY_Z) && GameManager.Instance.ZKeyReleased)
        {
            jumpBufferCounter = jumpBufferTime;
            GameManager.Instance.ZKeyReleased = false;
        }

        if (jumpBufferCounter > 0 && actualCoolDown <= 0 && jumpCounter < 2)
        {
            Jump();
            jumpCounter++;
            actualCoolDown = dobleJumpCoolDown;
            jumpBufferCounter = 0;

            jumpSound.PlayOnce(GameManager.Instance.audioMixer.JumpChannel);
        }

        // Disparar
        if (Engine.KeyPress(Engine.KEY_X) && Engine.KeyPress(Engine.KEY_UP) && shootTime <= 0)
        {
            weapon.ShootVertical();
            shootTime = shootCooldown;

            shootSound.PlayOnce(GameManager.Instance.audioMixer.ShootChannel);
        }

        if (Engine.KeyPress(Engine.KEY_X) && !Engine.KeyPress(Engine.KEY_UP) && shootTime <= 0)
        {
            weapon.ShootHorizontal(isLookingRight, isLookingLeft);
            shootTime = shootCooldown;

            shootSound.PlayOnce(GameManager.Instance.audioMixer.ShootChannel);
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
        landed = false;
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
        else if (playerY > (GroundHeight - PlayerHeight))
        {
            transform.Translate(new Vector2(0, (GroundHeight - PlayerHeight) - transform.Position.y));

            jumpCounter = 0;
            velocityY = 0;
            actualCoolDown = 0;

            if (isJumping && !landed)
            {
                landSound.PlayOnce(GameManager.Instance.audioMixer.WalkChannel);
                isJumping = false;
                landed = true;
            }
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
}
