using Moq;
using SumFiles;
using System;
using Xunit;

namespace SumFilesTest
{
    public class FileReaderTest
    {
        private const string FilePath = "c:/work/file.txt";
        private readonly Mock<IFileIo> _fileIoMock;
        private readonly FileReader _fileReader;

        public FileReaderTest()
        {
            _fileIoMock = new Mock<IFileIo>();

            _fileReader = new FileReader(_fileIoMock.Object);
        }

        [Fact]
        public void GetFileValues_GivenANonexistentFilePath_ShouldReturnProperException()
        {
            _fileIoMock.Setup(fio => fio.Exists(It.IsAny<string>())).Returns(false);

            var exception = Assert.Throws<Exception>(() => _fileReader.GetFileValues(FilePath));

            Assert.Equal(string.Format(FileReader.NonexistentFileMessageError, FilePath), exception.Message);
        }

        [Fact]
        public void GetFileValues_GivenAnEmptyFile_ShouldReturnProperException()
        {
            _fileIoMock.Setup(fio => fio.Exists(It.IsAny<string>())).Returns(true);
            _fileIoMock.Setup(fio => fio.ReadAllLines(It.IsAny<string>())).Returns(Array.Empty<string>());

            var exception = Assert.Throws<Exception>(() => _fileReader.GetFileValues(FilePath));

            Assert.Equal(string.Format(FileReader.EmptyFileMessageError, FilePath), exception.Message);
        }

        [Fact]
        public void GetFileValues_GivenAFileWithMoreLinesThanExpected_ShouldReturnProperException()
        {
            var fileContent = new[] { "12 anotherFile.txt", "25 anotherFileButThisTimeForReal.txt" };

            _fileIoMock.Setup(fio => fio.Exists(It.IsAny<string>())).Returns(true);
            _fileIoMock.Setup(fio => fio.ReadAllLines(It.IsAny<string>())).Returns(fileContent);

            var exception = Assert.Throws<Exception>(() => _fileReader.GetFileValues(FilePath));

            Assert.Equal(string.Format(FileReader.FileMoreLinesThanExpectedError, FilePath, fileContent.Length), exception.Message);
        }

        [Fact]
        public void GetFileValues_GivenAFileFullOfWhitespaces_ShouldReturnProperException()
        {
            _fileIoMock.Setup(fio => fio.Exists(It.IsAny<string>())).Returns(true);
            _fileIoMock.Setup(fio => fio.ReadAllLines(It.IsAny<string>())).Returns(new[] { "    " });

            var exception = Assert.Throws<Exception>(() => _fileReader.GetFileValues(FilePath));

            Assert.Equal(string.Format(FileReader.EmptyFileMessageError, FilePath), exception.Message);
        }

        [Fact]
        public void GetFileValues_GivenAValidFile_ShouldReturnValuesAsExpected()
        {
            var fileContent = new[] { "3 19 B.txt 50" };

            _fileIoMock.Setup(fio => fio.Exists(It.IsAny<string>())).Returns(true);
            _fileIoMock.Setup(fio => fio.ReadAllLines(It.IsAny<string>())).Returns(fileContent);

            var result = _fileReader.GetFileValues(FilePath);

            Assert.NotEmpty(result);
            Assert.Equal(4, result.Length);
            Assert.Equal("3", result[0]);
            Assert.Equal("19", result[1]);
            Assert.Equal("B.txt", result[2]);
            Assert.Equal("50", result[3]);
        }
    }
}
