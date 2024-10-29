using EditorAttributes;
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
#pragma warning disable IDE0051 // Remove unused private members. Used in inspector.
        [Button]
        private void LoadReferencedScene() {
            if (scene == null) return;

            SceneManager.LoadScene(scene.name, LoadSceneMode.Single);
        }

        [Button]
        private void ReloadCurrentScene() {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex, LoadSceneMode.Single);
        }
#pragma warning restore IDE0051 // Remove unused private members
#endif
    }
}