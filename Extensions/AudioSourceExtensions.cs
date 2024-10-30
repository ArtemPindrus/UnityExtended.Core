using UnityEngine;
using UnityEngine.Audio;

#nullable enable
namespace UnityExtended.Core.Extensions {
    public static class AudioSourceExtensions {
        public static void PlayRandomResource(this AudioSource source, AudioResource[] resources) {
            int randomInd = Random.Range(0, resources.Length);
            source.resource = resources[randomInd];

            source.Play();
        }
    }
}
