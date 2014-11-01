using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataProcessorLib
{
    class DataFileEventArgs : EventArgs
    {
        public string FileName { get; private set; }
        public string Description { get; private set; }
        public string ExceptionMessage { get; private set; }

        public DataFileEventArgs(string fileName, string description, string exceptionMessage)
        {
            this.FileName = fileName;
            this.Description = description;
            this.ExceptionMessage = exceptionMessage;
        }
    }
}
