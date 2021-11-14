using System.IO;

namespace SumFiles
{
    internal class FileIo: IFileIo
    {
        public bool Exists(string filePath)
        {
            return File.Exists(filePath);
        }

        public string[] ReadAllLines(string filePath)
        {
            return File.ReadAllLines(filePath);
        }
    }
}
