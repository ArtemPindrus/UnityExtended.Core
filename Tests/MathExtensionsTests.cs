using NUnit.Framework;
using UnityEngine.TestTools;
using UnityExtended.Core.Extensions;

public class MathExtensionsTests {
    [TestCase(60, -180, 180, 60)]
    [TestCase(180, -180, 180, 180)]
    [TestCase(181, -180, 180, -179)]
    [TestCase(360, -180, 180, 0)]
    [TestCase(-360, -180, 180, 0)]
    [TestCase(2, -1, 1, 0)]
    [TestCase(6, -3, 3, 0)]
    [TestCase(9, -3, 3, 3)]
    [TestCase(10, -5, 3, 2)]
    [TestCase(-8, -5, 3, 0)]
    [TestCase(11, -4, 3, -3)]
    [TestCase(-10.5f, -10.5f, 10, -10.5f)]
    [TestCase(10.5f, -10.5f, 10, -10f)]
    [TestCase(20f, -10.5f, 10, -0.5f)]
    public void RangeOverflowExclusive_ShouldReturnValidValues(float value, float min, float max, float expectedResult) {
        float result = MathExtensions.RangeOverflowExclusive(value, min, max);

        Assert.That(result, Is.EqualTo(expectedResult));
    }
}
