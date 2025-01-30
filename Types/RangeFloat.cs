using System;
using EditorAttributes;
using JetBrains.Annotations;
using UnityEngine;
using UnityExtended.Core.Extensions;

namespace UnityExtended.Core.Types {
    /// <summary>
    /// Float with upper and lower limit that uses <see cref="MathExtensions.RangeOverflowExclusive"/> on overflows.
    /// </summary>
    public struct RangeFloat {
        private float value;

        public float LowerLimit { get; private set; }

        public float UpperLimit { get; private set; }

        public float Value {
            get => value;
            set => this.value = MathExtensions.RangeOverflowExclusive(value, LowerLimit, UpperLimit);
        }

        public RangeFloat(float lowerLimit, float upperLimit, float initialValue) {
            ThrowIfLimitsAreInvalid(lowerLimit, upperLimit);

            LowerLimit = lowerLimit;
            UpperLimit = upperLimit;
            value = 0;

            Value = initialValue;
        }

        /// <summary>
        /// Set limits.
        /// </summary>
        /// <param name="lower">Lower limit. Must be less then <paramref name="upper"/>.</param>
        /// <param name="upper">Upper limit. Must be greater then <paramref name="lower"/>.</param>
        public void SetLimits(float lower, float upper) {
            ThrowIfLimitsAreInvalid(lower, upper);

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

        public static implicit operator float(RangeFloat rangeFloat) {
            return rangeFloat.Value;
        }

        private static void ThrowIfLimitsAreInvalid(float lower, float upper) {
            if (lower > upper) {
                throw new ArgumentException($"{nameof(lower)} should be less than {nameof(upper)}.");
            }
            else if (lower == upper) throw new ArgumentException($"{nameof(lower)} and {nameof(upper)} shouldn't be equal.");
        }
    }
}