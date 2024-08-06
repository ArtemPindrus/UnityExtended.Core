using UnityEngine;

namespace UnityExtended.Extensions {
    /// <summary>
    /// Provides additional functionality to the <see cref="Vector3"/> class.
    /// </summary>
    public static class VectorExtensions {
        /// <summary>
        /// Modifies a <see cref="Vector3"/> to have given components. 
        /// If a component is assigned a NaN - it'll get ignored.
        /// </summary>
        /// <param name="v">Modified <see cref="Vector3"/>.</param>
        /// <param name="x">New value of the x component.</param>
        /// <param name="y">New value of the y component.</param>
        /// <param name="z">New value of the z component.</param>
        /// <returns>Modified <see cref="Vector3"/>.</returns>
        public static Vector3 With(this Vector3 v, float x = float.NaN, float y = float.NaN, float z = float.NaN) {
            Vector3 result = v;
            
            if (float.IsNaN(x)) result.x = x;
            if (float.IsNaN(y)) result.y = y;
            if (float.IsNaN(z)) result.z = z;

            return result;
        }

        /// <summary>
        /// Adds given values to corresponding components of a <see cref="Vector3"/>.
        /// </summary>
        /// <param name="v">Modified <see cref="Vector3"/>.</param>
        /// <param name="x">Value added to the x component.</param>
        /// <param name="y">Value added to the y component.</param>
        /// <param name="z">Value added to the z component.</param>
        /// <returns>Modified <see cref="Vector3"/>.</returns>
        public static Vector3 Add(this Vector3 v, float x = 0, float y = 0, float z = 0) {
            Vector3 result = v;
            result.x += x;
            result.y += y;
            result.z += z;

            return result;
        }

        /// <summary>
        /// Adds a value to all the components of a <see cref="Vector3"/>.
        /// </summary>
        /// <param name="v">Modified <see cref="Vector3"/>.</param>
        /// <param name="value">Value added to every component of <paramref name="v"/>.</param>
        /// <returns></returns>
        public static Vector3 Add(this Vector3 v, float value) {
            Vector3 result = v;
            result.x += value;
            result.y += value;
            result.z += value;

            return result;
        }

        /// <summary>
        /// Projects a vector onto <paramref name="projectionVector"/> with ability to clamp it to both ends.
        /// </summary>
        /// <param name="modifiedVector">Modified <see cref="Vector3"/>.</param>
        /// <param name="projectionVector"><see cref="Vector3"/> on which to project.</param>
        /// <param name="clampTail">If the projection results in a vector of the opposite direction to the <paramref name="projectionVector"/>, whether to return zero vector.</param>
        /// <param name="clampHead">if the projection results in a vector longer then the <paramref name="projectionVector"/>, whether to clamp resulting vector to the length of <paramref name="projectionVector"/>.</param>
        /// <returns></returns>
        public static Vector3 ProjectClamped(this Vector3 modifiedVector, Vector3 projectionVector, bool clampTail = true, bool clampHead = true) {
            float length = projectionVector.magnitude;
            Vector3 direction = projectionVector.normalized;

            float dot = Vector3.Dot(modifiedVector, direction);
            float scalar = dot;

            if (clampTail && dot < 0) scalar = 0;
            else if (clampHead && dot > length) scalar = length;

            return direction * scalar;
        }
    }
}
