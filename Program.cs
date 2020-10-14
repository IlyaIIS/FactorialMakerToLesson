using System;
using System.Threading;

namespace NewNamespace
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.Write("Введите число факториала: ");
            double factorial = FindFactorial(GetIntToFactorial());

            Console.Clear();

            DrawResultAndDoSomeСalculations(factorial);
        }

        static double FindFactorial(int max)          //находит факториал
        {
         double x = 1;
         for (int i = 2; i <= max; i++) x *= i;
         return x;   
        }
        
        static int GetIntToFactorial()    //Требует у пользователя число, удавлетворяющее требованиям факториала
        {
            bool repeat;
            int output;
            do
            {
                repeat = !Int32.TryParse(Console.ReadLine(), out output);

                if (!repeat) 
                    if (output < 0)
                    {
                        repeat = true;
                        Console.WriteLine("От такого числа нельзя получить факториал!");
                    }

                if (repeat)
                {
                    Console.Write("Введите корректное число (например 5): ");
                }
            } while (repeat);

            return output;
        }

        //Создание массивов с цветами, доступными в консоли, чтобы получать к ним доступ по индексу
        static dynamic[] colorSign = {                           //Цвета символов
                                        ConsoleColor.Black,      //0
                                        ConsoleColor.DarkGray,   //1
                                        ConsoleColor.Gray,       //2
                                        ConsoleColor.White,      //3
                                     };
        static dynamic[] colorBack = {                           //Цвета заднего фона
                                        ConsoleColor.DarkBlue,   //0
                                        ConsoleColor.Blue,       //1
                                     };
        static dynamic[] colorFact = {                           //Цвета выводимого значения
                                        ConsoleColor.Red,        //0
                                        ConsoleColor.Magenta,    //1
                                     };

        //Глобальные переменные
        static int signNum;           //Число символов рамки
        static byte[] signColor;      //Массив, хранящий индекс цвета каждого символа рамки
        static int[] signOrder;       //Массив, хранящий порядок символов, для правельного "движения" светлой части
        static int signNow = 0;       //Какой символ по счёту отрисовывается

        static void DrawResultAndDoSomeСalculations(double factorial)
        {
            int sleep = 100;       //Скорость отрисовки в МС
            int timeBack = 0;      //Какой по счёту раз сменяется фон
            int timeFact = 0;      //Какой по счёту раз меняется цвет выводимого значения факториала
            int textSize = Convert.ToString(factorial).Length;

            int backgroudnColor = 0;  //Индекс цвета фона
            int factorialColor = 0;   //Индекс цвета выводимого значения

            signNum = textSize * 2 + 6;
            signColor = new byte[signNum];
            signOrder = new int[signNum];

            //Соотношение порядка отрисовки с порядком цветов символов
            for (int i = 0; i <= textSize + 1; i++) signOrder[i] = i;
            signOrder[textSize + 2] = signNum-1;
            signOrder[textSize + 3] = textSize + 2;
            for (int i = textSize + 4; i <= signOrder.Length - 1; i++) signOrder[i] = (signNum - 2) - (i - (textSize + 4));

            // Задать начальные цвета символов
            signColor[0] = 3;
            signColor[1] = 2;
            signColor[2] = 1;
            signColor[(signNum - 2) / 2 + 1] = 3;
            signColor[(signNum - 2) / 2 + 2] = 2;
            signColor[(signNum - 2) / 2 + 3] = 1;
            // Задать начальные цвета символов

            //Цикл отрисовки
            do
            {
                Console.SetCursorPosition(0, 0);

                timeBack = (timeBack + 1) % 40;
                timeFact = (timeFact + 1) % 5;
                if (timeBack == 0) backgroudnColor = (backgroudnColor + 1) % 2;
                if (timeFact == 0) factorialColor = (factorialColor + 1) % 2;

                Console.BackgroundColor = colorBack[backgroudnColor];

                WriteColor("╔");
                for (int i = 0; i < textSize; i++) WriteColor("═");
                WriteColor("╗");

                Console.WriteLine();

                WriteColor("║");
                Console.ForegroundColor = colorFact[factorialColor]; Console.Write(factorial);
                WriteColor("║");

                Console.WriteLine();

                WriteColor("╚");
                for (int i = 0; i < textSize; i++) WriteColor("═");
                WriteColor("╝");

                SignColorShift();
                Thread.Sleep(sleep);
            } while (true);
        }

        static void WriteColor(string text)   //Выводит текст с заданным цветом цветом
        {
            Console.ForegroundColor = colorSign[signColor[signOrder[signNow]]];
            Console.Write(text);
            signNow = (signNow + 1) % signNum;
        }

        static void SignColorShift()          //Смещение цветов рамки в нужном порядке
        {
            byte zero = signColor[0];
            for (int i = 1; i < signColor.Length; i++)
            {
               signColor[i - 1] = signColor[i];
               signColor[i] = 0;
            }

            signColor[signColor.Length - 1] = zero;
        }
    }
}