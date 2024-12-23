using UnityEngine;

namespace UnityExtended.Core.Extensions {
    public static class JointsExtensions {
        public static Vector3 GetAnchorWorldPosition(this HingeJoint joint) => joint.transform.TransformPoint(joint.anchor);
    }
}