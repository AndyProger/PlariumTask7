using System;
using UserSpace;
using LetterSpace;
using System.Threading.Tasks;
using System.IO;
using Newtonsoft.Json;
using VariantB;
using System.Collections.Generic;
using CollectionOfUsers;
using System.Xml.Serialization;

namespace SerTest
{

    class Program
    {
        static public void Main(string[] args)
        {
            XmlSerializer formatter = new XmlSerializer(typeof(UsersCollection));

            // десериализация
            using (FileStream fs = new FileStream("persons.xml", FileMode.OpenOrCreate))
            {
                UsersCollection newPerson = (UsersCollection)formatter.Deserialize(fs);

                Console.WriteLine(newPerson);

                Console.WriteLine("Объект десериализован");
            }
        }
    }
}
