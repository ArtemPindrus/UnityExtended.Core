using System;
using UnityEngine;
using UnityExtended.Core.Extensions;

namespace UnityExtended.Core.Utilities {
    public enum UpdateMode {
        Update,
        Manual,
    }
    
    public class RadiusConstraint : MonoBehaviour {
        [SerializeField] public bool DestroyCenterOnDestroy;
        
        [field: SerializeField]
        public UpdateMode UpdateMode { get; set; }
        
        [field: SerializeField]
        public Transform Center { get; set; }

        [field: SerializeField] 
        public Vector3 LocalUpAxis { get; set; }
        
        [field: SerializeField]
        public float Distance { get; set; }
        
        [field: Range(-180,180)]
        [field: SerializeField]
        public float MaxLimit { get; private set; }
        
        [field: Range(-180,180)]
        [field: SerializeField]
        public float MinLimit { get; private set; }
        
        public Vector3 WorldUpAxis => Center.TransformDirection(LocalUpAxis);

        public Vector3 MaxDirection => Center.forward.RotateAround(WorldUpAxis, MaxLimit);
        
        public Vector3 MinDirection => Center.forward.RotateAround(WorldUpAxis, MinLimit);

        public static RadiusConstraint Attach(GameObject gameObject, UpdateMode updateMode, Transform center, Vector3 localUpAxis,
            float distance, float maxLimit = 180, float minLimit = -180) {
            var component = gameObject.AddComponent<RadiusConstraint>();

            component.UpdateMode = updateMode;
            component.Center = center;
            component.LocalUpAxis = localUpAxis;
            component.Distance = distance;

            maxLimit = Mathf.Clamp(maxLimit, -180, 180);
            minLimit = Mathf.Clamp(minLimit, -180, 180);

            if (minLimit > maxLimit) {
                (minLimit, maxLimit) = (maxLimit, minLimit);
            }

            component.MaxLimit = maxLimit;
            component.MinLimit = minLimit;

            return component;
        }
        

        public void ManualUpdate() {
            if (UpdateMode != UpdateMode.Manual) return;
            
            Align();
        }

        public Vector3 CustomConstraint(Vector3 custom) {
            Vector3 perp = Perpendicularize(custom);
            Vector3 constricted = ConstrictToLimits(perp);

            return constricted;
        }
        
        private void Align() {
            transform.position = Perpendicularize(transform.position);
            transform.position = ConstrictToLimits(transform.position);
        }

        private Vector3 Perpendicularize(Vector3 target) {
            Vector3 directionToBody = (target - Center.position).normalized;

            Vector3 pos = directionToBody.Perpendicularize(WorldUpAxis) * Distance;
            target = Center.position + pos;
            
            return target;
        }

        private Vector3 ConstrictToLimits(Vector3 target) {
            Vector3 worldUpAxis = WorldUpAxis;
            
            Vector3 directionToBody = (target - Center.position).normalized;
            float angleToBody = Vector3.SignedAngle(Center.forward, directionToBody, worldUpAxis);

            if (angleToBody < MinLimit || angleToBody > MaxLimit) {
                // find closest
                float angleFromMax = Vector3.Angle(directionToBody, MaxDirection);
                float angleFromMin = Vector3.Angle(directionToBody, MinDirection);

                directionToBody = angleFromMax < angleFromMin ? MaxDirection : MinDirection;
            }

            target = Center.position + directionToBody * Distance;

            return target;
        }

        private void Update() {
            if (UpdateMode != UpdateMode.Update) return;
            
            Align();
        }

        private void OnValidate() {
            if (MinLimit > MaxLimit) {
                (MinLimit, MaxLimit) = (MaxLimit, MinLimit);
            }
        }

        private void OnDestroy() {
            if (DestroyCenterOnDestroy) {
                Destroy(Center.gameObject);
            }
        }

        private void OnDrawGizmos() {
            if (Center == null) return; 
            
            Gizmos.color = Color.green;
            Gizmos.DrawLine(Center.position, Center.position + WorldUpAxis);
            
            Gizmos.color = Color.cyan;
            Gizmos.DrawWireSphere(Center.position, Distance);

            Vector3 min = MinDirection * Distance;
            Vector3 max = MaxDirection * Distance;
            
            Gizmos.color = Color.red;
            Gizmos.DrawLine(Center.position, Center.position + min);
            Gizmos.DrawLine(Center.position, Center.position + max);
        }
    }
}