using MyGame;
using System;

public class Enemy
{
    private Transform transform;
    private EnemyMovement enemyMovement;
    private Enemyattackselect enemyattackselect;
    private int health;
    private IntPtr image = Engine.LoadImage("assets/untitled.png");

    public int Health => health;

    public Enemy(Vector2 position)
    {
        transform = new Transform(position);
        enemyMovement = new EnemyMovement(transform);
        enemyattackselect = new Enemyattackselect(transform.Position, enemyMovement);
    }

    public void Update(float deltaTime)
    {
        enemyattackselect.Update();
    }

    public void Render()
    {
        Engine.Draw(image, transform.Position.x, transform.Position.y);
    }

    public void Attack()
    {
        enemyattackselect.Update();
    }
}
