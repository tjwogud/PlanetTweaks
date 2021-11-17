using HarmonyLib;
using PlayTweaks.Components;
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