using System;
using UnityEngine;

namespace UnityExtended.Core.External.UnityExtended.Core.Utilities.Physics {
    /// <summary>
    /// Enforces Unity to ignore collisions between colliders (including child ones) of a GameObject with this script
    /// and other specified colliders on Awake.
    /// </summary>
    public class IgnoreCollisionsWith : MonoBehaviour {
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
    }
}