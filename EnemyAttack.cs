﻿using MyGame;
using System;
using System.Diagnostics;

public class EnemyAttack
{
    private Transform transform;
    private int enemyAttack;
    public int EnemyAttackSelect { set { enemyAttack = value; } get { return enemyAttack; } }
    private Random rnd;
    private EnemyMovement enemyMovement;

    private const string imagePath = "assets/evilBullet.png";

    private float attackTimer = 0;
    private float pauseTimer = 0;
    private float timeBetweenAttacks = 0.25f;

    private bool canAttack = true;
    private bool isAttacking = false;

    private bool effectPlayedDuringCooldown = false;
    private bool canTeleport = true;

    private float EnemyWidth = Enemy.EnemyWidth;
    private float EnemyHeight = Enemy.EnemyHeight;
    private float BulletWidth = EnemyBullet.BulletWidth;
    private float BulletHeight = EnemyBullet.BulletHeight;

    private float teleportCooldownTimer = 0;
    private float teleportCooldownDuration = 2.15f;
    private bool isTeleportOnCooldown = false;

    public bool IsTeleportOnCooldown { set { isTeleportOnCooldown = value; } get { return isTeleportOnCooldown; } }
    public bool IsAttacking { set { isAttacking = value; } get { return isAttacking; } }
    public float AttackTimer { set { attackTimer = value; } get { return attackTimer; } }

    public EnemyAttack(Vector2 position, EnemyMovement enemyMovement)
    {
        transform = new Transform(position);
        this.enemyMovement = enemyMovement;
        rnd = new Random();
    }
    public void Update(Vector2 Position)
    {
        timers(Position);
        selection(Position);
    }
    private void timers(Vector2 Position)
    {
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

            if (teleportCooldownTimer >= 1.5)
            {
                if (!effectPlayedDuringCooldown)
                Effect(Position);
                effectPlayedDuringCooldown = true;
            }

            if (teleportCooldownTimer >= teleportCooldownDuration)
            {
                isTeleportOnCooldown = false;
                teleportCooldownTimer = 0;
                effectPlayedDuringCooldown = false; 
            }
        }
    }

    public void ResetCurrent()
    {
        teleportCooldownTimer = 0;
        teleportCooldownDuration = 2.15f;
        isTeleportOnCooldown = false;
    }

    private void selection(Vector2 position)
    {
        if (attackTimer >= 1 && canAttack)
        {
            enemyAttack = rnd.Next(1, 3);
            isAttacking = true;
        }
        if (isAttacking)
        {
            switch (enemyAttack)
            {
                case 1:
                    Program.enemyBullets.Add(new EnemyBullet(position, Program.player.transform.Position, new Vector2(-BulletWidth, EnemyHeight / 2 - BulletHeight / 2), imagePath));
                    Program.enemyBullets.Add(new EnemyBullet(position, Program.player.transform.Position, new Vector2(EnemyWidth + EnemyBullet.BulletWidth, EnemyHeight / 2 - BulletHeight / 2), imagePath));
                    timeBetweenAttacks = 0.45f;
                    canAttack = false;
                    attackTimer = 0;
                    break;
                case 2:
                    if (canTeleport && !isTeleportOnCooldown)
                    {
                        enemyMovement.Teleport();
                        timeBetweenAttacks = 3.0f;
                        canAttack = false;
                        attackTimer = 0;
                        isTeleportOnCooldown = true;
                    }
                    break;
            }
            isAttacking = false;
        }

        if (pauseTimer >= timeBetweenAttacks)
        {
            canAttack = true;
            pauseTimer = 0;
        }
    }

    private void Effect(Vector2 position)
    {
        Program.TeleportList.Add(new Teleport((int)position.x + (int)Enemy.EnemyWidth, (int)position.y, new Vector2(-1, 0), "assets/Misery/Teleport/0.png"));
        Program.TeleportList.Add(new Teleport((int)position.x - (int)Enemy.EnemyWidth, (int)position.y, new Vector2(1, 0), "assets/Misery/Teleport/1.png"));
    }
}