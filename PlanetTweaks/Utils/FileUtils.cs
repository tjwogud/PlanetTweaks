using System.IO;
using System.Linq;

namespace PlanetTweaks.Utils
{
    public static class FileUtils
    {
        public static void DeepDelete(this DirectoryInfo dir)
        {
            dir.GetFiles().ToList().ForEach(file => file.Delete());
            dir.GetDirectories().ToList().ForEach(DeepDelete);
            dir.Delete();
        }

        public static void DeepDelete(this string dir)
        {
            if (!Directory.Exists(dir))
                return;
            Directory.GetFiles(dir).ToList().ForEach(File.Delete);
            Directory.GetDirectories(dir).ToList().ForEach(DeepDelete);
            Directory.Delete(dir);
        }

        public static DirectoryInfo CreateIfNotExists(this string dirName) => Directory.Exists(dirName) ? new DirectoryInfo(dirName) : Directory.CreateDirectory(dirName);
    }
}
