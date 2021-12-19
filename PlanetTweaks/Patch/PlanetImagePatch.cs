using HarmonyLib;
using System.Linq;
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
            if (__instance.isRed)
                renderer.transform.localScale = new Vector3(Main.Settings.redSize, Main.Settings.redSize);
            else
                renderer.transform.localScale = new Vector3(Main.Settings.blueSize, Main.Settings.blueSize);
        }
    }

    [HarmonyPatch(typeof(scrPlanet), "LoadPlanetColor")]
    public static class LoadPlanetColorPatch
    {
        public static void Postfix(scrPlanet __instance)
        {
            if (__instance.isRed)
                Sprites.RedSelected = Sprites.RedSelected;
            else
                Sprites.BlueSelected = Sprites.BlueSelected;
        }
    }

    [HarmonyPatch(typeof(scrPlanet), "Destroy")]
    public static class PlanetDestroyPatch
    {
        public static void Postfix(scrPlanet __instance)
        {
            __instance.transform.GetComponentsInChildren<SpriteRenderer>().Last().enabled = false;
        }
    }

    [HarmonyPatch(typeof(scrPlanet), "Die")]
    public static class PlanetDiePatch
    {
        public static void Postfix(scrPlanet __instance)
        {
            __instance.transform.GetComponentsInChildren<SpriteRenderer>().Last().enabled = false;
        }
    }
}
