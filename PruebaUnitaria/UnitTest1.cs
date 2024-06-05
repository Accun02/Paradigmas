using Microsoft.VisualStudio.TestTools.UnitTesting;
using MyGame;
using System;

namespace PruebasUnitaria
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void TestMethod1()
        {
            var position = new Vector2(0,0);
                
            var transform = new Transform(position);

            var direction = new Vector2(1,0);
            int speed = 10;

            transform.Translate(direction, speed);

            var realResult = transform.Position;
            var expectedResult = new Vector2(position.x + direction.x * speed, position.y + direction.y * speed);

            Assert.AreEqual(realResult, expectedResult);
        }
    }
}
