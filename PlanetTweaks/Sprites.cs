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
        private static VistaOpenFileDialog openFileDlg;
        public static int Size => 100;
        public static float FSize => Size;

        public static void Init()
        {
            openFileDlg = new VistaOpenFileDialog();
            var filesEnumerable = from string str in imageFiles select "*" + str;
            string files = string.Join(";", filesEnumerable);
            openFileDlg.Filter = "Image Files(" + files + ")|" + files;
            openFileDlg.Multiselect = false;
        }

        public static string ShowOpenFileDialog()
        {
            DialogResult result = openFileDlg.ShowDialog();
            if (result == DialogResult.OK)
            {
                try
                {
                    return openFileDlg.FileName;
                }
                catch (Exception e)
                {
                    Main.Logger.Log(e.Message);
                }
            }
            return null;
        }

        public static void Load()
        {
            DirectoryInfo dir = GetSpritesDirectory().CreateIfNotExists();
            sprites.Clear();
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
            DirectoryInfo dir = GetSpritesDirectory().CreateIfNotExists();
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

        public static string GetSpritesDirectory()
        {
            return Path.Combine(".", "Mods", "PlanetTweaks", "sprites");
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
