using System;

#nullable enable
namespace UnityExtended.Core.Attributes {
    [AttributeUsage(AttributeTargets.Method)]
    public class ButtonAttribute : Attribute {
        public readonly string? CustomButtonLabel;

        public ButtonAttribute(string? customButtonLabel = null) {
            CustomButtonLabel = customButtonLabel;
        }
    }
}