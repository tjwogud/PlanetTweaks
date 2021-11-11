using HarmonyLib;
using PlanetTweaks.Utils;
using PlayTweaks.Components;
using UnityEngine;

namespace PlanetTweaks.Patch
{
    [HarmonyPatch(typeof(scnLevelSelect), "Start")]
    public static class StartPatch
    {
        public static void Postfix()
        {
            FloorUtils.AddTeleportFloor(-2, -3, -15, -3, -18, -3.5f, false, action2: delegate
            {
                if (scrController.instance.redPlanet.isChosen)
                {
                    scrPlanet temp = scrController.instance.redPlanet;
                    scrController.instance.redPlanet = scrController.instance.bluePlanet;
                    scrController.instance.bluePlanet = temp;
                    scrController.instance.chosenplanet = scrController.instance.chosenplanet.other;
                }
                scrController.instance.camy.zoomSize = 0.5f;
                scrController.instance.camy.isPulsingOnHit = false;
                new GameObject().AddComponent<ImageChangePage>();
            });

            FloorUtils.AddEventFloor(-15, -3, null);

            var textMesh = new GameObject().AddComponent<TextMesh>();
            textMesh.text = "나가기";
            textMesh.SetLocalizedFont();
            textMesh.fontSize = 100;
            textMesh.transform.position = new Vector3(-15.2f, -5.29f);
            textMesh.transform.ScaleXY(0.05f, 0.05f);
            Sprites.RedPreview = Sprites.sprites["how"];
        }
    }
}
