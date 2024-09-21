using System;
using System.Collections;
using UnityEngine;
using UnityExtended.Core.Extensions;

namespace UnityExtended.Core.Utilities.Physics {
    public static class PhysicsHelper {
        /// <summary>
        /// Predicts linear velocity of a <see cref="Rigidbody"/> on the next physics update 
        /// after application of accumulated forces, but before the collision and friction forces.
        /// <para>Intended to be used before internal physics update (on FixedUpdate).</para>
        /// <para>When tested, method provided exclusively correct results
        /// (predictions before physics simulation coincided with actual forces after physics simulation without collisions).</para>
        /// </summary>
        /// <param name="rb"><see cref="Rigidbody"/>, linear velocity of which is calculated.</param>
        /// <param name="accountDrag">Whether to account rigidbody's drag into calculations. Set to true for accuracy.</param>
        /// <param name="accountGravity">Whether to account rigidbody's gravity into calculations. Set to true for accuracy.</param>
        /// <returns>Linear velocity of a rigidbody right after application of accumulated forces in the internal physics update.</returns>
        public static Vector3 PredictAccumulatedLinearVelocity(Rigidbody rb, bool accountDrag = true, bool accountGravity = true) {
            float fixedDT = Time.fixedDeltaTime;

            Vector3 accumulatedForce = rb.GetAccumulatedForce();
            Vector3 velocityDelta = accumulatedForce * fixedDT / rb.mass; // turn accumulated force into velocity delta

#if UNITY_6000_0_OR_NEWER
            Vector3 velocityPostApplication = rb.linearVelocity + velocityDelta;
#elif UNITY_2022_3_OR_NEWER
            Vector3 velocityPostApplication = rb.velocity + velocityDelta;
#endif

            if (accountGravity && rb.useGravity) { // gravity isn't an accumulated force so it's getting calculated separately in here
                velocityPostApplication += UnityEngine.Physics.gravity * fixedDT;
            }

            Vector3 predicted = velocityPostApplication;

            if (accountDrag) {
#if UNITY_6000_0_OR_NEWER
                float dragMultiplier = Mathf.Clamp01(1 - rb.linearDamping * fixedDT); // people figured out the drag formula a while ago
#elif UNITY_2022_3_OR_NEWER
                float dragMultiplier = Mathf.Clamp01(1 - rb.drag * fixedDT); // people figured out the drag formula a while ago
#endif
                predicted *= dragMultiplier;
            }

            return predicted;
        }

        /// <summary>
        /// Predicts <see cref="Rigidbody"/>'s angular velocity on the next physics update after application of accumulated forces, 
        /// but before collision and friction forces.
        /// <para>Intended to be used before internal physics update (on FixedUpdate).</para>
        /// <para>As <see cref="PredictAccumulatedLinearVelocity(Rigidbody, bool, bool)"/> provided exclusively correct results during testings.</para>
        /// </summary>
        /// <param name="rb"><see cref="Rigidbody"/>, angular velocity of which is calculated.</param>
        /// <param name="accountDrag">Whether to account rigidbody's angular drag into calculations. Set to true for accuracy.</param>
        /// <returns>Angular velocity of a rigidbody right after application of accumulated forces in the internal physics update.</returns>
        public static Vector3 PredictAccumulatedAngularVelocity(Rigidbody rb, bool accountDrag = true) {
            Vector3 angularAcceleration = CalculateAngularAcceleration(rb, rb.GetAccumulatedTorque());

            Vector3 predicted = rb.angularVelocity + angularAcceleration * Time.fixedDeltaTime;

            if (accountDrag) {
#if UNITY_6000_0_OR_NEWER
                float dragMultiplier = Mathf.Clamp01(1 - rb.angularDamping * Time.fixedDeltaTime);
#elif UNITY_2022_3_OR_NEWER
                float dragMultiplier = Mathf.Clamp01(1 - rb.angularDrag * Time.fixedDeltaTime);
#endif
                predicted *= dragMultiplier;
            }

            return predicted;
        }

        /// <summary>
        /// Makes Unity consistently invoke supplied function after physics update (technically after all the OnCollisionXXX messages). 
        /// </summary>
        /// <param name="monoBehaviour"><see cref="MonoBehaviour"/> to which a coroutine will be attached.</param>
        /// <param name="function">Invoked function.</param>
        public static void InvokePostPhysicsUpdate(this MonoBehaviour monoBehaviour, Action function) {
            monoBehaviour.StartCoroutine(Impl(function));

            static IEnumerator Impl(Action function) {
                while (true) {
                    yield return new WaitForFixedUpdate();

                    function();
                }
            }
        }

        /// <summary>
        /// Converts torque into angular acceleration for a specific <see cref="Rigidbody"/>. 
        /// </summary>
        /// <param name="rb"><see cref="Rigidbody"/> to which the torque is applied to.</param>
        /// <param name="torque">Applied torque.</param>
        /// <returns><see cref="Vector3"/> representing angular acceleration calculated from a <paramref name="torque"/> for a given <paramref name="rb"/></returns>
        public static Vector3 CalculateAngularAcceleration(Rigidbody rb, Vector3 torque) {
            Vector3 worldInertiaTensor = rb.inertiaTensorRotation * rb.inertiaTensor;

            return torque.Divide(worldInertiaTensor);
        }
    }
}
