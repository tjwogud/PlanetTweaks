using HarmonyLib;
using PlanetTweaks.Patch;
using PlanetTweaks.Utils;
using PlayTweaks.Components;
using System;
using System.IO;
using System.Linq;
using System.Reflection;
using UnityEngine;
using UnityModManagerNet;

namespace PlanetTweaks
{
    public static class Main
    {
        public static UnityModManager.ModEntry ModEntry;
        public static UnityModManager.ModEntry.ModLogger Logger;
        public static Harmony Harmony;
        public static bool IsEnabled = false;
        public static Settings Settings;
        public static AssetBundle Bundle;

        public static void Setup(UnityModManager.ModEntry modEntry)
        {
            ModEntry = modEntry;
            Logger = modEntry.Logger;
            modEntry.OnToggle = OnToggle;
            modEntry.OnGUI = OnGUI;
            modEntry.OnSaveGUI = OnSaveGUI;
            Logger.Log("Loading AssetBundle...");
            Bundle = AssetBundle.LoadFromFile(Path.Combine(modEntry.Path, "planettweaks"));
            Logger.Log("Load Completed!");
            Logger.Log("Loading Settings...");
            Settings = UnityModManager.ModSettings.Load<Settings>(modEntry);
            Logger.Log("Load Completed!");
            Logger.Log("Loading Sprites...");
            Sprites.Load();
            Logger.Log("Load Completed!");
            Sprites.Init();
            ImageChangePage.Init();
        }

        private static bool OnToggle(UnityModManager.ModEntry modEntry, bool value)
        {
            IsEnabled = value;
            if (value)
            {
                Harmony = new Harmony(modEntry.Info.Id);
                Harmony.PatchAll(Assembly.GetExecutingAssembly());
            }
            else
            {
                Harmony.UnpatchAll(modEntry.Info.Id);
            }
            return true;
        }

        private static GUIStyle labelStyle;

