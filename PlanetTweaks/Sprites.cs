using Ookii.Dialogs;
using PlanetTweaks.Utils;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using UnityEngine;

namespace PlanetTweaks
{
    public static class Sprites
    {
        public static Dictionary<string, PlanetImage> sprites = new Dictionary<string, PlanetImage>();
        public static string[] imageFiles = new string[] { ".png", ".jpg", ".jpeg", ".gif", ".gifinfo" };
        private static VistaOpenFileDialog fileDialog;
        private static VistaFolderBrowserDialog dirDialog;

        private static PlanetImage redPreview;
        public static PlanetImage RedPreview
        {
            get => redPreview;

            set
            {
                redPreview = value;
                var planet = scrController.instance?.redPlanet;
                if (planet == null)
                    return;
                if (value != null)
                    value.Apply(planet);
                else if (RedImage != null)
                    RedImage.Apply(planet);
                else
                    PlanetImage.ClearRenderer(planet);
            }
        }

        private static PlanetImage bluePreview;
        public static PlanetImage BluePreview
        {
            get => bluePreview;

            set
            {
                bluePreview = value;
                var planet = scrController.instance?.bluePlanet;
                if (planet == null)
                    return;
                if (value != null)
                    value.Apply(planet);
                else if (BlueImage != null)
                    BlueImage.Apply(planet);
                else
                    PlanetImage.ClearRenderer(planet);
            }
        }

        public static PlanetImage RedImage { get; private set; }
        public static PlanetImage BlueImage { get; private set; }
        public static string RedSelected
        {
            get
            {
                if (Main.Settings.redSelected == null)
                    return null;
                if (sprites.ContainsKey(Main.Settings.redSelected))
                    return Main.Settings.redSelected;
                else
                {
                    Main.Settings.redSelected = null;
                    return null;
                }
            }

            set
            {
                if (value == null)
                {
                    Main.Settings.redSelected = null;
                    RedImage = null;
                    var planet = scrController.instance?.redPlanet;
                    if (planet == null)
                        return;
                    PlanetImage.ClearRenderer(planet);
                }
                else
                {
                    if (!sprites.ContainsKey(value))
                        return;
                    Main.Settings.redSelected = value;
                    RedImage = sprites[value];
                    var planet = scrController.instance?.redPlanet;
                    if (planet == null)
                        return;
                    RedImage.Apply(planet);
                }
            }
        }
        public static string BlueSelected
        {
            get
            {
                if (Main.Settings.blueSelected == null)
                    return null;
                if (sprites.ContainsKey(Main.Settings.blueSelected))
                    return Main.Settings.blueSelected;
                else
                {
                    Main.Settings.blueSelected = null;
                    return null;
                }
            }

            set
            {
                if (value == null)
                {
                    Main.Settings.blueSelected = null;
                    BlueImage = null;
                    var planet = scrController.instance?.bluePlanet;
                    if (planet == null)
                        return;
                    PlanetImage.ClearRenderer(planet);
                }
                else
                {
                    if (!sprites.ContainsKey(value))
                        return;
                    Main.Settings.blueSelected = value;
                    BlueImage = sprites[value];
                    var planet = scrController.instance?.bluePlanet;
                    if (planet == null)
                        return;
                    BlueImage.Apply(planet);
                }
            }
        }

        public static void Init()
        {
            fileDialog = new VistaOpenFileDialog();
            var filesEnumerable = from string str in imageFiles select "*" + str;
            string files = string.Join(";", filesEnumerable);
            fileDialog.Filter = "Image Files(" + files + ")|" + files;
            fileDialog.Multiselect = false;

            dirDialog = new VistaFolderBrowserDialog();
            dirDialog.Set("_selectedPath", Main.Settings.spriteDirectory);
        }

        public static string ShowOpenFileDialog()
        {
            DialogResult result = fileDialog.ShowDialog();
            if (result == DialogResult.OK)
            {
                try
                {
                    return fileDialog.FileName;
                }
                catch (Exception e)
                {
                    Main.Logger.Log(e.StackTrace);
                }
            }
            return null;
        }

        public static string ShowFolderBrowserDialog()
        {
            DialogResult result = dirDialog.ShowDialog();
            if (result == DialogResult.OK)
            {
                try
                {
                    return dirDialog.SelectedPath;
                }
                catch (Exception e)
                {
                    Main.Logger.Log(e.StackTrace);
                }
            }
            return null;
        }

        public static void Load()
        {
            DirectoryInfo dir = Main.Settings.spriteDirectory.CreateIfNotExists();
            foreach (FileInfo file in dir.GetFiles())
            {
                try
                {
                    if (PlanetImage.Load(file.FullName, out PlanetImage image))
                        sprites.Add(image.Name.ToLower(), image);
                }
                catch (Exception)
                {
                }
            }
        }

        public static void Save()
        {
            DirectoryInfo dir = Main.Settings.spriteDirectory.CreateIfNotExists();
            foreach (FileInfo file in dir.GetFiles())
            {
                try
                {
                    file.Delete();
                }
                catch (Exception)
                {
                }
            }
            foreach (DirectoryInfo directory in dir.GetDirectories())
            {
                try
                {
                    directory.Delete();
                }
                catch (Exception)
                {
                }
            }
            foreach (var pair in sprites)
                if (!pair.Value.Save(dir.FullName))
                    Main.Logger.Log($"can't save image '{pair.Key}'!");
        }

        public static void Add(string fileName)
        {
            if (!fileName.IsImageFile())
                throw new ArgumentException("file is not image file! (supporting files : " + string.Join(", ", imageFiles) + ")");
            if (!File.Exists(fileName))
                throw new ArgumentException("file doesn't exists!");
            if (PlanetImage.Load(fileName, out PlanetImage image))
                sprites.Add(image.Name.ToLower(), image);
            else
                throw new ArgumentException("can't load the file!");
        }

        public static bool Remove(string name)
        {
            if (!sprites.ContainsKey(name))
                return false;
            if (Main.Settings.redSelected == name)
                RedSelected = null;
            if (Main.Settings.blueSelected == name)
                BlueSelected = null;
            sprites.Remove(name);
            return true;
        }

        public static bool Remove(int index)
        {
            if (index < 0 || index >= sprites.Count())
                return false;
            return Remove(sprites.Keys.ElementAt(index));
        }

        public static SpriteRenderer GetRenderer(this scrPlanet planet)
        {
            SpriteRenderer renderer;
            if ((renderer = planet.transform.Find("PlanetTweaks.Renderer")?.GetComponent<SpriteRenderer>()) == null)
            {
                renderer = new GameObject("PlanetTweaks.Renderer").AddComponent<SpriteRenderer>();
                renderer.sortingOrder = planet.sprite.sortingOrder + 1;
                renderer.sortingLayerID = planet.sprite.sortingLayerID;
                renderer.sortingLayerName = planet.sprite.sortingLayerName;
                renderer.transform.parent = planet.transform;
                if (planet.isRed)
                {
                    renderer.transform.localScale = new Vector3(Main.Settings.redSize, Main.Settings.redSize) * (100f / SpriteUtils.Size);
                    if (Main.Settings.redColor)
                        renderer.color = PlanetUtils.GetColor(true);
                }
                else
                {
                    renderer.transform.localScale = new Vector3(Main.Settings.blueSize, Main.Settings.blueSize) * (100f / SpriteUtils.Size);
                    if (Main.Settings.blueColor)
                        renderer.color = PlanetUtils.GetColor(false);
                }
            }
            return renderer;
        }

        public static bool IsImageFile(this string file) => imageFiles.Contains(Path.GetExtension(file));
    }
}
