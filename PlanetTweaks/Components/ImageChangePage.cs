using ByteSheep.Events;
using DG.Tweening;
using PlanetTweaks;
using PlanetTweaks.Components;
using PlanetTweaks.Utils;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityModManagerNet;

namespace PlayTweaks.Components
{
    public class ImageChangePage : MonoBehaviour
    {
        public static ImageChangePage instance;

        public static Sprite pageBtnNormal;
        public static Sprite pageBtnEntered;
        public static Sprite pageBtnDisabled;

        public static void Init()
        {
            // 페이지 버튼 그리기
            {
                int size = 50;
                Color transparent = new Color(0, 0, 0, 0);
                Color lightGray = new Color(0.9f, 0.9f, 0.9f);
                {
                    var texture = new Texture2D(size, size);
                    for (int i = 0; i < size; i++)
                        for (int j = 0; j < size; j++)
                            if (j >= size / 2 - 1 - i / 2 && j <= size / 2 + i / 2)
                                texture.SetPixel(j, i, Color.white);
                            else
                                texture.SetPixel(j, i, transparent);
                    texture.Apply();
                    pageBtnNormal = Sprite.Create(texture, new Rect(0, 0, size, size), new Vector2(0.5f, 0.5f));
                }
                {
                    var texture = new Texture2D(size, size);
                    for (int i = 0; i < size; i++)
                        for (int j = 0; j < size; j++)
                            if (j >= size / 2 - 1 - i / 2 && j <= size / 2 + i / 2)
                                texture.SetPixel(j, i, lightGray);
                            else
                                texture.SetPixel(j, i, transparent);
                    texture.Apply();
                    pageBtnEntered = Sprite.Create(texture, new Rect(0, 0, size, size), new Vector2(0.5f, 0.5f));
                }
                {
                    var texture = new Texture2D(size, size);
                    for (int i = 0; i < size; i++)
                        for (int j = 0; j < size; j++)
                            if (j >= size / 2 - 1 - i / 2 && j <= size / 2 + i / 2)
                                texture.SetPixel(j, i, Color.gray);
                            else
                                texture.SetPixel(j, i, transparent);
                    texture.Apply();
                    pageBtnDisabled = Sprite.Create(texture, new Rect(0, 0, size, size), new Vector2(0.5f, 0.5f));
                }
            }
            // 왼쪽 페이지 버튼
            events.Add(new Rect(18, 129, 43, 44), new ButtonEvent(
            delegate
            {
                if (instance.page == 0)
                    return;
                instance.leftPageBtn.sprite = pageBtnEntered;
            },
            delegate
            {
                if (instance.page == 0)
                    return;
                instance.leftPageBtn.sprite = pageBtnNormal;
            },
            delegate
            {
                if (instance.page == 0)
                    return;
                instance.ChangePage(instance.page - 1);
            }));
            // 오른쪽 페이지 버튼
            events.Add(new Rect(1232, 129, 43, 44), new ButtonEvent(
            delegate
            {
                instance.rightPageBtn.sprite = pageBtnEntered;
            },
            delegate
            {
                instance.rightPageBtn.sprite = pageBtnNormal;
            },
            delegate
            {
                instance.ChangePage(instance.page + 1);
            }));
            // 메인메뉴로 나가기
            events.Add(new Rect(1920 - 400, 1080 - 150, 400, 150), new ButtonEvent(
            delegate
            {
                var floor = FloorUtils.GetFloor(-13.9f, -5.65f);
                floor.DOKill(false);
                floor.transform.DOScale(new Vector3(0.55f, 0.55f), 0.5f);
            },
            delegate
            {
                var floor = FloorUtils.GetFloor(-13.9f, -5.65f);
                floor.DOKill(false);
                floor.transform.DOScale(new Vector3(0.5f, 0.5f), 0.5f);
            },
            delegate
            {
                scrController.instance.Set("exitingToMainMenu", true);
                Destroy();
                GCS.sceneToLoad = GCNS.sceneLevelSelect;
                scrUIController.instance.WipeToBlack(WipeDirection.StartsFromRight, delegate
                {
                    DOTween.KillAll(false);
                    SceneManager.LoadScene("scnLoading");
                });
            }));
            // 공 바꾸기
            events.Add(new Rect(1920 - 615, 1080 - 950, 600, 600), new ButtonEvent(
            delegate
            {
                var floor = FloorUtils.GetFloor(-15, -3);
                floor.transform.DOScale(new Vector3(1, 1), 0.5f);
            },
            delegate
            {
                var floor = FloorUtils.GetFloor(-15, -3);
                floor.transform.DOScale(new Vector3(0.8f, 0.8f), 0.5f);
            },
            delegate
            {
                instance.changing = true;
                scrUIController.instance.WipeToBlack(WipeDirection.StartsFromRight, delegate
                {
                    instance.changing = false;
                    List<scrPlanet> planets = scrController.instance.planetList;
                    scrController.instance.chosenplanet = scrController.instance.chosenplanet.next;
                    scrController.instance.chosenplanet.transform.LocalMoveXY(-15, -3);
                    scrController.instance.chosenplanet.transform.position = new Vector3(-15, -3);
                    if (RDString.language == SystemLanguage.Korean)
                    {
                        if (scrController.instance.chosenplanet.isRed)
                            instance.planetText.text = "<color=" + ColorUtils.GetRealColor(true).ToHex() + ">불 행성</color> 선택됨";
                        else if (!scrController.instance.chosenplanet.isExtra)
                            instance.planetText.text = "<color=" + ColorUtils.GetRealColor(false).ToHex() + ">얼음 행성</color> 선택됨";
                        else
                            instance.planetText.text = "<color=" + ColorUtils.GetRealThirdColor().ToHex() + ">세번째 행성</color> 선택됨";
                    }
                    {
                        if (scrController.instance.chosenplanet.isRed)
                            instance.planetText.text = "<color=" + ColorUtils.GetRealColor(true).ToHex() + ">Fire Planet</color> Selected";
                        else if (!scrController.instance.chosenplanet.isExtra)
                            instance.planetText.text = "<color=" + ColorUtils.GetRealColor(false).ToHex() + ">Ice Planet</color> Selected";
                        else
                            instance.planetText.text = "<color=" + ColorUtils.GetRealThirdColor().ToHex() + ">Third Planet</color> Selected";
                    }
                    instance.UpdateFloorIcons();
                    scrUIController.instance.WipeFromBlack();
                });
            }));
            // 이미지 타일들
            for (int i = 0; i < 6; i++)
            {
                for (int j = 0; j < 4; j++)
                {
                    int copyI = i;
                    int copyJ = j;
                    if (i == 5 && j == 3)
                        events.Add(new Rect(79 + i * 194 + (i > 1 ? i > 4 ? 2 : 1 : 0), 112 + j * 238 + (j > 1 ? -1 : 0), 164, 164), new ButtonEvent(
                        delegate
                        {
                            var floor = FloorUtils.GetFloor(-21.7f + copyI * 0.9f, -1.7f - copyJ * 1.1f);
                            floor.transform.DOKill(false);
                            floor.transform.DOScale(new Vector3(0.9f, 0.9f), 0.5f);
                        },
                        delegate
                        {
                            var floor = FloorUtils.GetFloor(-21.7f + copyI * 0.9f, -1.7f - copyJ * 1.1f);
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
                            }
                            catch (Exception e)
                            {
                                Main.Logger.Log("wrong file '" + file + "'!");
                                Main.Logger.Log(e.StackTrace);
                            }
                        }));
                    else
                    {
                        events.Add(new Rect(79 + i * 194 + (i > 1 ? i > 4 ? 2 : 1 : 0), 112 + j * 238 + (j > 1 ? -1 : 0), 164, 164), new ButtonEvent(
                        delegate
                        {
                            var floor = FloorUtils.GetFloor(-21.7f + copyI * 0.9f, -1.7f - copyJ * 1.1f);
                            if (floor.GetIcon().sprite == null)
                                return;
                            floor.transform.DOKill(false);
                            floor.transform.DOScale(new Vector3(1, 1), 0.5f).OnComplete(delegate
                            {
                                int index = instance.page * 23 + copyJ * 6 + copyI;
                                if ((scrController.instance.chosenplanet.isRed ? Sprites.RedSelected : (!scrController.instance.chosenplanet.isExtra ? Sprites.BlueSelected : Sprites.ThirdSelected)) == index)
                                    return;
                                floor.GetPreview().gameObject.SetActive(true);
                                if (scrController.instance.chosenplanet.isRed)
                                    Sprites.RedPreview = Sprites.sprites.ElementAt(index).Value;
                                else if (!scrController.instance.chosenplanet.isExtra)
                                    Sprites.BluePreview = Sprites.sprites.ElementAt(index).Value;
                                else
                                    Sprites.ThirdPreview = Sprites.sprites.ElementAt(index).Value;
                            }).SetAutoKill(false);
                            var sprite = floor.GetIcon();
                            sprite.transform.DOKill(false);
                            sprite.transform.DOScale(new Vector3(0.875f, 0.875f), 0.5f);
                        },
                        delegate
                        {
                            var floor = FloorUtils.GetFloor(-21.7f + copyI * 0.9f, -1.7f - copyJ * 1.1f);
                            floor.transform.DOKill(false);
                            floor.transform.DOScale(new Vector3(0.8f, 0.8f), 0.5f);
                            floor.GetPreview().gameObject.SetActive(false);
                            var sprite = floor.GetIcon();
                            sprite.transform.DOKill(false);
                            sprite.transform.DOScale(new Vector3(0.7f, 0.7f), 0.5f);
                            if (scrController.instance.chosenplanet.isRed)
                                Sprites.RedPreview = null;
                            else if (!scrController.instance.chosenplanet.isExtra)
                                Sprites.BluePreview = null;
                            else
                                Sprites.ThirdPreview = null;
                        },
                        delegate
                        {
                            var floor = FloorUtils.GetFloor(-21.7f + copyI * 0.9f, -1.7f - copyJ * 1.1f);
                            if (floor.GetIcon().sprite == null)
                                return;
                            floor.transform.DOComplete(false);
                            floor.GetPreview().gameObject.SetActive(false);
                            var sprite = floor.GetIcon();
                            sprite.transform.DOComplete(false);
                            int index = instance.page * 23 + copyJ * 6 + copyI;
                            if (Input.GetMouseButtonUp(0))
                                if ((scrController.instance.chosenplanet.isRed ? Sprites.RedSelected : (!scrController.instance.chosenplanet.isExtra ? Sprites.BlueSelected : Sprites.ThirdSelected)) == index)
                                {
                                    if (scrController.instance.chosenplanet.isRed)
                                        Sprites.RedSelected = -1;
                                    else if (!scrController.instance.chosenplanet.isExtra)
                                        Sprites.BlueSelected = -1;
                                    else
                                        Sprites.ThirdSelected = -1;
                                }
                                else
                                {
                                    if (scrController.instance.chosenplanet.isRed)
                                        Sprites.RedSelected = index;
                                    else if (!scrController.instance.chosenplanet.isExtra)
                                        Sprites.BlueSelected = index;
                                    else
                                        Sprites.ThirdSelected = index;
                                }
                            else if (Input.GetMouseButtonUp(1))
                            {
                                Sprites.Remove(index);
                                if (scrController.instance.chosenplanet.isRed)
                                    Sprites.RedPreview = null;
                                else if(!scrController.instance.chosenplanet.isExtra)
                                    Sprites.BluePreview = null;
                                else
                                    Sprites.ThirdPreview = null;
                            }
                            else
                                return;
                            instance.UpdateFloorIcons();
                        }));

                        events.Add(new Rect(79 + i * 194 + (i > 1 ? i > 4 ? 2 : 1 : 0), 112 + j * 238 + (j > 1 ? -1 : 0) + 168, 164, 40), new ButtonEvent(
                        delegate
                        {
                            var floor = FloorUtils.GetFloor(-21.7f + copyI * 0.9f, -1.7f - copyJ * 1.1f);
                            var text = floor.GetName();
                            text.DOKill(false);
                            DOTween.To(() => text.color, c => text.color = c, Color.yellow, 0.5f).SetTarget(text);
                        },
                        delegate
                        {
                            var floor = FloorUtils.GetFloor(-21.7f + copyI * 0.9f, -1.7f - copyJ * 1.1f);
                            var text = floor.GetName();
                            text.DOKill(false);
                            DOTween.To(() => text.color, c => text.color = c, Color.white, 0.5f).SetTarget(text);
                        },
                        delegate
                        {
                            var floor = FloorUtils.GetFloor(-21.7f + copyI * 0.9f, -1.7f - copyJ * 1.1f);
                            var text = floor.GetName();
                            int index = instance.page * 23 + copyJ * 6 + copyI;
                            if (index >= Sprites.sprites.Count)
                                return;
                            instance.input = true;
                            RenameInputField.Instance.Show(text.text, s =>
                            {
                                instance.input = false;
                                if (s.Trim().IsNullOrEmpty() || s == text.text)
                                    return;
                                s = Path.GetInvalidPathChars().Aggregate(s, (cur, c) => cur.Replace(c.ToString(), "_"));
                                string first = s;
                                for (int k = 1; Sprites.sprites.ContainsKey(s); k++)
                                    s = first + k;
                                if (index == Sprites.RedSelected)
                                    Main.Settings.redSelected = s;
                                if (index == Sprites.BlueSelected)
                                    Main.Settings.blueSelected = s;
                                text.text = s;
                                Sprites.sprites.Replace(index, s, Sprites.sprites.ElementAt(index).Value);
                            });
                        }));
                    }
                }
            }
        }

