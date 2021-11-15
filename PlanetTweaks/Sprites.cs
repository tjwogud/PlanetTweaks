using Ookii.Dialogs;
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
        public static Dictionary<string, Sprite> sprites = new Dictionary<string, Sprite>();
        public static string[] imageFiles = new string[] { ".png", ".jpg", ".jpeg" };
        private static VistaOpenFileDialog fileDialog;
        private static VistaFolderBrowserDialog dirDialog;
        public static int Size => 100;
        public static float FSize => Size;

        private static Sprite redPreview;
        public static Sprite RedPreview
        {
            get
            {
                return redPreview;
            }

            set
            {
                redPreview = value;
                var planet = scrController.instance?.redPlanet;
                if (planet == null)
                    return;
                var renderer = planet.transform.GetChild(planet.transform.childCount - 1).GetComponent<SpriteRenderer>();
                renderer.enabled = true;
                renderer.transform.position = planet.transform.position;
                renderer.sprite = value;

                if (value != null || RedSprite == null)
                    return;
                renderer.sprite = RedSprite;
            }
        }

        private static Sprite bluePreview;
        public static Sprite BluePreview
        {
            get
            {
                return bluePreview;
            }

            set
            {
                bluePreview = value;
                var planet = scrController.instance?.bluePlanet;
                if (planet == null)
                    return;
                var renderer = planet.transform.GetChild(planet.transform.childCount - 1).GetComponent<SpriteRenderer>();
                renderer.enabled = true;
                renderer.transform.position = planet.transform.position;
                renderer.sprite = value;

                if (value != null || BlueSprite == null)
                    return;
                renderer.sprite = BlueSprite;
            }
        }

        public static Sprite RedSprite { get; private set; }
        public static Sprite BlueSprite { get; private set; }
        public static int RedSelected
        {
            get => Main.Settings.redSelected;

            set
            {
                if (value < 0)
                {
                    Main.Settings.redSelected = value;
                    RedSprite = null;
                    var planet = scrController.instance?.redPlanet;
                    var renderer = planet.transform.GetComponentsInChildren<SpriteRenderer>().Last();
                    renderer.sprite = null;
                    return;
                }
                else
                {
                    if (value >= sprites.Count)
                        return;
                    Main.Settings.redSelected = value;
                    RedSprite = sprites.ElementAt(value).Value;

                    var planet = scrController.instance?.redPlanet;
                    if (planet == null)
                        return;
                    var renderer = planet.transform.GetComponentsInChildren<SpriteRenderer>().Last();
                    renderer.enabled = true;
                    renderer.transform.position = planet.transform.position;
                    renderer.sprite = RedSprite;
                }
            }
        }
        public static int BlueSelected
        {
            get => Main.Settings.blueSelected;

            set
            {
                if (value < 0)
                {
                    Main.Settings.blueSelected = value;
                    BlueSprite = null;
                    var planet = scrController.instance?.bluePlanet;
                    var renderer = planet.transform.GetComponentsInChildren<SpriteRenderer>().Last();
                    renderer.sprite = null;
                    return;
                }
                else
                {
                    if (value >= sprites.Count)
                        return;
                    Main.Settings.blueSelected = value;
                    BlueSprite = sprites.ElementAt(value).Value;

                    var planet = scrController.instance?.bluePlanet;
                    if (planet == null)
                        return;
                    var renderer = planet.transform.GetComponentsInChildren<SpriteRenderer>().Last();
                    renderer.enabled = true;
                    renderer.transform.position = planet.transform.position;
                    renderer.sprite = BlueSprite;
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
            dirDialog.SelectedPath = Main.Settings.spriteDirectory;
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
                    Add(Path.Combine(dir.FullName, file.Name));
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
                if (!file.Name.IsImageFile())
                    continue;
                try
                {
                    file.Delete();
                }
                catch (Exception)
                {
                }
            }
            foreach (var pair in sprites)
            {
                try
                {
                    Texture2D texture = pair.Value.texture;
                    byte[] bytes = texture.EncodeToPNG();
                    string path = Path.Combine(dir.FullName, pair.Key + ".png");
                    File.WriteAllBytes(path, bytes);
                }
                catch (Exception)
                {
                }
            }
        }

        public static void Add(string fileName)
        {
            if (!fileName.IsImageFile())
                throw new ArgumentException("file is not image file! (supporting files : " + string.Join(", ", imageFiles) + ")");
            if (!File.Exists(fileName))
                throw new ArgumentException("file doesn't exists!");
            Sprite sprite = File.ReadAllBytes(fileName).ToSprite();
            string name = new FileInfo(fileName).Name;
            name = name.Substring(0, name.LastIndexOf('.'));
            string first = name;
            for (int i = 1; sprites.ContainsKey(name); i++)
                name = first + i;
            sprite.name = name;
            sprites.Add(name, sprite);
        }

        public static Texture2D ResizeFix(this Texture2D texture)
        {
            int targetWidth
                = texture.width >= texture.height
                ? Size
                : (int)(FSize / texture.height * texture.width);
            int targetHeight
                = texture.width >= texture.height
                ? (int)(FSize / texture.width * texture.height)
                : Size;
            Texture2D result = new Texture2D(targetWidth, targetHeight, texture.format, true);
            Color[] pixels = result.GetPixels(0);
            float incX = 1.0f / targetWidth;
            float incY = 1.0f / targetHeight;
            for (int pixel = 0; pixel < pixels.Length; pixel++)
            {
                pixels[pixel] = texture.GetPixelBilinear(incX * ((float)pixel % targetWidth), incY * ((float)Mathf.Floor(pixel / targetWidth)));
            }
            result.SetPixels(pixels, 0);
            result.Apply();
            return result;
        }

        public static DirectoryInfo CreateIfNotExists(this string dirName)
        {
            if (!Directory.Exists(dirName))
                return Directory.CreateDirectory(dirName);
            return new DirectoryInfo(dirName);
        }

        public static Sprite ToSprite(this byte[] data)
        {
            Texture2D texture = new Texture2D(0, 0);
            if (texture.LoadImage(data))
            {
                texture = texture.ResizeFix();
                Rect rect = new Rect(0, 0, texture.width, texture.height);
                return Sprite.Create(texture, rect, new Vector2(0.5f, 0.5f));
            }
            return null;
        }

        public static bool IsImageFile(this string file)
        {
            string extension = file.Substring(file.LastIndexOf('.')).ToLower();
            foreach (var extension2 in imageFiles)
            {
                if (extension.Equals(extension2))
                    return true;
            }
            return false;
        }
    }
}
