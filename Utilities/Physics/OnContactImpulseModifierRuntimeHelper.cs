using EditorAttributes;
using System;
using UnityEngine;
using UnityExtended.Core.Types;

namespace UnityExtended.Core.Utilities {
#nullable enable
    /// <summary>
    /// Helps adding/removing items from <see cref="OnContactImpulseModifier"/> during Runtime through Editor Inspector.
    /// </summary>
    [Serializable]
    public class OnContactImpulseModifierRuntimeHelper {
        private readonly OnContactImpulseModifier modifier;
        private readonly ObservableDictionary<int, Rigidbody> affectedIDs;

        [Tooltip("Rigidbody to add or remove.")]
        [SerializeField]
        private Rigidbody? rb;

        public OnContactImpulseModifierRuntimeHelper(OnContactImpulseModifier modifier, ObservableDictionary<int, Rigidbody> affectedIDs) {
            this.modifier = modifier;
            this.affectedIDs = affectedIDs;
        }

        [HelpBox("Use Add/Remove Buttons to add/remove Rigidbody above (Rb) to the list of affected Rigidbodies.")]
        private EditorAttributes.Void helpBox;

        [Button]
        private void Add() {
            if (rb == null) return;

            modifier.Include(rb);
            rb = null;
        }

        [Button(nameof(Remove))]
        private void Remove() {
            if (rb == null) return;

            modifier.Include(rb, false);
            rb = null;
        }

        [Button("Print IDs of affected Rigidbodies")]
        private void PrintAffectedIDs() {
            if (!Application.isPlaying) {
                Debug.LogError($"Can print affected ID only when runtime is up/when in play mode.");
                return;
            }

            Debug.Log($"Affected Rigidbodies IDs for {modifier.name}:");

            foreach (var id in affectedIDs.Keys) Debug.Log(id);
        }
    }
}