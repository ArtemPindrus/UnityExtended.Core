using System;

namespace UnityExtended.Generators.Attributes {
    [AttributeUsage(AttributeTargets.Field)]
    public class GetComponentAheadAttribute : Attribute {
        public GetComponentAheadAttribute(In @in = In.Self, bool plural = false){}
    }
}