using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using UnityEngine;

namespace PlanetTweaks.Utils
{
    public class GifImage
    {
        public int Length { get; private set; }
        private readonly List<GifFrame> frames;

        public GifImage(string gif)
        {
            var frames = Image.FromFile(gif).GetFrames();
            this.frames = (from tuple in frames select new GifFrame(tuple.Item1, tuple.Item2)).ToList();
            Length = (from tuple in frames select tuple.Item1).Sum();
        }

        public static GifImage Load(string gifInfo)
        {
            return null;
        }

        public Sprite GetFrameAt(long millisecond) =>
            (frames.Find(f =>
            {
                if (f == null || millisecond <= f.Length)
                    return true;
                millisecond -= f.Length;
                return false;
            }) ?? frames.Last()).Sprite;

        public class GifFrame
        {
            public int Length { get; private set; }
            public Sprite Sprite { get; private set; }

            public GifFrame(int length, Sprite sprite)
            {
                Length = length;
                Sprite = sprite;
            }
        }
    }
}
