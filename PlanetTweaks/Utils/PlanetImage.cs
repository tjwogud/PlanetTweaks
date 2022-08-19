using PlanetTweaks.Components;
using PlanetTweaks.Utils;
using System;
using System.IO;
using UnityEngine;

namespace PlanetTweaks.Utils
{
    public abstract class PlanetImage
    {
        public string Name { get; private set; }

        public static bool Load(string file, out PlanetImage image)
        {
            string name = new FileInfo(file).Name;
            name = name.Substring(0, name.LastIndexOf('.')).ToLower();
            string first = name;
            for (int i = 1; Sprites.sprites.ContainsKey(name); i++)
                name = first + i;
            switch (Path.GetExtension(file))
            {
                case ".gifinfo":
                    {
                        throw new NotImplementedException("gifinfo is not supported yet!");
                    }
                case ".gif":
                    {
                        image = new PlanetGIF(new GifImage(file)) { Name = name };
                        return true;
                    }
                default:
                    {
                        Sprite sprite = File.ReadAllBytes(file).ToSprite();
                        sprite.name = name;
                        image = new PlanetPNG(sprite) { Name = name };
                        return true;
                    }
            }
        }

        public abstract bool Save(string dir);

        public abstract void Apply(scrPlanet planet);

        public abstract Sprite GetThumbnail();

        public static void ClearRenderer(scrPlanet planet)
        {
            foreach (var image in planet.gameObject.GetComponents<PlanetImageRenderer>())
                UnityEngine.Object.Destroy(image);
            planet.GetRenderer().sprite = null;
        }

        public class PlanetPNG : PlanetImage
        {
            public PlanetPNG(Sprite sprite) => Sprite = sprite;

            public Sprite Sprite { get; private set; }

            public override void Apply(scrPlanet planet)
            {
                ClearRenderer(planet);
                var png = planet.gameObject.AddComponent<PlanetImageRenderer.PlanetPNGRenderer>();
                png.sprite = Sprite;
            }

            public override Sprite GetThumbnail() => Sprite;

            public override bool Save(string dir)
            {
                try
                {
                    Texture2D texture = Sprite.texture;
                    byte[] bytes = texture.EncodeToPNG();
                    string path = Path.Combine(dir, Name + ".png");
                    if (File.Exists(path))
                        File.Delete(path);
                    File.WriteAllBytes(path, bytes);
                    return true;
                }
                catch (Exception)
                {
                    return false;
                }
            }
        }

        public class PlanetGIF : PlanetImage
        {
            public PlanetGIF(GifImage gif) => Gif = gif;

            public GifImage Gif { get; private set; }
            private Sprite Thumbnail;

            public override void Apply(scrPlanet planet)
            {
                ClearRenderer(planet);
                var gif = planet.gameObject.AddComponent<PlanetImageRenderer.PlanetGIFRenderer>();
                gif.gif = Gif;
            }

            public override Sprite GetThumbnail()
            {
                if (Thumbnail != null)
                    return Thumbnail;
                Sprite first = Gif.GetFrameAt(0);
                Thumbnail = Sprite.Create(first.texture, first.rect, new Vector2(0.5f, 0.5f));
                Thumbnail.name = Name;
                return Thumbnail;
            }

            public override bool Save(string dir)
            {
                return false;
            }
        }

        public class PlanetMultiImage : PlanetImage
        {
            public PlanetMultiImage(params Sprite[] sprites) => Sprites = sprites;

            public Sprite[] Sprites { get; private set; }
            private Sprite Thumbnail;

            public override void Apply(scrPlanet planet)
            {
                ClearRenderer(planet);
                var emoji = planet.gameObject.AddComponent<PlanetImageRenderer.PlanetMultiImageRenderer>();
                emoji.sprites = Sprites;
            }

            public override Sprite GetThumbnail()
            {
                if (Thumbnail != null)
                    return Thumbnail;
                Sprite first = Sprites[0];
                Thumbnail = Sprite.Create(first.texture, first.rect, new Vector2(0.5f, 0.5f));
                Thumbnail.name = Name;
                return Thumbnail;
            }

            public override bool Save(string dir)
            {
                return false;
            }
        }
    }
}
