using System;
using System.Collections.Generic;

namespace SumFiles
{
    public class SumFile
    {
        private readonly IFileReader _fileReader;
        private Dictionary<string, string> _sumByFile;

        public SumFile(IFileReader fileReader)
        {
            _fileReader = fileReader;
            _sumByFile = new Dictionary<string, string>();

        }

        public IDictionary<string, string> GetSumForFile(string filePath)
        {
            GetSumForFileRecursively(filePath);

            return _sumByFile;
        }

        public void GetSumForFileRecursively(string filePath)
        {
            long GetFileSumFromDictionary(string value)
            {
                if (_sumByFile.TryGetValue(value, out var subFileValue))
                {
                    if (long.TryParse(subFileValue, out var subFileSum))
                    {
                        return subFileSum;
                    }
                }

                return 0;
            }

            try
            {
                var fileValues = _fileReader.GetFileValues(filePath);

                long sum = 0;

                foreach (var value in fileValues)
                {
                    if (long.TryParse(value, out var valResult))
                    {
                        sum += valResult;
                    }
                    else
                    {
                        // We don't want to open a file that was already added to the dictionary
                        if (_sumByFile.TryGetValue(value ?? string.Empty, out var subFileValue))
                        {
                            sum += GetFileSumFromDictionary(subFileValue);
                        }
                        else
                        {
                            GetSumForFileRecursively(value);

                            sum += GetFileSumFromDictionary(value);
                        }
                    }
                }
                _sumByFile.TryAdd(filePath, sum.ToString());
            }
            catch (Exception e)
            {
                _sumByFile.TryAdd(filePath, e.Message);
            }
        }
    }
}