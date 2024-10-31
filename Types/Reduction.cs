using FirstPersonController;
using System.Collections.Generic;
using UnityEngine;

namespace UnityExtended.Core.Types {
    public class Reduction {
        private readonly List<Reducer> reducers = new();

        public float SumReductionPercentage { 
            get {
                float sumReductionPercentage = 0;

                foreach (var reducer in reducers) {
                    sumReductionPercentage += reducer.ReductionPercentage;
                }

                return Mathf.Clamp01(sumReductionPercentage);
            }    
        }

        public float SumLeftPercentage => 1 - SumReductionPercentage;

        public Reducer AddReducer(float percentageOfReduction) {
            Reducer reducer = new(percentageOfReduction);
            reducers.Add(reducer);

            return reducer;
        }

        public void RemoveReducer(Reducer reducer) => reducers.Remove(reducer);

        public float Reduce(float value) => value * SumLeftPercentage;

        public int Reduce(int value) => (int)(value * SumLeftPercentage);
    }
}
