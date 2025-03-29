using System;
using System.Linq;
using System.Reflection;
using UnityEngine;
using UnityEngine.UIElements;
using UnityExtended.Core.External.UnityExtended.Core.Helpers;
using UnityExtended.Generators.Attributes;
using VisualElementExtensions = UnityExtended.Core.Editor.EditorExtensions.VisualElementExtensions;

#nullable enable
namespace UnityExtended.Core.Editor.Drawers {
    public static class DisplayDrawer {
        /// <summary>
        /// Fills in container with <see cref="VisualElement"/>(s) created for <see cref="DisplayAttribute"/> on <paramref name="target"/>.
        /// </summary>
        /// <param name="container">Container that will be filled in.</param>
        /// <param name="target">Object containing fields decorated with <see cref="DisplayAttribute"/>.</param>
        public static void FillInFor(VisualElement container, object target) {
            var fieldInfos = target
                .GetType()
                .GetFields(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic)
                .Where(x => x.GetCustomAttribute<DisplayAttribute>() is not null);
            
            foreach (var fieldInfo in fieldInfos) {
                container.Add(CreateDisplay(fieldInfo, target));
            }
        }
        
        /// <summary>
        /// Creates visual representation of data, given <paramref name="field"/> that contains it and <paramref name="target"/> that contains the field.
        /// </summary>
        /// <param name="field">Field, containing data.</param>
        /// <param name="target">Object, containing <paramref name="field"/>.</param>
        /// <returns></returns>
        public static VisualElement CreateDisplay(FieldInfo field, object target) {
            var fieldName = ReflectionHelper.BeautifyBackingFieldName(field.Name);
            if (!VisualElementExtensions.DrawParameterField(field.FieldType, fieldName, out var fieldVisualElement)
                && field.FieldType.GetCustomAttribute<DisplayItemAttribute>() is {} displayItemAttribute) {
                fieldVisualElement = CreateVisualElementForDisplayItem(displayItemAttribute, fieldName, out var updateMethod, out var bagInstance);
                fieldVisualElement.enabledSelf = false;

                fieldVisualElement.schedule.Execute(() => {
                    updateMethod.Invoke(bagInstance, new[] { field.GetValue(target) });
                }).Every(50);
                
                return fieldVisualElement;
            }
            fieldVisualElement.enabledSelf = false;

            var visualElementValueField = fieldVisualElement.GetType().GetProperty("value", BindingFlags.Instance | BindingFlags.Public);
            
            if (visualElementValueField != null) {
                fieldVisualElement.schedule.Execute(() => {
                    visualElementValueField.SetValue(fieldVisualElement, field.GetValue(target));
                }).Every(20);
            }

            return fieldVisualElement;
        }

        /// <summary>
        /// Creates <see cref="VisualElement"/> for an instance of a type that is decorated with <see cref="DisplayItemAttribute"/> and provides valid <see cref="IDisplayBag{T}"/>.
        /// </summary>
        /// <param name="target">Instance of a type decorated with <see cref="DisplayItemAttribute"/>.</param>
        /// <param name="displayItemAttribute">Attribute instance.</param>
        /// <param name="name">Name of a returned control.</param>
        /// <returns><see cref="VisualElement"/> created using <see cref="IDisplayBag{T}.CreateVisualElement"/>.</returns>
        /// <exception cref="Exception">Didn't find a CreateVisualElement method on a type specified in <paramref name="displayItemAttribute"/>.</exception>
        public static VisualElement CreateVisualElementForDisplayItem(DisplayItemAttribute displayItemAttribute, string name, out MethodInfo updateMethod, out object bagInstance) {
            bagInstance = Activator.CreateInstance(displayItemAttribute.DisplayBagType);
            var type = bagInstance.GetType();
            var createVisualElementMethod = type.GetMethod("CreateVisualElement");
            updateMethod = type.GetMethod("Update");

            if (createVisualElementMethod == null || updateMethod == null) throw new Exception(); // TODO: handle;

            var ve = (VisualElement)createVisualElementMethod.Invoke(bagInstance, new object[] {name});
            ve.name = name;
            
            return ve;
        }
    }
}