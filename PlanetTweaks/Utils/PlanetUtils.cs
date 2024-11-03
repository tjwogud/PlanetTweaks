namespace PlanetTweaks.Utils
{
    public static class PlanetUtils
    {
        public static scrPlanet GetThirdPlanet()
        {
            if (!scrController.instance)
                return null;
            foreach (scrPlanet planet in scrController.instance.planetList)
                if (planet != scrController.instance.redPlanet
                    && planet != scrController.instance.bluePlanet)
                    return planet;
            return scrController.instance.allPlanets[2];
        }
    }
}
