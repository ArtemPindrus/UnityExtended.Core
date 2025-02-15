using UnityEngine;
using UnityEngine.Audio;

namespace UnityExtended.Core.Utilities.AudioSourceBetter {
    [CreateAssetMenu]
    public class AudioResourceBetter : ScriptableObject {
        public AudioResource resource;
        public float volume;

        public static implicit operator AudioResource(AudioResourceBetter a) => a.resource;
        public static implicit operator AudioResourceBetter(AudioResource a) {
            var instance = CreateInstance<AudioResourceBetter>();
            instance.resource = a;
            instance.volume = 1;

            return instance;
        }
    }
}