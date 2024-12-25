using NUnit.Framework;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Utilities;
using UnityExtended.Core.Types;

namespace UnityExtended.Core.Tests {
    public class TestInputActionCollection : IInputActionCollection2, IDisposable {
        public IEnumerable<InputBinding> bindings => throw new NotSupportedException();

        public InputBinding? bindingMask { get => throw new NotSupportedException(); set => throw new NotSupportedException(); }
        public ReadOnlyArray<InputDevice>? devices { get => throw new NotSupportedException(); set => throw new NotSupportedException(); }

        public ReadOnlyArray<InputControlScheme> controlSchemes => throw new NotSupportedException();

        public bool Contains(InputAction action) {
            throw new NotSupportedException();
        }

        public void Disable() {
        }

        public void Dispose() {
        }

        public void Enable() {
        }

        public InputAction FindAction(string actionNameOrId, bool throwIfNotFound = false) {
            throw new NotSupportedException();
        }

        public int FindBinding(InputBinding mask, out InputAction action) {
            throw new NotSupportedException();
        }

        public IEnumerator<InputAction> GetEnumerator() {
            throw new NotSupportedException();
        }

        IEnumerator IEnumerable.GetEnumerator() {
            throw new NotSupportedException();
        }
    }

    public class InputSingletonsManagerTests {
        [Test]
        public void GettingFromTheSameTypeMultipleTimesShouldGetTheSameInstance() {
            InputSingletonsManager.Init();

            var first = InputSingletonsManager.GetInstance<TestInputActionCollection>();
            var second = InputSingletonsManager.GetInstance<TestInputActionCollection>();
            var third = InputSingletonsManager.GetInstance<TestInputActionCollection>();

            Assert.That(first, Is.EqualTo(second));
            Assert.That(second, Is.EqualTo(third));
        }
    }
}