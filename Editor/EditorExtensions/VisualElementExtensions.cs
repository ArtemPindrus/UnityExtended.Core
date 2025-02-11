using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

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

        /// <summary>
        /// Adds all <see cref="SerializedProperty"/>(s) of <paramref name="serializedObject"/> to <paramref name="container"/>.
        /// </summary>
        /// <param name="container"><see cref="VisualElement"/> that gets new children.</param>
        /// <param name="serializedObject"><see cref="SerializedObject"/> SerializedProperties of which are added.</param>
        public static void AddAllSerializedProperties(this VisualElement container, SerializedObject serializedObject) {
            foreach (var serializedProperty in serializedObject.GetAllSerializedProperties()) {
                var propertyField = new PropertyField(serializedProperty) {
                    enabledSelf = serializedProperty.editable
                };
                
                if (serializedProperty.name == "m_Script") {
                    propertyField.enabledSelf = false;
                    container.Insert(0, propertyField);
                }
                else {
                    container.Add(propertyField);
                }
            }
        }
    }
}