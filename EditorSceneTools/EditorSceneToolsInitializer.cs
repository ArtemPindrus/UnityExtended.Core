using System;
using System.Linq;
using System.Reflection;
using UnityEngine;

namespace UnityExtended.Core.EditorSceneTools {
	public class EditorSceneToolsInitializer {
#if UNITY_EDITOR
		[RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
		public static void InitializeTools() {
			Assembly assembly = Assembly.GetExecutingAssembly();

			Type targetType = typeof(IEditorSceneTool);

			var tools = assembly
				.GetTypes()
				.Where(t => targetType.IsAssignableFrom(t) && t.IsClass && !t.IsAbstract);

			foreach (var t in tools) {
				var component = new GameObject(t.Name).AddComponent(t);
				UnityEngine.Object.DontDestroyOnLoad(component);
			}
		}
#endif
	}
}
