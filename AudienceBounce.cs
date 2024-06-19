using System;

namespace MyGame
{
    public class AudienceBounce
    {
        private IntPtr leftAudience = Engine.LoadImage("assets/leftAudience.png");
        private IntPtr rightAudience = Engine.LoadImage("assets/rightAudience.png");

        private int leftX;
        private int rightX;

        private float spriteY;
        private float velocityY;

        private float gravity;
        private float groundHeight;

        private bool isRising;

        private float jumpSpeed = 150f;

        public AudienceBounce(int leftX, int rightX, float groundHeight)
        {
            this.leftX = leftX;
            this.rightX = rightX;
            spriteY = groundHeight + 15;
            velocityY = 0f;
            gravity = 1200f;
            this.groundHeight = groundHeight - 20;
            isRising = false;
        }

        public void Update()
        {
            if (!isRising)
            {
                velocityY += gravity * Time.DeltaTime;
            }

            spriteY += velocityY * Time.DeltaTime;

            if (spriteY >= groundHeight)
            {
                spriteY = groundHeight;

                velocityY = -velocityY * 0.6f;

                if (Math.Abs(velocityY) < 50)
                {
                    velocityY = 0;
                }

                if (CameraShake.Instance.value > 3.5 && !CameraShake.Instance.ThunderShake && GameManager.Instance.LevelController.player.Health > 1
                    || GameManager.Instance.LevelController.enemy.Health <= 25 && GameManager.Instance.LevelController.player.Health == GameManager.Instance.LevelController.player.MaxHealthNormal && !GameManager.Instance.HardMode
                    || GameManager.Instance.LevelController.enemy.Health <= 25 && GameManager.Instance.LevelController.player.Health == GameManager.Instance.LevelController.player.MaxHealthHard && GameManager.Instance.HardMode
                    || !GameManager.Instance.LevelController.player.Vulnerable)
                {
                    velocityY = -jumpSpeed;
                    isRising = true;
                }
            }
            else
            {
                isRising = false;
            }
        }

        public void Render()
        {
            Engine.Draw(leftAudience, leftX + CameraShake.Instance.value, (int)spriteY);
            Engine.Draw(rightAudience, rightX + CameraShake.Instance.value, (int)spriteY - 20);
        }
    }
}
