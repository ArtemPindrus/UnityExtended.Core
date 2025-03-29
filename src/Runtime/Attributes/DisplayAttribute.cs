using System;

namespace UnityExtended.Generators.Attributes {
    /// <summary>
    /// Use to display field in inspector without serialization.
    /// </summary>
    [AttributeUsage(AttributeTargets.Field)]
    public class DisplayAttribute : Attribute{}
}