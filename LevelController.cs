using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyGame
{
    public  class LevelController
    {
        public int GroundHeight = 584;        // De arriba a abajo
        public int ScreenWidth = 1280;       // De izquierda a derecha
        float delayFrame = 60f;             // FPS
        public bool targetFrame = false;

        public Character player;
        public Enemy enemy;

         public List<Bullet> BulletList = new List<Bullet>();
         public List<Teleport> TeleportList = new List<Teleport>();
         public List<EnemyBullet> enemyBullets = new List<EnemyBullet>();

         Stopwatch stopwatch = new Stopwatch();


        public void Initialize() 
        {
            player = new Character(new Vector2(ScreenWidth / 2 - Character.PlayerWidth / 2, 584 - Character.PlayerHeight));
            enemy   = new Enemy(new Vector2(ScreenWidth / 2 - Enemy.EnemyWidth / 2, 100));
        }
    }
}
