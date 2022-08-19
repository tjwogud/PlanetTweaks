using PlanetTweaks.Components;
using System;
using System.IO;
using System.Reflection;
using UnityEngine;

namespace PlanetTweaks
{
    public static class Assets
    {
        public static AssetBundle Bundle { private set; get; }
        public static GameObject MenuObject { private set; get; }

        public static void Load()
        {
            AssetBundle bundle = AssetBundle.LoadFromFile(Path.Combine(Main.ModEntry.Path, "planettweaks"));
            Bundle = bundle ?? throw new Exception("can't load assetbundle!");
            MenuObject = bundle.LoadAsset<GameObject>("PlanetTweaksMenuPrefab") ?? throw new Exception("can't load asset from bundle!");
        }
    }
}
