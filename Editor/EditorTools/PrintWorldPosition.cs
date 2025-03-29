using UnityEditor;
using UnityEngine;
using UnityExtended.Core.Extensions;

namespace UnityExtended.Core.Editor.EditorTools {
    public static class PrintWorldPosition {
        [MenuItem(EditorToolsConst.GameObjectPath + "Print world position")]
        public static void Print() {
            Transform transform = Selection.activeTransform;
        
            transform.position.LogAccurate(transform.gameObject.name);
        }
    }
}