using Microsoft.VisualStudio.TestTools.UnitTesting;
using MyGame;
using System;

namespace UnitTests
{
    [TestClass]
    public class TransformTests
    {
        [TestMethod]
        public void TranslateTest()
        {
            var initialPosition = new Vector2(0, 0);
            var transform = new Transform(initialPosition);

            var direction = new Vector2(1, 0);
            int speed = 10;

            transform.Translate(direction, speed);

            var actualPosition = transform.Position;
            var expectedPosition = new Vector2(initialPosition.x + direction.x * speed, initialPosition.y + direction.y * speed);

            Assert.AreEqual(expectedPosition, actualPosition);
        }
    }
}
