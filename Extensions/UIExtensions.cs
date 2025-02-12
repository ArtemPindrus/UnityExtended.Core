using Cysharp.Threading.Tasks;
using LitMotion;
using TMPro;
using UnityEngine;
using UnityEngine.UIElements;
using Image = UnityEngine.UI.Image;

namespace UnityExtended.Core.Extensions {
    public static class UIExtensions {
        public static void SetAlpha(this Image image, float alpha) {
            Color currentColor = image.color;
            currentColor.a = alpha;
            image.color = currentColor;
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
    }
}