using Cysharp.Threading.Tasks;
using UnityEngine.Playables;

namespace UnityExtended.Core.Extensions {
    public static class TimelineExtensions {
        public static async UniTask PlayAndWait(this PlayableDirector director, float delta = 0) {
            director.Play();

            await UniTask.WaitForSeconds((float)director.duration + delta);
        }
    }
}