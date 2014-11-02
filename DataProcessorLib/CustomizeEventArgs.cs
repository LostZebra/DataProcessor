using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataProcessorLib
{
    public class DataFileEventArgs : EventArgs
    {
        public string FileName { get; private set; }
        public string Message { get; private set; }

        public DataFileEventArgs(string fileName, string message)
        {
            FileName = fileName;
            Message = message;
        }
    }
}
