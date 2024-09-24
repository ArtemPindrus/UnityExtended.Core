using UnityEngine;

namespace UnityExtended.Core.Utilities {
    /// <summary>
    /// In-editor tool to stick an object (with colliders) to the ground.
    /// </summary>
    public class StickDown : MonoBehaviour {
#if UNITY_EDITOR
        private void Reset() {
            Stick();
        }

        [ContextMenu(nameof(Stick))]
        private void Stick() {
            UnityEngine.Physics.Raycast(transform.position, Vector3.down, out RaycastHit hitDown);

            UnityEngine.Physics.Raycast(hitDown.point, Vector3.up, out RaycastHit traced);

            Vector3 difference = transform.position - traced.point;

            transform.position = hitDown.point + difference;

            DestroyImmediate(this);
        }
#endif
    }
}
