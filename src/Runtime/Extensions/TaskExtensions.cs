using System.Threading;

namespace UnityExtended.Core.Extensions {
    public static class TaskExtensions {
        public static void CancelAndInstantiate(ref CancellationTokenSource cancellationTokenSource) {
            cancellationTokenSource?.Cancel();
            cancellationTokenSource = new();
        }
    }
}