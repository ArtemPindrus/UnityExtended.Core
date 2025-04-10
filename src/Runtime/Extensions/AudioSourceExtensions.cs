using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.Audio;

#if UNITYEXTENDED_USE_LITMOTION
using LitMotion;
#endif

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

        public static async UniTask PlayAndWait(this AudioSource source, AudioResource? resource = null, int delta = 0) {
            if (resource != null) source.Play(resource);
            else source.Play();

            await UniTask.WaitForSeconds(source.clip.length + delta);
        }
        
#if UNITYEXTENDED_USE_LITMOTION
        public static async UniTask TweenVolume(this AudioSource source,float from, float to, float duration) {
            await LMotion.Create(from, to, duration)
                .WithCancelOnError()
                .Bind(source, (f, i) => source.volume = f);
        }
        
        public static async UniTask TweenVolume(this AudioSource source, float to, float duration) {
            await TweenVolume(source, source.volume, to, duration);
        }
#endif
    }
}
