using System;
using JetBrains.Annotations;
using UnityEditor;
using UnityEngine;
using UnityExtended.Core.Utilities.AudioSourceBetter;

namespace UnityExtended.Core.EditorTools {
    public static class ToAudioResourceBetter {
        public const string MenuItemPath = "Assets/Extensions/Convert to AudioResourceBetter";

        // TODO: use asset object instead of assetGUIDs and paths
        public static bool CreateAudioResourceBetterForPath(string path, [CanBeNull] out AudioResourceBetter audioResourceBetter) {
            AudioClip clip = AssetDatabase.LoadAssetAtPath<AudioClip>(path);
            if (clip == null) {
                audioResourceBetter = null;
                return false;
            }
        
            AudioResourceBetter resourceBetter = clip;
        
            int dotIndex = path.LastIndexOf(".", StringComparison.Ordinal);
            var assetPath = path.Substring(0, dotIndex) + ".asset";

            AudioResourceBetter existing = AssetDatabase.LoadAssetAtPath<AudioResourceBetter>(assetPath);
            if (existing != null) {
                Debug.Log($"{assetPath} already exists.");
                audioResourceBetter = existing;
                return false;
            }
            
            Debug.Log($"{assetPath} was created.");
            AssetDatabase.CreateAsset(resourceBetter, assetPath);

            audioResourceBetter = AssetDatabase.LoadAssetAtPath<AudioResourceBetter>(assetPath);

            return true;
        }
    
        [MenuItem(MenuItemPath)]
        public static void ConvertToAudioResourceBetter() {
            foreach (var guid in Selection.assetGUIDs) {
                var path = AssetDatabase.GUIDToAssetPath(guid);

                CreateAudioResourceBetterForPath(path, out _);
            }
        }

        [MenuItem(MenuItemPath, true)]
        public static bool ConvertToAudioResourceBetterValidate() {
            if (Selection.assetGUIDs.Length == 0) return false;
        
            bool valid = true;
        
            foreach (var guid in Selection.assetGUIDs) {
                var path = AssetDatabase.GUIDToAssetPath(guid);

                AudioClip clip = AssetDatabase.LoadAssetAtPath<AudioClip>(path);
                if (clip == null) valid = false;
            }

            return valid;
        }
    }
}