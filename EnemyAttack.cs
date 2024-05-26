using MyGame;
using System;
using System.Diagnostics;

public class EnemyAttack
{
    private int enemyAttack;
    private Random rnd;
    private EnemyMovement enemyMovement;

    private float attackTimer = 0;
    private float pauseTimer = 0;
    private float timeBetweenAttacks = 0.25f;

    private bool canAttack = true;
    private bool isAttacking = false;

    private bool effectDuringCooldown = false;
    private bool canTeleport = true;

    private float EnemyWidth = Enemy.EnemyWidth;
    private float EnemyHeight = Enemy.EnemyHeight;
    private float BulletWidth = EnemyBullet.BulletWidth;
    private float BulletHeight = EnemyBullet.BulletHeight;

    private float repeatCooldown = 0.75f;
    private float currentRepeat = 0.0f;

    private float teleportCooldownTimer = 0;
    private float teleportCooldownDuration = 1.9f;
    private bool isTeleportOnCooldown = false;

    private float initialCooldown = 1.0f;
    private bool initialCooldownCompleted = false;

    private int repetitionCount = 0;
    private bool nearAttacking = false;

    public int EnemyAttackSelect { set { enemyAttack = value; } get { return enemyAttack; }}
    public bool IsTeleportOnCooldown { set { isTeleportOnCooldown = value; } get { return isTeleportOnCooldown; }}
    public bool IsAttacking { set { isAttacking = value; } get { return isAttacking; } }
    public float AttackTimer { set { attackTimer = value; } get { return attackTimer; } }

    public EnemyAttack(EnemyMovement enemyMovement)
    {
        this.enemyMovement = enemyMovement;
        rnd = new Random();
    }

    public void Update(Vector2 Position)
    {
        Timers(Position);
        if (initialCooldownCompleted)
        {
            Selection(Position);
        }
    }

    private void Timers(Vector2 Position)
    {
        if (!initialCooldownCompleted)
        {
            initialCooldown -= Time.DeltaTime;
            if (initialCooldown <= 0)
            {
                initialCooldownCompleted = true;
            }
            return;
        }

        if (canAttack)
        {
            attackTimer += Time.DeltaTime;
        }
        else
        {
            pauseTimer += Time.DeltaTime;
        }

        if (isTeleportOnCooldown)
        {
            teleportCooldownTimer += Time.DeltaTime;

            if (teleportCooldownTimer >= 1.25f)
            {
                if (!effectDuringCooldown)
                {
                    Effect(Position);
                    effectDuringCooldown = true;
                }
            }

            if (teleportCooldownTimer >= teleportCooldownDuration)
            {
                isTeleportOnCooldown = false;
                teleportCooldownTimer = 0;
                effectDuringCooldown = false;
            }
        }

        currentRepeat -= Time.DeltaTime;
    }

    public void ResetCurrent()
    {
        teleportCooldownTimer = 0;
        isTeleportOnCooldown = false;
    }

    private void Selection(Vector2 enemyPosition)
    {
        float playerX = GameManager.Instance.LevelController.player.Transform.Position.x;
        float enemyX = enemyPosition.x;
        float distanceX = Math.Abs(playerX - enemyX);

        if (attackTimer >= 1 && canAttack)
        {
            nearAttacking = distanceX < 300;

            if (nearAttacking)
            {
                Engine.Debug("ataque CERCA");
                enemyAttack = rnd.Next(1, 3);
            }
            else
            {
                Engine.Debug("ataque LEJOS");
                int[] farAttacks = { 1, 3 };
                enemyAttack = farAttacks[rnd.Next(0, farAttacks.Length)];
            }
            isAttacking = true;
            repetitionCount = 0;
            currentRepeat = 0;
        }

        if (isAttacking)
        {
            if (!nearAttacking && distanceX < 300 && repetitionCount >= 2)
            {
                enemyAttack = 2;
            }

            switch (enemyAttack)
            {
                case 1:
                    ShootAtPlayer(enemyPosition);
                    break;
                case 2:
                    TeleportAway();
                    break;
                case 3:
                    LightningBolt(enemyPosition);
                    break;
            }
        }

        if (!isAttacking && pauseTimer >= timeBetweenAttacks)
        {
            canAttack = true;
            pauseTimer = 0;
        }
    }

