using System;
using UnityEditor;
using UnityEngine;
using UnityEngine.Serialization;
using UnityExtended.Core.Extensions;

namespace UnityExtended.Core.External.UnityExtended.Core.Utilities.Physics {
    /// <summary>
    /// Enforces Unity to ignore collisions between colliders (including child ones) of a GameObject with this script
    /// and other specified colliders on Awake.
    /// </summary>
    public class IgnoreCollisionsWith : MonoBehaviour {
        [SerializeField]
        [Min(1)]
        private float sizeMultiplier = 1;
        [SerializeField] private bool showIgnoredColliders = true;
        [SerializeField] private Collider[] otherColliders;
        
        private Collider[] myColliders;

        private void Awake() {
            myColliders = GetComponentsInChildren<Collider>();

            foreach (var myCollider in myColliders) {
                foreach (var other in otherColliders) {
                    UnityEngine.Physics.IgnoreCollision(myCollider, other);
                }
            }
            
            // dispose of
            otherColliders = null;
            myColliders = null;
            Destroy(this);
        }

        private void OnDrawGizmosSelected() {
            if(!showIgnoredColliders) return;
            
            Gizmos.color = Color.red;
            
            foreach (var other in otherColliders) {
                Vector3 worldPos = other.GetWorldPosition();
                
                // TODO: move drawing out?
                if (other is BoxCollider box) Gizmos.DrawCube(worldPos, box.bounds.size * sizeMultiplier);
                else if (other is SphereCollider sphere) 
                    Gizmos.DrawSphere(worldPos, sphere.radius * sphere.transform.localScale.Max() * sizeMultiplier);
                else if (other is CapsuleCollider capsule) {
                    float halfHeight = capsule.height * capsule.transform.localScale.y / 2;
                    float radius = capsule.radius * capsule.transform.localScale.Max(true, false, true) * sizeMultiplier;
                    
                    // upper sphere
                    Vector3 upperPos = worldPos.Add(y: halfHeight - radius);
                    Gizmos.DrawSphere(upperPos, radius);
                    
                    // lower
                    Vector3 lowerPos = worldPos.Add(y: -halfHeight + radius);
                    Gizmos.DrawSphere(lowerPos, radius);
                }
                else if (other is MeshCollider mesh) {
                    Gizmos.DrawMesh(mesh.sharedMesh, mesh.transform.position);
                }
                else throw new NotImplementedException();
            }
        }
    }
}