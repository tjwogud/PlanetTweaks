using HarmonyLib;
using System.Threading.Tasks;
using UnityEngine;

namespace PlanetTweaks.Patch
{
    [HarmonyPatch(typeof(scrPlanet), "Awake")]
    public static class PlanetAwakePatch
    {
        public static void Postfix(scrPlanet __instance)
        {
            var renderer = new GameObject().AddComponent<SpriteRenderer>();
            renderer.sortingOrder = 999;
            renderer.sortingLayerID = __instance.faceDetails.sortingLayerID;
            renderer.sortingLayerName = __instance.faceDetails.sortingLayerName;
            renderer.transform.parent = __instance.transform;
            Task.Delay(100).ContinueWith(delegate
            {
                if (__instance.isRed)
                    Sprites.RedSelected = Sprites.RedSelected;
                else
                    Sprites.BlueSelected = Sprites.BlueSelected;
            });
        }
    }
}
