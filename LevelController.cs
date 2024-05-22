using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tao.Sdl;

namespace MyGame
{
    public  class LevelController
    {
        public int GroundHeight = 584;        // De arriba a abajo
        public int ScreenWidth = 1280;       // De izquierda a derecha

        public Character player;
        public Enemy enemy;


        public List<Bullet> BulletList;
        public List<Teleport> TeleportList;
        public List<EnemyBullet> enemyBullets;
        public List<ThunderAttack> thunderattacks;

        public void Initialize() 
        {
            player = new Character(new Vector2(ScreenWidth / 2 - Character.PlayerWidth / 2, 584 - Character.PlayerHeight));
            enemy   = new Enemy(new Vector2(ScreenWidth / 2 - Enemy.EnemyWidth / 2, 100));
            BulletList = new List<Bullet>();
            TeleportList = new List<Teleport>();
            enemyBullets = new List<EnemyBullet>();
            thunderattacks = new List<ThunderAttack>();
        }

        public void Render()
        {
            Engine.Clear();

            if (player.Health > 0)
                BackgroundManager.Render();

            player.Render(player);

            if (player.Health > 0)
            {
                enemy.Render();

                foreach (Bullet bullet in BulletList)
                {
                    bullet.Render();
                }
                foreach (EnemyBullet enemyBullet in enemyBullets)
                {
                    enemyBullet.Render();
                }
                foreach (Teleport teleport in TeleportList)
                {
                    teleport.Render();
                }
                foreach (ThunderAttack thunderattack in thunderattacks)
                {
                    thunderattack.Render();
                }

                FontManager.Render(enemy);

                //BackgroundManager.Fade(); //Reduce demasiado el performance.
            }
            Engine.Show();
        }

        public void Update() 
        {
            player.Update(player);

            if (player.Health > 0)
            {
                enemy.Update(Time.DeltaTime);
                enemy.CheckCollision(player);

                foreach (Bullet bullet in BulletList.ToList())
                {
                    bullet.Update();
                    bullet.CheckCollisions(enemy);
                }

                foreach (EnemyBullet enemyBullet in enemyBullets.ToList())
                {
                    enemyBullet.Update();
                    enemyBullet.CheckCollisions(player);
                }

                foreach (Teleport teleport in TeleportList.ToList())
                {
                    teleport.Update();
                }

                foreach (ThunderAttack thunderattack in thunderattacks)
                {
                    thunderattack.Update();
                    thunderattack.CheckPositions(player);
                }
            }
        }

        public void Restart()
        {
            //Clear Scene
            TeleportList.Clear();
            BulletList.Clear();
            enemyBullets.Clear();

            //Reset Player
            player.transform.Position = new Vector2(ScreenWidth / 2 - Character.PlayerWidth / 2, 584 - Character.PlayerHeight);
            player.ResetMomentum();

            player.Health = player.MaxHealth;
            player.IsDead = false;

            //Reset Enemy
            enemy.ResetTransform(new Vector2(ScreenWidth / 2 - Enemy.EnemyWidth / 2, 100));
            enemy.ResetAttacks();
            enemy.Health = enemy.MaxHealth;

            //Reset Input
            GameManager.Instance.ZKeyReleased = false;
        }
    }
}
