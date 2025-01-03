﻿using System;
using UnityEngine;

namespace UnityExtended.Core.Extensions {
    /// <summary>
    /// Provides additional functionality to the <see cref="Vector3"/> class.
    /// </summary>
    public static class VectorExtensions {
        public static float Max(this Vector3 v) {
            float max = Single.MinValue;

            if (v.x > max) max = v.x;
            if (v.y > max) max = v.y;
            if (v.z > max) max = v.z;

            return max;
        }
        
        public static float Max(this Vector3 v, bool includeX, bool includeY, bool includeZ) {
            float max = Single.MinValue;

            if (includeX && v.x > max) max = v.x;
            if (includeY && v.y > max) max = v.y;
            if (includeZ && v.z > max) max = v.z;

            return max;
        }
        
        /// <summary>
        /// Modifies a <see cref="Vector3"/> to have given components. 
        /// If a component is assigned a null - it'll get ignored.
        /// </summary>
        /// <param name="v">Modified <see cref="Vector3"/>.</param>
        /// <param name="x">New value of the x component.</param>
        /// <param name="y">New value of the y component.</param>
        /// <param name="z">New value of the z component.</param>
        /// <returns>Modified <see cref="Vector3"/>.</returns>
        public static Vector3 With(this Vector3 v, float? x = null, float? y = null, float? z = null) {
            Vector3 result = v;

            if (x != null) result.x = x.Value;
            if (z != null) result.z = z.Value;
            if (y != null) result.y = y.Value;

            return result;
        }

        public static Vector2 With(this Vector2 v, float? x = null, float? y = null) {
            Vector3 result = v;

            if (x != null) result.x = x.Value;
            if (y != null) result.y = y.Value;

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

        /// <summary>
        /// Logs vector with accurate components values.
        /// <para>Normally Debug.Log() logs vector with components truncated to nearest hundredth. 
        /// Meaning that a vector with components {x = 1.00012, y = 0.01001, z = 10.10203} 
        /// will be logged as (1.00, 0.01, 10.10) which is annoying when you need accurate values.</para>
        /// <para>Logs message in "<paramref name="prefix"/>: (<paramref name="v"/>.x, <paramref name="v"/>.y, <paramref name="v"/>.z)" format.</para>
        /// </summary>
        /// <param name="v"><see cref="Vector3"/> to log.</param>
        /// <param name="prefix">Prefix before logged values.</param>
        public static void LogAccurate(this Vector3 v, string prefix) {
            Debug.Log($"{prefix}: ({v.x}, {v.y}, {v.z})");
        }

        /// <summary>
        /// Divides <see cref="Vector3"/> components component-wise.
        /// </summary>
        /// <param name="dividend"><see cref="Vector3"/>, components of which is getting divided.</param>
        /// <param name="divisor"><see cref="Vector3"/> that the other is divided by.</param>
        /// <returns><paramref name="dividend"/> with components divided by <paramref name="divisor"/> respectful components.</returns>
        public static Vector3 Divide(this Vector3 dividend, Vector3 divisor) {
            return new Vector3(dividend.x / divisor.x, dividend.y / divisor.y, dividend.z / divisor.z);
        }

        /// <summary>
        /// Aligns Vector3 to be perpendicular to the given reference.
        /// </summary>
        /// <param name="v">Aligned Vector3.</param>
        /// <param name="reference">Reference to which the returned Vector3 will be perpendicular.</param>
        /// <returns>Aligned <see cref="v"/> that is perpendicular to <see cref="reference"/>.</returns>
        public static Vector3 Perpendicularize(this Vector3 v, Vector3 reference) {
            Vector3 cross = Vector3.Cross(reference, v);
            Quaternion rotation = Quaternion.AngleAxis(90, cross);

            Vector3 aligned = rotation * reference;
            return aligned;
        }

        /// <summary>
        /// Rotate <see cref="Vector3"/> around an axis for an angle.
        /// </summary>
        /// <param name="v">Rotated vector.</param>
        /// <param name="axis">Axis of rotation.</param>
        /// <param name="angle">Angle in degrees.</param>
        /// <returns><paramref name="v"/> rotated around <paramref name="axis"/> for <paramref name="angle"/> degrees.</returns>
        public static Vector3 RotateAround(this Vector3 v, Vector3 axis, float angle) {
            Quaternion rotation = Quaternion.AngleAxis(angle, axis);

            return rotation * v;
        }
    }
}
