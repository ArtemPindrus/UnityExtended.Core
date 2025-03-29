using System.Linq;
using UnityEditor;
using UnityEngine;
using UnityExtended.Core.Editor.EditorTools;
using UnityExtended.Core.Utilities.AudioSourceBetter;

namespace UnityExtended.Core.Editor {
    public class AudioClipsPostprocessor : AssetPostprocessor {
        [MenuItem("UnityExtended/Delete empty AudioResourceBetter")]
        private static void DeleteEmptyAudioResourceBetter() {
            var deleted = AssetDatabaseHelper.LoadAllAssetsOfType<AudioResourceBetter>()
                .Where(x => x.resource == null);

            foreach (var d in deleted) {
                string name = d.name;
            
                if (AssetDatabase.DeleteAsset(AssetDatabase.GetAssetPath(d))) {
                    Debug.Log($"Deleted {name}.");
                }
            }
        }
    
        private static void OnPostprocessAllAssets(string[] importedAssets, string[] deletedAssets, string[] movedAssets,
            string[] movedFromAssetPaths) {
            var importedAudioClips = importedAssets
                .Select(AssetDatabase.LoadAssetAtPath<Object>)
                .OfType<AudioClip>();

            if (importedAudioClips.Any()) ToAudioResourceBetterWindow.Create(importedAudioClips.ToList());
        }
    }
}
