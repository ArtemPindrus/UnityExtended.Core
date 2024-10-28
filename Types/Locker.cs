using System.Collections.Generic;

namespace UnityExtended.Core.Types {
    public class Locker {
        private readonly HashSet<object> locks = new();

        public bool IsLocked => locks.Count > 0;

        public void Lock(object @lock, bool toLock) {
            if (toLock) Lock(@lock);
            else ReleaseLock(@lock);
        }

        public void Lock(object @lock) {
            locks.Add(@lock);
        }

        public void ReleaseLock(object @lock) {
            locks.Remove(@lock);
        }

        public static implicit operator bool(Locker locker) => locker.IsLocked;
    }
}