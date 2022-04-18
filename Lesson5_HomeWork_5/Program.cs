using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.IO;
using System.Xml;
using System.Xml.Serialization;

namespace Lesson5_HomeWork_5
{
    //класс "Задача"
    public class ToDo
    {
        //поля
        public string Title; //название задачи
        public bool IsDone;  //статус выполнения задачи (true/false - выполнена/не выполнена)

        //конструкторы 
        public ToDo() //без параметров
        {
            Title = "";
            IsDone = false;
        }

        public ToDo(string title, bool isDone)
        {
            Title = title;
            IsDone = isDone;
        }

        //вывести задачу
        public string GetCurrentTask(int iNumberTask)
        {
            string sStatusTask = (IsDone) ? "[x]" : "";
            return $"{iNumberTask + 1}. {sStatusTask} {Title}";
        }
    }

    internal class Program
    {
        //формирование начального списка задач
        private static List<ToDo> GetListTask()
        {
            List<ToDo> listTask = new List<ToDo>
            {
                new ToDo("Моя задача № 1", false),
                new ToDo("Моя задача № 2", false),
                new ToDo("Моя задача № 3", true),
                new ToDo("Моя Задача № 4", false),
                new ToDo("Моя Задача № 5", true)
            };
            return listTask;
        }

        //запись массива задач в файл "tasks.xml"
        private static void WriteDataToXmlFile(string fileNameXml, List<ToDo> listTask)
        {
            //удалить файл перед записью, так как при повторном запуске,
            //после установки статуса задачи как выполненой, файл не считывается 
            if (File.Exists(fileNameXml)) File.Delete(fileNameXml);

            //запись
            XmlSerializer formatter = new XmlSerializer(typeof(List<ToDo>));
            using (FileStream fs = new FileStream(fileNameXml, FileMode.OpenOrCreate))
            {
                formatter.Serialize(fs, listTask);
            }

            //
            Console.WriteLine($"Данные записаны в файл {fileNameXml}");
        }

        //считывание массива задач из файла "tasks.xml"
        private static List<ToDo> ReadDataToXmlFile(string fileNameXml)
        {
            List<ToDo> listTask;
            XmlSerializer formatter = new XmlSerializer(typeof(List<ToDo>));
            using (FileStream fs = new FileStream(fileNameXml, FileMode.OpenOrCreate))
            {
                listTask = formatter.Deserialize(fs) as List<ToDo>;
            }
            return listTask;
        }

        //вывод списка задач на консоль
        private static void PrintMassivTasks(List<ToDo> listTask)
        {
            Console.WriteLine("----- Вывод списка задач -----");
            for (int i = 0; i < listTask.Count; i++)
                Console.WriteLine(listTask[i].GetCurrentTask(i));
        }

        //добавить массив задач
        private static void AddTasks(string fileNameXml, List<ToDo> listTask)
        {
            Console.WriteLine("\n--- Ввод массива задач ---");
            ConsoleKeyInfo cki = new ConsoleKeyInfo();
            do
            {
                Console.Write("Введите название задачи (по окончании ввода нажмите ENTER) : ");
                ToDo addTask = new ToDo(Console.ReadLine(), false);
                listTask.Add(addTask);
                Console.WriteLine("Новая задача добавлена в список задач");
                Console.Write("Для продолжения ввода данных нажмите любую клавишу, для окончания клавишу n : ");
                cki = Console.ReadKey(true);
                Console.WriteLine();
            }
            while (cki.Key != ConsoleKey.N);
            WriteDataToXmlFile(fileNameXml, listTask); //запись данных
        }

        //получить корректное значения (число типа int)
        private static int GetCorrectNumber(string sNumber)
        {
            int outNumber;
            bool bNumber = int.TryParse(sNumber, out outNumber); //проверка, что введенная строка является числом
            return (bNumber) ? outNumber : 0;
        }

