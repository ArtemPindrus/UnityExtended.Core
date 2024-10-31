using EditorAttributes;
using System;
using UnityEngine;

namespace UnityExtended.Core.Types {
    [Serializable]
    public class ReducibleFloat {
        private Reduction reduction;

        [field: SerializeField]
        public float ValueUnchanged { get; private set; }

        /// <summary>
        /// Gets reduced value.
        /// </summary>
        public float Value {
            get {
                return reduction == null ? ValueUnchanged : reduction.Reduce(ValueUnchanged);
            }
        }

        public ReducibleFloat(float initialValue) { 
            reduction = new();   
            ValueUnchanged = initialValue;
        }

        public Reducer AddReducer(float reductionPercentage) {
            reduction ??= new();

            return reduction.AddReducer(reductionPercentage);
        }

        public void RemoveReducer(Reducer reducer) => reduction.RemoveReducer(reducer);

        private void LogReduced() => Debug.Log(Value);
    }
}
