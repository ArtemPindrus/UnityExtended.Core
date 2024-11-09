#nullable enable
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace UnityExtended.Core.Types {
    public static class InputSingletonsManager {
        private static Dictionary<Type, IInputActionCollection2>? instanceKeyedByType;

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
        private static void Init() {
            if (instanceKeyedByType != null) {
                foreach (var value in instanceKeyedByType.Values) {
                    value.Disable();
                    ((IDisposable)value).Dispose();
                } 
            }

            instanceKeyedByType = new();
        }

        public static T GetInstance<T>() where T : IInputActionCollection2, new() {
            if (instanceKeyedByType.TryGetValue(typeof(T), out IInputActionCollection2 instance)) {
                return (T)instance;
            } else {
                T newInstance = new T();
                instanceKeyedByType.Add(typeof(T), newInstance);
                newInstance.Enable();

                return newInstance;
            }
        }
    }
}
