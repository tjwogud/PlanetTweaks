using HarmonyLib;
using PlanetTweaks.Utils;
using System;

namespace PlanetTweaks.Patch
{
    [HarmonyPatch(typeof(Persistence), "SetPlayerColor")]
    public static class PlayerColorPatch
    {
        public static void Postfix(bool red)
        {
            try
            {
                if (red && Main.Settings.redColor)
                {
                    scrController.instance.redPlanet.GetOrAddRenderer().color = ColorUtils.GetRealColor(true);
                }
                else if (!red && Main.Settings.blueColor)
                {
                    scrController.instance.bluePlanet.GetOrAddRenderer().color = ColorUtils.GetRealColor(false);
                }
            }
            catch (Exception) {
            }
        }
    }
}
