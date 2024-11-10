using System;
using EditorAttributes;
using UnityEngine;
using UnityExtended.Core.Extensions;

namespace UnityExtended.Core.Types
{
    [Serializable]
    public class RangeFloat {
        [SerializeField]
        private float value, lowerLimit, upperLimit;

        public float LowerLimit {
            get => lowerLimit;
            private set => lowerLimit = value;
        }

        public float UpperLimit {
            get => upperLimit;
            private set => upperLimit = value;
        }

        public float Value {
            get => value;
            set => this.value = MathExtensions.RangeOverflowExclusive(value, LowerLimit, UpperLimit);
        }

        public RangeFloat(float lowerLimit, float upperLimit, float initialValue) {
            LowerLimit = lowerLimit;
            UpperLimit = upperLimit;
            Value = initialValue;
        }

        public void SetLimits(float lower, float upper) {
            if (lower > upper) {
                throw new ArgumentException($"{nameof(lower)} should be less than {nameof(upper)}.");
            } else if (lower == upper) throw new Exception($"{nameof(lower)} and {nameof(upper)} shouldn't be equal.");
            
            LowerLimit = lower;
            UpperLimit = upper;

            Value = value;
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
    }
}