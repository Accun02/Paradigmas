using MyGame;
using System;
using System.Collections.Generic;
public class Enemy : GameObject, IDamageable
{
    public const float EnemyWidth = 80;
    public const float EnemyHeight = 80;

    private bool vulnerable = true;
    private int health = 100;
    private int maxHealth = 100;
    private float levitationAmplitude = 90;
    private float levitationSpeed = 2;

    private bool isShaking = false;
    private float shakeOffsetX = 0;
    private float shakeTimer = 0;
    private float shakeDuration = 0.15f;
    private float shakeMagnitude = 2;
    private float shakeFrequency = 10;

    private float speedY = 0;
    private float acceleration = 550;

    private bool offScreen = false;
    private bool dashing = false;

    private Transform transform;
    private EnemyMovement enemyMovement;
    private EnemyAttack enemyAttack;

    private Animation Idle;
    private Animation enemyattack;
    private Animation enemyattack2;
    private Animation currentAnimation;
    private Animation teleport;

    public Transform Transform { get { return transform; } }
    public bool Vulnerable { set { vulnerable = value; } get { return vulnerable; } }
    public int Health { set { health = value; } get { return health; } }
    public int MaxHealth { set { maxHealth = value; } get { return maxHealth; } }

    public Enemy(Vector2 position) : base(position, EnemyWidth, EnemyHeight)
    {
        transform = new Transform(position);
        enemyMovement = new EnemyMovement(Transform);
        enemyAttack = new EnemyAttack(enemyMovement);
        CreateAnimations();
        currentAnimation = Idle;
    }
    public void Update()
    {
        vulnerable = !enemyAttack.IsTeleportOnCooldown;
        enemyAttack.Update(transform.Position);

        if (enemyAttack.attackNumber == 1 || enemyAttack.attackNumber == 5)
        {
            currentAnimation = enemyattack2;
            currentAnimation.Update();
        }
        else if (enemyAttack.attackNumber == 2)
        {
            if (enemyAttack.IsTeleportOnCooldown)
            {
                currentAnimation = teleport;
            }
            else
            {
                currentAnimation = Idle;
            }
            currentAnimation.Update();
        }
        else if (enemyAttack.attackNumber == 4)
        {
            currentAnimation = enemyattack;
            currentAnimation.Update();
        }
        else
        {
            currentAnimation = Idle;
            currentAnimation.Update();
        }

        if (isShaking)
        {
            shakeTimer -= Time.DeltaTime;
            if (shakeTimer <= 0)
            {
                isShaking = false;
            }
        }

        if (!enemyAttack.IsTeleportOnCooldown && !enemyAttack.DashAttacking)
        {
            float levitationOffset = (float)Math.Sin(levitationSpeed * DateTime.Now.Millisecond / 1000f * Math.PI) * levitationAmplitude;
            transform.Position = new Vector2(transform.Position.x, (transform.Position.y + levitationOffset * Time.DeltaTime));
        }
        else if (enemyAttack.DashAttacking && !offScreen)
        {
            speedY += acceleration * Time.DeltaTime;
            transform.Translate(new Vector2(0, -1), speedY * Time.DeltaTime);

            if (transform.Position.y < -EnemyHeight * 2)
            {
                speedY = 0;
                offScreen = true;
            }
        }
    }
    public void Render()
    {
        if (isShaking)
        {
            shakeOffsetX = (float)(shakeMagnitude * Math.Sin(2 * Math.PI * (shakeDuration - shakeTimer) * shakeFrequency));
        }
        Engine.Draw(currentAnimation.CurrentFrame, transform.Position.x + shakeOffsetX, transform.Position.y);
    }

    public void CheckCollision(Character player)
    {
        float enemyLeft = transform.Position.x;
        float enemyRight = transform.Position.x + EnemyWidth;
        float enemyTop = transform.Position.y;
        float enemyBottom = transform.Position.y + EnemyHeight;
        float playerLeft = player.Transform.Position.x;
        float playerRight = player.Transform.Position.x + Character.PlayerWidth;
        float playerTop = player.Transform.Position.y;
        float playerBottom = player.Transform.Position.y + Character.PlayerHeight;

        if (vulnerable && enemyRight >= playerLeft && enemyLeft <= playerRight && enemyBottom >= playerTop && enemyTop <= playerBottom)
        {
            player.TakeDamage(1);
        }
    }
    public void TakeDamage(int amount)
    {
        if (vulnerable)
        {
            health -= amount;
            isShaking = true;
            shakeTimer = shakeDuration;
        }
    }
    public void ResetTransform(Vector2 newPosition)
    {
        transform.Position = newPosition;
    }
    public void ResetEnemy()
    {
        isShaking = false;
        dashing = false;
        offScreen = false;
        enemyAttack.IsAttacking = false;
        enemyAttack.DashAttacking = false;
        enemyAttack.AttackTimer = -1;
        enemyAttack.attackNumber = 0;
        currentAnimation = Idle;
        currentAnimation.Update();
        enemyAttack.ResetCurrent();
    }
    private void CreateAnimations()
    {
        List<IntPtr> idle = new List<IntPtr>();
        for (int i = 0; i < 16; i++)
        {
            IntPtr frame = Engine.LoadImage($"assets/enemy/idle/{i}.png");
            idle.Add(frame);
        }
        Idle = new Animation("Idle", idle, 0.060f, true);

        List<IntPtr> Attack = new List<IntPtr>();
        for (int i = 0; i < 1; i++)
        {
            IntPtr frame = Engine.LoadImage($"assets/enemy/shooting/bullet.png");
            Attack.Add(frame);
        }
        enemyattack = new Animation("enemyattack", Attack, 0.45f, false);

        List<IntPtr> Attack2 = new List<IntPtr>();
        for (int i = 0; i < 1; i++)
        {
            IntPtr frame = Engine.LoadImage($"assets/enemy/shooting/thunder.png");
            Attack2.Add(frame);
        }
        enemyattack2 = new Animation("enemyattack2", Attack2, 0.45f, false);

        List<IntPtr> Teleport = new List<IntPtr>();
        for (int i = 0; i < 1; i++)
        {
            IntPtr frame = Engine.LoadImage($"assets/enemy/teleport/2.png");
            Teleport.Add(frame);
        }
        teleport = new Animation("teleport", Teleport, 0.1f, false);
    }

}