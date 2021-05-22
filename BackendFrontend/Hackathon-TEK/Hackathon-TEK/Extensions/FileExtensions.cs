using Microsoft.VisualBasic.FileIO;
using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.CompilerServices;
using System.Text;

namespace Hackathon_TEK.Extensions
{
    public static class FileExtensions
    {
        /// <summary>
        /// Запись текста в файл
        /// </summary>
        /// <param name="file">Файл</param>
        /// <param name="text">Текст</param>
        /// <returns></returns>
        public static bool WriteToFile(this FileInfo file, string text)
        {
            try
            {
                using (FileStream fstream = new FileStream(file.FullName, FileMode.OpenOrCreate))
                {
                    // преобразуем строку в байты
                    byte[] array = Encoding.UTF8.GetBytes(text);
                    // запись массива байтов в файл
                    fstream.Write(array, 0, array.Length);
                    Console.WriteLine($"Текст записан в файл: {file.FullName}");
                }
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return false;
            }
        }

        /// <summary>
        /// Парсинг CSV файла
        /// </summary>
        public static List<string[]> ParseCSV(this FileInfo file, string delimeter = ";")
        {
            using (TextFieldParser parser = new TextFieldParser(file.FullName))
            {
                parser.TextFieldType = FieldType.Delimited;
                parser.SetDelimiters(delimeter);

                var result = new List<string[]>();
                while (!parser.EndOfData)
                {
                    string[] fields = parser.ReadFields();
                    result.Add(fields);
                }
                return result;
            }
        }

        /// <summary>
        /// Получить весь текст из файла
        /// </summary>
        /// <param name="file">Файл</param>
        /// <param name="text">Переменная, куда сохранить текст из файла</param>
        /// <returns>
        /// Код ошибки
        ///  0 - Успешно
        /// -1 - Файл не существует
        /// </returns>
        public static int ReadAllFile(this FileInfo file, ref string text)
        {
            try
            {
                if (!file.Exists) return -1;
                using (FileStream fstream = File.OpenRead(file.FullName))
                {
                    // преобразуем строку в байты
                    byte[] array = new byte[fstream.Length];
                    // считываем данные
                    fstream.Read(array, 0, array.Length);
                    // декодируем байты в строку
                    string textFromFile = Encoding.UTF8.GetString(array);

                    text = textFromFile;
                    return 0;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                throw ex;
            }
        }
        public static string ReadAllFile(this FileInfo file)
        {
            try
            {
                if (!file.Exists)
                {
                    Console.WriteLine($"Не удалось наййти файл: {file.FullName} [FileInfo -> ReadAllFile]");
                    return null;
                }
                using (FileStream fstream = File.OpenRead(file.FullName))
                {
                    // преобразуем строку в байты
                    byte[] array = new byte[fstream.Length];
                    // считываем данные
                    fstream.Read(array, 0, array.Length);
                    // декодируем байты в строку
                    return Encoding.UTF8.GetString(array);
                    //return CodePagesEncodingProvider.Instance.GetEncoding(1251).GetString(array);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                throw ex;
            }
        }
    }
}
