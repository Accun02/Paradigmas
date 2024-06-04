using System.Collections.Generic;

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
        public List<EnemyBullet> EnemyBulletList;
        public List<EnemyThunderBubble> ThunderList;
        public List<EnemyLightningBolt> LightningList;
        public List<EnemyAnvil> AnvilList;

        public void Initialize()
        {
            player = new Character(new Vector2(0, 0));
            enemy = new Enemy(new Vector2(0, 0));
            BulletList = new List<Bullet>();
            TeleportList = new List<EnemyTeleport>();
            EnemyBulletList = new List<EnemyBullet>();
            ThunderList = new List<EnemyThunderBubble>();
            LightningList = new List<EnemyLightningBolt>();
            AnvilList = new List<EnemyAnvil>();
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
                for (int i = 0; i < EnemyBulletList.Count; i++)
                {
                    EnemyBulletList[i].Render();
                }
                for (int i = 0; i < TeleportList.Count; i++)
                {
                    TeleportList[i].Render();
                }
                for (int i = 0; i < ThunderList.Count; i++)
                {
                    ThunderList[i].Render();
                }
                for (int i = 0; i < LightningList.Count; i++)
                {
                    LightningList[i].Render();
                }
                for (int i = 0; i < AnvilList.Count; i++)
                {
                    AnvilList[i].Render();
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
                enemy.Update();
                enemy.CheckCollision(player);

                for (int i = 0; i < BulletList.Count; i++)
                {
                    BulletList[i].Update();
                    BulletList[i].CheckCollisions(enemy);
                }

                for (int i = 0; i < EnemyBulletList.Count; i++)
                {
                    EnemyBulletList[i].Update();
                    EnemyBulletList[i].CheckPositions(player);
                }

                for (int i = 0; i < TeleportList.Count; i++)
                {
                    TeleportList[i].Update();
                }

                for (int i = 0; i < ThunderList.Count; i++)
                {
                    ThunderList[i].Update();
                    ThunderList[i].CheckPositions(player);
                }

                for (int i = 0; i < LightningList.Count; i++)
                {
                    LightningList[i].Update();
                    LightningList[i].CheckPositions(player);
                }
                for (int i = 0; i < AnvilList.Count; i++)
                {
                    AnvilList[i].Update();
                    AnvilList[i].CheckPositions(player);
                }
            }
        }

        public void Restart()
        {
            // Clear Scene
            TeleportList.Clear();
            BulletList.Clear();
            EnemyBulletList.Clear();
            ThunderList.Clear();
            LightningList.Clear();
            AnvilList.Clear();

            // Reset Player
            player.Transform.Position = new Vector2(ScreenWidth / 4 - Character.PlayerWidth / 2, GroundHeight - Character.PlayerHeight);
            player.ResetMomentum();
            player.Health = player.MaxHealth;
            player.IsDead = false;

            // Reset Enemy
            enemy.ResetTransform(new Vector2((ScreenWidth / 4) * 3 - Enemy.EnemyWidth / 2, 250));
            enemy.ResetEnemy();
            enemy.Health = enemy.MaxHealth;
        }
    }
}
