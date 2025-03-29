using System;
using UnityEngine.UIElements;

namespace UnityExtended.Core.Attributes {
    /// <summary>
    /// Forces <see cref="VisualElement"/>(s) to be rearranged within an inspector root VisualElement.
    /// </summary>
    [AttributeUsage(AttributeTargets.Field, AllowMultiple = false)]
    public class SetVisualElementAtAttribute : Attribute {
        public readonly int Order;

        /// <summary>
        /// Creates <see cref="SetVisualElementAtAttribute"/> instance.
        /// </summary>
        /// <param name="order">Order of decorated field's <see cref="VisualElement"/> within its inspector's root VisualElement.</param>
        public SetVisualElementAtAttribute(int order) {
            Order = order;
        }
    }
}