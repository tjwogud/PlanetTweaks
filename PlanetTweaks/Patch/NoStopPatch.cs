using HarmonyLib;
using System;
using System.Reflection;
using UnityModManagerNet;

namespace PlanetTweaks.Patch
{
    public static class NoStopPatch
    {
        private static MethodInfo original;

        public static void TryPatch()
        {
            if (original != null)
                Main.Harmony.Unpatch(original, HarmonyPatchType.Postfix, Main.ModEntry.Info.Id);
            original = AccessTools.Method("NoStopMod.InputFixer.HitIgnore.HitIgnoreManager:ShouldBeIgnored");
            if (original != null)
                Main.Harmony.Patch(original, postfix: new HarmonyMethod(typeof(NoStopPatch), "ShouldBeIgnoredPostfix"));
        }

        public static void ShouldBeIgnoredPostfix(int keyCode, ref bool __result)
        {
            if (scrController.isGameWorld || (keyCode != 61000 && keyCode != 61008))
                return;
            float x = (float)Math.Round(scrController.instance.chosenplanet.transform.position.x);
            float y = (float)Math.Round(scrController.instance.chosenplanet.transform.position.y);
            if ((x == 3 || x == -3) && (y >= -18 && y <= -7))
                __result = true;
        }

        [HarmonyPatch(typeof(UnityModManager.ModEntry), "Load")]
        public static class LoadPatch
        {
            public static void Postfix(UnityModManager.ModEntry __instance)
            {
                if (__instance.Info.Id != "NoStopMod")
                    return;
                TryPatch();
            }
        }
    }
}
