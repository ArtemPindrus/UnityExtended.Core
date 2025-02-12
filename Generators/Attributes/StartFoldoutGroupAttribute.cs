using System;
using EditorAttributes;

namespace UnityExtended.Generators.Attributes {
    /// <summary>
    /// Use to attach <see cref="FoldoutGroupAttribute"/> to all the fields starting from THIS field.
    /// </summary>
    [AttributeUsage(AttributeTargets.Field)]
    public class StartFoldoutGroupAttribute : Attribute {
        public StartFoldoutGroupAttribute(string groupName, int propertyOrder = -1){}
    }
}