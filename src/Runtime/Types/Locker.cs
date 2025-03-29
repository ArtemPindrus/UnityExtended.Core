using System;
using System.Collections.Generic;
using JetBrains.Annotations;

namespace UnityExtended.Core.Types {
    public class Locker {
        private readonly HashSet<object> locks = new();

        [CanBeNull] public event Action OnUnlocked;
        
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
            
            if (locks.Count == 0) OnUnlocked?.Invoke();
        }

        public static implicit operator bool(Locker locker) => locker.IsLocked;
    }
}