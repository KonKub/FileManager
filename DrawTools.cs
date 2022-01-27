using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;


namespace FileManager
{
    public class DrawTools
    {
        public int PageSize;   //размер страницы для вывода дерева каталогов
        public int PageCurrent;
        public int PageCount;

        public DrawTools()
        {
            Console.SetBufferSize(80, 25);
            PageSize = Console.WindowHeight - 2 - 6;
            PageCurrent = 0;
            PageCount = 0;
        }
        public DrawTools(int width, int height)
        {
            Console.SetBufferSize(width, height);
            PageSize = Console.WindowHeight - 2 - 6;
            PageCurrent = 0;
            PageCount = 0;
        }

        //задать цвет выводимого текста
        public void SetColor(ConsoleColor bgColor, ConsoleColor fgColor)
        {
            Console.BackgroundColor = bgColor;
            Console.ForegroundColor = fgColor;
        }
        //вывод строки S в координатах X, Y цветами bgColor fgColor
        public void WriteStr(int X, int Y, string S, ConsoleColor bgColor, ConsoleColor fgColor)
        {
            SetColor(bgColor, fgColor);
            Console.SetCursorPosition(X, Y);
            Console.Write(S);
        }
        //вывод строки S в координатах X, Y цветом fgColor
        public void WriteStr(int X, int Y, string S, ConsoleColor fgColor)
        {
            Console.ForegroundColor = fgColor;
            Console.SetCursorPosition(X, Y);
            Console.Write(S);
        }
        //вывод строки S в координатах X, Y цветами bgColor fgColor. после завершения вернуть цвета как были до вызова
        public void WriteStrColorBack(int X, int Y, string S, ConsoleColor bgColor, ConsoleColor fgColor)
        {
            ConsoleColor bg = Console.BackgroundColor;
            ConsoleColor fg = Console.ForegroundColor;
            SetColor(bgColor, fgColor);
            Console.SetCursorPosition(X, Y);
            Console.Write(S);
            SetColor(bg, fg);
        }
        //вывод строки S в координатах X, Y без задания цвета
        public void WriteStr(int X, int Y, string S)
        {
            Console.SetCursorPosition(X, Y);
            Console.Write(S);
        }
        //вывод символа C в координатах X, Y без задания цвета
        public void WriteStr(int X, int Y, char C)
        {
            Console.SetCursorPosition(X, Y);
            Console.Write(C);
        }

        //рисование горизогтальной линии
        public void HorizLine(int X, int Y, int Len, char SCh, char BCh, char ECh)
        {
            WriteStr(X, Y, SCh);
            for (int i = 1; i < Len - 1; i++) Console.Write(BCh);
            Console.Write(ECh);
        }

        //рисование вертикальной линии
        public void VertLine(int X, int Y, int Len, char SCh, char BCh, char ECh)
        {
            WriteStr(X, Y, SCh);
            for (int i = 1; i < Len - 1; i++) WriteStr(X, Y + i, BCh);
            WriteStr(X, Y + Len - 1, ECh);
        }

        //рисование рамки приложения
        public void DrawFrame()
        {
            SetColor(ConsoleColor.DarkBlue, ConsoleColor.Yellow);
            Console.Clear();

            HorizLine(0, 0, Console.WindowWidth, '\u2554', '\u2550', '\u2557');                          //верхняя горизонтальная линия
            HorizLine(0, Console.WindowHeight - 2, Console.WindowWidth, '\u255A', '\u2550', '\u255D');   //нижняя горизонтальная линия

            VertLine(0, 1, Console.WindowHeight - 3, '\u2551', '\u2551', '\u2551');                      //левая вертикальная линия
            VertLine(Console.WindowWidth - 1, 1, Console.WindowHeight - 3, '\u2551', '\u2551', '\u2551');//левая вертикальная линия

            HorizLine(0, Console.WindowHeight - 6, Console.WindowWidth, '\u255F', '\u2500', '\u2562');   //средняя горизонтальная линия

            Console.SetCursorPosition(0, Console.WindowHeight - 1);

            WriteStr(4, 0, " KK File Manager ", ConsoleColor.White);

        }

        //Вывод приглашения и установка курсора в строке ввода
        public void DrawInputLine()
        {
            string S = ">";
            WriteStr(0, Console.WindowHeight - 1, S.PadRight(Console.WindowWidth - 1, ' '), ConsoleColor.Black, ConsoleColor.Gray);
            Console.SetCursorPosition(1, Console.WindowHeight - 1);
        }

        //Обрезка строки AStr до длины Len
        public string CutString(string AStr, int Len)
        {
            if (AStr.Length > Len) return AStr.Substring(0, Len - 1) + ">";
            else return AStr;
        }

    }
}
