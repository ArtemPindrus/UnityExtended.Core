using System;
using System.Collections.Generic;
using UnityEngine;

#nullable enable
namespace UnityExtended.Core.Types {
    public class UnityCollection<T> where T : Component {
        private readonly Dictionary<int,T> set;
        private readonly Func<T,int> itemToKey;

        public UnityCollection(bool initImmediately, Func<T, int> itemToKey, int initialCapacity) {
            this.itemToKey = itemToKey;
            set = new(initialCapacity);

            if (initImmediately) {
                ScanAndAdd();
            }
        }

        public bool TryGetValue(int key, out T value) {
            if (set.TryGetValue(key, out value)) {
                if (value == null) { // remove key-value if Native object is destroyed
                    set.Remove(key);
                
                    ScanAndAdd(); // rescan and try again
                    return set.TryGetValue(key, out value);
                } else return true;
            }
            else {
                ScanAndAdd(); // rescan and try again
                return set.TryGetValue(key, out value);
            }
        }

        private void ScanAndAdd() {
            T[] found = UnityEngine.Object.FindObjectsByType<T>(FindObjectsInactive.Include, FindObjectsSortMode.None);

            foreach (var item in found) set.TryAdd(itemToKey(item), item);
        }
    }
}