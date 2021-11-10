using HarmonyLib;
using PlanetTweaks.Utils;
using System;
using UnityEngine;

namespace PlanetTweaks.Patch
{
    [HarmonyPatch(typeof(PauseMenu), "UpdateButtonsContainer")]
    public static class UpdateButtonsContainerPatch
    {

        public static bool Prefix(PauseMenu __instance)
        {
            __instance.buttonsContainer.ScaleXY(1f);
            var btn = __instance.currentButtons.Find((GeneralPauseButton a) => a.isActiveAndEnabled);
            float x = btn != null ? btn.rectangleRT.position.x : 0;
            btn = __instance.currentButtons.FindLast((GeneralPauseButton a) => a.isActiveAndEnabled);
            float num = 52f + (btn != null ? btn.rectangleRT.position.x : 0);
            float num2 = Screen.width;
            __instance.Set("lastScreenWidth", num2);
            float num3 = Math.Max(num - x, 822f);
            float num4 = num2 * 0.8f;
            float xy = Mathf.Min(1f, num4 / num3);
            __instance.buttonsContainer.ScaleXY(xy);
            return false;
        }
    }
}
