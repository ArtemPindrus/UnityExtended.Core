using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

namespace UnityExtended.Core.EditorTools {
    public class TimeScaleController : EditorWindow {
        private Slider timeScaleSlider;
        private Toggle toggle;

        [MenuItem(EditorToolsConst.MenuPath + nameof(TimeScaleController))]
        public static void CreateWindow() {
            TimeScaleController wnd = GetWindow<TimeScaleController>();
            wnd.titleContent = new GUIContent(nameof(TimeScaleController));
        }

        public void CreateGUI() {
            // Each editor window contains a root VisualElement object
            VisualElement root = rootVisualElement;

            // VisualElements objects can contain other VisualElement following a tree hierarchy.
            toggle = new("Enable");
            root.Add(toggle);

            timeScaleSlider = new(0, 10);
            timeScaleSlider.label = "TimeScale";
            timeScaleSlider.value = Time.timeScale;
            timeScaleSlider.showInputField = true;
            timeScaleSlider.fill = true;
            root.Add(timeScaleSlider);

            OnGUI();

            SceneManager.sceneLoaded += SceneManager_sceneLoaded;
        }

        private void OnDestroy() {
            SceneManager.sceneLoaded -= SceneManager_sceneLoaded;
        }

        private void SceneManager_sceneLoaded(Scene arg0, LoadSceneMode arg1) {
            if (toggle.value && timeScaleSlider.value != 1) {
                Debug.LogWarning($"TimeScale is currently overriden by {nameof(TimeScaleController)} tool! TimeScale is set to {timeScaleSlider.value}.");
            }
        }

        public void OnGUI() {
            if (timeScaleSlider == null) return;
            
            timeScaleSlider.SetEnabled(toggle.value);

            if (toggle.value) {
                Time.timeScale = timeScaleSlider.value;
            } else {
                timeScaleSlider.value = Time.timeScale;
            }
        }
    }
}