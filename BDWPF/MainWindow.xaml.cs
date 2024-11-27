using System;
using System.Linq;
using System.Windows;
using iTextSharp.text;
using iTextSharp.text.pdf;
using System.IO;
using System.Text;
using OfficeOpenXml;
using Microsoft.Win32;
namespace BDWPF
{
    public partial class MainWindow : Window
    {
        BFEntities1 db;

        public MainWindow()
        {
            InitializeComponent();
            db = new BFEntities1();

            
        }

        // Загрузка данных при открытии окна
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                db = new BFEntities1();
                LoadEmployeeData();  // Загрузка данных сотрудников
                LoadRoleData();  // Загрузка ролей для фильтрации
                LoadMultiTableData();  // Загрузка данных о донорах
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при загрузке данных: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        // Загрузка данных сотрудников
        private void LoadEmployeeData()
        {
            try
            {
                if (db != null)
                {
                    dgEmployee.ItemsSource = db.Employee.ToList();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при загрузке сотрудников: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        // Загрузка данных о ролях для фильтрации
        private void LoadRoleData()
        {
            try
            {
                if (db != null)
                {
                    var roles = db.Employee.Select(e => e.Role).Distinct().ToList();
                    cbRoleFilter.ItemsSource = roles;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при загрузке ролей: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        // Загрузка данных о донорах
        private void LoadMultiTableData()
        {
            try
            {
                if (db != null)
                {
                    var donorData = db.Donor.ToList();
                    dgMultiTable.ItemsSource = donorData;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при загрузке данных доноров: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        // Добавление нового сотрудника
        private void Button_Add(object sender, RoutedEventArgs e)
        {
            try
            {
                EmployeeWindow employeeWindow = new EmployeeWindow(null);
                employeeWindow.ShowDialog();
                LoadEmployeeData();  // Обновляем данные сотрудников после добавления
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при добавлении сотрудника: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void Button_Delete(object sender, RoutedEventArgs e)
        {
            try
            {
                if (dgEmployee.SelectedItem is Employee selectedEmployee)
                {
                    db.Employee.Remove(selectedEmployee);
                    db.SaveChanges();
                    LoadEmployeeData();
                    MessageBox.Show("Сотрудник успешно удалён.", "Удаление", MessageBoxButton.OK, MessageBoxImage.Information);
                }
                else
                {
                    MessageBox.Show("Выберите сотрудника для удаления.", "Удаление", MessageBoxButton.OK, MessageBoxImage.Warning);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при удалении сотрудника: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        // Обработка двойного клика по строке с сотрудником для редактирования
        private void dgEmployee_MouseDoubleClick(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            try
            {
                if (dgEmployee.SelectedItem is Employee selectedEmployee)
                {
                    EmployeeWindow employeeWindow = new EmployeeWindow(selectedEmployee);
                    employeeWindow.ShowDialog();
                    LoadEmployeeData();  // Обновляем данные сотрудников после редактирования
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при редактировании сотрудника: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        // Фильтрация сотрудников по имени и роли
        private void FilterEmployees()
        {
            try
            {
                if (db != null)
                {
                    var filteredEmployees = db.Employee.AsQueryable();

                    // Фильтрация по имени
                    if (!string.IsNullOrWhiteSpace(tbNameFilter.Text) && tbNameFilter.Text != "Filter by Name")
                    {
                        filteredEmployees = filteredEmployees.Where(emp => emp.Name.Contains(tbNameFilter.Text));
                    }

                    // Фильтрация по роли
                    if (cbRoleFilter.SelectedItem != null && cbRoleFilter.SelectedItem.ToString() != "All Roles")
                    {
                        string selectedRole = cbRoleFilter.SelectedItem.ToString();
                        filteredEmployees = filteredEmployees.Where(emp => emp.Role == selectedRole);
                    }

                    dgEmployee.ItemsSource = filteredEmployees.ToList();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при фильтрации сотрудников: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        private void cbRoleFilter_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            FilterEmployees();
        }

        private void tbNameFilter_GotFocus(object sender, RoutedEventArgs e)
        {
            if (tbNameFilter.Text == "Filter by Name")
            {
                tbNameFilter.Text = "";
                tbNameFilter.Foreground = System.Windows.Media.Brushes.Black;
            }
        }
        private void tbNameFilter_LostFocus(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(tbNameFilter.Text))
            {
                tbNameFilter.Text = "Filter by Name";
                tbNameFilter.Foreground = System.Windows.Media.Brushes.Gray;
            }
        }

        private void tbNameFilter_TextChanged(object sender, System.Windows.Controls.TextChangedEventArgs e)
        {
            FilterEmployees();
        }

        // Сброс фильтров
        private void CancelFilterButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                tbNameFilter.Text = "Filter by Name";
                tbNameFilter.Foreground = System.Windows.Media.Brushes.Gray;
                cbRoleFilter.SelectedItem = null;
                LoadEmployeeData();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при сбросе фильтрации: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }


        // Двойной клик для редактирования донора
        private void dgDonor_MouseDoubleClick(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            try
            {
                var selectedDonor = dgMultiTable.SelectedItem as Donor;
                if (selectedDonor != null)
                {
                    DonorWindow donorWindow = new DonorWindow(selectedDonor);
                    donorWindow.ShowDialog();
                    LoadMultiTableData();  // Обновляем данные доноров после редактирования
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при редактировании донора: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        // Генерация PDF-отчета о сотрудниках
        private void GenerateEmployeeReportPDF(string filePath)
        {
            Document doc = new Document();

            try
            {
                PdfWriter.GetInstance(doc, new FileStream(filePath, FileMode.Create));
                doc.Open();

                // Шрифт для кириллицы
                string arialTtf = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Fonts), "arial.ttf");
                BaseFont bfArial = BaseFont.CreateFont(arialTtf, BaseFont.IDENTITY_H, BaseFont.NOT_EMBEDDED);
                Font font = new Font(bfArial, 12, Font.NORMAL);

                doc.Add(new Paragraph("Отчет по сотрудникам", font));
                doc.Add(new Paragraph("Дата: " + DateTime.Now.ToString(), font));
                doc.Add(new Paragraph(" ", font));

                PdfPTable table = new PdfPTable(4);
                table.AddCell(new PdfPCell(new Phrase("ID сотрудника", font)));
                table.AddCell(new PdfPCell(new Phrase("Имя", font)));
                table.AddCell(new PdfPCell(new Phrase("Должность", font)));
                table.AddCell(new PdfPCell(new Phrase("Контактная информация", font)));

                var employees = db.Employee.ToList();

                foreach (var employee in employees)
                {
                    table.AddCell(new PdfPCell(new Phrase(employee.EmployeeID.ToString(), font)));
                    table.AddCell(new PdfPCell(new Phrase(employee.Name, font)));
                    table.AddCell(new PdfPCell(new Phrase(employee.Role, font)));
                    table.AddCell(new PdfPCell(new Phrase(employee.ContactInfo, font)));
                }

                doc.Add(table);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка при создании отчета: " + ex.Message);
            }
            finally
            {
                doc.Close();
            }

            MessageBox.Show("Отчет по сотрудникам успешно сохранен в " + filePath);
        }

        private void Button_GenerateEmployeeReport_Click(object sender, RoutedEventArgs e)
        {
            string filePath = @"D:\колледж\МДК 11.01\EmployeeReport.pdf";
            GenerateEmployeeReportPDF(filePath);
        }

        private void OpenReportWindow_Click(object sender, RoutedEventArgs e)
        {
            ReportWindow reportWindow = new ReportWindow();
            reportWindow.ShowDialog();
        }


        private void ExportToCsv(string filePath)
        {
            var employees = db.Employee.ToList();
            StringBuilder csvcontent = new StringBuilder();

            csvcontent.AppendLine("EmployeeID,Name,Role,ContactInfo");

            foreach (var employee in employees)
            {
                csvcontent.AppendLine($"{employee.EmployeeID},{employee.Name},{employee.Role},{employee.ContactInfo}");
            }

            System.IO.File.WriteAllText(filePath, csvcontent.ToString());
        }

        private void ExportToExcel(string filePath)
        {
            var employees = db.Employee.ToList();
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
            using (var package = new ExcelPackage())
            {
                var worksheet = package.Workbook.Worksheets.Add("Employee");
                worksheet.Cells[1, 1].Value = "EmployeeID";
                worksheet.Cells[1, 2].Value = "Name";
                worksheet.Cells[1, 3].Value = "Role";
                worksheet.Cells[1, 4].Value = "ContactInfo";

                for (int i = 0; i < employees.Count; i++)
                {
                    worksheet.Cells[i + 2, 1].Value = employees[i].EmployeeID;
                    worksheet.Cells[i + 2, 2].Value = employees[i].Name;
                    worksheet.Cells[i + 2, 3].Value = employees[i].Role;
                    worksheet.Cells[i + 2, 4].Value = employees[i].ContactInfo;
                }
                package.SaveAs(new FileInfo(filePath));
            }
        }

        private void ExportToCsv_Click(object sender, EventArgs e)
        {
            var saveFileDialog = new Microsoft.Win32.SaveFileDialog
            {
                FileName = "employees.csv",
                Filter = "CSV files (*.csv)|*.csv"
            };

            if (saveFileDialog.ShowDialog() == true)
            {
                ExportToCsv(saveFileDialog.FileName);
                MessageBox.Show("Данные экспортированы");
            }
        }

        private void ExportToExcel_Click(object sender, EventArgs e)
        {
            var saveFileDialog = new Microsoft.Win32.SaveFileDialog
            {
                FileName = "employees.xlsx",
                Filter = "Excel Files (*.xlsx)|*xlsx"
            };

            if(saveFileDialog.ShowDialog() == true)
            {
                ExportToExcel(saveFileDialog.FileName);
                MessageBox.Show("Данные экспортированы");
            }
        }

        private void MainFrame_Navigated(object sender, System.Windows.Navigation.NavigationEventArgs e)
        {

        }

        private void OpenAddImageWindow(object sender, EventArgs e)
        {
            AddImageWindow addImageWindow = new AddImageWindow();
            addImageWindow.ShowDialog();
        }
    }
}
