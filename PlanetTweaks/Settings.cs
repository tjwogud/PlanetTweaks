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

        public string redSelected = null;
        public string blueSelected = null;
        public string spriteDirectory = Path.Combine(".", "Mods", "PlanetTweaks", "sprites");
    }
}
