using Cysharp.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.UIElements;
using Image = UnityEngine.UI.Image;

#if UNITYEXTENDED_USE_LITMOTION
using LitMotion;
#endif

namespace UnityExtended.Core.Extensions {
    public static class UIExtensions {
        public static void SetAlpha(this Image image, float alpha) {
            Color currentColor = image.color;
            currentColor.a = alpha;
            image.color = currentColor;
        }

#if UNITYEXTENDED_USE_LITMOTION
        public static async UniTask Blink(this Image image, float duration, float hold) {
            await image.TweenAlpha(0, 1, duration);
            await UniTask.WaitForSeconds(hold);
            await image.TweenAlpha(1, 0, duration);
        }

        public static async UniTask TweenAlpha(this Image image, float from, float to, float duration) {
            await LMotion.Create(from, to, duration)
                .WithCancelOnError()
                .Bind(image, (f, i) => SetAlpha(i, f));
        }
        
        public static async UniTask TweenAlpha(this TextMeshProUGUI text, float from, float to, float duration) {
            await LMotion.Create(from, to, duration)
                .WithCancelOnError()
                .Bind(text, (f, i) => text.alpha = f);
        }
#endif
    }
}