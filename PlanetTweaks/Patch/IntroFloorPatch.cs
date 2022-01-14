using HarmonyLib;
using PlanetTweaks.Utils;
using PlayTweaks.Components;
using UnityEngine;

namespace PlanetTweaks.Patch
{
    public static class IntroFloorPatch
    {
        [HarmonyPatch(typeof(scnLevelSelect), "Start")]
        public static class StartPatch
        {
            public static void Postfix()
            {
                FloorUtils.AddTeleportFloor(-2, -3, -15, -3, -18, -3.5f, false, action2: delegate
                {
                    if (scrController.instance.redPlanet.isChosen)
                        scrController.instance.chosenplanet = scrController.instance.chosenplanet.other;
                    scrController.instance.camy.zoomSize = 0.5f;
                    scrController.instance.camy.isPulsingOnHit = false;
                    new GameObject().AddComponent<ImageChangePage>();
                }, parent: GameObject.Find("outer ring").transform);

                FloorUtils.AddEventFloor(-15, -3, null);

                var exitFloor = FloorUtils.AddFloor(-13.9f, -5.65f);
                exitFloor.transform.ScaleXY(0.5f, 0.5f);
                exitFloor.isportal = true;
                exitFloor.dontChangeMySprite = true;

                var images = new GameObject();
                images.name = "PlanetTweaks_Images";
                for (int i = 0; i < 4; i++)
                    for (int j = 0; j < 6; j++)
                    {
                        var obj = new GameObject();
                        obj.transform.parent = images.transform;

                        var floor = FloorUtils.AddFloor(-21.7f + j * 0.9f, -1.9f - i * 1.1f, obj.transform);
                        floor.transform.ScaleXY(0.8f, 0.8f);
                        floor.dontChangeMySprite = true;
                        if (j == 5 && i == 3)
                            floor.isportal = true;
                        else
                        {
                            floor.floorRenderer.renderer.sortingOrder = 1;
                            floor.floorRenderer.renderer.sortingLayerID = 0;
                            floor.floorRenderer.renderer.sortingLayerName = "Default";
                        }

                        var name = new GameObject().AddComponent<TextMesh>();
                        name.transform.parent = obj.transform;
                        name.gameObject.GetOrAddComponent<MeshRenderer>().sortingOrder = 0;
                        name.SetLocalizedFont();
                        name.fontSize = 100;
                        if (j == 5 && i == 3)
                        {
                            if (RDString.language == SystemLanguage.Korean)
                                name.text = "불러오기";
                            else
                                name.text = "Import";
                            name.anchor = TextAnchor.MiddleCenter;
                            name.transform.position = new Vector3(floor.x, floor.y - 0.5f);
                            name.transform.ScaleXY(0.02f, 0.02f);
                            continue;
                        }
                        name.transform.position = new Vector3(floor.x - 0.35f, floor.y - 0.38f);
                        name.transform.ScaleXY(0.015f, 0.015f);

                        var preview = new GameObject().AddComponent<TextMesh>();
                        preview.transform.parent = obj.transform;
                        preview.gameObject.GetOrAddComponent<MeshRenderer>().sortingOrder = 3;
                        preview.SetLocalizedFont();
                        preview.fontSize = 100;
                        if (RDString.language == SystemLanguage.Korean)
                            preview.text = "미리보기중";
                        else
                            preview.text = "Preview";
                        preview.anchor = TextAnchor.MiddleRight;
                        preview.transform.position = new Vector3(-21.7f + j * 0.9f + 0.46f, -1.9f - i * 1.1f - 0.36f);
                        preview.transform.ScaleXY(0.018f, 0.018f);
                        preview.gameObject.SetActive(false);

                        var icon = new GameObject().AddComponent<SpriteRenderer>();
                        icon.transform.parent = obj.transform;
                        icon.sortingOrder = 2;
                        icon.transform.position = floor.transform.position;
                        icon.transform.ScaleXY(0.7f, 0.7f);
                    }
            }
        }
    }
}
