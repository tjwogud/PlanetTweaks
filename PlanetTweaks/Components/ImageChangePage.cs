using ByteSheep.Events;
using DG.Tweening;
using PlanetTweaks;
using PlanetTweaks.Utils;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace PlayTweaks.Components
{
    public class ImageChangePage : MonoBehaviour
    {
        public static ImageChangePage instance;

        public static void Init()
        {
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
                            } catch (Exception)
                            {
                                Main.Logger.Log("wrong file '" + file + "'!");
                            }
                        }));
                    else
                        events.Add(new Rect(79 + i * 194 + (i > 1 ? i > 4 ? 2 : 1 : 0), 69 + j * 238 + (j > 1 ? -1 : 0), 164, 164), new ButtonEvent(
                        delegate
                        {
                            var floor = FloorUtils.GetGameObjectAt(-21.7f + copyI * 0.9f, -1.7f - copyJ * 1.1f).GetComponent<scrFloor>();
                            floor.transform.DOKill(false);
                            floor.transform.DOScale(new Vector3(1, 1), 0.5f).OnComplete(delegate
                            {
                                floor.transform.parent.GetChild(2).gameObject.SetActive(true);
                                // 스킨 미리보기
                            }).OnKill(delegate
                            {
                                floor.transform.parent.GetChild(2).gameObject.SetActive(false);
                                // 스킨 되돌리기
                            }).SetAutoKill(false);
                        },
                        delegate
                        {
                            var floor = FloorUtils.GetGameObjectAt(-21.7f + copyI * 0.9f, -1.7f - copyJ * 1.1f).GetComponent<scrFloor>();
                            floor.transform.DOKill(false);
                            floor.transform.DOScale(new Vector3(0.8f, 0.8f), 0.5f);
                        },
                        delegate
                        {
                            // 스킨 바꾸기
                        }));
                }
        }

        public void UpdateFloorIcons()
        {
            // 아 귀찮다

            // 개귀찮다
            // floor.transform.parent.GetChild(0).GetComponent<scrFloor>().floorRenderer.material.DOFade(0, 0.5f).OnComplete(delegate
            // {
            //     floor.transform.parent.GetChild(0).GetComponent<scrFloor>().floorRenderer.material.DOFade(1, 0.5f);
            // });
            // floor.transform.parent.GetChild(1).gameObject.SetActive(false);
            // floor.transform.parent.GetChild(2).gameObject.SetActive(false);
        }

        private bool changing = false;

        // private int page = 0;

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
            for (int i = 0; i < 6; i++)
                for (int j = 0; j < 4; j++)
                {
                    var obj = new GameObject();
                    obj.transform.parent = images.transform;

                    var floor = FloorUtils.AddFloor(-21.7f + i * 0.9f, -1.7f - j * 1.1f, obj.transform);
                    floor.transform.ScaleXY(0.8f, 0.8f);
                    if (i == 5 && j == 3)
                        floor.isportal = true;

                    var name = new GameObject().AddComponent<TextMesh>();
                    name.transform.parent = obj.transform;
                    name.SetLocalizedFont();
                    name.fontSize = 100;
                    if (i == 5 && j == 3)
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
                    preview.SetLocalizedFont();
                    preview.fontSize = 100;
                    preview.text = "미리보기중";
                    preview.anchor = TextAnchor.MiddleRight;
                    preview.transform.position = new Vector3(floor.x + 0.46f, floor.y - 0.36f);
                    preview.transform.ScaleXY(0.018f, 0.018f);
                    preview.gameObject.SetActive(false);
                }
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
