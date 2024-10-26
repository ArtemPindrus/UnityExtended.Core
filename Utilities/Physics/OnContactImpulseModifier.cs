using System.Collections.Generic;
using System.Linq;
using TriInspector;
using Unity.Collections;
using UnityEngine;
using UnityExtended.Core.Types;

namespace UnityExtended.Core.Utilities {
	/// <summary>
	/// Scales inverse mass and inertia when interacting with specified Rigidbodies.
	/// </summary>
	[RequireComponent(typeof(Rigidbody))]
	public class OnContactImpulseModifier : MonoBehaviour {
		private static bool attachingInstance = false;

		/// <summary>
		/// A dictionary of affected RigidBodies keyed by their IDs, contact with which will be modified.
		/// </summary>
		private ObservableDictionary<int, Rigidbody> affectedIDsToRBs;
		private Rigidbody thisRB;
		private int thisRBID;

		[Tooltip("A list of Rigidbodies interaction with which is modified.\n" +
				 "This list isn't modifiable in Inspector during Runtime.\n" +
				 "To add items during Runtime either use public " + nameof(Include) + " method through other scripts or use Runtime Helper in this script to add items through Editor's inspector.")]
		[DisableInPlayMode]
		[SerializeField]
		private List<Rigidbody> affectedRigidbodies;

		[SerializeField]
		[DisableInEditMode]
		private OnContactImpulseModifierRuntimeHelper runtimeInspectorHelper;

		[InfoBox("Remember that setting the inverse mass/inertia scales to values < 1 makes the objects appear heavier, while setting them to values > 1 makes objects appear lighter.\n" +
				 "HINT: VelocityDelta = impulse / mass OR impulse * inverseMass. (so the less the inverseMass the less the resulting VelocityDelta)")]
		[Tooltip("When interacting with Affected Rigidbodies what's the inverse mass scale of THIS body (the body with the script attached).")]
		[SerializeField]
		[Min(0)]
		private float thisRBScaler = 1f;

		[Tooltip("When interacting with Affected Rigidbodies what's the inverse mass scale of other Rigidbody (the Rigidbody this body with the script collided).")]
		[SerializeField]
		[Min(0)]
		private float othersRBScaler = 1f;

#nullable enable

		/// <summary>
		/// Attach modifier component to a gameobject. Useful when multiple modifiers use the same set of affected Rigidbodies instantiated elsewhere.
		/// <para>NOT THREAD-SAFE!</para>
		/// </summary>
		/// <param name="gameObject">Gameobject to attach component to.</param>
		/// <param name="affectedIDsToRBs">Dictionary of affected Rigidbodies keyed by their instance ID.</param>
		/// <param name="thisRBScaler"></param>
		/// <param name="othersRBScaler"></param>
		/// <param name="component">Created component. Null if failed to attach.</param>
		/// <returns>Whether succeeded attaching component.</returns>
		public static bool Attach(GameObject gameObject, ObservableDictionary<int, Rigidbody> affectedIDsToRBs, float thisRBScaler, float othersRBScaler, out OnContactImpulseModifier component) {
			if (gameObject.TryGetComponent<OnContactImpulseModifier>(out _)) {
				component = null!; // ignore. User should check for bool before accessing created component.

				return false;
			} else {
				attachingInstance = true;

				component = gameObject.AddComponent<OnContactImpulseModifier>();

				component.affectedIDsToRBs = affectedIDsToRBs;
				component.runtimeInspectorHelper = new(component, affectedIDsToRBs);
				component.thisRBScaler = thisRBScaler;
				component.othersRBScaler = othersRBScaler;

#if UNITY_EDITOR
				affectedIDsToRBs.CollectionChanged += component.SyncInspectorValues;
#endif

				component.SyncInspectorValues();

				attachingInstance = false;

				return true;
			}
		}

		/// <summary>
		/// Include a <see cref="Rigidbody"/> into a list of affected Rigidbodies.
		/// </summary>
		/// <param name="affectedRB">A <see cref="Rigidbody"/> to include.</param>
		/// <param name="include">true to include, false to remove</param>
		public void Include(Rigidbody affectedRB, bool include = true) {
			if (include) {
				affectedIDsToRBs.Add(affectedRB.GetInstanceID(), affectedRB);

#if UNITY_EDITOR
				if (!affectedRigidbodies.Contains(affectedRB)) affectedRigidbodies.Add(affectedRB);
#endif
			} else {
				affectedIDsToRBs.Remove(affectedRB.GetInstanceID());

#if UNITY_EDITOR
				affectedRigidbodies.Remove(affectedRB);
#endif
			}
		}

		/// <summary>
		/// Syncs <see cref="affectedRigidbodies"/> to actual affected Rigidbodies during Runtime in the Play mode.
		/// <para>Generates garbage. Should be used only during Editor's Play mode. It doesn't make any sense to use it in a build.</para>
		/// </summary>
		public void SyncInspectorValues() {
#if UNITY_EDITOR
			affectedRigidbodies = affectedIDsToRBs.Values.ToList();
#endif
		}

		private void Awake() {
			thisRB = GetComponent<Rigidbody>();
			thisRBID = thisRB.GetInstanceID();

			if (!attachingInstance) {
				affectedIDsToRBs ??= new();
				runtimeInspectorHelper = new(this, affectedIDsToRBs);

#if UNITY_EDITOR
				affectedIDsToRBs.CollectionChanged += SyncInspectorValues;
#endif
			}

			foreach (var collider in GetComponentsInChildren<Collider>()) {
				collider.hasModifiableContacts = true;
			}

			UnityEngine.Physics.ContactModifyEvent += OnContactModifyEvent;

			if (affectedRigidbodies != null && !attachingInstance) {
				foreach (var initiallyAffected in affectedRigidbodies) affectedIDsToRBs.Add(initiallyAffected.GetInstanceID(), initiallyAffected);
			}
		}

		private void OnContactModifyEvent(PhysicsScene scene, NativeArray<ModifiableContactPair> contactPairs) {
			for (int i = 0; i < contactPairs.Length; i++) {
				ModifiableContactPair currentContactPair = contactPairs[i];

				if (thisRBID != currentContactPair.bodyInstanceID && thisRBID != currentContactPair.otherBodyInstanceID) continue;

				int otherBodyID = thisRBID == currentContactPair.bodyInstanceID ? currentContactPair.otherBodyInstanceID : currentContactPair.bodyInstanceID;

				if (affectedIDsToRBs.ContainsKey(otherBodyID)) {
					contactPairs[i] = ModifyContactProperties(currentContactPair);
				}
			}
		}

		private ModifiableContactPair ModifyContactProperties(ModifiableContactPair contactPair) {
			ModifiableMassProperties newProperties = new();

			if (contactPair.bodyInstanceID == thisRBID) {
				newProperties.inverseMassScale = thisRBScaler;
				newProperties.inverseInertiaScale = thisRBScaler;

				newProperties.otherInverseMassScale = othersRBScaler;
				newProperties.otherInverseInertiaScale = othersRBScaler;
			} else if (contactPair.otherBodyInstanceID == thisRBID) {
				newProperties.otherInverseMassScale = thisRBScaler;
				newProperties.otherInverseInertiaScale = thisRBScaler;

				newProperties.inverseMassScale = othersRBScaler;
				newProperties.inverseInertiaScale = othersRBScaler;
			}

			contactPair.massProperties = newProperties;

			return contactPair;
		}
	}
}
