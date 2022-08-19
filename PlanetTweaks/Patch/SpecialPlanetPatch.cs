using HarmonyLib;
using PlanetTweaks.Utils;

namespace PlanetTweaks.Patch
{
    [HarmonyPatch(typeof(scrPlanet), "DisableAllSpecialPlanets")]
    public static class DisableAllSpecialPlanetsPatch
    {
        public static void Postfix(scrPlanet __instance)
        {
            if (__instance.transform.Find("SparkPlanet").gameObject.activeSelf)
                __instance.RemoveSpark();

        }
    }

    [HarmonyPatch(typeof(scrPlanet), "ToggleAllSpecialPlanetsSamuraiMode")]
    public static class ToggleAllSpecialPlanetsSamuraiModePatch
    {
        public static void Postfix(scrPlanet __instance, bool on)
        {
            __instance.Method("ToggleSpecialPlanetSamuraiMode", new object[] { __instance.transform.Find("SparkPlanet").gameObject, on, "Spark" });
        }
    }
}
