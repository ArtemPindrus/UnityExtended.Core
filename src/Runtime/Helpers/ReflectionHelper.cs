using System;
using System.Reflection;

namespace UnityExtended.Core.External.UnityExtended.Core.Helpers {
    public static class ReflectionHelper {
        #nullable enable
        public static string BeautifyBackingFieldName(string name) => name.Replace("<", "").Replace(">k__BackingField", "");

        public static bool TryGetCustomAttribute<T>(this MemberInfo memberInfo, out T attribute) where T : Attribute {
            attribute = memberInfo.GetCustomAttribute<T>();

            return attribute != null;
        }
    }
}