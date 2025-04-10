using System;
using UnityEngine.UIElements;
using UnityExtended.Core.Generators.Attributes;

#nullable enable
namespace UnityExtended.Core.Generators.Attributes {
    /// <summary>
    /// Use to define how a custom type should be displayed without serialization.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct)]
    public class DisplayItemAttribute : Attribute {
        public readonly Type DisplayBagType;

        /// <summary>
        /// Creates a <see cref="DisplayItemAttribute"/> instance.
        /// </summary>
        /// <param name="displayBagType">Type that inherits from <see cref="IDisplayBag{T}"/> and uses decorated type as T parameter.</param>
        public DisplayItemAttribute(Type displayBagType) {
            DisplayBagType = displayBagType;
        }
    }
    
    /// <summary>
    /// Describes how T should be displayed without serialization.
    /// </summary>
    /// <typeparam name="T">Type that should be displayed.</typeparam>
    public interface IDisplayBag<T> {
        /// <summary>
        /// Created visual representation for data.
        /// </summary>
        /// <param name="name">Name of the control.</param>
        /// <returns></returns>
        public VisualElement CreateVisualElement(string name);

        public void Update(T? data);
    }
}