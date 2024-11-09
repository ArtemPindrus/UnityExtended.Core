using NUnit.Framework;
using UnityExtended.Core.Types;

namespace UnityExtended.Core.Tests {
    public class RangeFloatTests {
        [TestCase(-10, 10, 5, 5, 10)]
        [TestCase(-10, 10, 10, 5, -5)]
        [TestCase(-1, 1, 1, 2, 1)]
        public void AdditionYieldsExpectedValues(float min, float max, float initialValue, float added, float expected) {
            RangeFloat rf = new(min, max, initialValue);
            
            rf += added;

            Assert.That(rf.Value, Is.EqualTo(expected));
        }
    }
}