using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;

namespace WPF_RegistrationForm
{
    [Serializable]
    class UserModel : ISerializable
    {
        string name;
        string password;
        int age;
        string sex;
        string[] interests;
        string country;
        string city;
        string information;

        public UserModel(string name, string password, int age, string sex,
            string[] interests, string country, string city, string information)
        {
            this.name = name;
            this.password = password;
            this.age = age;
            this.sex = sex;
            if (interests.Length > 0)
            {
                this.interests = new string[interests.Length];
                for (int i = 0; i < interests.Length; i++)
                    this.interests[i] = interests[i];
            }
            this.country = country;
            this.city = city;
            if (information.Length > 0)
                this.information = information;
        }

        public UserModel() { }

        void ISerializable.GetObjectData(SerializationInfo info, StreamingContext context)
        {
            info.AddValue("Name: ", this.name);
            info.AddValue("Password: ", this.password);
            info.AddValue("Age", this.age);
            info.AddValue("Sex: ", this.sex);
            if (this.interests.Length > 0)
            {
                int k = 1;
                foreach(var i in this.interests)
                    info.AddValue($"Interest {k++}:", i);
            }
            info.AddValue("Country: ", this.country);
            info.AddValue("City: ", this.city);
            if (this.information != null)
                info.AddValue("Short information: ", this.information);
        }

    }
}
