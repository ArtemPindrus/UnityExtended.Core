using System;
using UnityEngine;

namespace UnityExtended.Core.Types {
    [Serializable]
    public class ReducibleFloat {
        private Reduction reduction;

        [field: SerializeField]
        public float ValueUnchanged { get; private set; }

        [field: SerializeField]
        public float MinimumValue { get; private set; }

        /// <summary>
        /// Gets reduced value.
        /// </summary>
        public float Value {
            get {
                if (reduction == null) return ValueUnchanged;
                else {
                    float reduced = reduction.Reduce(ValueUnchanged);

                    return reduced < MinimumValue ? MinimumValue : reduced; 
                }
            }
        }

        public ReducibleFloat(float initialValue, float minimumValue) {
            reduction = new();
            ValueUnchanged = initialValue;
            MinimumValue = minimumValue;
        }

        /// <summary>
        /// Implicitly convert into float by returning from the <see cref="Value"/>.
        /// </summary>
        /// <param name="reducibleFloat"></param>
        public static implicit operator float(ReducibleFloat reducibleFloat) => reducibleFloat.Value;

        public Reducer AddReducer(float reductionPercentage) {
            reduction ??= new();

            return reduction.AddReducer(reductionPercentage);
        }

        public void RemoveReducer(Reducer reducer) {
            if (reduction == null) return;

            reduction.RemoveReducer(reducer);
        }
    }
}
