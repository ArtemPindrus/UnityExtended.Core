using System.Collections.Generic;
using Unity.Collections;
using UnityEngine;
using System.Linq;

namespace UnityExtended.Utilities.RigidbodyU {
    /// <summary>
    /// Components that makes a <see cref="Rigidbody"/> ignore action forces from specified Rigidbodies.
    /// </summary>
    [RequireComponent(typeof(Rigidbody))]
    public class SelectiveKinematics : MonoBehaviour {
        [Tooltip(@"Specific rigidbodies that are initially ignored.
Adding or removing entries from this array during runtime has no effect. Use public Ignore method to add or remove entries.
Doesn't necessary reflect currently ignored Rigidbodies during runtime.")]
        [SerializeField]
        private Rigidbody[] initiallyIgnored;

        private List<int> ignoredIDs;

        private Rigidbody thisRB;
        private int thisRBID;

        /// <summary>
        /// Instruct Unity to ignore action force from a specified <see cref="Rigidbody"/>.
        /// </summary>
        /// <param name="ignoredRB"><see cref="Rigidbody"/> to ignore.</param>
        /// <param name="ignore">Whether to add (true) or remove (false) <paramref name="ignoredRB"/> from ignored.</param>
        public void Ignore(Rigidbody ignoredRB, bool ignore = true) {
            if (ignore) ignoredIDs.Add(ignoredRB.GetInstanceID());
            else ignoredIDs.Remove(ignoredRB.GetInstanceID());
        }

        private void Awake() {
            thisRB = GetComponent<Rigidbody>();
            thisRBID = thisRB.GetInstanceID();
            ignoredIDs = new();

            foreach (var collider in GetComponentsInChildren<Collider>()) {
                collider.hasModifiableContacts = true;
            }

            Physics.ContactModifyEvent += IgnoreInvalidForces;

            ignoredIDs.AddRange(initiallyIgnored.Select(x => x.GetHashCode()));
        }

        /// <summary>
        /// Ignores action forces from objects with ID present in <see cref="ignoredIDs"/>.
        /// </summary>
        /// <param name="scene"></param>
        /// <param name="contactPairs"></param>
        private void IgnoreInvalidForces(PhysicsScene scene, NativeArray<ModifiableContactPair> contactPairs) {
            for (int i = 0; i < contactPairs.Length; i++) {
                ModifiableContactPair contactPair = contactPairs[i];

                if (ignoredIDs.Contains(contactPair.bodyInstanceID) || ignoredIDs.Contains(contactPair.otherBodyInstanceID)) {
                    ModifiableMassProperties newProperties = new();

                    // Set inverse mass scale of ignoring Rigidbody to 0 and ignored to 1.
                    // Results in impulse applied to the ignoring to be 0. The other RB will gain original impulse.
                    if (contactPair.bodyInstanceID == thisRBID) newProperties.otherInverseMassScale = 1;
                    else newProperties.inverseMassScale = 1;

                    contactPair.massProperties = newProperties;

                    contactPairs[i] = contactPair;
                }
            }
        }
    }
}