        public static Color floorColor = new Color(0.78f, 0.78f, 0.886f);

        public void UpdateFloorIcons()
        {
            var images = GameObject.Find("PlanetTweaks_Images");
            for (int i = 0; i < images.transform.childCount - 1; i++)
            {
                var obj = images.transform.GetChild(i);
                Sprites.Apply(obj.GetIcon(), null);
                obj.GetName().text = null;
                if ((scrController.instance.chosenplanet.isRed ? Sprites.RedSelected : (!scrController.instance.chosenplanet.isExtra ? Sprites.BlueSelected : Sprites.ThirdSelected)) == i + page * 23)
                    obj.GetFloor().SetTileColor(Color.yellow);
                else
                    obj.GetFloor().SetTileColor(floorColor);
            }
            for (int i = 0; i < images.transform.childCount - 1; i++)
            {
                if (i + page * 23 >= Sprites.sprites.Count())
                    break;
                var pair = Sprites.sprites.ElementAt(i + page * 23);
                var obj = images.transform.GetChild(i);
                Sprites.Apply(obj.GetIcon(), pair.Value);
                obj.GetName().text = pair.Key;
            }
            if (RDString.language == SystemLanguage.Korean)
                pageText.text = page + 1 + "페이지";
            else
                pageText.text = "Page " + (page + 1);
            if (page == 0)
                leftPageBtn.sprite = pageBtnDisabled;
            else if (new Rect(18, 86, 43, 44).Contains(Event.current.mousePosition))
                leftPageBtn.sprite = pageBtnEntered;
            else
                leftPageBtn.sprite = pageBtnNormal;
        }

