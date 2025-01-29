using System;

namespace UnityExtended.Generators.Attributes {
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
    public class HandleInputAttribute : Attribute {
        public HandleInputAttribute(Type inputActionsType) {
        }
    }
}