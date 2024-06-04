using System.Collections.Generic;

namespace MyGame
{
    public class DynamicPool
    {
        private List<Bullet> bulletsInUse = new List<Bullet>();
        private List<Bullet> bulletsAvailable = new List<Bullet>();

        public Bullet GetBullet(Vector2 vecty, Vector2 Vecto, string image, bool horizontalCheck)
        {
            Bullet newBullet = null;

            if (bulletsAvailable.Count > 0)
            {
                newBullet = bulletsAvailable[0];
                bulletsAvailable.RemoveAt(0);
                newBullet.Initialize((int)vecty.x, (int)vecty.y, Vecto, image, horizontalCheck);
            }
            else
            {
                newBullet = new Bullet((int)vecty.x, (int)vecty.y, Vecto, image, horizontalCheck);
                newBullet.OnDestroy += RecycleBullet;
            }

            bulletsInUse.Add(newBullet);
            //Debug();
            return newBullet;
        }

        public void RecycleBullet(Bullet bullet)
        {
            if (bulletsInUse.Contains(bullet))
            {
                bulletsInUse.Remove(bullet);
                bulletsAvailable.Add(bullet);
            }
        }

        private void Debug()
        {
            Engine.Debug("Balas disponibles:" + bulletsAvailable.Count);
            Engine.Debug("Balas en uso:" + bulletsInUse.Count);
        }
    }
}