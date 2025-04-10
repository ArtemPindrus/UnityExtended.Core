using System;
using UnityEngine.InputSystem;

namespace UnityExtended.Core.Generators.Attributes {
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
    public class HandleInputAttribute : Attribute {
        public HandleInputAttribute(Type actionAssetType) {
        }
    }
}