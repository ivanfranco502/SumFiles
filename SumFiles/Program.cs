using System;

namespace SumFiles
{
    class Program
    {
        static void Main()
        {
            Console.WriteLine("+=====================================================+");
            Console.WriteLine("|                       SUM FILE                      |");
            Console.WriteLine("+=====================================================+");
            Console.WriteLine("Please insert the file path: (default A.txt): ");
            
            var filePathInput = Console.ReadLine();
            var filePath = string.IsNullOrWhiteSpace(filePathInput) ? "A.txt" : filePathInput;

            var fileIo = new FileIo();
            var fileReader = new FileReader(fileIo);

            var sumFile = new SumFile(fileReader);
            var sumByFile = sumFile.GetSumForFile(filePath);

            foreach (var keyValueSum in sumByFile)
            {
                Console.WriteLine("{0}: sum of {1}", keyValueSum.Key, keyValueSum.Value);
            }

            Console.WriteLine("Enter to exit...");
            Console.ReadLine();
        }
    }
}
