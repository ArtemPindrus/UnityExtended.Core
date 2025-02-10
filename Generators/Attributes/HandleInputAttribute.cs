using System;

namespace UnityExtended.Generators.Attributes {
    [AttributeUsage(AttributeTargets.Class)]
    public class HandleInputAttribute : Attribute {
        public HandleInputAttribute(params Type[] inputActionsType) {
        }
    }
}