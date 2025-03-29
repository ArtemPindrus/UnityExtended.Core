using UnityEngine;

namespace UnityExtended.Core.Helpers {
    public static class ColorHelper {
        public static Color From255RGBA(float rgb, float a = 255) => 
            new Color(rgb / 255, rgb / 255, rgb / 255, a / 255);
        
        public static Color From255RGBA(float r, float g, float b, float a = 255) 
            => new Color(r / 255, g / 255, b / 255, a / 255);
    }
}