using System;
using System.IO;
using System.Collections.Generic;
using System.Text;

namespace DataProcessorLib
{
    public abstract class DataFile : IComparable<DataFile>, IEquatable<DataFile>
    {
        private FileInfo _dataFileInfo;

        public long NumOfBytes { get; private set; }

        public string ParentDirectory { get; private set; }

        public string FullPath { get; private set; }

        public string Name { get; private set; }

        public bool Existed { get; private set; }

        private EventHandler _fileCreateCompletionHandler;

        public event EventHandler FileCreateCompletionHandler
        {
            add { _fileCreateCompletionHandler += value; }
            remove { _fileCreateCompletionHandler -= value; }
        }

        internal DataFile()
        {
            // Do nothing
        }

        protected DataFile(string fullPath, DataFormat format)
        {
            switch (format)
            {
                case DataFormat.Csv:
                {
                    if (!fullPath.EndsWith(".csv"))
                    {
                        throw new InvalidOperationException("文件格式错误，所指定的文件不是csv文件");
                    }
                    break;
                }
                case DataFormat.Excel97To03:
                {
                    if (!fullPath.EndsWith(".xls"))
                    {
                        throw new InvalidOperationException("文件格式错误，所指定的文件不是Excel97-03文件");
                    }
                    break;
                }
                case DataFormat.Excel07Later:
                {
                    if (fullPath.EndsWith(".xlsx"))
                    {
                        throw new InvalidOperationException("文件格式错误，所指定的文件不是Excel07文件");
                    }
                    break;
                }
                default:
                {
                    throw new InvalidOperationException("未识别的文件格式");
                }
            }

            _dataFileInfo = new FileInfo(fullPath);
            // File information extraction
            ParentDirectory = _dataFileInfo.DirectoryName;
            FullPath = _dataFileInfo.FullName;
            Name = _dataFileInfo.Name;
            Existed = _dataFileInfo.Exists;
            NumOfBytes = _dataFileInfo.Exists ? _dataFileInfo.Length : 0;
        }

        public void Create()
        {
            if (Existed)
            {
                throw new InvalidOperationException("已经存在同名文件");
            }
            _dataFileInfo.Create();
            // File information extraction
            UpdateFileInfo();
            // Completion
            if (_fileCreateCompletionHandler != null)
            {
                _fileCreateCompletionHandler(this, new DataFileEventArgs(Name, "has been reated!"));
            }
        }

        public virtual void Delete()
        {
            if (!Existed)
            {
                throw new InvalidOperationException("文件不存在，无法删除");
            }
            _dataFileInfo.Delete();
        }

        public virtual void MoveTo(string directory)
        {
            var directoryInfo = new DirectoryInfo(directory);
            if (!directoryInfo.Exists || !Existed)
            {
                throw new InvalidOperationException("源文件或目标文件不存在，无法完成移动");
            }
            _dataFileInfo.MoveTo(directory + "\\" + _dataFileInfo.Name);
        }

        public virtual StreamReader Open()
        {
            if (!Existed)
            {
                throw new InvalidOperationException("文件不存在，无法打开"); 
            }
            return new StreamReader(FullPath, Encoding.Default);
        }

        int IComparable<DataFile>.CompareTo(DataFile otherFile)
        {
            return CompareUtilities.ReferenceTypeComparer(this, otherFile) ??
                   CompareUtilities.ValueTypeComparer(Name, otherFile.Name) ??
                   CompareUtilities.ValueTypeComparer(NumOfBytes, otherFile.NumOfBytes) ??
                   0;
        }

        bool IEquatable<DataFile>.Equals(DataFile otherFile)
        {
            return Name.Equals(otherFile.Name) && NumOfBytes == otherFile.NumOfBytes;
        }

        protected string ConvertToFormatedFileSize(long fileLength)
        {
            return string.Format("{0:0.00}KB", fileLength/1024);
        }

        private void UpdateFileInfo()
        {
            _dataFileInfo = new FileInfo(_dataFileInfo.FullName);
            Existed = _dataFileInfo.Exists;
            NumOfBytes = Existed ? _dataFileInfo.Length : 0;
        }

        public abstract List<string> FetchAllColumnHeaders(Func<StreamReader> dataFileProvider);
        public abstract IEnumerable<string> DataLines(Func<StreamReader> dataFileProvider);
        public abstract List<string> DataLinesForHeader(Func<StreamReader> dataFileProvider, string header);
    }
}
