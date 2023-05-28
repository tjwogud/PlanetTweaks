using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using UnityEngine;

namespace PlanetTweaks.Utils
{
    public static class GifUtils
    {
        public static List<(int, Sprite)> GetFrames(this Image image)
        {
            FrameDimension fd = new FrameDimension(image.FrameDimensionsList[0]);
            int frameCount = image.GetFrameCount(fd);
            List<(int, Sprite)> frames = new List<(int, Sprite)>();
            if (frameCount > 1)
            {
                byte[] times = image.GetPropertyItem(0x5100).Value;
                for (int i = 0; i < frameCount; i++)
                {
                    image.SelectActiveFrame(fd, i);
                    int length = BitConverter.ToInt32(times, 4 * i) * 10;
                    Sprite spr = ToSprite(new Bitmap(image));
                    spr.name = "frame_" + i;
                    frames.Add((length, spr));
                }
            }
            else
                frames.Add((1000, ToSprite(new Bitmap(image))));
            return frames;
        }

        private static Sprite ToSprite(Image image)
        {
            MemoryStream stream = new MemoryStream();
            image.Save(stream, ImageFormat.Png);
            return stream.GetBuffer().ToSprite();
        }
    }
}
