using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;

namespace WPF_RegistrationForm
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    /// 

    public partial class MainWindow : Window
    {
        private CaptchaCode captcha = new CaptchaCode();

        public MainWindow()
        {
            InitializeComponent();
        }

        private void CountryBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (!CityBox.Items.IsEmpty)
                CityBox.Items.Clear();
            try
            {
                switch ((CountryBox.SelectedItem as ComboBoxItem).Content)
                {
                    case "Australia":
                        CityBox.Items.Add(new ComboBoxItem { Content = "Melbourne" });
                        CityBox.Items.Add(new ComboBoxItem { Content = "Sydney" });
                        CityBox.Items.Add(new ComboBoxItem { Content = "Brisbane" });
                        break;
                    case "Austria":
                        CityBox.Items.Add(new ComboBoxItem { Content = "Eisenstadt" });
                        CityBox.Items.Add(new ComboBoxItem { Content = "Salzburg" });
                        CityBox.Items.Add(new ComboBoxItem { Content = "Innsbruck" });
                        break;
                    case "Germany":
                        CityBox.Items.Add(new ComboBoxItem { Content = "Berlin" });
                        CityBox.Items.Add(new ComboBoxItem { Content = "Hamburg" });
                        CityBox.Items.Add(new ComboBoxItem { Content = "Munich" });
                        break;
                    case "Great Britain":
                        CityBox.Items.Add(new ComboBoxItem { Content = "London" });
                        CityBox.Items.Add(new ComboBoxItem { Content = "Manchester" });
                        CityBox.Items.Add(new ComboBoxItem { Content = "Glasgow" });
                        break;
                    case "France":
                        CityBox.Items.Add(new ComboBoxItem { Content = "Paris" });
                        CityBox.Items.Add(new ComboBoxItem { Content = "Marseille" });
                        CityBox.Items.Add(new ComboBoxItem { Content = "Lyon" });
                        break;
                    case "Ukraine":
                        CityBox.Items.Add(new ComboBoxItem { Content = "Cherkasy" });
                        CityBox.Items.Add(new ComboBoxItem { Content = "Chernihiv" });
                        CityBox.Items.Add(new ComboBoxItem { Content = "Chernivtsi" });
                        CityBox.Items.Add(new ComboBoxItem { Content = "Dnipro" });
                        CityBox.Items.Add(new ComboBoxItem { Content = "Donetsk" });
                        CityBox.Items.Add(new ComboBoxItem { Content = "Ivano-Frankivsk" });
                        CityBox.Items.Add(new ComboBoxItem { Content = "Kharkiv" });
                        CityBox.Items.Add(new ComboBoxItem { Content = "Kherson" });
                        CityBox.Items.Add(new ComboBoxItem { Content = "Khmelnytskyi" });
                        CityBox.Items.Add(new ComboBoxItem { Content = "Kyiv" });
                        CityBox.Items.Add(new ComboBoxItem { Content = "Kramatorsk" });
                        CityBox.Items.Add(new ComboBoxItem { Content = "Kropyvnytskyy" });
                        CityBox.Items.Add(new ComboBoxItem { Content = "Luhansk" });
                        CityBox.Items.Add(new ComboBoxItem { Content = "Lutsk" });
                        CityBox.Items.Add(new ComboBoxItem { Content = "Lviv" });
                        CityBox.Items.Add(new ComboBoxItem { Content = "Mykolaiv" });
                        CityBox.Items.Add(new ComboBoxItem { Content = "Odessa" });
                        CityBox.Items.Add(new ComboBoxItem { Content = "Poltava" });
                        CityBox.Items.Add(new ComboBoxItem { Content = "Rivne" });
                        CityBox.Items.Add(new ComboBoxItem { Content = "Sumy" });
                        CityBox.Items.Add(new ComboBoxItem { Content = "Ternopil" });
                        CityBox.Items.Add(new ComboBoxItem { Content = "Vinnytsia" });
                        CityBox.Items.Add(new ComboBoxItem { Content = "Zaporizhzhia" });
                        CityBox.Items.Add(new ComboBoxItem { Content = "Zhytomyr" });
                        CityBox.Items.Add(new ComboBoxItem { Content = "Uzhgorod" });
                        break;
                    case "USA":
                        CityBox.Items.Add(new ComboBoxItem { Content = "Florida" });
                        CityBox.Items.Add(new ComboBoxItem { Content = "New-York" });
                        CityBox.Items.Add(new ComboBoxItem { Content = "Texas" });
                        break;
                    default:
                        CityBox.SelectedIndex = -1;
                        break;
                }
            }
            catch (NullReferenceException nre)
            {
                CityBox.SelectedIndex = -1;
            }
        }

        private void Decline_Click(object sender, RoutedEventArgs e)
        {
            CleaningForm();
        }

        private void CleaningForm()
        {
            Name.Clear();
            Password.Clear();
            Age.Clear();
            Male.IsChecked = true;
            Interests1.IsChecked = Interests2.IsChecked = Interests3.IsChecked = false;
            CountryBox.SelectedIndex = -1;
            Information.Clear();
            Answer.Clear();
            Task.Content = "";
        }

        private void Confirm_Click(object sender, RoutedEventArgs e)
        {
            if (CheckingFilling() && CheckingCaptcha())
            {
                UserModel user = CollectingInfo(); ;
                BinaryFormatter formatter = new BinaryFormatter();
                string file_path = "UserModel.bin";
                try
                {
                    using (FileStream fstream = new FileStream(file_path,
                        FileMode.Append, FileAccess.Write, FileShare.None))
                    {
                        formatter.Serialize(fstream, user);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.ToString());
                }
            }
            else MessageBox.Show("All required information had to be filled.");
            CleaningForm();
        }

        private bool CheckingFilling()
        {
            if (Name.Text.Length < 1) return false;
            if (Password.Text.Length < 1) return false;
            if (Age.Text.Length < 1) return false;
            if (CityBox.SelectedIndex == -1) return false;
            return true;
        }

        private bool CheckingCaptcha()
        {
            int[] num = new int[2];
            string[] substring = new string[2];
            string captcha = Task.Content.ToString();
            int index, i;
            if (captcha.IndexOf('+') != -1)
            {
                index = captcha.IndexOf('+');
                i = 0;
            }
            else if (captcha.IndexOf('-') != -1)
            {
                index = captcha.IndexOf('-');
                i = 1;
            }
            else if (captcha.IndexOf('*') != -1)
            {
                index = captcha.IndexOf('*');
                i = 2;
            }
            else
            {
                index = captcha.IndexOf('/');
                i = 3;
            }
            substring[0] = captcha.Substring(0, index);
            substring[1] = captcha.Substring(index+1, captcha.Length - 1-index);
            for (int j = 0; j < substring.Length; j++)
                num[j] = Convert.ToInt32(substring[j]);
            int result = -1;
            switch (i)
            {
                case 0:
                    result = num[0] + num[1];
                    break;
                case 1:
                    result = num[0] - num[1];
                    break;
                case 2:
                    result = num[0] * num[1];
                    break;
                case 3:
                    result = num[0] / num[1];
                    break;
            }
            return result.ToString() == Answer.Text;
        }

        private UserModel CollectingInfo()
        {
            string name = Name.Text;
            string password = Password.Text;
            int age = Convert.ToInt32(Age.Text);
            string sex;
            if (Male.IsChecked == true)
                sex = "Male";
            else
                sex = "Female";
            string[] interests = new string[3];
            int i = 0;
            if (Interests1.IsChecked == true)
                interests[i++] = Interests1.Content.ToString();
            if (Interests2.IsChecked == true)
                interests[i++] = Interests2.Content.ToString();
            if (Interests3.IsChecked == true)
                interests[i++] = Interests3.Content.ToString();
            if (i != 3)
                Array.Resize(ref interests, i);
            if (i == 0) interests = null;
            string country = CountryBox.Text;
            string city = CityBox.Text;
            string information = "";
            if (Information.Text.Length > 0)
                information = Information.Text;
            UserModel user = new UserModel(name, password, age, sex, interests,
                country, city, information);
            return user;
        }

        private void CityBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            string fix_captcha = captcha.ToString();
            Task.Content = fix_captcha;
        }
    }
}
