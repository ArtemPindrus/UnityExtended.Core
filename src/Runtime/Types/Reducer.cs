using UnityEngine;

namespace UnityExtended.Core.Types {
    public class Reducer {
        private float reductionPercentage;

        /// <summary>
        /// Initializes a new instance of the <see cref="Reducer"/> class.
        /// </summary>
        /// <param name="percentageOfReduction">Initial percentage of sensitivity reduction.</param>
        public Reducer(float percentageOfReduction) {
            ReductionPercentage = percentageOfReduction;
        }

        /// <summary>
        /// Gets or sets percentage of sensitivity to reduce it by. A value in [0;1] range.
        /// </summary>
        public float ReductionPercentage {
            get => reductionPercentage;
            set => reductionPercentage = Mathf.Clamp01(value);
        }

        public override string ToString() => $"Reducer with {reductionPercentage} value.";
    }
}