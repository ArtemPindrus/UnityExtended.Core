using TriInspector;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace UnityExtended.Core.EditorSceneTools {
	[AddComponentMenu("")]
	public class SceneReLoad : MonoBehaviour, IEditorSceneTool {
#if UNITY_EDITOR
		[SerializeField]
		private SceneAsset scene;

#nullable enable
		[Button(nameof(LoadReferencedScene))]
		private void LoadReferencedScene() {
			if (scene == null) return;

			SceneManager.LoadScene(scene.name, LoadSceneMode.Single);
		}

		[Button(nameof(ReloadCurrentScene))]
		public void ReloadCurrentScene() {
			SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex, LoadSceneMode.Single);
		}
#endif
	}
}