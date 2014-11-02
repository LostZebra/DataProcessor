using System;
using System.IO;

namespace DataProcessorLib
{
    public abstract class DataFile
    {
        private readonly FileInfo _dataFileInfo;

        public long NumOfBytes { get; private set; }

        public string ParentDirectory { get; private set; }

        public string FullPath { get; private set; }

        public string Name { get; private set; }

        public bool Existed { get; private set; }

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
            
            if ((Existed = _dataFileInfo.Exists) != true) return;
            
            // File information extraction
            RetrieveFileInfo();
        }

        public virtual void Create()
        {
            if (Existed)
            {
                throw new InvalidOperationException("已经存在同名文件");
            }
            _dataFileInfo.Create();
            // File information extraction
            RetrieveFileInfo();
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

        protected string ConvertToFormatedFileSize(long fileLength)
        {
            return string.Format("{0:0.00}KB", fileLength / 1024);
        }

        private void RetrieveFileInfo()
        {
            NumOfBytes = _dataFileInfo.Length;
            ParentDirectory = _dataFileInfo.DirectoryName;
            FullPath = _dataFileInfo.FullName;
            Name = _dataFileInfo.Name;
            Existed = _dataFileInfo.Exists;
        }
    }
}
