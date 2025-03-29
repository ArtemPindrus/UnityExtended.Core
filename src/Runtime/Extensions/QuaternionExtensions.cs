using UnityEngine;

namespace UnityExtended.Core.Extensions {
    public static class QuaternionExtensions {
        public static Quaternion Scale(this Quaternion q, float scalar) {
            q.ToAngleAxis(out float angle, out Vector3 axis);

            angle *= scalar;

            return Quaternion.AngleAxis(angle, axis);
        }
    }
}