        public void ChangePage(int page)
        {
            if (page == this.page)
                return;
            this.page = page;
            changing = true;
            scrFloor floor = null;
            Tween fade = null;
            for (int i = 0; i < 6; i++)
                for (int j = 0; j < 4; j++)
                {
                    if (i == 5 && j == 3)
                    {
                        fade.OnComplete(delegate
                        {
                            UpdateFloorIcons();
                            for (i = 0; i < 6; i++)
                                for (j = 0; j < 4; j++)
                                {
                                    if (i == 5 && j == 3)
                                    {
                                        fade.OnComplete(delegate
                                        {
                                            changing = false;
                                        });
                                        break;
                                    }
                                    floor = FloorUtils.GetFloor(-21.7f + i * 0.9f, -1.7f - j * 1.1f);
                                    var fr2 = floor.floorRenderer;
                                    fr2.color = fr2.color.WithAlpha(0);
                                    fade = DOTween.To(() => fr2.color, c => fr2.color = c, fr2.color.WithAlpha(1), 0.5f);
                                    floor.GetName().gameObject.GetComponent<MeshRenderer>().material.DOFade(1, 0.5f);
                                    floor.GetIcon().material.DOFade(1, 0.5f);
                                }
                        });
                        break;
                    }
                    floor = FloorUtils.GetFloor(-21.7f + i * 0.9f, -1.7f - j * 1.1f);
                    var fr = floor.floorRenderer;
                    fade = DOTween.To(() => fr.color, c => fr.color = c, fr.color.WithAlpha(0), 0.5f);
                    floor.GetName().gameObject.GetComponent<MeshRenderer>().material.DOFade(0, 0.5f);
                    floor.GetIcon().material.DOFade(0, 0.5f);
                }
        }

