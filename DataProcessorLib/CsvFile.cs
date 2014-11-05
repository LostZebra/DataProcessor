using System;
using System.IO;
using System.Collections.Generic;

namespace DataProcessorLib
{
    public class CsvFile : DataFile
    {
        private List<string> _columnHeaders;

        public string FormatedSize { get; private set; }

        public List<string> ColumnHeaders
        {
            get { return _columnHeaders ?? (_columnHeaders = FetchAllColumnHeaders(() => File.OpenText(FullPath))); }
        }

        public CsvFile(string fullPath) : base(fullPath, DataFormat.Csv)
        {
            FormatedSize = ConvertToFormatedFileSize(NumOfBytes);
        }

        protected new string ConvertToFormatedFileSize(long numberOfBytes)
        {
            return (numberOfBytes <= Math.Pow(1024, 2))
                ? string.Format("{0:0.00}KB", numberOfBytes/1024)
                : string.Format("{0:0.00}MB", numberOfBytes/Math.Pow(1024, 2));
        }

        public override List<string> FetchAllColumnHeaders(Func<StreamReader> dataFileProvider)
        {
            List<string> headersList = null;
            using (var dataFileReader = dataFileProvider())
            {
                string headerLine;
                if ((headerLine = dataFileReader.ReadLine()) != null)
                {
                    var headers = headerLine.Split(',');
                    headersList = new List<string>(headers.Length);
                    headersList.AddRange(headers);
                }
            }
            return headersList;
        }

        public override IEnumerable<string> DataLines(Func<StreamReader> dataFileProvider)
        {
            int line = 0;
            using (var dataFileReader = dataFileProvider())
            {
                string dataLine;
                while((dataLine = dataFileReader.ReadLine()) != null)
                {
                    if (line != 0)
                    {
                        yield return dataLine;
                    }
                    ++line;
                }
            }
        }

        public override List<string> DataLinesForHeader(Func<StreamReader> dataFileProvider, string header)
        {
            if (header == null)
            {
                throw new ArgumentNullException("数据项不能为空");
            }

            int indexForHeader = ColumnHeaders.IndexOf(header);
            if (indexForHeader < 0)
            {
                throw new InvalidDataException("无此数据项");
            }

            int line = 0;
            int listBaseCapacity = 1000;
            List<string> dataLinesListForHeader = new List<string>(listBaseCapacity);
            using (var dataFileReader = dataFileProvider())
            {
                string dataLine;
                while ((dataLine = dataFileReader.ReadLine()) != null)
                {
                    if (line > 0)
                    {
                        if (dataLinesListForHeader.Count == listBaseCapacity)
                        {
                            var tempDataLinesForHeader = new List<string>(listBaseCapacity);
                            tempDataLinesForHeader.AddRange(dataLinesListForHeader);
                            listBaseCapacity *= 2;
                            dataLinesListForHeader = new List<string>(listBaseCapacity);
                            dataLinesListForHeader.AddRange(tempDataLinesForHeader);
                        }
                        dataLinesListForHeader.Add(dataLine.Split(',')[indexForHeader]);
                    }
                    ++line;
                }
            }
            return dataLinesListForHeader;
        }
    }
}
