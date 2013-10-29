using System.IO;

namespace MedArchon.Web.Infrastructure
{
    public interface IFileWriter
    {
        void WriteFile(Stream fileToSave, string path);
    }
}