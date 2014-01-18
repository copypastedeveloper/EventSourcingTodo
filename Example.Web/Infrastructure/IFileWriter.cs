using System.IO;

namespace Example.Web.Infrastructure
{
    public interface IFileWriter
    {
        void WriteFile(Stream fileToSave, string path);
    }
}