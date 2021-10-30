using System;
using System.Drawing;
using UserSpace;
using System.Xml.Serialization;

namespace LetterSpace
{
    // пример использования generics
    interface IAttach<T>
    {
        /// <summary>
        /// Прикрепить что-то (в случае с письмом, будет реализован метод прикрепления изображения к письму)
        /// </summary>
        public void Attach(T obj);
    }

    [Serializable]
    public class Letter : IAttach<Image>
    {
        public User Sender { get; set; }
        public User Recipient { get; set; }
        public DateTime SendingDate { get; set; }
        public string Topic { get; set; } = string.Empty;
        public string Text { get; set; } = string.Empty;

        public Image AttachedImage { get; set; }

        public Letter(string topic, string text)
        {
            Topic = topic;
            Text = text;
        }

        public Letter(Letter other)
        {
            Sender = other.Sender;
            Recipient = other.Recipient;
            Topic = other.Topic;
            Text = other.Text;
        }

        // к-ор по-умолч. для сериализации/десериализации
        public Letter()
        {
        }
        
        public override string ToString()
        {
            return $"Sender:\n{Sender}\nRecipient:\n{Recipient}\nTopic: {Topic}\nText: {Text}\nDate: {SendingDate}";
        }

        public void Attach(Image img)
        {
            if(img is not null)
            {
                AttachedImage = img;
            }
            else
            {
                throw new ArgumentNullException();
            }
        }
    }
}
