#nullable enable
using System;
using System.Linq;
using System.Reflection;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;
using UnityExtended.Core.Extensions;
using UnityExtended.Core.Attributes;
using Object = UnityEngine.Object;

namespace UnityExtended.Core.Drawers {
	public static class ButtonDrawer {
		public static VisualElement DrawButtons(object target) {
			var root = new VisualElement();
			var methods = target.GetType().GetMethods();

			foreach (var method in methods) {
				if (method.GetCustomAttribute<ButtonAttribute>() is not null) {
					root.Add(CreateButton(method, target));
				}
			}

			return root;
		}
		
		public static VisualElement CreateButton(MethodInfo method, object target) {
			VisualElement root = new();
			root.style.backgroundColor = new StyleColor(Color.gray);

			var paramsFoldout = new Foldout() {
				text = "Parameters:"
			};

			var parameters = method.GetParameters();

			foreach (var parameterInfo in parameters) {
				var parameterType = parameterInfo.ParameterType;

				var parameterField = Extensions.VisualElementExtensions.DrawParameterField(parameterType, parameterInfo.Name);
				paramsFoldout.Add(parameterField);
			}

			Button button = new(() => method.Invoke(target, paramsFoldout.Children().Select(x => x.GetValue()).ToArray())) {
				text = method.Name
			};
        
			root.Add(button, paramsFoldout);

			return root;
		}
	}
}