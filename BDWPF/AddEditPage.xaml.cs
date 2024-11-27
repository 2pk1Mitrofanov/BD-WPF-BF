using System;
using System.Windows;
using System.Windows.Controls;

namespace BDWPF
{
    public partial class AddEditPage : Page
    {
        private BFEntities1 db;
        private Employee _employee;

        public AddEditPage(Employee selectedEmployee)
        {
            InitializeComponent();
            db = new BFEntities1();

            // Если передан объект сотрудника, то это редактирование
            if (selectedEmployee != null)
            {
                _employee = selectedEmployee;
                NameTextBox.Text = _employee.Name;
                RoleTextBox.Text = _employee.Role;
                ContactInfoTextBox.Text = _employee.ContactInfo;
            }
            else
            {
                _employee = new Employee();
            }
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                // Проверка заполненности полей
                if (string.IsNullOrWhiteSpace(NameTextBox.Text) ||
                    string.IsNullOrWhiteSpace(RoleTextBox.Text) ||
                    string.IsNullOrWhiteSpace(ContactInfoTextBox.Text))
                {
                    MessageBox.Show("Все поля должны быть заполнены.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                // Заполнение свойств объекта _employee
                _employee.Name = NameTextBox.Text;
                _employee.Role = RoleTextBox.Text;
                _employee.ContactInfo = ContactInfoTextBox.Text;

                // Если это новый сотрудник, добавляем его в базу
                if (_employee.EmployeeID == 0)
                {
                    db.Employee.Add(_employee);
                }

                // Сохраняем изменения в базе данных
                db.SaveChanges();

                MessageBox.Show("Сотрудник успешно сохранён.", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);

                // Возвращаемся на предыдущую страницу
                Manager.MainFrame.GoBack();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при сохранении: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
