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
            if (scrController.isGameWorld)
                return;
            scrPlanet chosenplanet = scrController.instance.chosenplanet;
            float x = (float)Math.Round(chosenplanet.transform.position.x);
            float y = (float)Math.Round(chosenplanet.transform.position.y);
            if ((x == 3 || x == -3) && y >= -18 && y <= -7)
            {
                int offset;
                if (y <= -8 && (Input.GetKeyDown(KeyCode.UpArrow) || Input.mouseScrollDelta.y > 0.4f))
                    offset = 1;
                else if (y >= -17 && (Input.GetKeyDown(KeyCode.DownArrow) || Input.mouseScrollDelta.y < -0.4f))
                    offset = -1;
                else return;
                chosenplanet.transform.DOComplete();
                IntroFloorPatch.leftMovingFloor.transform.DOComplete();
                IntroFloorPatch.rightMovingFloor.transform.DOComplete();
                chosenplanet.transform.DOLocalMoveY(y + offset, 0.2f);
                IntroFloorPatch.leftMovingFloor.transform.DOLocalMoveY(y + offset, 0.2f);
                IntroFloorPatch.rightMovingFloor.transform.DOLocalMoveY(y + offset, 0.2f);
                scrCamera.instance.frompos = scrCamera.instance.pos;
                scrCamera.instance.topos = new Vector3(x, y + offset);
                scrCamera.instance.timer = 0;
            }
        }
    }
}
