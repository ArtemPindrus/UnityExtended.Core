using System;
using System.Linq;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityExtended.Core.Extensions;

namespace UnityExtended.Core.Utilities.Triggering {
    [RequireComponent(typeof(Rigidbody))]
    [RequireComponent(typeof(Collider))]
    public abstract class OneTimeTrigger : MonoBehaviour {
        private bool triggered;

        [SerializeField] private bool supportStaticColliders;
    
        [Tooltip("Colliders that can trigger this.")]
        [SerializeField] private Collider[] targets;

#nullable enable
        public Action? OnTriggered;

        private void OnValidate() {
            foreach (var coll in GetComponentsInChildren<Collider>()) {
                coll.isTrigger = true;
            }

            if (supportStaticColliders) {
                Rigidbody rb = gameObject.GetOrAddComponent<Rigidbody>();
                rb.isKinematic = true;
                rb.hideFlags = HideFlags.NotEditable;
            }
            else {
                Rigidbody rb = GetComponent<Rigidbody>();

                if (rb != null && rb.hideFlags == HideFlags.NotEditable) {
                    rb.hideFlags = HideFlags.None;
                    Debug.LogWarning($"{name} is not supporting static colliders, but contains Rigidbody!");
                }
            }
        }

        private async void OnTriggerEnter(Collider other) {
            if(triggered || !targets.Contains(other)) return;

            triggered = true;
            await React();
            OnTriggered?.Invoke();
            Destroy(gameObject);
        }

        protected abstract UniTask React();
    }
}