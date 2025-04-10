using System;

namespace UnityExtended.Core.Generators.Attributes {
    [AttributeUsage(AttributeTargets.Field)]
    public class GetComponentAttribute : Attribute {
        public GetComponentAttribute(In @in = In.Self, bool plural = false){}
    }
}
