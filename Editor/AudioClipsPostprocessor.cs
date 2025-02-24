using System.Linq;
using UnityEditor;
using UnityEngine;

public class AudioClipsPostprocessor : AssetPostprocessor {
    private static void OnPostprocessAllAssets(string[] importedAssets, string[] deletedAssets, string[] movedAssets,
        string[] movedFromAssetPaths) {
        var importedAudioClips = importedAssets
            .Select(AssetDatabase.LoadAssetAtPath<Object>)
            .OfType<AudioClip>();

        if (importedAudioClips.Any()) ToAudioResourceBetterWindow.Create(importedAudioClips.ToList());
    }
}
