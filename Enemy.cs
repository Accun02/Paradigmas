using MyGame;
using System;
using System.Collections.Generic;
using Tao.Sdl;
public class Enemy
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

    private Transform transform;
    private EnemyMovement enemyMovement;
    private EnemyAttack enemyattackselect;
    private Animation Idle;
    private Animation enemyattack;
    private Animation currentAnimation;
    private Animation teleport;

    public Transform Transform { get { return transform; } }
    public bool Vulnerable { set { vulnerable = value; } get { return vulnerable; } }
    public int Health { set { health = value; } get { return health; } }
    public int MaxHealth { set { maxHealth = value; } get { return maxHealth; } }

    public Enemy(Vector2 position)
    {
        transform = new Transform(position);
        enemyMovement = new EnemyMovement(Transform);
        enemyattackselect = new EnemyAttack(enemyMovement);
        CreateAnimations();
        currentAnimation = Idle;
    }
    public void Update(float deltaTime)
    {
        vulnerable = !enemyattackselect.IsTeleportOnCooldown;
        enemyattackselect.Update(transform.Position);
        if (enemyattackselect.EnemyAttackSelect == 1)
        {
            currentAnimation = enemyattack;
            currentAnimation.Update();
        }
        else if (enemyattackselect.EnemyAttackSelect == 2)
        {
            if (enemyattackselect.IsTeleportOnCooldown)
            {
                currentAnimation = teleport;
            }
            else
            {
                currentAnimation = Idle;
            }
            currentAnimation.Update();
        }
        else
        {
            currentAnimation = Idle;
            currentAnimation.Update();
        }
        if (!enemyattackselect.IsTeleportOnCooldown)
        {
            float levitationOffset = (float)Math.Sin(levitationSpeed * DateTime.Now.Millisecond / 1000f * Math.PI) * levitationAmplitude;
            SetPositionY(transform.Position.y + levitationOffset * deltaTime);
        }
        if (isShaking)
        {
            shakeTimer -= deltaTime;
            if (shakeTimer <= 0)
            {
                isShaking = false;
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
    private void SetPositionY(float y)
    {
        transform.Position = new Vector2(transform.Position.x, y);
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
            Console.WriteLine(player.Health);
            player.Health -= 1;
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
    public void ResetAttacks()
    {
        enemyattackselect.IsAttacking = false;
        enemyattackselect.AttackTimer = -1;
        enemyattackselect.EnemyAttackSelect = 0;
        enemyattackselect.ResetCurrent();
    }
    private void CreateAnimations()
    {
        List<IntPtr> idle = new List<IntPtr>();
        for (int i = 0; i < 2; i++)
        {
            IntPtr frame = Engine.LoadImage($"assets/Misery/Idle/{i}.png");
            idle.Add(frame);
        }
        Idle = new Animation("Idle", idle, 0.5f, true);
        List<IntPtr> Attack = new List<IntPtr>();
        for (int i = 0; i < 1; i++)
        {
            IntPtr frame = Engine.LoadImage($"assets/Misery/Bullet/{i}.png");
            Attack.Add(frame);
        }
        enemyattack = new Animation("enemyattack", Attack, 0.025f, false);
        List<IntPtr> Teleport = new List<IntPtr>();
        for (int i = 0; i < 1; i++)
        {
            IntPtr frame = Engine.LoadImage($"assets/Misery/Teleport/2.png");
            Teleport.Add(frame);
        }
        teleport = new Animation("teleport", Teleport, 0.1f, false);
    }
}