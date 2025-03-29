using System;
using System.IO;
using System.Linq;
using External.UnityExtended.Core.Editor;
using JetBrains.Annotations;
using UnityEditor;
using UnityEngine;
using UnityExtended.Core.Utilities.AudioSourceBetter;

namespace UnityExtended.Core.Editor.EditorTools {
    public static class ToAudioResourceBetter {
        public const string MenuItemPath = "Assets/Extensions/Convert to AudioResourceBetter";
        public const string AudioResourceBetterPath = "Assets/Resources/AudioResourceBetter/";
        
        public static bool CreateAudioResourceBetterAsset(AudioClip clip, [CanBeNull] out AudioResourceBetter audioResourceBetter) {
            if (clip == null) {
                audioResourceBetter = null;
                return false;
            }

            AudioResourceBetter resourceBetter = clip;

            var assetPath = $"{AudioResourceBetterPath}{clip.name}.asset";

            AudioResourceBetter existing = AssetDatabase.LoadAssetAtPath<AudioResourceBetter>(assetPath);
            if (existing != null) {
                Debug.Log($"{assetPath} already exists.");
                audioResourceBetter = existing;
                return false;
            }
            
            Debug.Log($"{assetPath} was created.");
            audioResourceBetter = AssetDatabaseHelper.CreateAndLoadAsset(resourceBetter, assetPath);

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