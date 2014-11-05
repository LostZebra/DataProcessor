using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataProcessorLib
{
    public class ExcelFile97To03 : DataFile
    {
        public string FormatedSize { get; private set; }

        public ExcelFile97To03(string fullPath) : base(fullPath, DataFormat.Excel97To03)
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
