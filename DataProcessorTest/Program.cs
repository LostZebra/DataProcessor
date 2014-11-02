using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using DataProcessorLib;

namespace DataProcessorTest
{
    class Program
    {
        static void Main(string[] args)
        {
            var csvFile = new CsvFile(@"F:\WirelessDialog.csv");
            csvFile.FileCreateCompletionHandler += (sender, e) =>
            {
                Console.WriteLine("{0} {1}", (e as DataFileEventArgs).FileName, (e as DataFileEventArgs).Message);
            };
            csvFile.Create();
            Console.ReadLine();
        }
    }
}
