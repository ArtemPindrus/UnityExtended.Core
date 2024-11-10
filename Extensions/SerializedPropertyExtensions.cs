using System.Reflection;
using UnityEditor;

#nullable  enable
namespace UnityExtended.Core.Extensions {
    public static class SerializedPropertyExtensions {
        /// <summary>
        /// Gets object that contains this SerializableProperty.
        /// </summary>
        /// <param name="property"></param>
        /// <returns></returns>
        public static object GetContainingObject(this SerializedProperty property) {
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