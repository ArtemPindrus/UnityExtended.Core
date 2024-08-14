using System;
using UnityEngine;

namespace UnityExtended.Extensions {
    public static class MathExtended {
        /// <summary>
        /// Just like Math.Sign() but returns 0 if the given value is zero.
        /// </summary>
        /// <param name="number"></param>
        /// <returns>1 if <paramref name="number"/> > 0, -1 if it's < 0 or 0 if it's 0</returns>
        /// <exception cref="ArgumentException"><paramref name="number"/> is NaN.</exception>
        public static float SignZero(this float number) => number switch {
            0 => 0,
            > 0 => 1,
            < 0 => -1,
            _ => throw new ArgumentException("Sign for the given number is not defined!"),
        };

        /// <summary>
        /// Keeps value in [-180; 180] range, simulating overflow.
        /// <para>For example, for (value: 181, min: -180, max: 180) method will return -179 (value overflows max into the min by one).</para>
        /// <para>Used for keeping the value returned from eulerAngles.(Component) in [-180; 180] range.</para>
        /// </summary>
        /// <param name="value">Value to put in range.</param>
        /// <returns><see cref="float"/> in [-180; 180] range.</returns>
        public static float Overflow180(this float value) {
            return RangeOverflow(value, -180, 180);
        }

        /// <summary>
        /// Simulates overflow of a value within a given range.
        /// <para>For example, for (value: 181, min: -180, max: 180) method will return -179 (value overflows max into the min by one).</para>
        /// </summary>
        /// <param name="value">Value to put in range.</param>
        /// <param name="min">Min value of the range. Should be the least number in the range.</param>
        /// <param name="max">Max value of the range. Should be the greatest number in the range.</param>
        /// <returns>Overflowed <paramref name="value"/>.</returns>
        public static float RangeOverflow(float value, float min, float max) { // value 721, min -180, max 180
            float clamped = value;

            if (value > max) {
                float overflow = value - max;

                clamped = min + overflow;
            } else if (value < min) {
                float overflow = value - min;

                clamped = max + overflow;
            }

            bool overflowed = clamped < min || clamped > max;

            return overflowed ? RangeOverflow(clamped, min, max) : clamped;
        }
    }
}
