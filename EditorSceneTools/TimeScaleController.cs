using UnityEngine;

namespace UnityExtended.Core.EditorSceneTools {
    [AddComponentMenu("")]
    public class TimeScaleController : MonoBehaviour, IEditorSceneTool {
        private const string LastValueString = "LastTimeScaleValue";

        [SerializeField]
        [Min(0)]
        private float timeScale = 1;

        private void Awake() {
            timeScale = PlayerPrefs.GetFloat(LastValueString, 1);

            Application.quitting += Application_quitting;

            if (timeScale != 1) {
                Debug.Log("Time scale is modified.");
            }
        }

        private void Application_quitting() {
            PlayerPrefs.SetFloat(LastValueString, timeScale);

            Application.quitting -= Application_quitting;
        }

        private void Update() {
            Time.timeScale = timeScale;
        }
    }
}