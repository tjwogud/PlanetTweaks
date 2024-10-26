using DG.Tweening;
using HarmonyLib;
using System;
using UnityEngine;

namespace PlanetTweaks.Patch
{
    [HarmonyPatch(typeof(scrController), "Update")]
    public static class ControllerUpdatePatch
    {
        public static void Postfix()
        {
            if (!scnLevelSelect.instance || !scrController.instance || !scrCamera.instance || !IntroFloorPatch.leftMovingFloor || !IntroFloorPatch.rightMovingFloor)
                return;
            float x = (float)Math.Round(scrController.instance.chosenplanet.transform.position.x);
            float y = (float)Math.Round(scrController.instance.chosenplanet.transform.position.y);
            if ((x == 3 || x == -3) && (y >= -18 && y <= -7))
            {
                if (y <= -8 && (KeyCode.UpArrow.WentDown() || Input.mouseScrollDelta.y > 0.4f))
                {
                    scrController.instance.chosenplanet.transform.DOComplete();
                    IntroFloorPatch.leftMovingFloor.transform.DOComplete();
                    IntroFloorPatch.rightMovingFloor.transform.DOComplete();
                    scrController.instance.chosenplanet.transform.DOMoveY(y + 1, 0.2f);
                    IntroFloorPatch.leftMovingFloor.transform.DOMoveY(y + 1, 0.2f);
                    IntroFloorPatch.rightMovingFloor.transform.DOMoveY(y + 1, 0.2f);
                    scrCamera.instance.frompos = scrCamera.instance.pos;
                    scrCamera.instance.topos = new Vector3(x, y + 1, -10);
                    scrCamera.instance.timer = 0;
                }
                else if (y >= -17 && (KeyCode.DownArrow.WentDown() || Input.mouseScrollDelta.y < -0.4f))
                {
                    scrController.instance.chosenplanet.transform.DOComplete();
                    IntroFloorPatch.leftMovingFloor.transform.DOComplete();
                    IntroFloorPatch.rightMovingFloor.transform.DOComplete();
                    scrController.instance.chosenplanet.transform.DOMoveY(y - 1f, 0.2f);
                    IntroFloorPatch.leftMovingFloor.transform.DOMoveY(y - 1, 0.2f);
                    IntroFloorPatch.rightMovingFloor.transform.DOMoveY(y - 1, 0.2f);
                    scrCamera.instance.frompos = scrCamera.instance.pos;
                    scrCamera.instance.topos = new Vector3(x, y - 1, -10);
                    scrCamera.instance.timer = 0;
                }
            }
        }
    }
}
