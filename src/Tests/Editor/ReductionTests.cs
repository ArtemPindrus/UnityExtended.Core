using NUnit.Framework;
using UnityExtended.Core.Types;

namespace UnityExtended.Core.Tests {
    public class ReductionTests {
        [TestCase(100, 50, 0.5f, 0.25f, 0.25f)]
        [TestCase(75, 37.5f, 0.5f, 0.25f, 0.25f)]
        [TestCase(20, 10, 0.5f, 0.5f)]
        [TestCase(20, 0, 1, 0.1f, 0.2f, 0.5f, 0.15f, 0.05f)]
        [TestCase(36, 0, 1, 1)]
        [TestCase(75, 0, 1, 25)]
        [TestCase(36, 27, 0.25f, 0.12f, 0.13f)]
        public void ShouldCalculateValidValue(float value, float expectedReduced, float expectedPercentage, params float[] reductionPercentages) {
            Reduction reduction = new();

            foreach(var redPerc in reductionPercentages) reduction.AddReducer(redPerc);
            Assert.That(reduction.SumReductionPercentage, Is.EqualTo(expectedPercentage));

            float result = reduction.Reduce(value);

            Assert.That(result, Is.EqualTo(expectedReduced));
        }
    }
}
