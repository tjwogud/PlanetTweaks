using HarmonyLib;
using UnityEngine;

namespace PlanetTweaks.Patch
{
    [HarmonyPatch(typeof(scrPlanet), "Awake")]
    public static class AwakePatch
    {
        public static void Postfix(scrPlanet __instance, Sprite ___whiteSprite, RuntimeAnimatorController ___altController)
        {
            if (__instance.goldPlanet)
            {
                Transform spark = Object.Instantiate(__instance.goldPlanet.transform, __instance.transform);
                spark.gameObject.SetActive(false);
                spark.name = "SparkPlanet";
                foreach (Transform child in spark)
                {
                    if (child.name.Contains("Planet"))
                    {
                        child.GetComponent<SpriteRenderer>().sprite = ___whiteSprite;
                        child.GetComponent<Animator>().runtimeAnimatorController = ___altController;
                    }
                    child.name = child.name.Replace("Gold", "Spark");
                }
            }
            if (__instance.dummyPlanets)
            {
            } else
            {
            }
            //__instance.GetRenderer();
        }
    }

    [HarmonyPatch(typeof(scrPlanet), "LoadPlanetColor")]
    public static class LoadPlanetColorPatch
    {
        public static void Postfix(scrPlanet __instance)
        {
            //if (__instance.sprite.enabled)
                if (__instance.isRed)
                    Sprites.RedSelected = Sprites.RedSelected;
                else
                    Sprites.BlueSelected = Sprites.BlueSelected;
        }
    }

    [HarmonyPatch(typeof(scrPlanet), "Destroy")]
    public static class DestroyPatch
    {
        public static void Postfix(scrPlanet __instance)
        {
            __instance.GetRenderer().enabled = false;
        }
    }

    [HarmonyPatch(typeof(scrPlanet), "Die")]
    public static class DiePatch
    {
        public static void Postfix(scrPlanet __instance)
        {
            __instance.GetRenderer().enabled = false;
        }
    }
}
