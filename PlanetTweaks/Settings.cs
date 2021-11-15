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

        public int redSelected = -1;
        public int blueSelected = -1;
        public string spriteDirectory = Path.Combine(".", "Mods", "PlanetTweaks", "sprites");
    }
}
