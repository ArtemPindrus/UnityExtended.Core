using System;
using System.Reflection;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;
using UnityExtended.Core.Generators.Attributes;
using Object = UnityEngine.Object;

#nullable enable
namespace UnityExtended.Core.Editor.EditorExtensions {
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
        
        public static object? GetValue(this VisualElement baseField) => baseField switch {
            TextField textField => textField.value,
            IntegerField integerField => integerField.value,
            UnsignedIntegerField unsignedIntegerField => unsignedIntegerField.value,
            LongField longField => longField.value,
            UnsignedLongField unsignedLongField => unsignedLongField.value,
            FloatField floatField => floatField.value,
            DoubleField doubleField => doubleField.value,
            Toggle toggle => toggle.value,
            EnumField enumField => enumField,
            Vector2Field vector2Field => vector2Field.value,
            Vector2IntField vector2IntField => vector2IntField.value,
            Vector3Field vector3Field => vector3Field.value,
            Vector3IntField vector3IntField => vector3IntField.value, 
            Vector4Field vector4Field => vector4Field.value,
            ColorField colorField => colorField.value,
            GradientField gradientField => gradientField.value,
            CurveField curveField => curveField.value,
            LayerMaskField layerMaskField => layerMaskField.value,
            RectField rectField => rectField.value,
            RectIntField rectIntField => rectIntField.value,
            BoundsField boundsField => boundsField.value,
            BoundsIntField boundsIntField => boundsIntField.value,
            ObjectField objectField => objectField.value,
            _ => null
        };

        public static bool DrawParameterField(Type fieldType, string fieldName, out VisualElement visualElement) {
	        VisualElement? drawn = DrawParameterField(fieldType, fieldName);
	        
	        if (drawn == null) {
		        visualElement = new HelpBox($"Type {fieldType} is not supported.", HelpBoxMessageType.Warning);
		        return false;
	        }

	        drawn.name = fieldName;

	        visualElement = drawn;
	        return true;
        }

        private static VisualElement? DrawParameterField(Type fieldType, string fieldName) {
			if (fieldType == typeof(string)) {
				return new TextField(fieldName);
			}
			else if (fieldType == typeof(int))
			{
				return new IntegerField(fieldName);
			}
			else if (fieldType == typeof(uint))
			{
				return new UnsignedIntegerField(fieldName);
			}
			else if (fieldType == typeof(long))
			{
				return new LongField(fieldName);
			}
			else if (fieldType == typeof(ulong))
			{
				return new UnsignedLongField(fieldName);
			}
			else if (fieldType == typeof(float))
			{
				return new FloatField(fieldName);
			}
			else if (fieldType == typeof(double))
			{
				return new DoubleField(fieldName);
			}
			else if (fieldType == typeof(bool))
			{
				return new Toggle(fieldName);
			}
		
			// TODO: not checked
			else if (fieldType.IsEnum)
			{
				return new EnumField(fieldName);
			}
			else if (fieldType == typeof(Vector2))
			{
				return new Vector2Field(fieldName);
			}
			else if (fieldType == typeof(Vector2Int))
			{
				return new Vector2IntField(fieldName);
			}
			else if (fieldType == typeof(Vector3))
			{
				return new Vector3Field(fieldName);
			}
			else if (fieldType == typeof(Vector3Int)) {
				return new Vector3IntField(fieldName);
			}
			else if (fieldType == typeof(Vector4))
			{
				return new Vector4Field(fieldName);
			}
			else if (fieldType == typeof(Color))
			{
				return new ColorField(fieldName);
			}
			else if (fieldType == typeof(Gradient))
			{
				return new GradientField(fieldName);
			}
			else if (fieldType == typeof(AnimationCurve))
			{
				return new CurveField(fieldName);
			}
			else if (fieldType == typeof(LayerMask))
			{
				return new LayerMaskField(fieldName);
			}
			else if (fieldType == typeof(Rect))
			{
				return new RectField(fieldName);
			}
			else if (fieldType == typeof(RectInt))
			{
				return new RectIntField(fieldName);
			}
			else if (fieldType == typeof(Bounds))
			{
				return new BoundsField(fieldName);
			}
			else if (fieldType == typeof(BoundsInt))
			{
				return new BoundsIntField(fieldName);
			}
			else if (fieldType.IsSubclassOf(typeof(Object))) {
				return new ObjectField(fieldName) {
					objectType = fieldType
				};
			}
			else {
				return null;
			}
		}
    }
}