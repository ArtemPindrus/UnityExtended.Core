using System.Linq;
using UnityEditor;
using UnityEngine;

public class AudioClipsPostprocessor : AssetPostprocessor {
    private static void OnPostprocessAllAssets(string[] importedAssets, string[] deletedAssets, string[] movedAssets,
        string[] movedFromAssetPaths) {
        var importedAudioClips = importedAssets
            .Select(AssetDatabase.LoadAssetAtPath<Object>)
            .OfType<AudioClip>()
            .ToList();

        if (importedAudioClips.Any()) ToAudioSourceBetterWindow.Create(importedAudioClips);
    }
}
