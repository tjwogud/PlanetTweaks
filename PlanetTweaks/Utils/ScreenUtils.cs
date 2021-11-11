using UnityEngine;

namespace PlanetTweaks.Utils
{
    public static class ScreenUtils
    {
        public static Rect Fix(this Rect rect, int width = 1920, int height = 1080)
        {
            float xMultiply = (float)Screen.width / width;
            float yMultiply = (float)Screen.height / height;
            Rect newRect = new Rect(rect);
            newRect.x *= xMultiply;
            newRect.y *= yMultiply;
            newRect.width *= xMultiply;
            newRect.height *= yMultiply;
            return newRect;
        }
    }
}
