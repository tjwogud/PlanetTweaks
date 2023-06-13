using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace PlanetTweaks.Patch
{
    [HarmonyPatch(typeof(scrPlanet), "Update_RefreshAngles")]
    public static class ShapedAnglePatch
    {
        public static void Postfix(scrPlanet __instance, Vector3 ___tempTransPos, scrPlanet ___movingToNext)
        {
            if (!Main.Settings.shapedRotation || !__instance.isChosen)
                return;
            float angle = 360f / Main.Settings.shapedAngle * Mathf.Deg2Rad;
            int planets = __instance.currfloor.numPlanets;
            if (planets <= 2)
            {
                Vector3 substract = (___movingToNext.transform.position - ___tempTransPos) / __instance.cosmeticRadius;
                float realAngle = (Mathf.Asin(substract.x) + Mathf.PI * 2) % (Mathf.PI * 2);
                if (substract.y < 0)
                    realAngle = (-realAngle + Mathf.PI * 3) % (Mathf.PI * 2);
                float shaped = (int)(realAngle / angle) * angle;
                ___movingToNext.transform.position = new Vector3(___tempTransPos.x + Mathf.Sin(shaped) * __instance.cosmeticRadius, ___tempTransPos.y + Mathf.Cos(shaped) * __instance.cosmeticRadius, ___tempTransPos.z);
            }
        }
    }
}
