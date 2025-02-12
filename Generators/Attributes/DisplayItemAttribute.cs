using System;
using UnityEngine.UIElements;

namespace UnityExtended.Generators.Attributes {
    /// <summary>
    /// Use to define how a custom type should be displayed without serialization.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct)]
    public class DisplayItemAttribute : Attribute {
        /// <summary>
        /// Create <see cref="DisplayItemAttribute"/>.
        /// </summary>
        /// <param name="containerType">Type that is used to hold all the fields. For example: <see cref="Foldout"/>.</param>
        /// <param name="fieldsAndProperties">Included fields and properties. Will be childrens of <paramref name="containerType"/>.</param>
        public DisplayItemAttribute(Type containerType, params string[] fieldsAndProperties){}
    }
}