    // Tipos de ataques

    private void ShootAtPlayer(Vector2 position)
    {
        float playerX = GameManager.Instance.LevelController.player.Transform.Position.x;
        float enemyX = position.x;
        float distanceX = Math.Abs(playerX - enemyX);

        if (!nearAttacking && distanceX < 300 && repetitionCount >= 2)
        {
            enemyAttack = 2;
            TeleportAway();
            return;
        }

        repeatCooldown = 1f;

        int maxRepetitions = GameManager.Instance.LevelController.enemy.Health <= GameManager.Instance.LevelController.enemy.MaxHealth / 2 ? 3 : 1;

        if (repetitionCount < maxRepetitions && currentRepeat <= 0)
        {
            GameManager.Instance.LevelController.enemyBullets.Add(new EnemyBullet(position, new Vector2(-BulletWidth, EnemyHeight / 2 - BulletHeight / 2)));
            GameManager.Instance.LevelController.enemyBullets.Add(new EnemyBullet(position, new Vector2(EnemyWidth + EnemyBullet.BulletWidth - 20, EnemyHeight / 2 - BulletHeight / 2)));
            timeBetweenAttacks = 0.45f;
            canAttack = false;
            attackTimer = 0;
            currentRepeat = repeatCooldown;
            repetitionCount++;
        }
        else if (repetitionCount < maxRepetitions && currentRepeat > 0)
        {
            currentRepeat -= Time.DeltaTime;
        }
        else
        {
            repetitionCount = 0;
            isAttacking = false;
        }
    }

    private void TeleportAway()
    {
        if (canTeleport && !isTeleportOnCooldown)
        {
            enemyMovement.Teleport();
            timeBetweenAttacks = 3.0f;
            canAttack = false;
            attackTimer = 0;
            isTeleportOnCooldown = true;
            isAttacking = false;
        }
    }

    private void LightningBolt(Vector2 position)
    {
        float playerX = GameManager.Instance.LevelController.player.Transform.Position.x;
        float enemyX = position.x;
        float distanceX = Math.Abs(playerX - enemyX);

        if (!nearAttacking && distanceX < 300 && repetitionCount >= 2)
        {
            enemyAttack = 2;
            TeleportAway();
            return;
        }

        repeatCooldown = 1.25f;

        if (repetitionCount <= 2 && currentRepeat <= 0)
        {
            GameManager.Instance.LevelController.thunderattacks.Add(new EnemyThunderBubble(position, GameManager.Instance.LevelController.player.Transform.Position, new Vector2(BulletWidth / 2, EnemyHeight / 2 - BulletHeight / 2)));
            timeBetweenAttacks = 0.45f;
            canAttack = false;
            attackTimer = 0;
            currentRepeat = repeatCooldown;
            repetitionCount++;
        }
        else if (repetitionCount <= 3 && currentRepeat > 0)
        {
            currentRepeat -= Time.DeltaTime;
        }
        else
        {
            repetitionCount = 0;
            isAttacking = false;
        }
    }

    private void Effect(Vector2 position)
    {
        GameManager.Instance.LevelController.TeleportList.Add(new EnemyTeleport((int)position.x + (int)Enemy.EnemyWidth, (int)position.y, new Vector2(-1, 0), "assets/Misery/Teleport/0.png"));
        GameManager.Instance.LevelController.TeleportList.Add(new EnemyTeleport((int)position.x - (int)Enemy.EnemyWidth, (int)position.y, new Vector2(1, 0), "assets/Misery/Teleport/1.png"));
    }
}
