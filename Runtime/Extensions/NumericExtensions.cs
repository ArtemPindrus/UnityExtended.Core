using System;
using UnityEngine;

namespace UnityExtended.Extensions {
    public static class NumericExtensions {
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
    }
}
