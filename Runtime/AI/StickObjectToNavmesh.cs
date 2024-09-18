using Unity.AI.Navigation;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Assertions;

namespace UnityExtended.AI.Utilities {
    /// <summary>
    /// In-editor tool to stick an object to specified <see cref="NavMeshSurface"/>.
    /// </summary>
    public class StickObjectToNavmesh : MonoBehaviour {
        [Tooltip("Radius around the object for sampling agains navmesh surface.")]
        [SerializeField]
        private float radius;

        [SerializeField]
        private NavMeshSurface surface;

        [ContextMenu(nameof(Perform))]
        private void Perform() {
            Assert.IsNotNull(surface);

            NavMeshQueryFilter filter = new() { agentTypeID = surface.agentTypeID, areaMask = -1 };

            if (NavMesh.SamplePosition(transform.position, out NavMeshHit hit, radius, filter)) {
                transform.position = hit.position;
            } else Debug.LogError($"No navmesh {surface.name} was found in a specified radius around the object to stick.");
        }
    }
}