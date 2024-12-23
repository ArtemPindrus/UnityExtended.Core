using System.Linq;
using System.Reflection;
using UnityEditor;

#nullable  enable
namespace UnityExtended.Core.Extensions {
    public static class SerializedPropertyExtensions {
#nullable  enable
        /// <summary>
        /// Gets object containing this SerializedProperty.
        /// </summary>
        /// <param name="property">SerializedProperty contained withing an instance.</param>
        /// <returns>Object that contains given SerializedProperty.</returns>
        public static object? GetContainingObject(this SerializedProperty property) {
            var pathParts = property.propertyPath.Split(".").SkipLast(1);
            object currentObject = property.serializedObject.targetObject;

            foreach (var part in pathParts) {
                var field = currentObject
                    .GetType()
                    .GetField(part, BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);

                if (field == null) return null;
				
                currentObject = field.GetValue(currentObject);
            }

            return currentObject;
        }
#nullable disable
    }
}