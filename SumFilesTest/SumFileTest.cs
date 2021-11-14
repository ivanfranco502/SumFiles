using Moq;
using SumFiles;
using System;
using System.Globalization;
using Xunit;

namespace SumFilesTest
{
    public class SumFileTest
    {
        private const string FileAPath = "c:/work/A.txt";
        private readonly Mock<IFileReader> _fileReader;
        private readonly SumFile _sumFile;

        public SumFileTest()
        {
            _fileReader = new Mock<IFileReader>();
            _sumFile = new SumFile(_fileReader.Object);
        }

        [Fact]
        public void GetSumFile_GivenAFileWithOnlyNumbers_ShouldReturnExpectedResult()
        {
            _fileReader.Setup(fr => fr.GetFileValues(It.IsAny<string>())).Returns(new[] {"2", "4", "6", "8"});

            var result = _sumFile.GetSumForFile(FileAPath);

            Assert.Single(result);
            Assert.Contains(FileAPath, result);
            Assert.Equal(20.ToString(CultureInfo.InvariantCulture), result[FileAPath]);
        }

        [Fact]
        public void GetSumFile_GivenAFileWithSubFiles_ShouldReturnExpectedResult()
        {
            var fileBPath = "c:/work/B.txt";
            var fileCPath = "c:/work/C.txt";

            _fileReader.Setup(fr =>
                    fr.GetFileValues(It.Is<string>(val => val.Equals(FileAPath, StringComparison.InvariantCulture))))
                .Returns(new[] {"3", "19", fileBPath, "50"});
            _fileReader.Setup(fr =>
                    fr.GetFileValues(It.Is<string>(val => val.Equals(fileBPath, StringComparison.InvariantCulture))))
                .Returns(new[] {fileCPath, "27"});
            _fileReader.Setup(fr =>
                    fr.GetFileValues(It.Is<string>(val => val.Equals(fileCPath, StringComparison.InvariantCulture))))
                .Returns(new[] {"10", "2"});

            var result = _sumFile.GetSumForFile(FileAPath);

            Assert.NotEmpty(result);
            Assert.Equal(3, result.Count);
            Assert.Contains(FileAPath, result);
            Assert.Equal(111.ToString(CultureInfo.InvariantCulture), result[FileAPath]);
            Assert.Contains(fileBPath, result);
            Assert.Equal(39.ToString(CultureInfo.InvariantCulture), result[fileBPath]);
            Assert.Contains(fileCPath, result);
            Assert.Equal(12.ToString(CultureInfo.InvariantCulture), result[fileCPath]);
        }

        [Fact]
        public void GetSumFile_GivenAFileWithInvalidSubFiles_ShouldSkipInvalidSubFiles()
        {
            var subFilePath = "C:/work/anotherSubFile.txt";
            _fileReader.Setup(fr =>
                    fr.GetFileValues(It.Is<string>(val => val.Equals(FileAPath, StringComparison.InvariantCulture))))
                .Returns(new[] {"2", "4", "6", "8", subFilePath});
            var exceptionMessage = "Exception Message";
            _fileReader.Setup(fr =>
                    fr.GetFileValues(It.Is<string>(val => val.Equals(subFilePath, StringComparison.InvariantCulture))))
                .Throws(new Exception(exceptionMessage));

            var result = _sumFile.GetSumForFile(FileAPath);

            Assert.NotEmpty(result);
            Assert.Equal(2, result.Count);
            Assert.Contains(FileAPath, result);
            Assert.Equal(20.ToString(CultureInfo.InvariantCulture), result[FileAPath]);
            Assert.Contains(subFilePath, result);
            Assert.Equal(exceptionMessage, result[subFilePath]);
        }

        [Fact]
        public void GetSumFile_GivenAnInvalidFile_ShouldReturnErrorMessage()
        {
            var exceptionMessage = "Exception Message";
            _fileReader.Setup(fr =>
                    fr.GetFileValues(It.Is<string>(val => val.Equals(FileAPath, StringComparison.InvariantCulture))))
                .Throws(new Exception(exceptionMessage));

            var result = _sumFile.GetSumForFile(FileAPath);

            Assert.NotEmpty(result);
            Assert.Single(result);
            Assert.Contains(FileAPath, result);
            Assert.Equal(exceptionMessage, result[FileAPath]);
        }
    }
}