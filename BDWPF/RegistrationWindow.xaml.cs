using BDWPF;
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
using System.Windows.Shapes;

namespace BDWPF
{
    /// <summary>
    /// Логика взаимодействия для Register.xaml
    /// </summary>
    public partial class RegistrationWindow : Window
    {
        private BFEntities1 db;

        public RegistrationWindow()
        {
            InitializeComponent();
            db = new BFEntities1();
        }

        private void Button_Register_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                // Проверка на наличие пользователя с таким логином
                if (db.Users.Any(u => u.Login == tbLogin.Text))
                {
                    MessageBox.Show("Пользователь с таким логином уже существует.");
                    return;
                }
                else
                {
                    // Генерация соли и хеширование пароля
                    string salt = PasswordHasher.GenerateSalt();
                    // Создание нового пользователя
                    Users newUser = new Users
                    {
                        Login = tbLogin.Text,
                        Salt = salt,
                        Password = PasswordHasher.HashPassword(pbPassword.Password, salt)
                    };

                    // Сохранение в БД
                    db.Users.Add(newUser);
                    db.SaveChanges();
                    MessageBox.Show("Регистрация прошла успешно!");
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка: {ex.Message}");
            }
        }
    }
}


