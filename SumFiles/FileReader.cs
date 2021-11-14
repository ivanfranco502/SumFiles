using System;
using System.Collections.Generic;
using System.Linq;

namespace SumFiles
{
    public class FileReader: IFileReader
    {
        private readonly IFileIo _fileIo;
        public const string NonexistentFileMessageError = "The file {0} doesn't exist.";
        public const string EmptyFileMessageError = "The file {0} is empty.";
        public const string FileMoreLinesThanExpectedError = "The file {0} doesn't contain the expected format, expected one line but it has {1} lines.";

        public FileReader(IFileIo fileIo)
        {
            _fileIo = fileIo;
        }

        public string[] GetFileValues(string filePath)
        {
            var fileContent = ValidateFile(filePath);

            var fileValues = fileContent.First().Trim().Split(" ");

            if (fileValues.Length == 1 && string.IsNullOrWhiteSpace(fileValues.First()))
            {
                throw new Exception(string.Format(EmptyFileMessageError, filePath));
            }

            return fileValues;
        }

        private IEnumerable<string> ValidateFile(string filePath)
        {
            var fileExists = _fileIo.Exists(filePath);
            if (!fileExists)
            {
                throw new Exception(string.Format(NonexistentFileMessageError, filePath));
            }

            var fileContent = _fileIo.ReadAllLines(filePath);

            if (fileContent.Length == 0)
            {
                throw new Exception(string.Format(EmptyFileMessageError, filePath));
            }

            if (fileContent.Length > 1)
            {
                throw new Exception(string.Format(FileMoreLinesThanExpectedError, filePath, fileContent.Length));
            }

            return fileContent;
        }
    }
}