using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using UnityEngine;

namespace PlanetTweaks.Utils
{
    public static class SpriteUtils
    {
        public static int Size => 200;

        public static Texture2D ResizeFix(this Texture2D texture)
        {
            int targetWidth
                = texture.width >= texture.height
                ? Size
                : (int)((float)Size / texture.height * texture.width);
            int targetHeight
                = texture.width >= texture.height
                ? (int)((float)Size / texture.width * texture.height)
                : Size;
            Texture2D result = new Texture2D(targetWidth, targetHeight, texture.format, true);
            UnityEngine.Color[] pixels = result.GetPixels(0);
            float incX = 1.0f / targetWidth;
            float incY = 1.0f / targetHeight;
            for (int pixel = 0; pixel < pixels.Length; pixel++)
                pixels[pixel] = texture.GetPixelBilinear(incX * ((float)pixel % targetWidth), incY * ((float)Mathf.Floor(pixel / targetWidth)));
            result.SetPixels(pixels, 0);
            result.Apply();
            return result;
        }

        public static Sprite ToSprite(this byte[] data)
        {
            Texture2D texture = new Texture2D(0, 0);
            if (texture.LoadImage(data))
            {
                texture = texture.ResizeFix();
                Rect rect = new Rect(0, 0, texture.width, texture.height);
                return Sprite.Create(texture, rect, new Vector2(0.5f, 0.5f));
            }
            return null;
        }

        public static Sprite ToSprite(this Image image)
        {
            MemoryStream stream = new MemoryStream();
            image.Save(stream, ImageFormat.Png);
            return stream.GetBuffer().ToSprite();
        }

        public static Rect Fix(this Rect rect, int width = 1920, int height = 1080)
        {
            float xMultiply = (float)Screen.width / width;
            float yMultiply = (float)Screen.height / height;
            Rect newRect = new Rect(rect.x, rect.y, rect.width, rect.height);
            newRect.x *= xMultiply;
            newRect.y *= yMultiply;
            newRect.width *= xMultiply;
            newRect.height *= yMultiply;
            return newRect;
        }

        public static Texture2D Copy(this Texture2D texture)
        {
            if (texture == null)
                return null;
            RenderTexture renderTex = RenderTexture.GetTemporary(
               texture.width,
               texture.height,
               0,
               RenderTextureFormat.Default,
               RenderTextureReadWrite.Linear);

            UnityEngine.Graphics.Blit(texture, renderTex);
            RenderTexture previous = RenderTexture.active;
            RenderTexture.active = renderTex;
            Texture2D readableText = new Texture2D(texture.width, texture.height);
            readableText.ReadPixels(new Rect(0, 0, renderTex.width, renderTex.height), 0, 0);
            readableText.Apply();
            RenderTexture.active = previous;
            RenderTexture.ReleaseTemporary(renderTex);
            return readableText;
        }
    }
}
