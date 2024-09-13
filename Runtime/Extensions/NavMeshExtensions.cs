using System;
using UnityEngine;
using UnityEngine.AI;

namespace UnityExtended.Extensions {
    public static class NavMeshExtensions {
        public static bool IsAtPos(this NavMeshAgent agent, Vector3 position, float destinationTolerance = 0, float remainingDistanceTolerance = 0) {
            if (destinationTolerance < 0) throw new ArgumentException($"{destinationTolerance} shouln't be < 0");
            if (remainingDistanceTolerance < 0) throw new ArgumentException($"{remainingDistanceTolerance} shouln't be < 0");

            bool destinationValid;
            bool remainingDistanceValid;

            if (destinationTolerance == 0) destinationValid = agent.destination == position;
            else destinationValid = Vector3.Distance(agent.destination, position) <= destinationTolerance;

            if (remainingDistanceTolerance == 0) remainingDistanceValid = agent.remainingDistance == 0;
            else remainingDistanceValid = agent.remainingDistance <= destinationTolerance;

            return destinationValid && remainingDistanceValid;
        }
    }
}
