using System;
using UnityEngine;

namespace UnityExtended.Core.Extensions {
    public static class ColliderExtensions {
        public static Vector3 GetWorldPosition(this Collider collider) {
            return collider switch {
                BoxCollider box => collider.transform.position + box.center,
                SphereCollider sphere => sphere.transform.position + sphere.center,
                CapsuleCollider capsule => capsule.transform.position + capsule.center,
                MeshCollider mesh => mesh.transform.position,
                _ => throw new NotImplementedException()
            };
        }
    }
}