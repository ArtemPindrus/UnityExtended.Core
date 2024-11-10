using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

namespace UnityExtended.Core.EditorTools {
    public class SceneLoadTool : EditorWindow {
        private ObjectField sceneAssetField;
        private Button loadReferencedBtn;
        private Button reloadActiveBtn;
        private EnumField modeField;

        [MenuItem(EditorToolsConst.MenuPath + nameof(SceneLoadTool))]
        public static void CreateWindow() {
            SceneLoadTool wnd = GetWindow<SceneLoadTool>();
            wnd.titleContent = new GUIContent(nameof(SceneLoadTool));
        }

        public void CreateGUI() {
            // Each editor window contains a root VisualElement object
            VisualElement root = rootVisualElement;

            // Instantiate UXML
            sceneAssetField = new("Scene Asset");
            sceneAssetField.objectType = typeof(SceneAsset);
            root.Add(sceneAssetField);

            modeField = new("LoadSceneMode", LoadSceneMode.Single);
            root.Add(modeField);

            loadReferencedBtn = new(LoadReferencedScene);
            loadReferencedBtn.text = "Load referenced scene";
            root.Add(loadReferencedBtn);

            reloadActiveBtn = new(ReloadActiveScene);
            reloadActiveBtn.text = "ReloadActiveScene";
            root.Add(reloadActiveBtn);

            OnGUI();
        }

        private void OnGUI() {
            rootVisualElement.visible = Application.isPlaying;
            
            if (loadReferencedBtn == null) return;

            loadReferencedBtn.visible = sceneAssetField.value != null && Application.isPlaying;
        }

        private void LoadReferencedScene() {
            if (sceneAssetField.value == null) return;

            SceneManager.LoadScene(sceneAssetField.value.name, (LoadSceneMode)modeField.value);
        }

        private void ReloadActiveScene() {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex, LoadSceneMode.Single);
        }
    }
}