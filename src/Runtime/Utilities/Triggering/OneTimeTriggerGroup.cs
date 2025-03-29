using UnityEngine;
using UnityExtended.Core.Attributes;

namespace UnityExtended.Core.Utilities.Triggering {
    public class OneTimeTriggerGroup : MonoBehaviour {
        [SerializeField] private OneTimeTrigger[] triggerGroup;

        private void Awake() {
            foreach (var trigger in triggerGroup) {
                trigger.OnTriggered += OnTriggered;
            }
        }

        private void OnTriggered() {
            foreach (var trigger in triggerGroup) {
                Destroy(trigger.gameObject);
            }
        
            Destroy(gameObject);
        }

        [Button]
        private void CreateFromChildren() {
            OneTimeTrigger[] triggers = GetComponentsInChildren<OneTimeTrigger>();
            triggerGroup = triggers;
        }
    }
}