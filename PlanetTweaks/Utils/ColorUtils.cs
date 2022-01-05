using UnityEngine;

namespace PlanetTweaks.Utils
{
    public static class ColorUtils
    {
        public static Color GetColor(bool red)
        {
            Color playerColor = Persistence.GetPlayerColor(red);
            if (playerColor == scrPlanet.goldColor || GCS.d_forceGoldPlanets)
                playerColor = new Color(1f, 0.8078431f, 0.1607843f);
            else if (playerColor == scrPlanet.rainbowColor)
                playerColor = Color.white;
            else if (playerColor == scrPlanet.transBlueColor)
                playerColor = new Color(0.3607843f, 0.7882353f, 0.9294118f);
            else if (playerColor == scrPlanet.transPinkColor)
                playerColor = new Color(0.9568627f, 0.6431373f, 0.7098039f);
            else if (playerColor == scrPlanet.nbYellowColor)
                playerColor = new Color(0.996f, 0.953f, 0.18f);
            else if (playerColor == scrPlanet.nbPurpleColor)
                playerColor = new Color(0.612f, 0.345f, 0.82f);
            return playerColor;
        }
    }
}
