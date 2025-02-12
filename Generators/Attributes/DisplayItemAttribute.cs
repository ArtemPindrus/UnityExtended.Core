using System;

namespace UnityExtended.Generators.Attributes {
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct)]
    public class DisplayItemAttribute : Attribute {
        public DisplayItemAttribute(Type containerType, params string[] fieldsAndProperties){}
    }
}