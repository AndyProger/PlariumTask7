using System.Collections;
using System.Collections.Generic;
using LetterSpace;
using UserSpace;
using System.IO;
using System;
using System.Xml.Serialization;

namespace CollectionOfUsers
{
    public delegate List<User> SortingAlgorithmDelegate(List<User> colection);

    [Serializable]
    public partial class UsersCollection 
    {
        // ключ - пользователь, значение - список писем
        private static Dictionary<User, List<Letter>> _dictionary;
        public static Dictionary<User, List<Letter>> Dictionary 
        {
            get => new Dictionary<User, List<Letter>>(_dictionary);
            set => _dictionary = value;
        }

        // статический к-ор для инициализации словаря 
        static UsersCollection() => _dictionary = new Dictionary<User, List<Letter>>();



        public static void AddUser(User user) => _dictionary.Add(user, user.Letters);

        // индексатор, возвращает копию письма пользователя по указанному индексу
        public Letter this[User user, int letterIndex]
        {
            get => new Letter(_dictionary [user][letterIndex]);
        }

        // индексатор, возвращает копию списка писем указанного пользователя
        public List<Letter> this[User user]
        {
            get => new List<Letter>(_dictionary[user]);
        }

        // итератор по пользователям
        public IEnumerator GetEnumerator() => _dictionary.Keys.GetEnumerator();

        /*
         * сортировка пользователей с возвратом отсортированного списка 
         * по алгоритму заданным поздним связыванием
        */
        public static List<User> SpecificUsersSort(SortingAlgorithmDelegate sorting)
        {
            return sorting(new List<User>(_dictionary.Keys));
        }

        public static void WriteToFileResult(string result, string comment = "No comments")
        {
            using (StreamWriter sw = new StreamWriter("Results.txt", true))
            {
                sw.WriteLine(new string('*',20) + comment + new string('*', 20) + "\n");
                sw.WriteLine(result);
            }
        }

        public static string[] GetResultsFromFile()
        {
            if (File.Exists("Results.txt"))
            {
                string[] text = File.ReadAllLines("Results.txt");

                return text;
            }
            else
            {
                return null;
            }
        }
    }
}
