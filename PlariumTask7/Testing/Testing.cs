using System;
using System.IO;
using UserSpace;
using LetterSpace;
using CollectionOfUsers;
using System.Collections.Generic;
using PlariumTask7;
using Newtonsoft.Json;

namespace VariantB
{
    /// <summary>
    /// Убрал из класса проверки работы других методов из прошлых заданий (делегаты, события, индексаторы и т.д.)
    /// т.к. выходит слишком много кода
    /// </summary>
    class Testing
    {
        static void Main(string[] args)
        {
            List<User> users;
            List<Letter> letters;

            if (RestoreCollectionState())
            {
                // если программа завершилась аварийно в прошлый раз, берем данные не с БД, а с коллекции
                users = UsersCollection.GetListOfUser();

                // выполняем основные методы с учетом сохраненных пользователей
                CallMainMethods(users[1], users[0]);

                // и завершаем программу корректно, в следующий раз снова будет вызвано исключение и так по кругу
                return;
            }

            // получаем пользователей с БД
            users = UsersDataBase.GetUsersFromDatabase("DataBase");
            // получаем письма
            letters = LettersDataBase.GetLettersFromDatabase("LettersDataBase");

            // общение между пользователями 
            users[0].SendLetter(users[1], letters[0]);
            users[3].SendLetter(users[2], letters[1]);
            users[2].SendLetter(users[0], letters[2]);
            users[4].SendLetter(users[3], letters[3]);
            // Направить письмо заданного человека с заданной темой всем адресатам.
            users[4].SendToAll(letters[4]);

            // Проверка работы методов работы с коллекциями с записью и чтением из файла
            CallMainMethods(users[1], users[0]);

            // удаляем двух пользователей из БД
            UsersDataBase.DeleteUserFromDatabase(users[4], "DataBase");
            UsersDataBase.DeleteUserFromDatabase(users[3], "DataBase");
            // возвращаем их в БД
            UsersDataBase.AddUserToDatabase(new User("Dima", "Pogrib", new DateTime(2000, 9, 17), false), "DataBase");
            UsersDataBase.AddUserToDatabase(new User("Roman", "Romanov", new DateTime(1989, 7, 1), false), "DataBase");
            // удаляем два письма из БД
            LettersDataBase.DeleteLetterFromDatabase(letters[4], "LettersDataBase");
            LettersDataBase.DeleteLetterFromDatabase(letters[3], "LettersDataBase");
            // возвращаем их (письма) в БД
            LettersDataBase.AddLetterToDatabase(new Letter("Study", "Can you share your homework?"), "LettersDataBase");
            LettersDataBase.AddLetterToDatabase(new Letter("Study", "Link to videoconference"), "LettersDataBase");

            // вызываем исключение и аварийно завершаем программу
            CallExceptionAndSaveUsers();
        }

        /// <summary>
        /// Записывает и читает из файла результаты выполненных методов (из задания) к коллекции
        /// </summary>
        /// <param name="userSony"></param>
        /// <param name="userAndrey"></param>
        private static void CallMainMethods(User userSony, User userAndrey)
        {
            // Найти пользователя, длина писем которого наименьшая.
            UsersCollection.WriteToFileResult(UsersCollection.FindUserWithTheShortesLetter().ToString(),
                "Пользователь, длина писем которого наименьшая");

            // Вывести информацию о пользователях, а также количестве полученных и отправленных ими письмах.
            UsersCollection.WriteToFileResult(UsersCollection.GetUsersInfo(), 
                "Информация о пользователях, а также количестве полученных и отправленных ими письмах.");

            // Вывести информацию о пользователях, которые получили хотя бы одно сообщение с заданной темой.
            UsersCollection.WriteToFileResult(UsersCollection.GetUsersWithSuchTopic("Work"), 
                "Информация о пользователях, которые получили хотя бы одно сообщение с заданной темой (Work)");

            // Вывести информацию о пользователях, которые не получали сообщения с заданной темой.
            UsersCollection.WriteToFileResult(UsersCollection.GetUsersWithoutSuchTopic("Study"), 
                "Информация о пользователях, которые не получали сообщения с заданной темой (Study)");

            // чтение с файла и вывод в консоль
            var text = UsersCollection.GetResultsFromFile();

            foreach(var lines in text)
            {
                Console.WriteLine(lines);
            }

            File.Delete("Results.txt");
        }

        /// <summary>
        /// Вызываем исключение и сохраняем состояние коллекции
        /// </summary>
        public static void CallExceptionAndSaveUsers()
        {
            try
            {
                // вызываем исключение, созданием невалидного пользователя
                User notValidUser = new User("", "", new DateTime());
            }
            catch
            {
                // если возникло исключение сохраняем состояние коллекции в файл
                using (StreamWriter file = File.CreateText("SavedUsers.json"))
                {
                    JsonSerializer serializer = new JsonSerializer()
                    {
                        PreserveReferencesHandling = PreserveReferencesHandling.All
                    };

                    serializer.Serialize(file, UsersCollection.Dictionary.Keys);
                }

                // принудительно завершаем программу
                Environment.Exit(-1);
            }
        }

        /// <summary>
        /// Если в прошлый раз, программа завершилась аварийно, тогда
        /// востанавливаем состояние коллекции
        /// </summary>
        /// <returns>
        /// True, если в прошлый раз программа завершилась аварийно, иначе false
        /// </returns>
        public static bool RestoreCollectionState()
        {
            if (File.Exists("SavedUsers.json"))
            {
                using (StreamReader file = File.OpenText("SavedUsers.json"))
                {
                    JsonSerializer serializer = new JsonSerializer()
                    {
                        PreserveReferencesHandling = PreserveReferencesHandling.All
                    };

                    List<User> list = (List<User>)serializer.Deserialize(file, typeof(List<User>));
                    UsersCollection.AddUsers(list);
                }

                File.Delete("SavedUsers.json");

                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
