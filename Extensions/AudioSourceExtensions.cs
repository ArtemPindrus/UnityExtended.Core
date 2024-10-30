using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.Audio;

#nullable enable
namespace UnityExtended.Core.Extensions {
    public static class AudioSourceExtensions {
        public static void Play(this AudioSource source, AudioResource resource) {
            source.resource = resource;

            source.Play();
        }

        public static void PlayRandom(this AudioSource source, AudioResource[] resources) {
            int randomInd = Random.Range(0, resources.Length);
            source.Play(resources[randomInd]);
        }

    }
}