        //private bool propertyPage = false;
        private bool input = false;
        private bool changing = false;

        private int page = 0;

        public TextMesh planetText;
        public TextMesh pageText;
        public SpriteRenderer leftPageBtn;
        public SpriteRenderer rightPageBtn;

        public void Awake()
        {
            if (instance != null)
                Destroy(instance.gameObject);
            instance = this;

            planetText = new GameObject().AddComponent<TextMesh>();
            planetText.richText = true;
            if (RDString.language == SystemLanguage.Korean)
                planetText.text = "<color=" + ColorUtils.GetRealColor(true).ToHex() + ">불 행성</color> 선택됨";
            else
                planetText.text = "<color=" + ColorUtils.GetRealColor(true).ToHex() + ">Fire Planet</color> Selected";
            planetText.SetLocalizedFont();
            planetText.fontSize = 100;
            planetText.anchor = TextAnchor.UpperCenter;
            planetText.transform.position = new Vector3(-15.05f, -4.25f);
            planetText.transform.ScaleXY(0.045f, 0.045f);

            var exit = new GameObject().AddComponent<TextMesh>();
            if (RDString.language == SystemLanguage.Korean)
                exit.text = "나가기";
            else
                exit.text = "Exit";
            exit.SetLocalizedFont();
            exit.fontSize = 100;
            if (RDString.language == SystemLanguage.Korean)
            {
                exit.transform.position = new Vector3(-15.2f, -5.29f);
                exit.transform.ScaleXY(0.05f, 0.05f);
            } else
            {
                exit.transform.position = new Vector3(-15.15f, -5.23f);
                exit.transform.ScaleXY(0.06f, 0.06f);
            }

            pageText = new GameObject().AddComponent<TextMesh>();
            if (RDString.language == SystemLanguage.Korean)
                pageText.text = "1페이지";
            else
                pageText.text = "Page 1";
            pageText.SetLocalizedFont();
            pageText.fontSize = 100;
            pageText.transform.position = new Vector3(-22, -1.23f);
            pageText.transform.ScaleXY(0.02f, 0.02f);

            leftPageBtn = new GameObject().AddComponent<SpriteRenderer>();
            leftPageBtn.sprite = pageBtnNormal;
            leftPageBtn.transform.Rotate(0, 0, -90);
            leftPageBtn.transform.position = new Vector3(-22.26f, -1.7f);
            leftPageBtn.transform.ScaleXY(0.4f, 0.4f);

            rightPageBtn = new GameObject().AddComponent<SpriteRenderer>();
            rightPageBtn.sprite = pageBtnNormal;
            rightPageBtn.transform.Rotate(0, 0, 90);
            rightPageBtn.transform.position = new Vector3(-16.64f, -1.7f);
            rightPageBtn.transform.ScaleXY(0.4f, 0.4f);

            instance.UpdateFloorIcons();
        }

