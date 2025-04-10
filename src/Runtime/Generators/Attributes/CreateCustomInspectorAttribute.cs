using System;

namespace UnityExtended.Core.Generators.Attributes {
    /// <summary>
    /// Marker that will force generator to create a custom inspector for this type with all inspector features included.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class)]
    public class CreateCustomInspectorAttribute : Attribute {
        
    }
}