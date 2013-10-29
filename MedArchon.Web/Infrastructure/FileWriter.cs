using System.IO;

namespace MedArchon.Web.Infrastructure
{
    public class FileWriter : IFileWriter
    {
        public void WriteFile(Stream fileToSave, string path)
        {
            var dir = Path.GetDirectoryName(path);
            if (!Directory.Exists(dir))
                Directory.CreateDirectory(dir);

            using (var fileStream = new FileStream(path, FileMode.Create))
            {
                fileToSave.CopyTo(fileStream);
            }
        }
    }
}