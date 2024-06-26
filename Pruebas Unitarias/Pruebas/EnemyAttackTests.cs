using Microsoft.VisualStudio.TestTools.UnitTesting;
using MyGame;
using System;

namespace UnitTests
{
    [TestClass]
    public class EnemyAttackTests
    {
        private EnemyAttack enemyAttack;

        [TestInitialize]
        public void Initialize()
        {
            var enemyMovement = new EnemyMovement(new Transform(new Vector2(100, 200)));
            enemyAttack = new EnemyAttack(enemyMovement);
        }

        [TestMethod]
        public void EnemyAttackSelectionNearPlayer()
        {
            Vector2 enemyPosition = new Vector2(150, 250);
            enemyAttack.AttackTimer = 1.5f;
            enemyAttack.IsTeleportOnCooldown = false;

            enemyAttack.Update(enemyPosition);

            Assert.IsTrue(enemyAttack.IsAttacking);
            Assert.IsTrue(enemyAttack.AttackNumber >= 2 && enemyAttack.AttackNumber <= 4);
        }

        [TestMethod]
        public void EnemyAttackSelectionFarFromPlayer()
        {
            Vector2 enemyPosition = new Vector2(500, 250);
            enemyAttack.AttackTimer = 1.5f;
            enemyAttack.IsTeleportOnCooldown = false;

            enemyAttack.Update(enemyPosition);

            Assert.IsTrue(enemyAttack.IsAttacking);
            Assert.IsTrue(enemyAttack.AttackNumber >= 3 && enemyAttack.AttackNumber <= 5);
        }
    }
}
