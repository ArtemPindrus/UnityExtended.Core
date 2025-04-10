using System;
using System.Collections.Generic;
using UnityEngine;
using UnityExtended.Core.Generators.Attributes;

namespace UnityExtended.Core.Types {
    [RequireComponent(typeof(Rigidbody))]
    [Collect]
    public partial class RigidbodyWrapper : MonoBehaviour {
        public Rigidbody Rigidbody { get; private set; }
        
        private void Awake() {
            Rigidbody = GetComponent<Rigidbody>();
        }

        public override int GetHashCode() => Rigidbody.GetHashCode();
    }
}