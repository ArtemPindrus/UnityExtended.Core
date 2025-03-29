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
                _ => throw new NotSupportedException()
            };
        }

        public static void DrawCollider(this Collider collider, float sizeMultiplier) {
            Vector3 worldPos = collider.GetWorldPosition();
            
            if (collider is BoxCollider box) Gizmos.DrawCube(worldPos, box.bounds.size * sizeMultiplier);
            else if (collider is SphereCollider sphere) 
                Gizmos.DrawSphere(worldPos, sphere.radius * sphere.transform.localScale.Max() * sizeMultiplier);
            else if (collider is CapsuleCollider capsule) {
                float halfHeight = capsule.height * capsule.transform.localScale.y / 2;
                float radius = capsule.radius * capsule.transform.localScale.Max(true, false, true) * sizeMultiplier;
                    
                // upper sphere
                Vector3 upperPos = worldPos.Add(y: halfHeight - radius);
                Gizmos.DrawSphere(upperPos, radius);
                    
                // lower
                Vector3 lowerPos = worldPos.Add(y: -halfHeight + radius);
                Gizmos.DrawSphere(lowerPos, radius);
            }
            else if (collider is MeshCollider mesh) {
                Gizmos.DrawMesh(mesh.sharedMesh, mesh.transform.position);
            }
            else throw new NotSupportedException();
        }
    }
}