using HarmonyLib;
using PlanetTweaks.Utils;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;

namespace PlanetTweaks.Patch
{
    [HarmonyPatch(typeof(scrPlanet), "Awake")]
    public static class PlanetAwakePatch
    {
        public static void Postfix(scrPlanet __instance)
        {
            if (__instance.transform.Find("PlanetTweaksRenderer"))
                return;
            var renderer = new GameObject("PlanetTweaksRenderer").AddComponent<SpriteRenderer>();
            renderer.sortingOrder = __instance.sprite.sortingOrder + 1;
            renderer.sortingLayerID = __instance.faceDetails.sortingLayerID;
            renderer.sortingLayerName = __instance.faceDetails.sortingLayerName;
            renderer.transform.parent = __instance.transform;
            if (__instance.isRed)
            {
                renderer.transform.localScale = new Vector3(Main.Settings.redSize, Main.Settings.redSize);
                if (Main.Settings.redColor)
                    renderer.color = ColorUtils.GetColor(true);
                Main.Logger.Log("RedPlanet " + __instance.name);
            }
            else if (!__instance.isExtra)
            {
                renderer.transform.localScale = new Vector3(Main.Settings.blueSize, Main.Settings.blueSize);
                if (Main.Settings.blueColor)
                    renderer.color = ColorUtils.GetColor(false);
                Main.Logger.Log("BluePlanet " + __instance.name);
            }
            renderer.enabled = __instance.sprite.enabled;
        }
    }

    [HarmonyPatch(typeof(scrPlanet), "LoadPlanetColor")]
    public static class LoadPlanetColorPatch
    {
        public static void Postfix(scrPlanet __instance)
        {
            if (__instance.sprite.enabled)
                if (__instance.isRed)
                    Sprites.RedSelected = Sprites.RedSelected;
                else if (!__instance.isExtra)
                    Sprites.BlueSelected = Sprites.BlueSelected;
                else
                {
                    Sprites.ThirdSelected = Sprites.ThirdSelected;
                    __instance.SetPlanetColor(Main.Settings.ThirdColor());
                    __instance.SetTailColor(Main.Settings.ThirdColor());
                }
            __instance.transform.GetComponentsInChildren<SpriteRenderer>().Last().enabled = __instance.sprite.enabled;
        }
    }

    [HarmonyPatch(typeof(scrController), "Awake")]
    [HarmonyPatch(typeof(scrController), "ColorPlanets")]
    public static class ThirdPlanetPatch
    {
        public static void Postfix()
        {
            Sprites.ThirdSelected = Sprites.ThirdSelected;
            scrController.instance.allPlanets[2].SetPlanetColor(Main.Settings.ThirdColor());
            scrController.instance.allPlanets[2].SetTailColor(Main.Settings.ThirdColor());
            SpriteRenderer renderer = scrController.instance.allPlanets[2].transform.GetComponentsInChildren<SpriteRenderer>().Last();
            renderer.enabled = scrController.instance.allPlanets[2].sprite.enabled;
            renderer.transform.localScale = new Vector3(Main.Settings.thirdSize, Main.Settings.thirdSize);
            if (Main.Settings.thirdColor)
                renderer.color = Main.Settings.ThirdColor();
            else
                renderer.color = Color.white;
        }
    }

    [HarmonyPatch(typeof(scrPlanet), "DisableParticles")]
    public static class DisableParticlesPatch
    {
        public static void Postfix(scrPlanet __instance)
        {
            if (__instance.dummyPlanets)
                Object.Destroy(__instance.GetComponentsInChildren<SpriteRenderer>().Last());
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
