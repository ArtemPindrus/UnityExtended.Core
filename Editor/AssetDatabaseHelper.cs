using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;
using UnityExtended.Core.Utilities.AudioSourceBetter;

namespace External.UnityExtended.Core.Editor {
    public static class AssetDatabaseHelper {
        public static IEnumerable<T> LoadAllAssetsOfType<T>() where T : Object {
            var assets = AssetDatabase.FindAssets($"t:{typeof(T)}");
            
            foreach (var e in assets) {
                var assetPath = AssetDatabase.GUIDToAssetPath(e);
                yield return AssetDatabase.LoadAssetAtPath<T>(assetPath);
            }
        }

        public static T CreateAndLoadAsset<T>(T obj, string path) where T : Object {
            AssetDatabase.CreateAsset(obj, path);
            var asset = AssetDatabase.LoadAssetAtPath<T>(path);

            return asset;
        }

    }
}