        private static void OnGUI(UnityModManager.ModEntry modEntry)
        {
            if (labelStyle == null)
            {
                labelStyle = new GUIStyle(GUI.skin.label);
                labelStyle.fontStyle = FontStyle.Bold;
            }
            GUILayout.Label(RDString.language == SystemLanguage.Korean ? "이미지 폴더 경로" : "Image Directory Path", labelStyle);
            GUILayout.BeginHorizontal();
            if (GUILayout.Button(RDString.language == SystemLanguage.Korean ? "폴더 변경" : "Change"))
            {
                string selected = Sprites.ShowFolderBrowserDialog();
                if (selected != null && Directory.Exists(selected))
                {
                    Settings.spriteDirectory = selected;
                    Sprites.Load();
                }
            }
            GUILayout.Label(Settings.spriteDirectory);
            GUILayout.FlexibleSpace();
            GUILayout.EndHorizontal();
            GUILayout.Space(10);
            GUILayout.Label(RDString.language == SystemLanguage.Korean ? "불 행성 이미지 크기" : "Fire Planet Image Size", labelStyle);
            GUILayout.BeginHorizontal();
            float redSize = GUILayout.HorizontalSlider(Settings.redSize, 0, 2, GUI.skin.horizontalSlider, GUI.skin.horizontalSliderThumb, GUILayout.Width(600));
            bool redChange = redSize != Settings.redSize;
            if (redChange)
                GUILayout.TextField($"{Settings.redSize}", GUILayout.Width(75));
            else
                try
                {
                    redSize = Convert.ToSingle(GUILayout.TextField($"{Settings.redSize}", GUILayout.Width(75)));
                    if (!(redSize > 2 || redSize < 0))
                        if (redSize != Settings.redSize)
                            redChange = true;
                }
                catch (Exception)
                {
                }
            GUILayout.EndHorizontal();
            GUILayout.Space(10);
            GUILayout.Label(RDString.language == SystemLanguage.Korean ? "얼음 행성 이미지 크기" : "Ice Planet Image Size", labelStyle);
            GUILayout.BeginHorizontal();
            float blueSize = GUILayout.HorizontalSlider(Settings.blueSize, 0, 2, GUI.skin.horizontalSlider, GUI.skin.horizontalSliderThumb, GUILayout.Width(600));
            bool blueChange = blueSize != Settings.blueSize;
            if (blueChange)
                GUILayout.TextField($"{Settings.blueSize}", GUILayout.Width(75));
            else
                try
                {
                    blueSize = Convert.ToSingle(GUILayout.TextField($"{Settings.blueSize}", GUILayout.Width(75)));
                    if (!(blueSize > 2 || blueSize < 0))
                        if (blueSize != Settings.blueSize)
                            blueChange = true;
                }
                catch (Exception)
                {
                }
            GUILayout.EndHorizontal();
            GUILayout.Space(10);
            bool redColor = GUILayout.Toggle(Settings.redColor,
                (RDString.language == SystemLanguage.Korean
                ? $"<color={ColorUtils.GetColor(true).ToHex()}>불 행성</color> 색을 이미지에 적용"
                : $"Apply <color={ColorUtils.GetColor(true).ToHex()}>Fire Planet</color> Color To Image"
                ) + $"  <color=grey>{(Settings.redColor ? "O" : "Ⅹ")}</color>",
                labelStyle);
            bool redColorChange = Settings.redColor != redColor;
            GUILayout.Space(5);
            bool blueColor = GUILayout.Toggle(Settings.blueColor,
                (RDString.language == SystemLanguage.Korean
                ? $"<color={ColorUtils.GetColor(false).ToHex()}>얼음 행성</color> 색을 이미지에 적용"
                : $"Apply <color={ColorUtils.GetColor(false).ToHex()}>Ice Planet</color> Color To Image"
                ) + $"  <color=grey>{(Settings.blueColor ? "O" : "Ⅹ")}</color>",
                labelStyle);
            bool blueColorChange = Settings.blueColor != blueColor;
            if (redChange)
            {
                Settings.redSize = redSize;
                scrController.instance.redPlanet.transform.GetComponentsInChildren<SpriteRenderer>().Last().transform.localScale = new Vector2(redSize, redSize);
            }
            if (blueChange)
            {
                Settings.blueSize = blueSize;
                scrController.instance.bluePlanet.transform.GetComponentsInChildren<SpriteRenderer>().Last().transform.localScale = new Vector2(blueSize, blueSize);
            }
            if (redColorChange)
            {
                Settings.redColor = redColor;
                if (redColor)
                    scrController.instance.redPlanet.transform.GetComponentsInChildren<SpriteRenderer>().Last().color = ColorUtils.GetColor(true);
                else
                    scrController.instance.redPlanet.transform.GetComponentsInChildren<SpriteRenderer>().Last().color = Color.white;
            }
            if (blueColorChange)
            {
                Settings.blueColor = blueColor;
                if (blueColor)
                    scrController.instance.bluePlanet.transform.GetComponentsInChildren<SpriteRenderer>().Last().color = ColorUtils.GetColor(false);
                else
                    scrController.instance.bluePlanet.transform.GetComponentsInChildren<SpriteRenderer>().Last().color = Color.white;
            }

            if (ADOBase.hasTaroDLC && ADOBase.ownsTaroDLC)
            {
                GUILayout.Space(60);
                GUILayout.Label($"<size=20>{(RDString.language == SystemLanguage.Korean ? "DLC 설정" : "DLC Settings")}</size>", labelStyle);
                GUILayout.Space(10);
                Settings.thirdPlanet = GUILayout.Toggle(Settings.thirdPlanet, (RDString.language == SystemLanguage.Korean ? "이미지 선택 창에서 세번째 행성 켜기" : "Turn on Third Planet in Image Select") + " <color=grey>" + (Settings.thirdPlanet ? "O" : "Ⅹ") + "</color>", labelStyle);
                GUILayout.Space(10);
                GUILayout.Label(RDString.language == SystemLanguage.Korean ? "세번쩨 행성 이미지 크기" : "Third Planet Image Size", labelStyle);
                GUILayout.BeginHorizontal();
                float thirdSize = GUILayout.HorizontalSlider(Settings.thirdSize, 0, 2, GUI.skin.horizontalSlider, GUI.skin.horizontalSliderThumb, GUILayout.Width(600));
                bool thirdChange = thirdSize != Settings.thirdSize;
                if (thirdChange)
                    GUILayout.TextField($"{Settings.thirdSize}", GUILayout.Width(75));
                else
                    try
                    {
                        thirdSize = Convert.ToSingle(GUILayout.TextField($"{Settings.thirdSize}", GUILayout.Width(75)));
                        if (!(thirdSize > 2 || thirdSize < 0))
                            if (thirdSize != Settings.thirdSize)
                                thirdChange = true;
                    }
                    catch (Exception)
                    {
                    }
                GUILayout.EndHorizontal();
                GUILayout.Space(10);
                bool thirdColor = GUILayout.Toggle(Settings.thirdColor,
                    (RDString.language == SystemLanguage.Korean
                    ? $"<color={Settings.ThirdColor().ToHex()}>세번째 행성</color> 색을 이미지에 적용"
                    : $"Apply <color={Settings.ThirdColor().ToHex()}>Third Planet</color> Color To Image"
                    ) + $"  <color=grey>{(Settings.thirdColor ? "O" : "Ⅹ")}</color>",
                    labelStyle);
                bool thirdColorChange = Settings.thirdColor != thirdColor;
                GUILayout.Space(10);
                GUILayout.BeginHorizontal();
                GUILayout.Label(RDString.language == SystemLanguage.Korean ? $"<color={Settings.ThirdColor().ToHex()}>세번째 행성 색</color>" : $"<color={Settings.ThirdColor().ToHex()}>Third Planet Color</color>");
                GUILayout.Space(10);
                Settings.thirdColorType = GUILayout.Toolbar(Settings.thirdColorType,
                    RDString.language == SystemLanguage.Korean
                    ? new string[] { "<color=4CB200>기본</color>", $"<color={ColorUtils.GetColor(true).ToHex()}>불 행성</color>", $"<color={ColorUtils.GetColor(false).ToHex()}>얼음 행성</color>", $"<color={Settings.CustomThirdColor().ToHex()}>커스텀</color>" }
                    : new string[] { "<color=4CB200>Default</color>", $"<color={ColorUtils.GetColor(true).ToHex()}>Fire Planet</color>", $"<color={ColorUtils.GetColor(false).ToHex()}>Ice Planet</color>", $"<color={Settings.CustomThirdColor().ToHex()}>Custom</color>" });
                GUILayout.FlexibleSpace();
                GUILayout.EndHorizontal();
                GUILayout.Space(5);
                GUILayout.BeginHorizontal();
                GUILayout.Label(RDString.language == SystemLanguage.Korean ? "커스텀 색" : "Custom Color");
                GUILayout.Space(10);
                Settings.thirdColorRed = int.TryParse(GUILayout.TextField(Settings.thirdColorRed.ToString()), out int v) ? v : Settings.thirdColorRed;
                GUILayout.Space(5);
                Settings.thirdColorGreen = int.TryParse(GUILayout.TextField(Settings.thirdColorGreen.ToString()), out v) ? v : Settings.thirdColorGreen;
                GUILayout.Space(5);
                Settings.thirdColorBlue = int.TryParse(GUILayout.TextField(Settings.thirdColorBlue.ToString()), out v) ? v : Settings.thirdColorBlue;
                GUILayout.FlexibleSpace();
                GUILayout.EndHorizontal();
                if (scrController.instance.allPlanets[2].GetPlanetColor(default) != Settings.ThirdColor())
                {
                    scrController.instance.allPlanets[2].SetPlanetColor(Settings.ThirdColor());
                    scrController.instance.allPlanets[2].SetTailColor(Settings.ThirdColor());
                }

                if (thirdChange)
                {
                    Settings.thirdSize = thirdSize;
                    scrController.instance.allPlanets[2].transform.GetComponentsInChildren<SpriteRenderer>().Last().transform.localScale = new Vector2(thirdSize, thirdSize);
                }
                if (thirdColorChange)
                {
                    Settings.thirdColor = thirdColor;
                    if (thirdColor)
                        scrController.instance.allPlanets[2].transform.GetComponentsInChildren<SpriteRenderer>().Last().color = Settings.ThirdColor();
                    else
                        scrController.instance.allPlanets[2].transform.GetComponentsInChildren<SpriteRenderer>().Last().color = Color.white;
                }
            }
        }

        private static void OnSaveGUI(UnityModManager.ModEntry modEntry)
        {
            Logger.Log("Saving Settings...");
            Settings.Save(modEntry);
            Logger.Log("Save Completed!");
            Logger.Log("Saving Sprites...");
            Sprites.Save();
            Logger.Log("Save Completed!");
        }
    }
}