        //отметить задачу как выполненную по вводу порядкового номера
        private static void SetStatusTask(string fileNameXml, List<ToDo> listTask)
        {
            Console.WriteLine("\n--- Отметить выполненные задачи ---");
            if (listTask.Count > 0)  //задачи есть
            {
                ConsoleKeyInfo cki = new ConsoleKeyInfo();
                do
                {
                    Console.Write($"Введите номер (от 1 до {listTask.Count}) задачи (по окончании ввода нажмите ENTER) : ");
                    string sNumber = Console.ReadLine();
                    int numTask = GetCorrectNumber(sNumber) - 1; //номер задачи в массиве задач listTask
                    if (numTask >= 0 && numTask < listTask.Count)
                    {
                        if (listTask[numTask].IsDone) //задача уже выполнена
                        {
                            Console.WriteLine($"Задача № {numTask + 1} '{listTask[numTask].Title}' уже выполнена");
                        }
                        else                          //задача не выполнена
                        {
                            listTask[numTask].IsDone = true; //установка статуса: выполнена  
                            Console.WriteLine($"Для Задачи № {numTask + 1} '{listTask[numTask].Title}' установлен статус : ВЫПОЛНЕНА");
                        }
                    }
                    else
                    {
                        Console.WriteLine($"Задача № {sNumber} отсутствует в списке задач");
                    }
                    Console.Write("Для продолжения ввода данных нажмите любую клавишу, для окончания клавишу n : ");
                    cki = Console.ReadKey(true);
                    Console.WriteLine();
                }
                while (cki.Key != ConsoleKey.N);
                WriteDataToXmlFile(fileNameXml, listTask); //запись данных
            }
            else
            {
                Console.WriteLine("Список задач пуст");
            }
        }

        //вывод меню
        private static void GetMenu()
        {
            Console.WriteLine("\n----- МЕНЮ РЕШЕНИЯ ЗАДАЧИ -----");
            Console.WriteLine("1 - Добавить задачу");
            Console.WriteLine("2 - Установить выполнение задачи");
            Console.WriteLine("3 - Вывести список задач");
        }

        //получить пункт меню
        private static int GetPunktMenu()
        {
            int numPunktMenu;
            do
            {
                Console.Write("\nВведите пункт меню (число от 1 до 3). Далее нажмите ENTER : ");
                string sNumber = Console.ReadLine();
                numPunktMenu = GetCorrectNumber(sNumber);
                if (numPunktMenu >= 1 && numPunktMenu <= 3)
                    break;
                else
                    Console.WriteLine($"Пункт меню № {numPunktMenu} отсутствует.");
            }
            while (true);
            return numPunktMenu;
        }

        //решение задачи
        private static void Solve(string fileNameXml)
        {
            List<ToDo> listTask;
            if (File.Exists(fileNameXml)) //файл "tasks.xml" существует
            {
                //считывание массива задач из файла "tasks.xml" и вывод его на консоль
                Console.WriteLine($"Файл {fileNameXml} существует");
                listTask = ReadDataToXmlFile(fileNameXml);
                PrintMassivTasks(listTask);
            }
            else                          //файл "tasks.xml" не существует
            {
                //формирование начального списка задач, его запись и вывод на консоль
                Console.WriteLine($"Файл {fileNameXml} не существует");
                Console.WriteLine("Автоматическое формирование начального списка задач");
                listTask = GetListTask();
                PrintMassivTasks(listTask);
                WriteDataToXmlFile(fileNameXml, listTask);
            }

            //работа с меню (добавление задач, отметка о выполнении задачи, вывод списка задач)
            GetMenu();
            ConsoleKeyInfo cki = new ConsoleKeyInfo();
            do
            {
                int numPunktMenu = GetPunktMenu();
                switch (numPunktMenu)
                {
                    case 1: AddTasks(fileNameXml, listTask); break;           //добавить массив задач
                    case 2: SetStatusTask(fileNameXml, listTask); break;      //отметить задачу как выполненную
                    case 3: PrintMassivTasks(listTask); break;                //вывод списка задач на консоль
                }
                Console.Write("\nДля продолжения работы с пунктами меню нажмите любую клавишу, для окончания клавишу n : ");
                cki = Console.ReadKey(true);
                Console.WriteLine();
            }
            while (cki.Key != ConsoleKey.N);
        }

        static void Main(string[] args)
        {
            //
            Console.WriteLine("----- Урок № 5, задание № 5 -----");
            Console.WriteLine();

            //имя записываемого файла
            string fileNameXml = "tasks.xml";

            //решение задачи
            Solve(fileNameXml);

            //
            Console.ReadLine();
        }
    }
}
