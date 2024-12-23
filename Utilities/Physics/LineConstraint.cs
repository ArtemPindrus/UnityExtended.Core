using EditorAttributes;
using UnityEngine;
using UnityExtended.Core.Extensions;

namespace UnityExtended.Core.Utilities.Physics {
    /// <summary>
    /// Component that restricts rigidbody movement to the line between two points.
    /// </summary>
    [RequireComponent(typeof(Rigidbody))]
    public class LineConstraint : MonoBehaviour {
        private const float DebugSpheresRadius = 0.1f;

        [SerializeField]
        [Tooltip("Visualize two endpoints and the line between them.")]
        private bool debug;
        [Tooltip(@"Whether to perform second iteration (RECOMMENDED ON).
Normally component aligns both position and velocity BEFORE the physics simulation. With this bool set to true it repeats the process AFTER physics simulation.
Produces more accurate results, obviously at the cost of performance (really little overhead).
CHANGING VALUE REQUIRES RELOAD.")]
        [SerializeField]
        private bool performSecondIteration = true;
        [SerializeField]
        private Vector3 firstPoint;
        [SerializeField]
        private Vector3 secondPoint;

        private Rigidbody rb;
        private Vector3 lastAlignedPosition;

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

            if (performSecondIteration) this.InvokePostPhysicsUpdate(AfterPhysics);
        }

        private void FixedUpdate() {
            // Aligning position and velocity before any forces are applied by PhysX. This makes physics simulation itself accurate.

            AlignPosition();
            AlignRestrictVelocity();
        }

        /// <summary>
        /// Align current position of the rigidbody to be along the line constraint.
        /// </summary>
        private void AlignPosition(bool useTransformInstead = false) {
            Vector3 firstToBody = transform.position - firstPoint;
            Vector3 aligned = firstToBody.ProjectClamped(FirstToSecond);

            Vector3 target = firstPoint + aligned;

            if (!useTransformInstead) rb.position = target;
            else transform.position = target;

            lastAlignedPosition = rb.position;
        }

        private void AlignRestrictVelocity() {
            Vector3 currentPosition = rb.position;

            Vector3 projectedVelocity = Vector3.Project(rb.linearVelocity, FirstToSecondDir);

            Vector3 projectedDirection = projectedVelocity.normalized;

            if (currentPosition == firstPoint && projectedDirection == -FirstToSecondDir) {
                projectedVelocity = Vector3.zero;
            } else if (currentPosition == secondPoint && projectedDirection == FirstToSecondDir) {
                projectedVelocity = Vector3.zero;
            }

            rb.linearVelocity = projectedVelocity;
        }

        /// <summary>
        /// Aligning position and velocity again AFTER PhysX simulation.
        /// <para>Ensures that the object is at the right position before render.</para>
        /// </summary>
        private void AfterPhysics() {
            AlignRestrictVelocity();

            // Intercept movement made by physics simulation and instead move the body along the line constraint.
            Vector3 targetPoint = lastAlignedPosition + rb.linearVelocity * Time.fixedDeltaTime;
            rb.position = targetPoint;

            // Align new position (overshot may happen on previous movement)
            AlignPosition(true);
        }

        [Button]
        private void FromLocalPoints(Vector3 first, Vector3 second) {
            Transform parent = transform.parent;
            firstPoint = parent.TransformPoint(first);
            secondPoint = parent.TransformPoint(second);
        }

        private void OnDrawGizmos() {
            if (debug) {
                Gizmos.DrawWireSphere(secondPoint, DebugSpheresRadius);
                Gizmos.DrawWireSphere(firstPoint, DebugSpheresRadius);

                Debug.DrawLine(firstPoint, secondPoint);
            }
        }
    }
}
