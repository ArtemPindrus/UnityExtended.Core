using System;
using UnityEngine.InputSystem;

namespace UnityExtended.Generators.Attributes {
    [AttributeUsage(AttributeTargets.Class)]
    public class HandleInputAttribute : Attribute {
        public HandleInputAttribute(Type actionAssetType) {
        }
    }
}