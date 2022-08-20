using HarmonyLib;
using PlanetTweaks.Patch;
using PlanetTweaks.UI;
using PlanetTweaks.Utils;
using PlayTweaks.Components;
using System;
using System.IO;
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

        public static void Setup(UnityModManager.ModEntry modEntry)
        {
            ModEntry = modEntry;
            Logger = modEntry.Logger;
            modEntry.OnToggle = OnToggle;
            modEntry.OnUpdate = OnUpdate;
            modEntry.OnGUI = OnGUI;
            modEntry.OnSaveGUI = OnSaveGUI;
            Logger.Log("Loading AssetBundle...");
            Assets.Load();
            PlanetTweaksMenu.Init(Assets.MenuObject);
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
                NoStopPatch.TryPatch();
            }
            else
            {
                Harmony.UnpatchAll(modEntry.Info.Id);
            }
            return true;
        }

        private static void OnUpdate(UnityModManager.ModEntry modEntry, float fl)
        {
            if (Input.GetKey(KeyCode.LeftControl) && Input.GetKey(KeyCode.LeftShift) && Input.GetKeyDown(KeyCode.P))
            {
                PlanetTweaksMenu.Instance.Toggle();
            }
            if (Input.GetKey(KeyCode.LeftControl) && Input.GetKey(KeyCode.LeftShift) && Input.GetKeyDown(KeyCode.T))
            {
                scrController.instance.redPlanet.SwitchToSpark(new Color(0.95f, 0.95f, 0.95f), new Color(0.72f, 0.72f, 0.72f), new Color(0.7f, 0.7f, 0.7f), new Color(0.52f, 0.52f, 0.52f), new Color(0.5f, 0.5f, 0.5f));
            }
            if (Input.GetKey(KeyCode.LeftControl) && Input.GetKey(KeyCode.LeftShift) && Input.GetKeyDown(KeyCode.R))
            {
                scrController.instance.redPlanet.SetColor(PlanetColor.DefaultRed);
                scrController.instance.bluePlanet.SetColor(PlanetColor.DefaultBlue);
            }
            if (Input.GetKey(KeyCode.LeftControl) && Input.GetKey(KeyCode.LeftShift) && Input.GetKeyDown(KeyCode.G))
            {
                scrController.instance.redPlanet.SwitchToGold();
                scrController.instance.bluePlanet.SwitchToGold();

                string ToHex(Color c)
                {
                    return string.Format("#{0:X2}{1:X2}{2:X2}{3:X2}", (byte)(Mathf.Clamp01(c.r) * 255), (byte)(Mathf.Clamp01(c.g) * 255), (byte)(Mathf.Clamp01(c.b) * 255), (byte)(Mathf.Clamp01(c.a) * 255));
                }

                Logger.Log("---------------------------");

                ParticleSystem particleSystem = scrController.instance.redPlanet.goldPlanet.transform.Find("SparksGold").GetComponent<ParticleSystem>();
                ParticleSystem.ColorOverLifetimeModule colorOverLifetime = particleSystem.colorOverLifetime;
                {
                    Gradient gradient = colorOverLifetime.color.gradientMin;
                    Logger.Log("---------------------------");
                    foreach (var v in gradient.colorKeys)
                        Logger.Log(ToHex(v.color) + " for " + v.time);
                    foreach (var v in gradient.alphaKeys)
                        Logger.Log(v.alpha + " for " + v.time);
                    Gradient gradient2 = colorOverLifetime.color.gradientMax;
                    Logger.Log("---------------------------");
                    foreach (var v in gradient2.colorKeys)
                        Logger.Log(ToHex(v.color) + " for " + v.time);
                    foreach (var v in gradient2.alphaKeys)
                        Logger.Log(v.alpha + " for " + v.time);
                    Logger.Log("---------------------------");
                }
            }
        }

        private static GUIStyle bold;

        private static int r = 255, g = 255, b = 255;

        private static void OnGUI(UnityModManager.ModEntry modEntry)
        {
            GUILayout.BeginHorizontal();
            r = (int)GUILayout.HorizontalSlider(r, 0, 255);
            GUILayout.Label($"{r} ({r / 255f})");
            GUILayout.EndHorizontal();
            GUILayout.BeginHorizontal();
            g = (int)GUILayout.HorizontalSlider(g, 0, 255);
            GUILayout.Label($"{g} ({g / 255f})");
            GUILayout.EndHorizontal();
            GUILayout.BeginHorizontal();
            b = (int)GUILayout.HorizontalSlider(b, 0, 255);
            GUILayout.Label($"{b} ({b / 255f})");
            GUILayout.EndHorizontal();

            if (bold == null)
            {
                bold = new GUIStyle(GUI.skin.label);
                bold.fontStyle = FontStyle.Bold;
            }
            GUILayout.Label(RDString.language == SystemLanguage.Korean ? "이미지 폴더 경로" : "Image Directory Path", bold);
            GUILayout.BeginHorizontal();
            if (GUILayout.Button(RDString.language == SystemLanguage.Korean ? "폴더 변경" : "Change", GUILayout.Width(70)))
            {
                string selected = Sprites.ShowFolderBrowserDialog();
                if (selected != null)
                    Settings.spriteDirectory = selected;
            }
            GUILayout.Label(Settings.spriteDirectory);
            GUILayout.EndHorizontal();
            GUILayout.Space(10);
            GUILayout.Label(RDString.language == SystemLanguage.Korean ? "불 행성 이미지 크기" : "Fire Planet Image Size", bold);
            GUILayout.BeginHorizontal();
            float redSize = GUILayout.HorizontalSlider(Settings.redSize, 0, 2, GUILayout.Width(600));
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
            GUILayout.Label(RDString.language == SystemLanguage.Korean ? "얼음 행성 이미지 크기" : "Ice Planet Image Size", bold);
            GUILayout.BeginHorizontal();
            float blueSize = GUILayout.HorizontalSlider(Settings.blueSize, 0, 2, GUILayout.Width(600));
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
            Settings.redColor = GUILayout.Toggle(Settings.redColor,
                (RDString.language == SystemLanguage.Korean ? "불 행성 이미지 색 적용" : "Apply Fire Planet Image Color") + $"  <color=grey>{(Settings.redColor ? "O" : "Ⅹ")}</color>",
                bold);
            GUILayout.Space(5);
            Settings.blueColor = GUILayout.Toggle(Settings.blueColor,
                (RDString.language == SystemLanguage.Korean ? "얼음 행성 이미지 색 적용" : "Apply Ice Planet Image Color") + $"  <color=grey>{(Settings.blueColor ? "O" : "Ⅹ")}</color>",
                bold);
            if (redChange)
            {
                Settings.redSize = redSize;
                scrController.instance.redPlanet.GetRenderer().transform.localScale = new Vector2(redSize, redSize) * (100f / SpriteUtils.Size);
            }
            if (blueChange)
            {
                Settings.blueSize = blueSize;
                scrController.instance.bluePlanet.GetRenderer().transform.localScale = new Vector2(blueSize, blueSize) * (100f / SpriteUtils.Size);
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