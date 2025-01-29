using System;
using System.Collections.Generic;
using UnityEngine;
using UnityExtended.Generator.Attributes;

namespace UnityExtended.Core.Types {
    [RequireComponent(typeof(Rigidbody))]
    [Collect]
    public partial class RigidbodyWrapper : MonoBehaviour {
        public Rigidbody Rigidbody { get; private set; }
    
        partial void PreAwake() {
            Rigidbody = GetComponent<Rigidbody>();
        }

        public override int GetHashCode() => Rigidbody.GetInstanceID();
    }
}