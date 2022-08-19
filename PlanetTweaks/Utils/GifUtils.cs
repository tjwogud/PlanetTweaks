using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using UnityEngine;

namespace PlanetTweaks.Utils
{
    public static class GifUtils
    {
        public static List<Tuple<int, Sprite>> GetFrames(this Image image)
        {
            var fd = new FrameDimension(image.FrameDimensionsList[0]);
            int frameCount = image.GetFrameCount(fd);
            var frames = new List<Tuple<int, Sprite>>();
            if (frameCount > 1)
            {
                byte[] times = image.GetPropertyItem(0x5100).Value;
                for (int i = 0; i < frameCount; i++)
                {
                    image.SelectActiveFrame(fd, i);
                    int length = BitConverter.ToInt32(times, 4 * i) * 10;
                    frames.Add(new Tuple<int, Sprite>(length, new Bitmap(image).ToSprite()));
                }
            }
            else
                frames.Add(new Tuple<int, Sprite>(1000, new Bitmap(image).ToSprite()));
            return frames;
        }
    }
}
