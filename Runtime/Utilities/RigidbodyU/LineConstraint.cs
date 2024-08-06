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
        [Tooltip(@"Whether to perform second iteration (RECOMMENDED ON).
Normally component alligns both position and velocity BEFORE the physics simulation. With this bool set to true it repeats the process AFTER physics simulation.
Produces more accurate results, obviously at the cost of performance (really little overhead).
CHANGING VALUE REQUIRES RELOAD.")]
        [SerializeField]
        private bool performSecondIteration = true;
        [SerializeField]
        private Vector3 firstPoint;
        [SerializeField]
        private Vector3 secondPoint;

        private Rigidbody rb;
        private Vector3 lastAllignedPosition;

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
            // Alligning position and velocity before any forces are applied by PhysX. Makes physics simulation itself accurate.

            AllignPosition();
            AllignRestrictVelocity();
        }

        private void OnDrawGizmos() {
            if (debug) {
                Gizmos.DrawWireSphere(secondPoint, DebugSpheresRadius);
                Gizmos.DrawWireSphere(firstPoint, DebugSpheresRadius);

                Debug.DrawLine(firstPoint, secondPoint);
            }
        }

        /// <summary>
        /// Align current position of the rigidbody to be along the line constraint.
        /// </summary>
        private void AllignPosition() {
            Vector3 firstToBody = transform.position - firstPoint;
            Vector3 aligned = firstToBody.ProjectClamped(FirstToSecond);

            rb.position = firstPoint + aligned;

            lastAllignedPosition = rb.position;
        }

        private void AllignRestrictVelocity() {
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

        /// <summary>
        /// Alligning position and velocity again AFTER PhysX simulation.
        /// <para>Ensures that the object is at the right position before render.</para>
        /// </summary>
        private void AfterPhysics() {
            AllignRestrictVelocity();

            // Intercept movement made by physics simulation and instead move the body along the line constraint.
            Vector3 targetPoint = lastAllignedPosition + rb.velocity * Time.fixedDeltaTime;
            transform.position = targetPoint;
        }
    }
}
