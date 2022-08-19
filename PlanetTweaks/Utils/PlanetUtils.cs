using UnityEngine;

namespace PlanetTweaks.Utils
{
    public static class PlanetUtils
    {
        private static Color gold = new Color(1f, 0.8078431f, 0.1607843f);
        private static Color transPink = new Color(0.9568627f, 0.6431373f, 0.7098039f);
        private static Color transBlue = new Color(0.3607843f, 0.7882353f, 0.9294118f);
        private static Color nbYellow = new Color(0.996f, 0.953f, 0.18f);
        private static Color nbPurple = new Color(0.612f, 0.345f, 0.82f);
        private static Color overseer = new Color(0.1058824f, 0.6470588f, 0.7843137f);

        public static Color GetColor(bool red)
        {
            return scrMisc.PlayerColorToRealColor(Persistence.GetPlayerColor(red));
        }

        public static void SwitchToSpark(this scrPlanet planet, Color color, Color minColor1, Color minColor2, Color maxColor1, Color maxColor2)
        {
            planet.ToggleSpecialPlanet(planet.transform.Find("SparkPlanet").gameObject, true, "Spark");
            planet.tailParticles.ApplyColor(color, color);
            planet.sparks.ApplySparkColor(minColor1, minColor2, maxColor1, maxColor2);
            planet.ring.color = color;
            planet.glow.color = color.WithAlpha(planet.glow.color.a);
            planet.sprite.color = color;
            planet.faceSprite.color = color;
        }

        public static void RemoveSpark(this scrPlanet planet)
        {
            planet.ToggleSpecialPlanet(planet.transform.Find("SparkPlanet").gameObject, false);
        }
    }
}
