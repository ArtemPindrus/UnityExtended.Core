using System;

namespace UnityExtended.Generators.Attributes {
    [AttributeUsage(AttributeTargets.Field)]
    public class StartFoldoutGroupAttribute : Attribute {
        public StartFoldoutGroupAttribute(string groupName){}
    }
}