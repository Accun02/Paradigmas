using System;
using System.Collections.Generic;

namespace MyGame
{
    public class AudioMixer
    {
        private int lightingHitChannel = 0;
        private int shootChannel = 1;
        private int walkChannel = 2;
        private int jumpChannel = 3;
        private int hitChannel = 4;
        private int anvilFallChannel = 5;
        private int anvilHitChannel = 6;
        private int landChannel = 7;
        private int teleportChannel = 8;

        public int HitEnemyChannel = 9;
        public int UIChannel = 10;
        public int MusicChannel = 11;
        public int UIClickChannel = 12;
        public int UIDifficulty = 13;

        public int LightingHitChannel
        {
            get { return lightingHitChannel; }
            set { lightingHitChannel = value; }
        }

        public int ShootChannel
        {
            get { return shootChannel; }
            set { shootChannel = value; }
        }

        public int WalkChannel
        {
            get { return walkChannel; }
            set { walkChannel = value; }
        }

        public int JumpChannel
        {
            get { return jumpChannel; }
            set { jumpChannel = value; }
        }

        public int HitChannel
        {
            get { return hitChannel; }
            set { hitChannel = value; }
        }

        public int AnvilFallChannel
        {
            get { return anvilFallChannel; }
            set { anvilFallChannel = value; }
        }

        public int AnvilHitChannel
        {
            get { return anvilHitChannel; }
            set { anvilHitChannel = value; }
        }

        public int LandChannel
        {
            get { return landChannel; }
            set { landChannel = value; }
        }

        public int TeleportChannel
        {
            get { return teleportChannel; }
            set { teleportChannel = value; }
        }
    }
}
