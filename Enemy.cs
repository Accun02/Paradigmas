using MyGame;
using System;
using System.Collections.Generic;

public class Enemy
{
    public const float EnemyWidth = 32;
    public const float EnemyHeight = 39;

    private int health = 100;

    private Transform transform;
    public Transform Transform

    {  get { return transform; } }


    private EnemyMovement enemyMovement;
    private Enemyattackselect enemyattackselect;

    private IntPtr image;

    private Animation Idle;
    private Animation enemyattack;
    private Animation currentAnimation;

    public int Health => health;

    public Enemy(Vector2 position)
    {
        transform = new Transform(position);
        enemyMovement = new EnemyMovement(Transform);
        enemyattackselect = new Enemyattackselect(transform.Position, enemyMovement);
        image = Engine.LoadImage("assets/Misery/Idle/0.png");
        CreateAnimations();
        currentAnimation = Idle;
    }

    public void Update(float deltaTime)
    {
        enemyattackselect.Update(transform.Position);

        if (enemyattackselect.EnemyAttack == 1)
        {
            currentAnimation = enemyattack;
            currentAnimation.Update();
        }
        else
        {
            currentAnimation = Idle;
            currentAnimation.Update();
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
        Idle = new Animation("Idle", idle, 0.4f, true);


        List<IntPtr> Attack = new List<IntPtr>();
        for (int i = 0; i < 3; i++)
        {
            IntPtr frame = Engine.LoadImage($"assets/Misery/Bullet/{i}.png");
            Attack.Add(frame);
        }
        enemyattack = new Animation("enemyattack", Attack, 0.025f, false);
    }
}
