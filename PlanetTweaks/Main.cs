using HarmonyLib;
using PlayTweaks.Components;
using System;
using System.Linq;
using System.Reflection;
using UnityEngine;
using UnityModManagerNet;

namespace PlanetTweaks
{
    public static class Main
    {
        public static UnityModManager.ModEntry.ModLogger Logger;
        public static Harmony harmony;
        public static bool IsEnabled = false;
        public static Settings Settings;

        public static void Setup(UnityModManager.ModEntry modEntry)
        {
            Logger = modEntry.Logger;
            modEntry.OnToggle = OnToggle;
            modEntry.OnGUI = OnGUI;
            modEntry.OnSaveGUI = OnSaveGUI;
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
                harmony = new Harmony(modEntry.Info.Id);
                harmony.PatchAll(Assembly.GetExecutingAssembly());
            }
            else
            {
                harmony.UnpatchAll(modEntry.Info.Id);
            }
            return true;
        }

        private static GUIStyle labelStyle;
        private static GUIStyle btnStyle;
        private static GUIStyle sliderStyle;
        private static GUIStyle tfStyle;

        private static void OnGUI(UnityModManager.ModEntry modEntry)
        {
            if (labelStyle == null)
            {
                labelStyle = new GUIStyle(GUI.skin.label);
                labelStyle.fontStyle = FontStyle.Bold;
            }
            if (btnStyle == null)
            {
                btnStyle = new GUIStyle(GUI.skin.button);
                btnStyle.fixedWidth = 70;
            }
            if (sliderStyle == null)
            {
                sliderStyle = new GUIStyle(GUI.skin.horizontalSlider);
                sliderStyle.fixedWidth = 600;
            }
            if (tfStyle == null)
            {
                tfStyle = new GUIStyle(GUI.skin.textField);
                tfStyle.fixedWidth = 75;
            }
            GUILayout.Label("이미지 폴더 경로", labelStyle);
            GUILayout.BeginHorizontal();
            if (GUILayout.Button("폴더 선택", btnStyle))
            {
                string selected = Sprites.ShowOpenFileDialog();
                if (selected != null)
                {
                    Settings.spriteDirectory = selected;
                    Sprites.Load();
                }
            }
            GUILayout.Label(Settings.spriteDirectory);
            GUILayout.EndHorizontal();
            GUILayout.Space(10);
            GUILayout.Label("불 행성 이미지 크기", labelStyle);
            GUILayout.BeginHorizontal();
            bool redChange = false;
            float redSize = GUILayout.HorizontalSlider(Settings.redSize, 0, 2, sliderStyle, GUI.skin.horizontalSliderThumb);
            if (redSize != Settings.redSize)
                redChange = true;
            if (redChange)
                GUILayout.TextField($"{Settings.redSize}", tfStyle);
            else
                try
                {
                    redSize = Convert.ToSingle(GUILayout.TextField($"{Settings.redSize}", tfStyle));
                    if (!(redSize > 2 || redSize < 0))
                        if (redSize != Settings.redSize)
                            redChange = true;
                } catch (Exception) {
                }
            GUILayout.EndHorizontal();
            GUILayout.Space(10);
            GUILayout.Label("얼음 행성 이미지 크기", labelStyle);
            GUILayout.BeginHorizontal();
            bool blueChange = false;
            float blueSize = GUILayout.HorizontalSlider(Settings.blueSize, 0, 2, sliderStyle, GUI.skin.horizontalSliderThumb);
            if (blueSize != Settings.blueSize)
                blueChange = true;
            if (blueChange)
                GUILayout.TextField($"{Settings.blueSize}", tfStyle);
            else
                try
                {
                    blueSize = Convert.ToSingle(GUILayout.TextField($"{Settings.blueSize}", tfStyle));
                    if (!(blueSize > 2 || blueSize < 0))
                        if (blueSize != Settings.blueSize)
                            blueChange = true;
                }
                catch (Exception)
                {
                }
            GUILayout.EndHorizontal();
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