using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Exercise22
{
    //Сформировать массив случайных целых чисел (размер  задается пользователем).
    //Вычислить сумму чисел массива и максимальное число в массиве.
    //Реализовать  решение  задачи  с  использованием  механизма  задач продолжения.
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Введите размер массива");
            int n = Convert.ToInt32(Console.ReadLine());

            Func<object, int[]> func1 = new Func<object, int[]>(GetArray);
            Task<int[]> task1 = new Task<int[]>(func1, n);

            Func<Task<int[]>, int> func4 = new Func<Task<int[]>, int>(SummaArray);
            Task<int> task4 = task1.ContinueWith<int>(func4);

            Func<Task<int[]>, int> func5 = new Func<Task<int[]>, int>(MaxArray);
            Task<int> task5 = task1.ContinueWith<int>(func5);

            Action<Task<int[]>> action = new Action<Task<int[]>>(PrintArray);
            Task task3 = task1.ContinueWith(action);

            task1.Start();
            Console.WriteLine("Сумма чисел массива = {0}", task4.Result);
            Console.WriteLine("Максимальное число в массиве: {0}", task5.Result);

            Console.ReadKey();
        }

        static int[] GetArray(object a)
        {
            int n = (int)a;
            int[] array = new int[n];
            Random random = new Random();
            for (int i = 0; i < n; i++)
            {
                array[i] = random.Next(0, 100);
            }
            return array;
        }
                
        static int SummaArray(Task<int[]> task)
        {
            int[] array = task.Result;
            int summa = 0;
            for (int i = 0; i < array.Count(); i++)
            {
                summa += array[i];
            }
            Thread.Sleep(500);
            return summa;
        }

        static int MaxArray(Task<int[]> task)
        {
            int[] array = task.Result;
            int max = array[0];
            foreach (int a in array)
            {
                if (a > max)
                    max = a;
            }
            Thread.Sleep(500);
            return max;
        }

        static void PrintArray(Task<int[]> task)
        {
            int[] array = task.Result;
            for (int i = 0; i < array.Count(); i++)
            {
                Console.Write($"{array[i]} ");
            }
            Console.WriteLine();
        }
    }
}
