using System.Reflection;
using UnityEditor;

#nullable  enable
namespace UnityExtended.Core.Extensions {
    public static class SerializedPropertyExtensions {
        public static object GetValue(this SerializedProperty property) {
            string[] pathParts = property.propertyPath.Split(".");
            object currentObject = property.serializedObject.targetObject;
            FieldInfo? field = null;

            foreach (var part in pathParts) {
                field = currentObject
                    .GetType()
                    .GetField(part, BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
                currentObject = field.GetValue(currentObject);
            }

            return currentObject;
        }
    }
}