using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine.UIElements;
using UnityExtended.Core.Extensions;

namespace UnityExtended.Core.EditorTools.Extensions {
    public static class VisualElementExtensions {
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