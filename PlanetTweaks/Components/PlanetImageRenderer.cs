using PlanetTweaks.Utils;
using System.Diagnostics;
using UnityEngine;

namespace PlanetTweaks.Components
{
    [RequireComponent(typeof(SpriteRenderer))]
    public class PlanetImageRenderer : MonoBehaviour
    {
        private scrPlanet planet;
        private SpriteRenderer renderer;

        private void Awake()
        {
            planet = GetComponent<scrPlanet>();
            renderer = planet.GetRenderer();
            renderer.enabled = true;
            renderer.transform.position = planet.transform.position;
        }

        private void Update()
        {
            if (planet != null)
                renderer.color = (planet.isRed ? Main.Settings.redColor : Main.Settings.blueColor) ? PlanetUtils.GetColor(planet.isRed) : Color.white;
        }

        public class PlanetPNGRenderer : PlanetImageRenderer
        {
            public Sprite sprite;

            private void LateUpdate()
            {
                renderer.sprite = sprite;
            }
        }

        public class PlanetGIFRenderer : PlanetImageRenderer
        {
            public GifImage gif;
            private Stopwatch stop;
            private long offset;

            private void Start()
            {
                stop = new Stopwatch();
                stop.Start();
            }

            private void LateUpdate()
            {
                long elapsed = stop.ElapsedMilliseconds + offset;
                if (elapsed >= gif.Length)
                {
                    stop.Restart();
                    elapsed -= gif.Length;
                    offset = elapsed;
                }
                renderer.sprite = gif.GetFrameAt(elapsed);
            }
        }

        public class PlanetMultiImageRenderer : PlanetImageRenderer
        {
            public Sprite[] sprites;

            private void LateUpdate()
            {
                
            }
        }
    }
}
