#nullable enable
using System.Linq;
using System.Reflection;
using UnityExtended.Core.Editor.EditorExtensions;
using UnityEngine;
using UnityEngine.UIElements;
using UnityExtended.Core.Attributes;
using UnityExtended.Core.Extensions;
using UnityExtended.Core.External.UnityExtended.Core.Helpers;
using UnityExtended.Core.Helpers;

namespace UnityExtended.Core.Editor.Drawers {
	public static class ButtonDrawer {
		public static void FillIn(VisualElement container, object target) {
			container.Add(DrawButtons(target));
		}
		
		public static VisualElement DrawButtons(object target) {
			var root = new VisualElement();
			var methods = target.GetType().GetMethods();

			foreach (var method in methods) {
				if (method.GetCustomAttribute<ButtonAttribute>() is { } buttonAttribute) {
					var buttonLabel = buttonAttribute.CustomButtonLabel ?? method.Name;
					root.Add(CreateButton(method, target, buttonLabel));
				}
			}

			return root;
		}
		
		public static VisualElement CreateButton(MethodInfo method, object target, string buttonLabel) {
			VisualElement root = new();
			root.style.backgroundColor = ColorHelper.From255RGBA(53);

			var paramsFoldout = new Foldout() {
				text = "Parameters:"
			};

			var parameters = method.GetParameters();

			foreach (var parameterInfo in parameters) {
				var parameterType = parameterInfo.ParameterType;

				EditorExtensions.VisualElementExtensions.DrawParameterField(parameterType, parameterInfo.Name, out var parameterField);
				paramsFoldout.Add(parameterField);
			}

			Button button = new(() => method.Invoke(target, paramsFoldout.Children().Select(x => x.GetValue()).ToArray())) {
				text = buttonLabel
			};
        
			root.Add(button, paramsFoldout);

			return root;
		}
	}
}