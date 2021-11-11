using HarmonyLib;
using PlayTweaks.Components;
using System.Reflection;
using UnityModManagerNet;

namespace PlanetTweaks
{
    public static class Main
    {
        public static UnityModManager.ModEntry.ModLogger Logger;
        public static Harmony harmony;
        public static bool IsEnabled = false;

        public static void Setup(UnityModManager.ModEntry modEntry)
        {
            Logger = modEntry.Logger;
            modEntry.OnToggle = OnToggle;
            modEntry.OnSaveGUI = OnSaveGUI;
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

        private static void OnSaveGUI(UnityModManager.ModEntry modEntry)
        {
            Logger.Log("Saving Sprites...");
            Sprites.Save();
            Logger.Log("Save Completed!");
        }
    }
}