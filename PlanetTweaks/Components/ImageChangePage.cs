using ByteSheep.Events;
using DG.Tweening;
using PlanetTweaks;
using PlanetTweaks.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace PlayTweaks.Components
{
    public class ImageChangePage : MonoBehaviour
    {
        public static ImageChangePage instance;

        public static void Init()
        {
            // 메인메뉴로 나가기
            events.Add(new Rect(1920 - 400, 1080 - 150, 400, 150), new ButtonEvent(
            delegate
            {
                var floor = FloorUtils.GetGameObjectAt(-13.9f, -5.65f).GetComponent<scrFloor>();
                floor.transform.DOKill(false);
                floor.transform.DOScale(new Vector3(0.55f, 0.55f), 0.5f);
            },
            delegate
            {
                var floor = FloorUtils.GetGameObjectAt(-13.9f, -5.65f).GetComponent<scrFloor>();
                floor.transform.DOKill(false);
                floor.transform.DOScale(new Vector3(0.5f, 0.5f), 0.5f);
            },
            delegate
            {
                RDUtils.SetGarbageCollectionEnabled(true);
                scrController.instance.Set("exitingToMainMenu", true);
                Destroy();
                scrUIController.instance.WipeToBlack(WipeDirection.StartsFromRight, delegate
                {
                    scrController.instance.printe("killing all tweens");
                    DOTween.KillAll(false);
                    SceneManager.LoadScene("scnLoading");
                });
                GCS.sceneToLoad = (GCS.customLevelPaths == null) ? GCNS.sceneLevelSelect : "scnCLS";
                GCS.standaloneLevelMode = false;
                scrController.deaths = 0;
                GCS.currentSpeedTrial = 1f;
            }));
            // 공 바꾸기
            events.Add(new Rect(1920 - 615, 1080 - 950, 600, 600), new ButtonEvent(
            delegate
            {
                var floor = FloorUtils.GetGameObjectAt(-15, -3).GetComponent<scrFloor>();
                floor.transform.DOKill(false);
                floor.transform.DOScale(new Vector3(1, 1), 0.5f);
            },
            delegate
            {
                var floor = FloorUtils.GetGameObjectAt(-15, -3).GetComponent<scrFloor>();
                floor.transform.DOKill(false);
                floor.transform.DOScale(new Vector3(0.8f, 0.8f), 0.5f);
            },
            delegate
            {
                instance.changing = true;
                scrUIController.instance.WipeToBlack(WipeDirection.StartsFromRight, delegate
                {
                    instance.changing = false;
                    scrPlanet temp = scrController.instance.redPlanet;
                    scrController.instance.redPlanet = scrController.instance.bluePlanet;
                    scrController.instance.bluePlanet = temp;
                    scrController.instance.chosenplanet = scrController.instance.chosenplanet.other;
                    scrController.instance.chosenplanet.transform.LocalMoveXY(-15, -3);
                    scrController.instance.chosenplanet.transform.position = new Vector3(-15, -3);
                    instance.UpdateFloorIcons();
                    scrUIController.instance.WipeFromBlack();
                });
            }));
            for (int i = 0; i < 6; i++)
                for (int j = 0; j < 4; j++)
                {
                    int copyI = i;
                    int copyJ = j;
                    if (i == 5 && j == 3)
                        events.Add(new Rect(79 + i * 194 + (i > 1 ? i > 4 ? 2 : 1 : 0), 69 + j * 238 + (j > 1 ? -1 : 0), 164, 164), new ButtonEvent(
                        delegate
                        {
                            var floor = FloorUtils.GetGameObjectAt(-21.7f + copyI * 0.9f, -1.7f - copyJ * 1.1f).GetComponent<scrFloor>();
                            floor.transform.DOKill(false);
                            floor.transform.DOScale(new Vector3(0.9f, 0.9f), 0.5f);
                        },
                        delegate
                        {
                            var floor = FloorUtils.GetGameObjectAt(-21.7f + copyI * 0.9f, -1.7f - copyJ * 1.1f).GetComponent<scrFloor>();
                            floor.transform.DOKill(false);
                            floor.transform.DOScale(new Vector3(0.8f, 0.8f), 0.5f);
                        },
                        delegate
                        {
                            var file = Sprites.ShowOpenFileDialog();
                            try
                            {
                                Sprites.Add(file);
                                instance.UpdateFloorIcons();
                            } catch (Exception e)
                            {
                                Main.Logger.Log("wrong file '" + file + "'!");
                                Main.Logger.Log(e.StackTrace);
                            }
                        }));
                    else
                        events.Add(new Rect(79 + i * 194 + (i > 1 ? i > 4 ? 2 : 1 : 0), 69 + j * 238 + (j > 1 ? -1 : 0), 164, 164), new ButtonEvent(
                        delegate
                        {
                            var floor = FloorUtils.GetGameObjectAt(-21.7f + copyI * 0.9f, -1.7f - copyJ * 1.1f).GetComponent<scrFloor>();
                            if (floor.GetIcon().sprite == null)
                                return;
                            floor.transform.DOKill(false);
                            floor.transform.DOScale(new Vector3(1, 1), 0.5f).OnComplete(delegate
                            {
                                floor.GetPreview().gameObject.SetActive(true);
                                if (scrController.instance.redPlanet.isChosen)
                                    Sprites.BluePreview = floor.GetIcon().sprite;
                                else
                                    Sprites.RedPreview = floor.GetIcon().sprite;
                            }).OnKill(delegate
                            {
                                floor.GetPreview().gameObject.SetActive(false);
                                if (scrController.instance.redPlanet.isChosen)
                                    Sprites.BluePreview = null;
                                else
                                    Sprites.RedPreview = null;
                            }).SetAutoKill(false);
                            var sprite = floor.GetIcon();
                            sprite.transform.DOKill(false);
                            sprite.transform.DOScale(new Vector3(0.875f, 0.875f), 0.5f);
                        },
                        delegate
                        {
                            var floor = FloorUtils.GetGameObjectAt(-21.7f + copyI * 0.9f, -1.7f - copyJ * 1.1f).GetComponent<scrFloor>();
                            floor.transform.DOKill(false);
                            floor.transform.DOScale(new Vector3(0.8f, 0.8f), 0.5f);
                            var sprite = floor.GetIcon();
                            sprite.transform.DOKill(false);
                            sprite.transform.DOScale(new Vector3(0.7f, 0.7f), 0.5f);
                        },
                        delegate
                        {
                            var floor = FloorUtils.GetGameObjectAt(-21.7f + copyI * 0.9f, -1.7f - copyJ * 1.1f).GetComponent<scrFloor>();
                            if (floor.GetIcon().sprite == null)
                                return;
                            floor.transform.DOComplete(false);
                            floor.GetPreview().gameObject.SetActive(false);
                            var sprite = floor.GetIcon();
                            sprite.transform.DOComplete(false);
                            if (scrController.instance.redPlanet.isChosen)
                                Sprites.BlueSelected = instance.page * 23 + copyI * 6 + copyJ;
                            else
                                Sprites.RedSelected = instance.page * 23 + copyI * 6 + copyJ;
                        }));
                }
        }

        public void UpdateFloorIcons()
        {
            var images = GameObject.Find("PlanetTweaks_Images");
            for (int i = 0; i < images.transform.childCount - 1; i++)
            {
                var obj = images.transform.GetChild(i);
                obj.GetIcon().sprite = null;
                obj.GetName().text = null;
            }
            for (int i = 0; i < images.transform.childCount - 1; i++)
            {
                if (i + page * 23 >= Sprites.sprites.Count())
                    break;
                var pair = Sprites.sprites.ElementAt(i + page * 23);
                var obj = images.transform.GetChild(i);
                obj.GetIcon().sprite = pair.Value;
                obj.GetName().text = pair.Key;
            }
        }

        public void ChangePage(int page)
        {
            if (page == this.page)
                return;
            this.page = page;
            changing = true;
            bool first = true;
            for (int i = 0; i < 6; i++)
                for (int j = 0; j < 4; j++)
                {
                    if (i == 5 && j == 3)
                        break;
                    var floor = FloorUtils.GetGameObjectAt(-21.7f + i * 0.9f, -1.7f - j * 1.1f).GetComponent<scrFloor>();

                    var fade = floor.floorRenderer.material.DOFade(0, 0.5f);
                    floor.GetName().gameObject.GetComponent<MeshRenderer>().material.DOFade(0, 0.5f);
                    floor.GetIcon().material.DOFade(0, 0.5f);
                    if (first)
                    {
                        first = false;
                        fade.OnComplete(delegate
                        {
                            UpdateFloorIcons();
                            first = true;
                            for (i = 0; i < 6; i++)
                                for (j = 0; j < 4; j++)
                                {
                                    if (i == 5 && j == 3)
                                        break;
                                    floor = FloorUtils.GetGameObjectAt(-21.7f + i * 0.9f, -1.7f - j * 1.1f).GetComponent<scrFloor>();
                                    fade = floor.floorRenderer.material.DOFade(1, 0.5f);
                                    floor.GetName().gameObject.GetComponent<MeshRenderer>().material.DOFade(1, 0.5f);
                                    floor.GetIcon().material.DOFade(1, 0.5f);
                                    if (first)
                                    {
                                        first = false;
                                        fade.OnComplete(delegate
                                        {
                                            changing = false;
                                        });
                                    }
                                }
                        });
                    }
                }
        }

        private bool changing = false;

        private int page = 0;

        public void Awake()
        {
            if (instance != null)
                Destroy(instance.gameObject);
            instance = this;

            if (FloorUtils.GetGameObjectAt(-13.9f, -5.65f)?.GetComponent<scrFloor>() != null)
                return;

            var exitFloor = FloorUtils.AddFloor(-13.9f, -5.65f);
            exitFloor.transform.ScaleXY(0.5f, 0.5f);
            exitFloor.isportal = true;

            var images = new GameObject();
            images.name = "PlanetTweaks_Images";
            for (int i = 0; i < 4; i++)
                for (int j = 0; j < 6; j++)
                {
                    var obj = new GameObject();
                    obj.transform.parent = images.transform;

                    var floor = FloorUtils.AddFloor(-21.7f + j * 0.9f, -1.7f - i * 1.1f, obj.transform);
                    floor.transform.ScaleXY(0.8f, 0.8f);
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
                        name.text = "불러오기";
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
                    preview.text = "미리보기중";
                    preview.anchor = TextAnchor.MiddleRight;
                    preview.transform.position = new Vector3(-21.7f + j * 0.9f + 0.46f, -1.7f - i * 1.1f - 0.36f);
                    preview.transform.ScaleXY(0.018f, 0.018f);
                    preview.gameObject.SetActive(false);

                    var icon = new GameObject().AddComponent<SpriteRenderer>();
                    icon.transform.parent = obj.transform;
                    icon.sortingOrder = 2;
                    icon.transform.position = floor.transform.position;
                    icon.transform.ScaleXY(0.7f, 0.7f);
                }
            instance.UpdateFloorIcons();
        }

        private static readonly Dictionary<Rect, ButtonEvent> events = new Dictionary<Rect, ButtonEvent>();

        public void OnGUI()
        {
            if (scrController.instance.paused || changing)
                return;

            HandleButtonEvent();
        }

        public static void Destroy()
        {
            if (instance == null)
                return;
            Destroy(instance);
            instance = null;
        }

        public static void HandleButtonEvent()
        {
            Vector2 mouse = Event.current.mousePosition;

            foreach (var key in events.Keys)
            {
                try
                {
                    var rect = key.Fix();
                    var btnEvent = events[rect];

                    if (!btnEvent.Entered && rect.Contains(mouse))
                    {
                        btnEvent.Entered = true;
                        btnEvent.OnEntered.Invoke();
                    }
                    else if (btnEvent.Entered && !rect.Contains(mouse))
                    {
                        btnEvent.Entered = false;
                        btnEvent.OnExited.Invoke();
                    }

                    if (GUI.Button(rect, "", GUIStyle.none))
                    {
                        btnEvent.OnClicked.Invoke();
                    }
                }
                catch (Exception)
                {
                }
            }
        }

        private class ButtonEvent
        {
            public bool Entered { get; set; }
            public QuickAction OnEntered { get; }
            public QuickAction OnExited { get; }
            public QuickAction OnClicked { get; }

            public ButtonEvent(QuickAction onEntered, QuickAction onExited, QuickAction onClicked)
            {
                Entered = false;
                OnEntered = onEntered;
                OnExited = onExited;
                OnClicked = onClicked;
            }
        }
    }
}
