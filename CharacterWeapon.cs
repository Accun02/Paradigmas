namespace MyGame
{
    public class CharacterWeapon
    {
        private DynamicPool pool;

        public CharacterWeapon()
        {
            pool = new DynamicPool();
        }

        public void ShootVertical()
        {
            Bullet newBullet = pool.GetBullet(new Vector2((int)GameManager.Instance.LevelController.player.Transform.Position.x + ((int)Character.PlayerWidth / 2) - 10 / 2, (int)GameManager.Instance.LevelController.player.Transform.Position.y - 60 / 2), new Vector2(0, -1), "assets/bullet/bulletY.png", false);
            GameManager.Instance.LevelController.BulletList.Add(newBullet);
        }

        public void ShootHorizontal(bool isLookingRight, bool isLookingLeft)
        {
            if (isLookingRight)
            {
                Bullet newBullet = pool.GetBullet(new Vector2((int)GameManager.Instance.LevelController.player.Transform.Position.x + ((int)Character.PlayerWidth / 2) + 0 / 2, (int)GameManager.Instance.LevelController.player.Transform.Position.y + 16 + (int)Character.PlayerHeight / 2), new Vector2(1, 0), "assets/bullet/bulletX.png", true);
                GameManager.Instance.LevelController.BulletList.Add(newBullet);
            }
            else if (isLookingLeft)
            {
                Bullet newBullet = pool.GetBullet(new Vector2((int)GameManager.Instance.LevelController.player.Transform.Position.x + ((int)Character.PlayerWidth / 2) - 60, (int)GameManager.Instance.LevelController.player.Transform.Position.y + 16 + (int)Character.PlayerHeight / 2), new Vector2(-1, 0), "assets/bullet/bulletX.png", true);
                GameManager.Instance.LevelController.BulletList.Add(newBullet);
            }
        }
    }
}
