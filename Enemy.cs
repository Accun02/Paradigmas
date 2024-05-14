using MyGame;
using System;
using System.Collections.Generic;

public class Enemy
{
    public const float EnemyWidth = 64;
    public const float EnemyHeight = 64;

    private bool vulnerable = true;
    public bool Vulnerable { set { vulnerable = value; } get { return vulnerable; } }

    private int health = 100;
    private float levitationAmplitude = 90;
    private float levitationSpeed = 2;

    private Transform transform;
    public Transform Transform { get { return transform; } }
    public int Health { set { health = value; } get { return health; } }

    private EnemyMovement enemyMovement;
    private EnemyAttack enemyattackselect;

    private IntPtr image;

    private Animation Idle;
    private Animation enemyattack;
    private Animation currentAnimation;
    private Animation teleport;

    public Enemy(Vector2 position)
    {
        transform = new Transform(position);
        enemyMovement = new EnemyMovement(Transform);
        enemyattackselect = new EnemyAttack(transform.Position, enemyMovement);
        image = Engine.LoadImage("assets/Misery/Idle/0.png");
        CreateAnimations();
        currentAnimation = Idle;
    }

    public void ResetAttacks()
    {
        enemyattackselect.IsAttacking = false;
        enemyattackselect.AttackTimer = -1;
        enemyattackselect.EnemyAttackSelect = 0;
        enemyattackselect.ResetCurrent();
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
    }

    public void Render()
    {
        Engine.Draw(currentAnimation.CurrentFrame, transform.Position.x, transform.Position.y);
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

        float playerLeft = player.transform.Position.x;
        float playerRight = player.transform.Position.x + Character.PlayerWidth;
        float playerTop = player.transform.Position.y;
        float playerBottom = player.transform.Position.y + Character.PlayerHeight;

        if (vulnerable && enemyRight >= playerLeft && enemyLeft <= playerRight && enemyBottom >= playerTop && enemyTop <= playerBottom)
        {
            player.Health = 0;
        }
    }

    public void TakeDamage(int amount)
    {
        if (vulnerable)
        {
            health -= amount;
        }
    }


    public void ResetTransform(Vector2 newPosition)
    {
        transform.Position = newPosition;
    }


}
