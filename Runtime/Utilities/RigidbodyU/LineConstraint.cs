using UnityEngine;
using UnityExtended.Extensions;

namespace UnityExtended.Utilities.RigidbodyU {
    [RequireComponent(typeof(Rigidbody))]
    /// <summary>
    /// Component that restricts rigidbody movement to the line between two points.
    /// </summary>
    public class LineConstraint : MonoBehaviour {
        private const float DebugSpheresRadius = 0.1f;

        [SerializeField]
        [Tooltip("Visualize two endpoints and the line between them.")]
        private bool debug;
        [SerializeField]
        private Vector3 firstPoint;
        [SerializeField]
        private Vector3 secondPoint;

        private Rigidbody rb;

        /// <summary>
        /// Gets a vector pointing from <see cref="firstPoint"/> to the <see cref="secondPoint"/>.
        /// </summary>
        public Vector3 FirstToSecond { get; private set; }

        /// <summary>
        /// Gets normalized <see cref="FirstToSecond"/>.
        /// </summary>
        public Vector3 FirstToSecondDir { get; private set; }

        private void Awake() {
            rb = GetComponent<Rigidbody>();

            FirstToSecond = secondPoint - firstPoint;
            FirstToSecondDir = FirstToSecond.normalized;
        }

        private void FixedUpdate() {
            AllignPosition();
            RestrictVelocity();
        }

        private void OnDrawGizmos() {
            if (debug) {
                Gizmos.DrawWireSphere(secondPoint, DebugSpheresRadius);
                Gizmos.DrawWireSphere(firstPoint, DebugSpheresRadius);

                Debug.DrawLine(firstPoint, secondPoint);
            }
        }

        private void AllignPosition() {
            Vector3 firstToBody = transform.position - firstPoint;
            Vector3 aligned = firstToBody.ProjectClamped(FirstToSecond);

            rb.position = firstPoint + aligned;
        }

        private void RestrictVelocity() {
            Vector3 currentPosition = rb.position;
            Vector3 projectedVelocity = Vector3.Project(rb.velocity, FirstToSecondDir);
            Vector3 projectedDirection = projectedVelocity.normalized;

            if (currentPosition == firstPoint && projectedDirection == -FirstToSecondDir) {
                projectedVelocity = Vector3.zero;
            } else if (currentPosition == secondPoint && projectedDirection == FirstToSecondDir) {
                projectedVelocity = Vector3.zero;
            }

            rb.velocity = projectedVelocity;
        }
    }
}
