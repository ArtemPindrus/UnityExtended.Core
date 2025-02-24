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

        public static void MakeSureFolderExists(string relativePath) {
            string[] pathParts = relativePath.Split('/');
            string parentFolder = "Assets";

            for (var i = 0; i < pathParts.Length; i++) {
                var path = pathParts[i];
                
                if (i == 0 && path == "Assets") continue;
                if (string.IsNullOrWhiteSpace(path)) continue;

                string newFolderName = path;
                string currentPath = Path.Combine(parentFolder, newFolderName);

                if (!AssetDatabase.IsValidFolder(currentPath)) {
                    AssetDatabase.CreateFolder(parentFolder, newFolderName);
                    Debug.Log($"Created {currentPath}.");
                }

                parentFolder = currentPath;
            }

            AssetDatabase.CreateFolder(Path.GetDirectoryName(relativePath), relativePath);
        }
    }
}