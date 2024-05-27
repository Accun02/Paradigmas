using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tao.Sdl;

namespace MyGame
{
    public class LevelController
    {
        public int GroundHeight = 574;        // De arriba a abajo
        public int ScreenWidth = 1280;       // De izquierda a derecha

        public Character player;
        public Enemy enemy;


        public List<Bullet> BulletList;
        public List<EnemyTeleport> TeleportList;
        public List<EnemyBullet> enemyBullets;
        public List<EnemyThunderBubble> thunderattacks;

        public void Initialize()
        {
            player = new Character(new Vector2(ScreenWidth / 2 - Character.PlayerWidth / 2, GroundHeight - Character.PlayerHeight));
            enemy = new Enemy(new Vector2(ScreenWidth / 2 - Enemy.EnemyWidth / 2, 100));
            BulletList = new List<Bullet>();
            TeleportList = new List<EnemyTeleport>();
            enemyBullets = new List<EnemyBullet>();
            thunderattacks = new List<EnemyThunderBubble>();
        }

        public void Render()
        {
            Engine.Clear();

            if (player.Health > 0)
                BackgroundManager.Render();

            player.Render();

            if (player.Health > 0)
            {
                enemy.Render();

                for (int i = 0; i < BulletList.Count; i++)
                {
                    BulletList[i].Render();
                }
                for (int i = 0; i < enemyBullets.Count; i++)
                {
                    enemyBullets[i].Render();
                }
                for (int i = 0; i < TeleportList.Count; i++)
                {
                    TeleportList[i].Render();
                }
                for (int i = 0; i < thunderattacks.Count; i++)
                {
                    thunderattacks[i].Render();
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

                for (int i = 0; i < BulletList.Count; i++)
                {
                    BulletList[i].Update();
                    BulletList[i].CheckCollisions(enemy);
                }

                for (int i = 0; i < enemyBullets.Count; i++)
                {
                    enemyBullets[i].Update();
                    enemyBullets[i].CheckCollisions(player);
                }

                for (int i = 0; i < TeleportList.Count; i++)
                {
                    TeleportList[i].Update();
                }

                for (int i = 0; i < thunderattacks.Count; i++)
                {
                    thunderattacks[i].Update();
                    thunderattacks[i].CheckPositions(player);
                }
            }
        }

        public void Restart()
        {
            //Clear Scene
            TeleportList.Clear();
            BulletList.Clear();
            enemyBullets.Clear();
            thunderattacks.Clear();

            //Reset Player
            player.Transform.Position = new Vector2(ScreenWidth / 4 - Character.PlayerWidth / 2, GroundHeight - Character.PlayerHeight);
            player.ResetMomentum();

            player.Health = player.MaxHealth;
            player.IsDead = false;

            //Reset Enemy
            enemy.ResetTransform(new Vector2((ScreenWidth / 4) * 3 - Enemy.EnemyWidth / 2, 250));
            enemy.ResetAttacks();
            enemy.Health = enemy.MaxHealth;

            //Reset Input
            GameManager.Instance.ZKeyReleased = false;
        }
    }
}
