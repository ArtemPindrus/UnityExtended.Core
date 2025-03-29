using System;
using System.Threading;
using System.Threading.Tasks;
using Cysharp.Threading.Tasks;
using JetBrains.Annotations;
using UnityEngine;
using UnityExtended.Core.Attributes;
using UnityExtended.Core.Extensions;
using Random = UnityEngine.Random;
using TaskExtensions = UnityExtended.Core.Extensions.TaskExtensions;

namespace UnityExtended.Core.Utilities.AudioSourceBetter {
    [RequireComponent(typeof(AudioSource))]
    public class AudioSourceBetter : MonoBehaviour {
        /// <summary>
        /// State of the clip that finished playing.
        /// </summary>
        public enum FinishPlayState {
            /// <summary>
            /// Clip was played fully.
            /// </summary>
            Completed,
            
            /// <summary>
            /// Clip play was canceled.
            /// </summary>
            Canceled,
        }
        
        [CanBeNull] private CancellationTokenSource playCancellation;
        
        /// <summary>
        /// Invoked when resource is played.
        /// </summary>
        [CanBeNull] public event Action<AudioSource, AudioResourceBetter> StartedPlay;
        
        /// <summary>
        /// Invoked when resource stops playing.
        /// </summary>
        [CanBeNull] public event Action<AudioSource, AudioResourceBetter, FinishPlayState> FinishedPlay;
    
        [HideInInspector]
        public AudioSource audioSource;

        private void Awake() {
            audioSource = gameObject.GetComponent<AudioSource>();
        }

        private void Reset() {
            audioSource = gameObject.GetOrAddComponent<AudioSource>();
            audioSource.volume = 1;
            audioSource.playOnAwake = false;
        }

        public async UniTask Play(AudioResourceBetter audioResource) {
            TaskExtensions.CancelAndInstantiate(ref playCancellation);
            
            audioSource.resource = audioResource.resource;
            audioSource.volume = audioResource.volume;

            audioSource.Play();
        
            StartedPlay?.Invoke(audioSource, audioResource);

            var finishPlayState = FinishPlayState.Completed;
            
            try {
                await UniTask.WaitForSeconds(audioSource.clip.length, true, PlayerLoopTiming.Update,
                    playCancellation!.Token);
                finishPlayState = FinishPlayState.Completed;
            }
            catch (TaskCanceledException) {
                audioSource.Stop();
                finishPlayState = FinishPlayState.Canceled;
                return;
            }
            finally {
                FinishedPlay?.Invoke(audioSource, audioResource, finishPlayState);
            }
        }

        public async UniTask PlayRandom(AudioResourceBetter[] audioResources) {
            int randomInd = Random.Range(0, audioResources.Length);

            await Play(audioResources[randomInd]);
        }

        [Button]
        public void Stop() {
            TaskExtensions.CancelAndInstantiate(ref playCancellation);
        }
    }
}