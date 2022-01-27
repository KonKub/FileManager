using System;
using System.Collections.Generic;
using System.IO;

namespace FileManager
{
    class Program
    {
        static void Main(string[] args)
        {
            string Command = "";                           //Введенная команда целиком
            string Cmd = "";                               //Непосредственно команда без параметров

            Config config = new Config();
            FileSys FS = new FileSys();

            config.Read();                                 //чтение файла конфигурации

            try
            {
                FS._currentDirectory = config.SavedPath;
                Directory.SetCurrentDirectory(FS._currentDirectory);
                FS.RebuildTree();
            }
            catch (Exception e)
            {
                FS._error = e.Message;
                FS._currentDirectory = "";
            }

            while (true)
            {
                
                FS.RefreshScreen();                        //обновление экрана

                Command = Console.ReadLine();              //чтение введенной команды

                Command = Command.ToLower().Trim();

                if (Command.Length > 0)                    //что-то введено - надо обрабатывать
                {
                    string s;
                                                           //отделяем команду от возможных параметров
                    if (Command.IndexOf(' ') >= 0) 
                        Cmd = Command.Substring(0, Command.IndexOf(' '));
                    else 
                        Cmd = Command; 

                    switch(Cmd)                            //ищем команду
                    {
                        case "tree"://обновить дерево каталогов
                            FS.RebuildTree();
                            break;
                        case "cd"://смена каталога
                            s = Command.Substring(3);
                            if (Directory.Exists(s))
                            {
                                FS._currentDirectory = s;
                                Directory.SetCurrentDirectory(FS._currentDirectory);
                                FS.RebuildTree();
                            }
                            break;
                        case "md"://создание каталога
                            s = Command.Substring(3);
                            try
                            {
                                Directory.CreateDirectory(s);
                                FS.RebuildTree();
                            }
                            catch (Exception e)
                            {
                                FS._error = e.Message;
                            }
                            break;
                        case "del"://удаление файла или каталога
                            s = Command.Substring(4);
                            try
                            {
                                if (FS.IsDir(s))
                                    Directory.Delete(s, true);
                                else
                                    if (FS.IsFile(s)) File.Delete(s);
                                FS.RebuildTree();
                            }
                            catch (Exception e)
                            {
                                FS._error = e.Message;
                            }
                            break;
                        case "info"://иныормация о файле или каталоге
                            if (!FS._objInfo.Prepare(Command.Substring(4).Trim()))
                            {
                                FS._error = FS._objInfo._error;
                            }
                            break;
                        case "copy"://копирование файлов и каталогов
                            string[] subs = Command.Split(FS._separators, StringSplitOptions.RemoveEmptyEntries);
                            if (subs.Length == 4)
                            {
                                if (FS.IsDir(subs[1]))     //копирование каталога
                                {
                                    FS.CopyDirectory(subs[1], subs[3]);
                                    FS.RebuildTree();
                                }
                                else
                                {
                                    try
                                    {
                                        File.Copy(subs[1], subs[3]);
                                        FS.RebuildTree();
                                    }
                                    catch (Exception e)
                                    {
                                        FS._error = e.Message;
                                    }
                                }
                            }
                            else
                                FS._error = "Command syntax error: " + Command;
                            break;
                        case "mask":// задать маску для фильтрации файлов и каталогов
                            FS.SetMask(Command.Substring(4));
                            FS.RebuildTree();
                            break;
                        case "exit":// выход из приложения
                            config.Save(FS._currentDirectory);
                            return;
                        default:
                            if (int.TryParse(Command, out int V))  //введен номер страницы
                            {
                                FS.PageCurrent = V;
                                if (FS.PageCurrent < 1) FS.PageCurrent = 1;
                            }
                            else
                                FS._error = "Command syntax error: " + Command;
                            break;
                    }
                }
            }
        }
    }
}
