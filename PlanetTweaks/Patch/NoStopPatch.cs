using System;
using UnityEngine;

namespace PlanetTweaks.Patch
{
    public static class NoStopPatch
    {
        public static void Postfix(int keyCode, ref bool __result)
        {
            if (scrController.isGameWorld || (keyCode != 61000 && keyCode != 61008))
                return;
            float x = (float)Math.Round(scrController.instance.chosenplanet.transform.position.x);
            float y = (float)Math.Round(scrController.instance.chosenplanet.transform.position.y);
            if ((x == 3 || x == -3) && (y >= -18 && y <= -7))
            {
                __result = true;
            }
        }
    }
}
