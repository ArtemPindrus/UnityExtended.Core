using System;
using System.Linq;
using JetBrains.Annotations;
using UnityEditor;
using UnityEngine;
using UnityExtended.Core.Utilities.AudioSourceBetter;

namespace UnityExtended.Core.EditorTools {
    public static class ToAudioResourceBetter {
        public const string MenuItemPath = "Assets/Extensions/Convert to AudioResourceBetter";
        
        public static bool CreateAudioResourceBetterAsset(AudioClip clip, [CanBeNull] out AudioResourceBetter audioResourceBetter) {
            if (clip == null) {
                audioResourceBetter = null;
                return false;
            }

            string path = AssetDatabase.GetAssetPath(clip);
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
        private static void ConvertToAudioResourceBetter() {
            foreach (var clip in Selection.objects.OfType<AudioClip>()) {
                CreateAudioResourceBetterAsset(clip, out _);
            }
        }

        [MenuItem(MenuItemPath, true)]
        private static bool ConvertToAudioResourceBetterValidate() => Selection.objects.All(x => x is AudioClip);
    }
}