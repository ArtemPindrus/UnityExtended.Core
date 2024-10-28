using NUnit.Framework;
using System;
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
    [TestCase(-7, -6, -3, -4)]
    [TestCase(16, 10, 15, 11)]
    public void RangeOverflowExclusive_ShouldReturnValidValues(float value, float min, float max, float expectedResult) {
        float result = MathExtensions.RangeOverflowExclusive(value, min, max);

        Assert.That(result, Is.EqualTo(expectedResult));
    }

    [TestCase(10, 15)]
    [TestCase(-10, 20)]
    [TestCase(-10, -5)]
    [TestCase(1000, 2000)]
    public void RangeOverflowExclusive_ThrowsOnSwappedMinAndMax(float min, float max) {
        Assert.Throws(typeof(Exception), () => MathExtensions.RangeOverflowExclusive(0, max, min));
    }

    [TestCase(-10)]
    [TestCase(0)]
    [TestCase(10)]
    public void RangeOverflowExclusive_ThrowsWhenMinMaxEqual(float value) {
        Assert.Throws(typeof(Exception), () => MathExtensions.RangeOverflowExclusive(0, value, value));
    }

    [TestCase(180, 180)]
    [TestCase(-180, -180)]
    [TestCase(-181, 179)]
    [TestCase(181, -179)]
    [TestCase(360, 0)]
    [TestCase(721, 1)]
    public void Overflow180(float value, float expected) {
        float result = MathExtensions.Overflow180(value);

        Assert.That(result, Is.EqualTo(expected));
    }
}
