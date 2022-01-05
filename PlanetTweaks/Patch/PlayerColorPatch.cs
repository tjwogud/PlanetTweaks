using HarmonyLib;
using PlanetTweaks.Utils;
using System;
using System.Linq;
using UnityEngine;

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
                    scrController.instance.redPlanet.transform.GetComponentsInChildren<SpriteRenderer>().Last().color = ColorUtils.GetColor(true);
                }
                else if (!red && Main.Settings.blueColor)
                {
                    scrController.instance.bluePlanet.transform.GetComponentsInChildren<SpriteRenderer>().Last().color = ColorUtils.GetColor(false);
                }
            }
            catch (Exception) {
            }
        }
    }
}
