using UnityEngine;

namespace PlanetTweaks.Components
{
    public class RendererController : MonoBehaviour
    {
        private scrPlanet planet;
        private SpriteRenderer renderer;

        private void Awake()
        {
            planet = GetComponentInParent<scrPlanet>();
            renderer = GetComponent<SpriteRenderer>();
        }

        private void Update()
        {
            if (!planet)
                planet = GetComponentInParent<scrPlanet>();
            if (!renderer)
                renderer = GetComponent<SpriteRenderer>();
            if (planet && renderer)
                renderer.enabled = planet.sprite.enabled;
        }
    }
}
