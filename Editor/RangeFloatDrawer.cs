using System.Reflection;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;
using UnityExtended.Core.Types;

namespace UnityExtended.Core.EditorTools {
    [CustomPropertyDrawer(typeof(RangeFloat))]
    public class RangeFloatDrawer : PropertyDrawer {
        public override VisualElement CreatePropertyGUI(SerializedProperty property) {
            VisualElement container = new();

            var lowerLimitProperty = property.FindPropertyRelative("lowerLimit");
            var upperLimitProperty = property.FindPropertyRelative("upperLimit");
            
            var valueField = new PropertyField(property.FindPropertyRelative("value"));
            var lowerLimitField = new PropertyField(lowerLimitProperty);
            var upperLimitField = new PropertyField(upperLimitProperty);

            var function = typeof(RangeFloat).GetMethod("OnValueChanged", BindingFlags.Instance | BindingFlags.NonPublic);
            var field = property.serializedObject.targetObject.GetType().GetField(property.name, BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance);
            var instance = field.GetValue(property.serializedObject.targetObject);
            
#warning doesn't work if nested custom Serializable types are used
            valueField.RegisterValueChangeCallback((_) => function.Invoke(instance, null));
            lowerLimitField.RegisterValueChangeCallback((_) => function.Invoke(instance, null));
            upperLimitField.RegisterValueChangeCallback((_) => function.Invoke(instance, null));
            
            container.Add(valueField);
            container.Add(lowerLimitField);
            container.Add(upperLimitField);
            
            return container;
        }
    }
}