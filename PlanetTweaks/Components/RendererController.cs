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
            renderer = planet.GetOrAddRenderer();
        }

        private void Update()
        {
            if (!planet)
                planet = GetComponentInParent<scrPlanet>();
            if (!renderer)
                renderer = planet.GetOrAddRenderer();
            if (planet && renderer)
            {
                if (planet.dummyPlanets)
                {
                    Destroy(gameObject);
                    return;
                }
                renderer.enabled = planet.sprite.visible;
            }
        }
    }
}
