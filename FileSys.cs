using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace FileManager
{
    public class FileSys : DrawTools
    {
        private DriveInfo[] _drives = DriveInfo.GetDrives();
        private int _dirDepth = 0;                                          //глубина вывода дерева каталогов
        public const char _dirSign = '>';
        public string _error = "";                                          //текст ошибки
        public char[] _separators = new char[] { '"' };
        public string _currentDirectory = "";                               //текущий каталог для отображения дерева
        private string _mask = "*.*";                                       //маска для поиска файлов
        List<string> _treeList = new List<string>();                        //дерево каталогов
        public ObjectInformation _objInfo = new ObjectInformation();


        //получение списка дисков
        public DriveInfo GetDrive(int Ind)
        {
            if (Ind > _drives.Length - 1) 
                return null;
            else 
                return _drives[Ind];
        }

        //формирование строки из списка дисков
        public string GetDrivesString()
        {
            string res = " ";

            foreach (DriveInfo d in _drives)
                res = res + d.Name+" ";
            return res;
        }

        //определяет по этому пути файл или нет
        public bool IsFile(string path)
        {
            if (File.Exists(path))
                return true;
            else
                return false;
        }

        //определяет по этому пути каталог или нет
        public bool IsDir(string path)
        {
            if (Directory.Exists(path))
                return true;
            else
                return false;
        }

        //задать маску для фильтрации файлов
        public void SetMask(string Mask)
        {
            Mask = Mask.Trim();
            if (Mask.Length == 0) _mask = "*.*";
            else _mask = Mask;
        }

        //получение списка каталогов и файлов
        private List<string> GetDirsAndFiles(string path)
        {
            List<string> ResList = new List<string>();
            string S="";

            try
            {
                string[] Dirs = Directory.GetDirectories(path);
                if (Dirs != null)
                {
                    _dirDepth+=1;
                    if (_dirDepth <= 2)
                    {
                        foreach (string dir in Dirs)
                        {
                            DirectoryInfo DirInfo = new DirectoryInfo(dir);
                            ResList.Add(_dirSign+S.PadRight(_dirDepth * 2, ' ') + '\u2514' + ' ' + DirInfo.Name);
                            ResList.AddRange(GetDirsAndFiles(dir));
                        }
                        string[] Files = Directory.GetFiles(path, _mask);
                        foreach (string file in Files)
                        {
                            ResList.Add(S.PadRight(_dirDepth * 2 + 2, ' ') + Path.GetFileName(file));
                        }
                    }
                    if (_dirDepth > 0) _dirDepth--;
                }
            }
            catch (Exception e)
            {
                _error = e.Message;
            }
            return ResList;
        }

        //построение дерева каталогов
        public void RebuildTree()
        {
            _dirDepth = 0;
            _treeList = GetDirsAndFiles(_currentDirectory);

            PageCount = _treeList.Count / PageSize;
            if ((_treeList.Count % PageSize) > 0) PageCount++;
            if (PageCount > 0) PageCurrent = 1;
            else PageCurrent = 0;
        }

        //копирование каталога со всеми подкаталогами и файлами
        public void CopyDirectory(string DirFrom, string DirTo)
        {
            try
            {
                DirectoryInfo DirInfo = new DirectoryInfo(DirFrom);         //Берём нашу исходную папку
                if (!Directory.Exists(DirTo)) Directory.CreateDirectory(DirTo); //Проверяем - если директории не существует, то создаём
                foreach (DirectoryInfo dir in DirInfo.GetDirectories())     //Перебираем все внутренние папки
                {
                    if (Directory.Exists(DirTo + "\\" + dir.Name) != true)  //Проверяем - если директории не существует, то создаём
                        Directory.CreateDirectory(DirTo + "\\" + dir.Name);
                    CopyDirectory(dir.FullName, DirTo + "\\" + dir.Name);   //Рекурсия (перебираем вложенные папки и делаем для них то-же самое).
                }
                foreach (string file in Directory.GetFiles(DirFrom))        //Перебираем файлики в папке источнике.
                {
                    string f = file.Substring(file.LastIndexOf('\\'), file.Length - file.LastIndexOf('\\'));//Определяем (отделяем) имя файла с расширением - без пути (но с слешем "\").
                    File.Copy(file, DirTo + "\\" + f, true);                //Копируем файлик с перезаписью из источника в приёмник.
                }
            }
            catch (Exception e)
            {
                _error = e.Message;
            }
        }

        //отображение страницы дерева каталогов
        public void ShowTreePage()
        {
            if (PageCurrent > PageCount) PageCurrent = PageCount;
            WriteStrColorBack(1, 1, CutString($"({PageCurrent}/{PageCount}) " + _currentDirectory, Console.WindowWidth - 3), ConsoleColor.DarkBlue, ConsoleColor.Gray);
            if (PageCurrent > 0)
            {
                int YPos = 2;
                for (int i = (PageCurrent - 1) * PageSize; i < PageCurrent * PageSize; i++)
                {
                    if (i >= _treeList.Count) break;
                    if (_treeList[i][0] == FileSys._dirSign)
                        WriteStr(1, YPos,
                                    CutString(_treeList[i].Remove(0, 1), Console.WindowWidth - 3),
                                    ConsoleColor.White);
                    else
                        WriteStr(1, YPos,
                                    CutString(_treeList[i], Console.WindowWidth - 3),
                                    ConsoleColor.Yellow);
                    YPos++;
                }
            }
        }

        //Оьновить экран
        public void RefreshScreen()
        {
            DrawFrame();
            ShowTreePage();
            _objInfo.Show();
            WriteStrColorBack(3, Console.WindowHeight - 2, GetDrivesString(), ConsoleColor.DarkBlue, ConsoleColor.White);
            WriteStrColorBack(3, Console.WindowHeight - 6, CutString(_error, Console.WindowWidth - 3), ConsoleColor.DarkBlue, ConsoleColor.Red);
            if (_mask.Length>50) 
                WriteStrColorBack(Console.WindowWidth - 54, 0,$"({CutString(_mask,40)})" , ConsoleColor.DarkBlue, ConsoleColor.White);
            else
                WriteStrColorBack(Console.WindowWidth - _mask.Length-4, 0, $"({_mask})", ConsoleColor.DarkBlue, ConsoleColor.White);
            DrawInputLine();
            _error = "";
        }
    }
}