        private static readonly Dictionary<Rect, ButtonEvent> events = new Dictionary<Rect, ButtonEvent>();

        public void OnGUI()
        {
            if (/*propertyPage || */scrController.instance.paused)
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

        public void HandleButtonEvent()
        {
            Vector2 mouse = Event.current.mousePosition;

            foreach (var pair in events)
            {
                try
                {
                    var rect = pair.Key.Fix();
                    var btnEvent = pair.Value;
                    if (!btnEvent.Entered && rect.Contains(mouse))
                    {
                        if (!input && !changing && !UnityModManager.UI.Instance.Opened)
                        {
                            btnEvent.Entered = true;
                            btnEvent.OnEntered.Invoke();
                        }
                    }
                    else if (btnEvent.Entered && !rect.Contains(mouse))
                    {
                        btnEvent.Entered = false;
                        btnEvent.OnExited.Invoke();
                    }

                    if (!input && !changing && !UnityModManager.UI.Instance.Opened)
                        if (GUI.Button(rect, "", GUIStyle.none))
                            btnEvent.OnClicked.Invoke();
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

        private class SliderField
        {
            public Func<float> Getter { get; }
            public Action<float> Setter { get; }
            public float Max { get; }
            public float Min { get; }

            public SliderField(Func<float> getter, Action<float> setter, float max, float min)
            {
                Getter = getter;
                Setter = setter;
                Max = max;
                Min = min;
            }
        }
    }
}
