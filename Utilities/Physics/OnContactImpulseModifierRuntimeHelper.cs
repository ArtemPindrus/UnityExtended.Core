﻿using System;
using System.Collections.Generic;
using TriInspector;
using UnityEngine;

namespace UnityExtended.Core.Utilities {
#nullable enable
    /// <summary>
    /// Helps adding/removing items from <see cref="OnContactImpulseModifier"/> during Runtime through Editor Inspector.
    /// </summary>
    [Serializable]
    public class OnContactImpulseModifierRuntimeHelper {
        private readonly OnContactImpulseModifier modifier;
        private readonly Dictionary<int, Rigidbody> affectedIDs;

        [Tooltip("Rigidbody to add or remove.")]
        [SerializeField]
        private Rigidbody? rb;

        public OnContactImpulseModifierRuntimeHelper(OnContactImpulseModifier modifier, Dictionary<int, Rigidbody> affectedIDs) {
            this.modifier = modifier;
            this.affectedIDs = affectedIDs;
        }

        [InfoBox("Use Add/Remove Buttons to add/remove Rigidbody above (Rb) to the list of affected Rigidbodies.")]

        [Button(nameof(Add))]
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

        [Button(nameof(PrintAffectedIDs))]
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