using UnityEngine;

namespace PlanetTweaks.Utils
{
    public static class ColorUtils
    {
        public static Color GetRealColor(bool red)
        {
            return scrMisc.PlayerColorToRealColor(Persistence.GetPlayerColor(red));
        }

        public static Color GetRealThirdColor()
        {
            return scrMisc.PlayerColorToRealColor(GetThirdColor());
        }

        public static Color GetRealDefaultThirdColor()
        {
            return scrMisc.PlayerColorToRealColor(GetDefaultThirdColor());
        }

        public static Color GetThirdColor()
        {
            switch (Main.Settings.thirdColorType)
            {
                case 0:
                    return GetDefaultThirdColor();
                case 1:
                case 2:
                    return Persistence.GetPlayerColor(Main.Settings.thirdColorType == 1);
                case 3:
                    return GetCustomThirdColor();
                default:
                    return new Color(0.3f, 0.7f, 1);
            }
        }

        public static void SetThirdColor(Color? c = null)
        {
            if (scrController.instance.levelName == "T5-X" && GCS.enableCutsceneT5)
                return;
            scrPlanet planet = PlanetUtils.GetThirdPlanet();
            if (!planet)
                return;
            if (c == null)
                c = GetThirdColor();
            if (c == scrPlanet.goldColor)
            {
                planet.SwitchToGold();
            }
            else if (c == scrPlanet.rainbowColor)
            {
                planet.DisableAllSpecialPlanets();
                planet.EnableCustomColor();
                planet.SetRainbow(true);
            }
            else if (c == scrPlanet.overseerColor)
            {
                planet.SwitchToOverseer();
            }
            else
            {
                Color realColor = scrMisc.PlayerColorToRealColor(c.Value);
                Color tailColor = realColor;
                if (realColor == scrPlanet.transBlueColor || realColor == scrPlanet.transPinkColor || realColor == scrPlanet.nbYellowColor)
                {
                    tailColor = Color.white;
                }
                else if (realColor == scrPlanet.nbPurpleColor)
                {
                    tailColor = Color.black;
                }
                planet.EnableCustomColor();
                planet.SetRainbow(false);
                planet.SetPlanetColor(realColor);
                planet.SetTailColor(tailColor);
            }
        }

        public static Color GetDefaultThirdColor()
        {
            Color redOrigin = Persistence.GetPlayerColor(true);
            Color blueOrigin = Persistence.GetPlayerColor(false);
            Color red = scrMisc.PlayerColorToRealColor(redOrigin);
            Color blue = scrMisc.PlayerColorToRealColor(blueOrigin);
            if (red == Color.red && blue == Color.blue)
            {
                return new Color(0.3f, 0.7f, 0);
            }
            if (red == blue)
            {
                return redOrigin;
            }
            if ((redOrigin == scrPlanet.transPinkColor && blueOrigin == scrPlanet.transBlueColor)
                || (blueOrigin == scrPlanet.transPinkColor && redOrigin == scrPlanet.transBlueColor)
                || (redOrigin == scrPlanet.nbYellowColor && blueOrigin == scrPlanet.nbPurpleColor)
                || (blueOrigin == scrPlanet.nbYellowColor && redOrigin == scrPlanet.nbPurpleColor))
            {
                return Color.white;
            }
            Color.RGBToHSV(red, out float redH, out float redS, out float redV);
            Color.RGBToHSV(blue, out float blueH, out float blueS, out float blueV);
            float n1 = (1 - Mathf.Abs(blueH - redH) > Mathf.Abs(blueH - redH)) ? Mathf.Max(redH, blueH) : Mathf.Min(redH, blueH);
            float n2 = (1 - Mathf.Abs(blueH - redH) > Mathf.Abs(blueH - redH)) ? Mathf.Min(redH, blueH) : Mathf.Max(redH, blueH);
            float n3 = (n1 == redH) ? redS : blueS;
            float n4 = (n1 == redH) ? redV : blueV;
            float n5 = (n1 == redH) ? blueS : redS;
            float n6 = (n1 == redH) ? blueV : redV;
            if (n2 < n1)
            {
                n2 += 1;
            }
            float h = (n1 + (n2 - n1) / 2) % 1;
            float s = n3 + (n5 - n3) / 2;
            float v = n4 + (n6 - n4) / 2;
            return Color.HSVToRGB(h, s, v);
        }

        public static Color GetCustomThirdColor()
        {
            return new Color((float)Main.Settings.thirdColorRed / 255, (float)Main.Settings.thirdColorGreen / 255, (float)Main.Settings.thirdColorBlue / 255);
        }

        public static string Hex(this Color c, bool useAlpha = false, bool hash = true)
        {
            return $"{(hash ? "#" : "")}{ToByte(c.r):X2}{ToByte(c.g):X2}{ToByte(c.b):X2}{(useAlpha ? $"{ToByte(c.a)}:X2" : "")}";
        }

        private static byte ToByte(float f)
        {
            return (byte)(Mathf.Clamp01(f) * 255f);
        }
    }
}
