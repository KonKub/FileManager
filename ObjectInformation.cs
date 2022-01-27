using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileManager
{
    public class ObjectInformation : DrawTools
    {

        private bool _infoIsOK = false;
        public string _error = "";
        private string _path;
        private bool _isDir;
        private string _creationTime, _accessTime, _attributes, _fileSize;

        //показать информацию о файле или каталоге
        public bool Prepare(string Path)
        {
            _error = "";
            this._path = Path;
            _infoIsOK = false;
            if (Path.Length == 0) return false;

            try
            {
                FileAttributes FA = File.GetAttributes(Path);

                _isDir = ((FA & FileAttributes.Directory) == FileAttributes.Directory);
                _attributes = (((FA & FileAttributes.ReadOnly) == FileAttributes.ReadOnly) ? "R" : "-") +
                          (((FA & FileAttributes.Hidden) == FileAttributes.Hidden) ? "H" : "-") +
                          (((FA & FileAttributes.Archive) == FileAttributes.Archive) ? "A" : "-") +
                          (((FA & FileAttributes.System) == FileAttributes.System) ? "S" : "-") +
                          (_isDir ? "D" : "-");

                _creationTime = File.GetCreationTime(Path).ToString();
                _accessTime = File.GetLastAccessTime(Path).ToString();
                if (!_isDir)
                {
                    _fileSize = new System.IO.FileInfo(Path).Length.ToString();
                }
                _infoIsOK = true;
                return true;
            }
            catch (Exception e)
            {
                _error=e.Message;
                return false;
            }
        }

        public void Show()
        {
            if (_infoIsOK)
            {
                WriteStr(3, Console.WindowHeight - 4, $"Creation time: {_creationTime}");
                WriteStr(3, Console.WindowHeight - 3, $"Access time:   {_accessTime}");
                WriteStr(45, Console.WindowHeight - 4, $"Attributes: {_attributes}");
                if (_isDir)
                {
                    WriteStr(3, Console.WindowHeight - 5, $"Dir: {_path}");
                }
                else
                {
                    WriteStr(3, Console.WindowHeight - 5, $"File: {_path}");
                    WriteStr(45, Console.WindowHeight - 3, $"File size: {_fileSize} b");
                }
            }
        }
    }
}
