using BDWPF;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
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
    /// Логика взаимодействия для Login.xaml
    /// </summary>
    /// 

    public partial class LoginWindow : Window
    {
        private BFEntities1 db;
        private int failedLoginAttempts = 0;
        private string captchaText;

        public LoginWindow()
        {
            InitializeComponent();
            db = new  BFEntities1();
        }

        private void Login_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                string login = tbL.Text;
                string password = pbP.Password;

                // Ищем пользователя по логину
                var user = db.Users.FirstOrDefault(u => u.Login == login);

                if (user != null)
                {
                    // Проверяем, что соль не пустая
                    if (string.IsNullOrEmpty(user.Salt))
                    {
                        MessageBox.Show("Ошибка: соль не найдена.");
                        return;
                    }

                    // Проверяем пароль
                    bool isValid = PasswordHasher.VerifyPassword(password, user.Password, user.Salt);
                    if (isValid)
                    {
                        // Сброс попыток при успешном входе
                        failedLoginAttempts = 0;
                        MainWindow mainWindow = new MainWindow();
                        mainWindow.Show();
                        this.Close();
                    }
                    else
                    {
                        failedLoginAttempts++;
                        MessageBox.Show("Неверный логин или пароль.");

                        if (failedLoginAttempts >= 3)
                        {
                            using (var captchaImageStream = CaptchaGenerator.GenerateCaptchaImage(out captchaText))
                            {
                                ShowCaptchaWindow(captchaImageStream);
                            }
                        }
                    }
                }
                else
                {
                    MessageBox.Show("Неверный логин или пароль.");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка: {ex.Message}");
            }
        }

        private void ShowCaptchaWindow(MemoryStream captchaImageStream)
        {
            CaptchaWindow captchaWindow = new CaptchaWindow(captchaImageStream, captchaText);
            bool? result = captchaWindow.ShowDialog();

            if (result == true)
            {
                // Пользователь прошел капчу, можно продолжить обработку входа
                failedLoginAttempts = 0; // Сбрасываем счетчик попыток
            }
        }

        private void Button_Register_Click(object sender, RoutedEventArgs e)
        {
            RegistrationWindow newUserWindow = new RegistrationWindow();
            newUserWindow.Show();
            this.Close();
        }
    }
}

