using System;
using System.Collections.Generic;
using System.IO;

namespace DataProcessorLib
{
    public class ExcelFile07Later : DataFile
    {
        public string FormatedSize { get; private set; }

        public ExcelFile07Later(string fullPath) : base(fullPath, DataFormat.Excel07Later)
        {
            if (Existed)
            {
                FormatedSize = ConvertToFormatedFileSize(NumOfBytes);
            }
        }

        new protected string ConvertToFormatedFileSize(long numberOfBytes)
        {
            return (numberOfBytes <= Math.Pow(1024, 2))
                ? string.Format("{0:0.00}KB", numberOfBytes / 1024)
                : string.Format("{0:0.00}MB", numberOfBytes / Math.Pow(1024, 2));
        }

        public override List<string> FetchAllColumnHeaders(Func<StreamReader> dataFileProvider)
        {
            throw new NotImplementedException();
        }

        public override IEnumerable<string> DataLines(Func<StreamReader> dataFileProvider)
        {
            throw new NotImplementedException();
        }

        public override List<string> DataLinesForHeader(Func<StreamReader> dataFileProvider, string header)
        {
            throw new NotImplementedException();
        }
    }
}
