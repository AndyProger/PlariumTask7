using System;
using System.Text.RegularExpressions;

namespace VariantB.Models
{
    abstract public class Person : ICloneable
    {
        public string Name { get; set; }
        public string Surname { get; set; }
        public DateTime Birthdate { get; set; }

        public Person() { }
        protected Person(string name, string surname)
        {
            name = Regex.Replace(name.Trim(), "\\s+", " ");
            surname = Regex.Replace(surname.Trim(), "\\s+", " ");

            if (IsNameValid(name) && IsNameValid(surname))
            {
                Name = name;
                Surname = surname;
            }
            else
            {
                throw new ArgumentException();
            }
        }

        public override string ToString()
        {
            return $"Name: {Name} \nSurname: {Surname} \nBirthdate: {Birthdate} \n";
        }

        // классы наследники сами определяют логику валидации фио
        protected abstract bool IsNameValid(string name);

        public virtual object Clone() => MemberwiseClone();
    }
}
