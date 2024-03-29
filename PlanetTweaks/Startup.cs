﻿using HarmonyLib;
using System;
using System.IO;
using UnityModManagerNet;

namespace PlanetTweaks
{
    public static class Startup
    {
        public static void Load(UnityModManager.ModEntry modEntry)
        {
            LoadAssembly("Mods/PlanetTweaks/Newtonsoft.Json.dll");
            AccessTools.Method($"{typeof(Startup).Namespace}.Main:Setup").Invoke(null, new object[] { modEntry });
        }

        private static void LoadAssembly(string path)
        {
            using (FileStream stream = new FileStream(path, FileMode.Open))
            {
                byte[] data = new byte[stream.Length];
                stream.Read(data, 0, data.Length);
                AppDomain.CurrentDomain.Load(data);
            }
        }
    }
}
