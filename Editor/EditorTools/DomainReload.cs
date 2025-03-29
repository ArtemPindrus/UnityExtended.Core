using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

namespace UnityExtended.Core.Editor.EditorTools {
    public class DomainReload : EditorWindow {
        private Button reloadDomainBtn;
        private Label topLabel;

        [MenuItem(EditorToolsConst.MenuPath + nameof(DomainReload))]
        public static void CreateWindow() {
            DomainReload wnd = GetWindow<DomainReload>();
            wnd.titleContent = new GUIContent("DomainReload");
        }

        public void CreateGUI() {
            VisualElement root = rootVisualElement;

            topLabel = new();
            root.Add(topLabel);

            reloadDomainBtn = new(ReloadDomain);
            reloadDomainBtn.text = "Trigger Domain Reload";
            root.Add(reloadDomainBtn);

            OnGUI();
        }

        private void OnGUI() {
            if (reloadDomainBtn == null) return;
            
            reloadDomainBtn.SetEnabled(true);
            topLabel.text = "";

            if (Application.isPlaying) DisableAndSetText("Cannot reload in Play Mode.");
            else if (EditorApplication.isCompiling) DisableAndSetText("Compiling scripts...");
            else if (EditorApplication.isUpdating) DisableAndSetText("Refreshing AssetDatabase...");

            void DisableAndSetText(string text) {
                reloadDomainBtn.SetEnabled(false);

                topLabel.text = text;
            }
        }

        private void ReloadDomain() {
            EditorUtility.RequestScriptReload();
        }
    }
}