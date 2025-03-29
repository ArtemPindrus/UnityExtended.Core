using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;
using UnityEngine.UIElements;
using UnityExtended.Core.Attributes;
using UnityExtended.Core.External.UnityExtended.Core.Helpers;

#nullable enable
namespace UnityExtended.Core.Editor.Drawers {
    public static class SetVisualElementAtDrawer {
        /// <summary>
        /// Rearranges <see cref="VisualElement"/> representing inspector by analyzing fields decorated with <see cref="SetVisualElementAtAttribute"/>.
        /// </summary>
        /// <param name="inspectorVisualElement"><see cref="VisualElement"/> representing inspector created for <paramref name="target"/>.</param>
        /// <param name="target">Target object, for which <paramref name="inspectorVisualElement"/> is created.</param>
        public static void Rearrange(VisualElement inspectorVisualElement, object target) {
            var fieldInfos = target
                .GetType()
                .GetFields(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);

            List<Ordering> orderings = new();
            foreach (var fieldInfo in fieldInfos) {
                if (fieldInfo.TryGetCustomAttribute(out SetVisualElementAtAttribute attribute)) {
                    var order = new Ordering(fieldInfo.Name, attribute.Order);
                    orderings.Add(order);
                }
            }

            orderings = orderings.OrderBy(x => x.Order).ToList();

            foreach (var order in orderings) {
                var children = inspectorVisualElement.Children();
                
                var child = children.FirstOrDefault(x => NormalizeChildElementName(x.name) == order.FieldName);
                
                if (child == null) continue;
                
                inspectorVisualElement.Remove(child);
                inspectorVisualElement.Insert(order.Order, child);
            }

            static string NormalizeChildElementName(string name) {
                return name.Replace("PropertyField:", "");
            }
        }
    }

    public struct Ordering {
        public readonly string FieldName;
        public readonly int Order;

        public Ordering(string fieldName, int order) {
            FieldName = fieldName;
            Order = order;
        }
    }
}