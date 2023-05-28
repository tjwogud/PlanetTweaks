using PlanetTweaks.Utils;
using UnityEngine;

namespace PlanetTweaks.Components
{
    public class GifRenderer : MonoBehaviour
    {
        public GifImage Image { get; set; }
        public SpriteRenderer Renderer { get; set; }

        private float timePassed;
        private float offset;

        public void Update()
        {
            if (Image == null || !Renderer || !Renderer.enabled)
                return;
            long elapsed = (long)(((timePassed += Time.unscaledDeltaTime) * 1000) + offset);
            if (elapsed >= Image.Length)
            {
                timePassed = 0;
                elapsed -= Image.Length;
                offset = elapsed;
            }
        }

        public void LateUpdate()
        {
            if (Image == null || !Renderer || !Renderer.enabled)
                return;
            Renderer.sprite = Image.GetFrameAt((long)((timePassed * 1000) + offset));
        }
    }
}
