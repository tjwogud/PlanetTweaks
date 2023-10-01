using HarmonyLib;
using PlanetTweaks.Components;
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
            SpriteRenderer renderer = Sprites.GetOrAddRenderer(__instance);
            if (__instance.isRed)
            {
                renderer.transform.localScale = new Vector3(Main.Settings.redSize, Main.Settings.redSize);
                if (Main.Settings.redColor)
                    renderer.color = ColorUtils.GetRealColor(true);
            }
            else if (!__instance.isExtra)
            {
                renderer.transform.localScale = new Vector3(Main.Settings.blueSize, Main.Settings.blueSize);
                if (Main.Settings.blueColor)
                    renderer.color = ColorUtils.GetRealColor(false);
            }
        }
    }

    [HarmonyPatch(typeof(scrPlanet), "LoadPlanetColor")]
    public static class LoadPlanetColorPatch
    {
        public static void Postfix(scrPlanet __instance)
        {
            if (__instance.sprite.visible)
                if (__instance.isRed)
                    Sprites.RedSelected = Sprites.RedSelected;
                else if (!__instance.isExtra)
                    Sprites.BlueSelected = Sprites.BlueSelected;
                else
                {
                    Sprites.ThirdSelected = Sprites.ThirdSelected;
                    ColorUtils.SetThirdColor();
                }
        }
    }

    [HarmonyPatch(typeof(scrController), "Awake")]
    [HarmonyPatch(typeof(scrController), "ColorPlanets")]
    public static class ThirdPlanetPatch
    {
        public static void Postfix()
        {
            Sprites.ThirdSelected = Sprites.ThirdSelected;
            ColorUtils.SetThirdColor();
            SpriteRenderer renderer = scrController.instance.allPlanets[2].GetOrAddRenderer();
            renderer.transform.localScale = new Vector3(Main.Settings.thirdSize, Main.Settings.thirdSize);
            if (Main.Settings.thirdColor)
                renderer.color = ColorUtils.GetThirdColor();
            else
                renderer.color = Color.white;
        }
    }
}
