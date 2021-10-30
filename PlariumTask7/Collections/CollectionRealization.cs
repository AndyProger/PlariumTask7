using System.Linq;
using System.Text;
using UserSpace;

namespace CollectionOfUsers
{
    partial class UsersCollection
    {
        /// <summary>
        /// Найти пользователя, длина писем которого наименьшая.
        /// </summary>
        public static User FindUserWithTheShortesLetter()
        {
            var minLength = int.MaxValue;
            User soughtUser = null;

            foreach (var item in Dictionary)
            {
                foreach (var letter in item.Value)
                {
                    if (letter.Text.Length < minLength)
                    {
                        minLength = letter.Text.Length;
                        soughtUser = item.Key;
                    }
                }
            }

            return soughtUser;
        }

        /// <summary>
        /// Информация о пользователях, а также количестве полученных и отправленных ими письмах
        /// </summary>
        public static string GetUsersInfo()
        {
            StringBuilder info = new StringBuilder();

            foreach (var item in Dictionary)
            {
                info.Append(item.Key + $"Sent letters: {item.Key.LettersSent}\n" +
                    $"Received letters: {item.Key.LettersReceived}\n\n");
            }

            return info.ToString();
        }

        /// <summary>
        /// Информация о пользователях, которые получили хотя бы одно сообщение с заданной темой
        /// </summary>
        public static string GetUsersWithSuchTopic(string topic)
        {
            StringBuilder info = new StringBuilder();

            foreach (var item in Dictionary)
            {
                foreach (var letter in item.Value)
                {
                    if (letter.Topic == topic)
                    {
                        info.Append(item.Key + "\n");
                    }
                }
            }

            return info.ToString();
        }

        /// <summary>
        /// Информация о пользователях, которые не получали сообщения с заданной темой
        /// </summary>
        public static string GetUsersWithoutSuchTopic(string topic)
        {
            StringBuilder info = new StringBuilder();
            bool hasSuchTopic;

            foreach (var item in Dictionary)
            {
                hasSuchTopic = false;

                if (!item.Value.Any())
                {
                    info.Append(item.Key + "\n");
                    break;
                }

                foreach (var letter in item.Value)
                {
                    if (letter.Topic == topic)
                    {
                        hasSuchTopic = true;
                    }
                }

                if (!hasSuchTopic)
                    info.Append(item.Key + "\n");
            }

            return info.ToString();
        }
    }
}
