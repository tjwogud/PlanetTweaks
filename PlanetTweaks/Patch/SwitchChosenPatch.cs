using DG.Tweening;
using HarmonyLib;
using System;
using UnityEngine;

namespace PlanetTweaks.Patch
{
    [HarmonyPatch(typeof(scrPlanet), "SwitchChosen")]
    public static class SwitchChosenPatch
    {
        public static bool Prefix(scrPlanet __instance, ref scrPlanet __result)
        {
            if (scnLevelSelect.instance == null)
                return true;
            float x = (float)Math.Round(__instance.transform.position.x);
            float y = (float)Math.Round(__instance.transform.position.y);
            if ((x == 3 || x == -3) && (y >= -18 && y <= -7) && (Input.GetKey(KeyCode.UpArrow) || Input.GetKey(KeyCode.DownArrow)))
            {
                if (x == 3)
                {
                    __instance.currfloor = IntroFloorPatch.leftMovingFloor;
                    __instance.other.currfloor = IntroFloorPatch.leftMovingFloor;
                } else
                {
                    __instance.currfloor = IntroFloorPatch.rightMovingFloor;
                    __instance.other.currfloor = IntroFloorPatch.rightMovingFloor;
                }
                __result = __instance;
                return false;
            }
            return true;
        }

        public static void Postfix(scrPlanet __instance, ref scrPlanet __result)
        {
            if (scnLevelSelect.instance == null)
                return;
            if (__instance == __result)
                return;
            float x = scrController.instance.chosenplanet.other.transform.position.x;
            float y = scrController.instance.chosenplanet.other.transform.position.y;
            if (x > -6 && x < 6 && y >= -18 && y <= -6)
            {
                IntroFloorPatch.leftMovingFloor.transform.DOMoveY(y >= -7 ? -7 : y, 0.5f);
                IntroFloorPatch.rightMovingFloor.transform.DOMoveY(y >= -7 ? -7 : y, 0.5f);
            }
        }
    }
}
