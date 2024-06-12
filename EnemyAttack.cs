using MyGame;
using System;
using System.Collections.Generic;

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

    private bool dashAttack = false;

    private float initialWait = 1.0f;
    private bool initialWaitDone = false;

    private int repetitionCount = 0;
    private bool nearAttacking = false;
    private bool playerIsDown = false;

    private Dictionary<int, string> attackMethods;

    public bool DashAttacking { set { dashAttack = value; } get { return dashAttack; } }
    public int AttackNumber { set { enemyAttack = value; } get { return enemyAttack; } }
    public bool IsTeleportOnCooldown { set { isTeleportOnCooldown = value; } get { return isTeleportOnCooldown; } }
    public bool IsAttacking { set { isAttacking = value; } get { return isAttacking; } }
    public float AttackTimer { set { attackTimer = value; } get { return attackTimer; } }

    public EnemyAttack(EnemyMovement enemyMovement)
    {
        this.enemyMovement = enemyMovement;
        rnd = new Random();

        attackMethods = new Dictionary<int, string>
        {
            { 1, "FallingAnvil" },
            { 2, "TeleportAway" },
            { 3, "DashAttack" },
            { 4, "Shooting" },
            { 5, "Lightning" }
        };
    }

    // Lógica de selector de ataques
    public void Update(Vector2 Position)
    {
        Timers(Position);
        if (initialWaitDone && !dashAttack)
        {
            Selection(Position);
        }
    }

    private void Timers(Vector2 Position)
    {
        if (!initialWaitDone)
        {
            initialWait -= Time.DeltaTime;
            if (initialWait <= 0)
            {
                initialWaitDone = true;
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
                    TeleportEffect(Position);
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

    private void Selection(Vector2 enemyPosition)
    {
        float playerX = GameManager.Instance.LevelController.player.Transform.Position.x;
        float enemyX = enemyPosition.x;
        float distanceX = Math.Abs(playerX - enemyX);

        if (attackTimer >= 1 && canAttack)
        {
            nearAttacking = distanceX < 300;
            playerIsDown = distanceX < 30;

            if (nearAttacking && !playerIsDown)
            {
                enemyAttack = rnd.Next(2, 4 + 1);
                Engine.Debug($"CERCA - {attackMethods[enemyAttack]} ({enemyAttack})");
            }
            else if (!nearAttacking && !playerIsDown)
            {
                enemyAttack = rnd.Next(3, 5 + 1);
                Engine.Debug($"LEJOS - {attackMethods[enemyAttack]} ({enemyAttack})");
            }
            else
            {
                enemyAttack = rnd.Next(1, 2 + 1);
                Engine.Debug($"ABAJO - {attackMethods[enemyAttack]} ({enemyAttack})");
            }
            isAttacking = true;
            repetitionCount = 0;
            currentRepeat = 0;
        }

        if (isAttacking)
        {
            switch (enemyAttack)
            {
                case 1:
                    FallingAnvil(enemyPosition);
                    break;
                case 2:
                    TeleportAway();
                    break;
                case 3:
                    DashAttack();
                    break;
                case 4:
                    ShootMagic(enemyPosition);
                    break;
                case 5:
                    Lightning(enemyPosition);
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
    private void ShootMagic(Vector2 position)
    {
        float playerX = GameManager.Instance.LevelController.player.Transform.Position.x;
        float enemyX = position.x;
        float distanceX = Math.Abs(playerX - enemyX);

        if (!nearAttacking && distanceX < 300 && repetitionCount >= 2)
        {
            canAttack = true;
            pauseTimer = 0;
            enemyAttack = 2;
            TeleportAway();
            return;
        }

        repeatCooldown = 1f;

        int maxRepetitions = GameManager.Instance.LevelController.enemy.Health <= GameManager.Instance.LevelController.enemy.MaxHealth / 2 ? 3 : 1;

        if (repetitionCount < maxRepetitions && currentRepeat <= 0)
        {
            GameManager.Instance.LevelController.EnemyBulletList.Add(new EnemyBullet(position, new Vector2(-BulletWidth, EnemyHeight / 2 - BulletHeight / 2)));
            GameManager.Instance.LevelController.EnemyBulletList.Add(new EnemyBullet(position, new Vector2(EnemyWidth + EnemyBullet.BulletWidth - 20, EnemyHeight / 2 - BulletHeight / 2)));
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

    private void DashAttack()
    {
        dashAttack = true;

        timeBetweenAttacks = 1f;
        canAttack = false;
        attackTimer = 0;
        isAttacking = false;
    }

    private void TeleportAway()
    {
        if (canTeleport && !isTeleportOnCooldown)
        {
            enemyMovement.Teleport();
            timeBetweenAttacks = 1.5f;
            canAttack = false;
            attackTimer = 0;
            isTeleportOnCooldown = true;
            isAttacking = false;
        }
    }

    private void Lightning(Vector2 position)
    {
        float playerX = GameManager.Instance.LevelController.player.Transform.Position.x;
        float enemyX = position.x;
        float distanceX = Math.Abs(playerX - enemyX);

        if (!nearAttacking && distanceX < 300 && repetitionCount >= 2)
        {
            canAttack = true;
            pauseTimer = 0;
            enemyAttack = 2;
            TeleportAway();
            return;
        }

        repeatCooldown = 1.25f;

        if (repetitionCount <= 2 && currentRepeat <= 0)
        {
            GameManager.Instance.LevelController.ThunderList.Add(new EnemyThunderBubble(position, GameManager.Instance.LevelController.player.Transform.Position, new Vector2(BulletWidth / 2, EnemyHeight / 2 - BulletHeight / 2)));
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

    private void FallingAnvil(Vector2 position)
    {
        float anvilX = position.x - EnemyWidth / 2 - BulletWidth / 2;
        float anvilY = -BulletHeight - 70;
        GameManager.Instance.LevelController.AnvilList.Add(new EnemyAnvil(new Vector2(anvilX, anvilY), new Vector2(0, 0)));
        timeBetweenAttacks = 0.3f;
        canAttack = false;
        attackTimer = 0;
        repetitionCount = 0;
        isAttacking = false;
    }

    // Otro
    private void TeleportEffect(Vector2 position)
    {
        GameManager.Instance.LevelController.TeleportList.Add(new EnemyTeleport((int)position.x + (int)Enemy.EnemyWidth, (int)position.y, new Vector2(-1, 0), "assets/enemy/teleport/0.png"));
        GameManager.Instance.LevelController.TeleportList.Add(new EnemyTeleport((int)position.x - (int)Enemy.EnemyWidth, (int)position.y, new Vector2(1, 0), "assets/enemy/teleport/1.png"));
    }

    public void ResetCurrent()
    {
        teleportCooldownTimer = 0;
        isTeleportOnCooldown = false;
    }
}
