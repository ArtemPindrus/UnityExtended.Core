#nullable enable
using System;
using UnityEngine;
using UnityEngine.UIElements;
using Object = System.Object;

namespace UnityExtended.Core.Extensions {
    public static class VisualElementExtensions {
        /// <summary>
        /// Add a range of VisualElements at the end.
        /// </summary>
        /// <param name="visualElement">VisualElement to call add on.</param>
        /// <param name="children">All the added VisualElements.</param>
        public static void Add(this VisualElement visualElement, params VisualElement[] children) {
            foreach (var child in children) {
                visualElement.Add(child);
            }
        }
    }
}