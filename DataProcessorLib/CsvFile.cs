using System;
using System.IO;

namespace DataProcessorLib
{
    public class CsvFile : DataFile
    {
        public string FormatedSize { get; private set; }

        public CsvFile(string fullPath) : base(fullPath, DataFormat.Csv)
        {
            FormatedSize = ConvertToFormatedFileSize(NumOfBytes);
        }

        new protected string ConvertToFormatedFileSize(long numberOfBytes)
        {
            return (numberOfBytes <= Math.Pow(1024, 2))
                 ? string.Format("{0:0.00}KB", numberOfBytes / 1024)
                 : string.Format("{0:0.00}MB", numberOfBytes / Math.Pow(1024, 2));
        }
    }
}
