using UnityEngine;

namespace PlanetTweaks.Utils
{
    public static class ColorUtils
    {
        public static Color GetColor(bool red)
        {
            return scrMisc.PlayerColorToRealColor(Persistence.GetPlayerColor(red));
        }
    }
}
