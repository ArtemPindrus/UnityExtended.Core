﻿using System;
using UnityEngine;
using UnityExtended.Core.Extensions;

namespace UnityExtended.Core.Utilities {
    public enum UpdateMode {
        Update,
        Manual,
    }
    
    public class RadiusConstraint : MonoBehaviour {
        [field: SerializeField]
        public UpdateMode UpdateMode { get; set; }
        
        [field: SerializeField]
        public Transform Center { get; set; }

        [field: SerializeField] 
        public Vector3 LocalUpAxis { get; set; }

        public Vector3 WorldUpAxis => Center.TransformDirection(LocalUpAxis);
        
        [field: SerializeField]
        public float Distance { get; set; }
        
        [field: Range(-180,180)]
        [field: SerializeField]
        public float MaxLimit { get; private set; }
        
        [field: Range(-180,180)]
        [field: SerializeField]
        public float MinLimit { get; private set; }

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
        
        private void Align() {
            PerpendicularizeTransform();
            ConstrictToLimits();
        }

        private void PerpendicularizeTransform() {
            Vector3 directionToBody = (transform.position - Center.position).normalized;

            Vector3 pos = directionToBody.Perpendicularize(WorldUpAxis) * Distance;
            transform.position = Center.position + pos;
        }

        private void ConstrictToLimits() {
            Vector3 worldUpAxis = WorldUpAxis;
            
            Vector3 directionToBody = (transform.position - Center.position).normalized;
            float angleToBody = Vector3.SignedAngle(Center.forward, directionToBody, worldUpAxis);

            if (angleToBody < MinLimit) directionToBody = Center.forward.RotateAround(worldUpAxis, MinLimit);
            else if (angleToBody > MaxLimit) directionToBody = Center.forward.RotateAround(worldUpAxis, MaxLimit);

            transform.position = Center.position + directionToBody * Distance;
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

        private void OnDrawGizmos() {
            if (Center == null) return; 
            
            Gizmos.color = Color.green;
            Gizmos.DrawLine(Center.position, Center.position + WorldUpAxis);
            
            Gizmos.color = Color.cyan;
            Gizmos.DrawWireSphere(Center.position, Distance);

            Vector3 forward = Center.forward;
            Vector3 min = forward.RotateAround(WorldUpAxis, MinLimit) * Distance;
            Vector3 max = forward.RotateAround(WorldUpAxis, MaxLimit) * Distance;
            
            Gizmos.color = Color.red;
            Gizmos.DrawLine(Center.position, Center.position + min);
            Gizmos.DrawLine(Center.position, Center.position + max);
        }
    }
}