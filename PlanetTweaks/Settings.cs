using System.IO;
using System.Xml.Serialization;
using UnityModManagerNet;

namespace PlanetTweaks
{
    public class Settings : UnityModManager.ModSettings
    {
        public override void Save(UnityModManager.ModEntry modEntry)
        {
            var filepath = Path.Combine(modEntry.Path, "Settings.xml");
            using (var writer = new StreamWriter(filepath))
                new XmlSerializer(GetType()).Serialize(writer, this);
        }

        public string spriteDirectory = Path.Combine(".", "Mods", "PlanetTweaks", "sprites");

        public string redSelected = null;
        public string blueSelected = null;

        public float redSize = 1;
        public float blueSize = 1;

        public bool redColor = false;
        public bool blueColor = false;

        public bool shapedRotation = false;
        public int shapedAngle = 4;

        public string thirdSelected = null;
        public float thirdSize = 1;
        public bool thirdColor = false;
        public bool thirdPlanet = false;
        public int thirdColorType;
        public int thirdColorRed = 76;
        public int thirdColorGreen = 178;
        public int thirdColorBlue = 0;
    }
}
