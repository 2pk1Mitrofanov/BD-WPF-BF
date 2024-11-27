using System.Windows;

namespace BDWPF
{
    public partial class EmployeeWindow : Window
    {
        private Employee _employee;

        public EmployeeWindow(Employee employee)
        {
            InitializeComponent();
            _employee = employee;

            if (_employee != null)
            {
                tbName.Text = _employee.Name;
                tbRole.Text = _employee.Role;
                tbContactInfo.Text = _employee.ContactInfo;
            }
        }

        private void Button_Save(object sender, RoutedEventArgs e)
        {
            if (_employee == null)
            {
                // Создание нового сотрудника
                _employee = new Employee
                {
                    Name = tbName.Text,
                    Role = tbRole.Text,
                    ContactInfo = tbContactInfo.Text
                };

                using (var db = new BFEntities1())
                {
                    db.Employee.Add(_employee);
                    db.SaveChanges();
                }
            }
            else
            {

                _employee.Name = tbName.Text;
                _employee.Role = tbRole.Text;
                _employee.ContactInfo = tbContactInfo.Text;

            }

            this.DialogResult = true;
            this.Close();
        }

        private void Button_Cancel(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
            this.Close();
        }
    }
}
