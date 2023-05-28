using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using UnityEngine;

namespace PlanetTweaks.Utils
{
    public class GifImage
    {
        public int Length { get; private set; }
        public GifFrame[] Frames { get; private set; }
        public Sprite Thumbnail => Frames.Length == 0 ? null : Frames[0].Sprite;

        public GifImage(string gif)
        {
            List<(int, Sprite)> frames = Image.FromFile(gif).GetFrames();
            int i = 0;
            Frames = (from tuple in frames select new GifFrame(i++, tuple.Item1, tuple.Item2)).ToArray();
            Length = (from tuple in frames select tuple.Item1).Sum();
        }

        public GifImage(GifFrame[] frames)
        {
            Frames = frames;
            Length = (from frame in frames select frame.Length).Sum();
        }

        public Sprite GetFrameAt(long millisecond) =>
            (Array.Find(Frames, f =>
            {
                if (f == null || millisecond <= f.Length)
                    return true;
                millisecond -= f.Length;
                return false;
            }) ?? Frames.Last()).Sprite;

        public class GifFrame
        {
            public int Index { get; private set; }
            public int Length { get; private set; }
            public Sprite Sprite { get; private set; }

            public GifFrame(int index, int length, Sprite sprite)
            {
                Index = index;
                Length = length;
                Sprite = sprite;
            }

            public JObject ToJson(string path)
            {
                JObject obj = new JObject();
                obj["index"] = Index;
                obj["length"] = Length;
                obj["path"] = Path.Combine(path, Index + ".png");
                return obj;
            }
        }

        public void Save(string path, string name)
        {
            File.WriteAllText(Path.Combine(path, name + ".gifmeta"), JsonConvert.SerializeObject(Frames.Select(f => f.ToJson(Path.Combine(path, name)))));
            Directory.CreateDirectory(Path.Combine(path, name));
            foreach (GifFrame frame in Frames)
            {
                Texture2D texture = frame.Sprite.texture;
                File.WriteAllBytes(Path.Combine(path, name, frame.Index + ".png"), texture.EncodeToPNG());
            }
        }

        public static GifImage Load(string path)
        {
            JObject[] frameMetas = JsonConvert.DeserializeObject<JObject[]>(File.ReadAllText(path));
            GifFrame[] frames = new GifFrame[frameMetas.Length];
            foreach (JObject meta in frameMetas)
            {
                int index = (int)meta["index"];
                int length = (int)meta["length"];
                Sprite sprite = File.ReadAllBytes((string)meta["path"]).ToSprite();
                sprite.name = "frame_" + index;
                frames[index] = new GifFrame(index, length, sprite);
            }
            return new GifImage(frames);
        }
    }
}
