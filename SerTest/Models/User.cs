using System;
using VariantB;
using LetterSpace;
using VariantB.Models;
using CollectionOfUsers;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Xml.Serialization;

namespace UserSpace
{
    [Serializable]
    public class User : Person, ICloneable, IComparable<User>
    {
        // У каждого пользователья генерируется уникальный id
        public string ID { get; set; } = Guid.NewGuid().ToString();
        public int LettersSent { get; set; }
        public int LettersReceived { get => Letters.Count; }

        // событие "письмо отправлено"
        public event SendLetterHandler LetterSent;

        // В списке храним письма, которые получил юзер
        public List<Letter> Letters = new List<Letter>();

        public User(string name, string surname, DateTime birthdate) : base(name, surname)
        {
            if(birthdate > new DateTime(1900,1,1) && birthdate < DateTime.Now)
            {
                Birthdate = birthdate;
                UsersCollection.AddUser(this);
            }
            else
            {
                throw new ArgumentException();
            }
        }

        public User() { }

        public override object Clone() => MemberwiseClone();

        // Отправить письмо получателю
        public void SendLetter(User reciper, Letter letter)
        {
            Letter copyLetter = new Letter(letter);

            copyLetter.Sender = this;
            copyLetter.Recipient = reciper;
            copyLetter.SendingDate = DateTime.Now;

            reciper.Letters.Add(copyLetter);
            LettersSent++;

            // вызываем переданный метод на возникшее событие (отправка письма)
            var eventArgs = new SendLetterEventArgs();

            if(LetterSent is not null)
            {
                eventArgs.Letter = copyLetter;
                LetterSent(this, eventArgs);
            }
        }

        // Отправить письмо заданного человека с заданной темой всем адресатам.
        public void SendToAll(Letter letter)
        {
            List<User> users = new List<User>(UsersCollection.Dictionary.Keys);
            users.Remove(this);

            foreach(User user in users)
            {
                SendLetter(user, letter);
            }
        }

        public override string ToString() => $"ID: {ID} \n" + base.ToString();

        public Letter this[int index] { get => Letters[index]; }

        protected override bool IsNameValid(string name)
        {
            return !string.IsNullOrEmpty(name) && Regex.IsMatch(name, @"^[a-zA-Z]+$");
        }

        public int CompareTo(User other)
        {

            return Surname.CompareTo(other.Surname);
        }
    }
}
