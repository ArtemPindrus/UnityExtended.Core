using System;
using EditorAttributes;

namespace UnityExtended.Generators.Attributes {
    /// <summary>
    /// Stop attaching <see cref="FoldoutGroupAttribute"/> at THIS field (including).
    /// </summary>
    [AttributeUsage(AttributeTargets.Field)]
    public class EndFoldoutGroupAttribute : Attribute {
        
    }
}