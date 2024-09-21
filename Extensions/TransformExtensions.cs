using UnityEngine;

namespace UnityExtended.Core.Extensions {
    /// <summary>
    /// Extends <see cref="Transform"/>.
    /// </summary>
    public static class TransformExtensions {
        /// <summary>
        /// Positions transform in the way that the <paramref name="localPosition"/> is set at the <paramref name="newPosition"/> in world space.
        /// </summary>
        /// <param name="transform"><see cref="Transform"/> to position.</param>
        /// <param name="localPosition">Local point to position in the world space.</param>
        /// <param name="newPosition">World position of the local point.</param>
        public static void SetPositionOf(this Transform transform, Vector3 localPosition, Vector3 newPosition) { 
            transform.position = newPosition - localPosition;
        }
    }
}
