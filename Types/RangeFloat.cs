using System;
using UnityEngine;
using UnityExtended.Core.Extensions;

namespace UnityExtended.Core.Types
{
    [Serializable]
    public class RangeFloat {
        [SerializeField]
        private float value;
        
        [field: SerializeField]
        public float LowerLimit { get; private set; }
        
        [field: SerializeField]
        public float UpperLimit { get; private set; }

        public float Value {
            get => value;
            set => this.value = MathExtensions.RangeOverflowExclusive(value, LowerLimit, UpperLimit);
        }

        public RangeFloat(float lowerLimit, float upperLimit, float initialValue) {
            LowerLimit = lowerLimit;
            UpperLimit = upperLimit;
            Value = initialValue;
        }

        public static RangeFloat operator +(RangeFloat rf, float f) {
            rf.Value += f;
            return rf;
        }

        public static RangeFloat operator -(RangeFloat rf, float f) {
            rf.Value -= f;
            return rf;
        }
        
        public static implicit operator float(RangeFloat rangeFloat) => rangeFloat.Value;

        private void OnValidate() {
            // reevaluate
            Value = value;
        }
    }
}