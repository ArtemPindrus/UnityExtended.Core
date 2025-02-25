using System;

namespace UnityExtended.Generators.Attributes {
    [AttributeUsage(AttributeTargets.Field)]
    public class GetComponentAttribute : Attribute {
        public enum In {
            Self,
            Children,
            Parent,
        }
        
        public GetComponentAttribute(In @in = In.Self, bool plural = false){}
    }
}
