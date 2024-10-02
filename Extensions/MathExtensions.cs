using System;

namespace UnityExtended.Core.Extensions {
    public static class MathExtensions {
        /// <summary>
        /// Just like Math.Sign() but returns 0 if the given value is zero.
        /// </summary>
        /// <param name="number"></param>
        /// <returns>1 if <paramref name="number"/> is greater then 0, -1 if it's less then 0 or 0 if it's 0</returns>
        /// <exception cref="ArgumentException"><paramref name="number"/> is NaN.</exception>
        public static float SignZero(this float number) => number switch {
            0 => 0,
            > 0 => 1,
            < 0 => -1,
            _ => throw new ArgumentException("Sign for the given number is not defined!", nameof(number)),
        };

        /// <summary>
        /// Just like Math.Sign() but returns 0 if the given value is zero.
        /// </summary>
        /// <param name="number"></param>
        /// <returns>1 if <paramref name="number"/> is greater then 0, -1 if it's less then 0 or 0 if it's 0</returns>
        public static int SignZero(this int number) => number switch {
            0 => 0,
            > 0 => 1,
            < 0 => -1,
        };

        /// <summary>
        /// Keeps value in [-180; 180] range, simulating overflow.
        /// <para>For example, for (value: 181, min: -180, max: 180) method will return -179 (value overflows max into the min by one).</para>
        /// <para>Used for keeping the value returned from eulerAngles.(Component) in [-180; 180] range.</para>
        /// </summary>
        /// <param name="value">Value to put in range.</param>
        /// <returns><see cref="float"/> in [-180; 180] range.</returns>
        public static float Overflow180(this float value) {
            return RangeOverflowExclusive(value, -180, 180);
        }

        /// <summary>
        /// Simulates overflow of a value within a given range.
        /// <para>For example, for (value: 181, min: -180, max: 180) method will return -179 (value overflows max into the min by one).</para>
        /// </summary>
        /// <param name="value">Value to put in range.</param>
        /// <param name="min">Min value of the range. Should be the least number in the range.</param>
        /// <param name="max">Max value of the range. Should be the greatest number in the range.</param>
        /// <returns>Overflowed <paramref name="value"/>.</returns>
        /// <exception cref="Exception">Passed arguments for <paramref name="min"/> and <paramref name="max"/> are swapped or equal.</exception>
        public static float RangeOverflowExclusive(float value, float min, float max) {
            float range = max - min;

            if (range <= 0) throw new Exception("Invalid range. Ensure the order of min and max arguments and that they aren't equal.");

            float clamped = value;

            if (value > max) {
                float overflow = value - max;

                if (overflow < range) {
                    clamped = min + overflow;
                } else {
                    float modulus = overflow % range;

                    if (modulus == 0) clamped = max;
                    else clamped = min + modulus;
                }
            } else if (value < min) {
                float overflow = value - min;

                if (overflow < range) {
                    clamped = max + overflow;
                } else {
                    float modulus = range % overflow;

                    if (modulus == 0) clamped = min;
                    else clamped = max + modulus;
                }
            }

            return clamped;
        }
    }
}
