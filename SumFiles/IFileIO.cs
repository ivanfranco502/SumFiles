namespace SumFiles
{
    public interface IFileIo
    {
        bool Exists(string filePath);

        string[] ReadAllLines(string filePath);
    }
}