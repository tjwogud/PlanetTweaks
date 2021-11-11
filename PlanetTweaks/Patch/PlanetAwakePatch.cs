using HarmonyLib;
using UnityEngine;

namespace PlanetTweaks.Patch
{
    [HarmonyPatch(typeof(scrPlanet), "Awake")]
    public static class PlanetAwakePatch
    {
        public static void Postfix(scrPlanet __instance)
        {
            var renderer = new GameObject().AddComponent<SpriteRenderer>();
            SpriteRenderer faceRenderer = __instance.faceDetails;
            renderer.sortingOrder = faceRenderer.sortingOrder + 1;
            renderer.sortingLayerID = faceRenderer.sortingLayerID;
            renderer.sortingLayerName = faceRenderer.sortingLayerName;
            renderer.transform.parent = __instance.transform;
        }
    }
}
