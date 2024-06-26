using Microsoft.VisualStudio.TestTools.UnitTesting;
using MyGame;
using System;

namespace UnitTests
{
    [TestClass]
    public class CharacterTests
    {
        [TestMethod]
        public void TakeDamageIfVulnerable()
        {
            var initialPosition = new Vector2(0, 0);
            var character = new Character(initialPosition);
            character.Health = 3;

            character.TakeDamage(1);

            Assert.AreEqual(2, character.Health);
        }

        [TestMethod]
        public void TakeDamageIfNotVulnerable()
        {
            var initialPosition = new Vector2(0, 0);
            var character = new Character(initialPosition);
            character.Health = 3;
            character.Vulnerable = false;

            character.TakeDamage(1);

            Assert.AreEqual(3, character.Health);
        }
    }
}
