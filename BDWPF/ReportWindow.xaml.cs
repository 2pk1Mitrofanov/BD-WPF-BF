using System;
using System.Linq;
using System.Windows;
using iTextSharp.text;
using iTextSharp.text.pdf;
using System.IO;

namespace BDWPF
{
    public partial class ReportWindow : Window
    {
        BFEntities1 db;

        public ReportWindow()
        {
            InitializeComponent();
            db = new BFEntities1();
        }

        private void GenerateReport_Click(object sender, RoutedEventArgs e)
        {
            DateTime startDate = dpStartDate.SelectedDate.HasValue ? dpStartDate.SelectedDate.Value : DateTime.MinValue;
            DateTime endDate = dpEndDate.SelectedDate.HasValue ? dpEndDate.SelectedDate.Value : DateTime.MaxValue;

            var reportData = db.Donation
                .Where(t => t.Date >= startDate && t.Date <= endDate)
                .Select(t => new
                {
                    DonorName = t.Donor.Name,
                    t.Date,
                    t.Amount
                }).ToList();

            dgReport.ItemsSource = reportData;
        }

        private void ExportReportToPDF_Click(object sender, RoutedEventArgs e)
        {
            string filePath = @"D:\колледж\МДК 11.01\DonationReport.pdf";
            ExportToPDF(filePath);
        }

        private void ExportToPDF(string filePath)
        {
            Document doc = new Document();

            try
            {
                PdfWriter.GetInstance(doc, new FileStream(filePath, FileMode.Create));

                doc.Open();

                string arialTtf = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Fonts), "arial.ttf");
                BaseFont bfArial = BaseFont.CreateFont(arialTtf, BaseFont.IDENTITY_H, BaseFont.NOT_EMBEDDED);
                Font font = new Font(bfArial, 12, Font.NORMAL);

                doc.Add(new Paragraph("Отчет по пожертвованиям", font));
                doc.Add(new Paragraph("Дата отчета: " + DateTime.Now.ToString(), font));
                doc.Add(new Paragraph(" ", font));

                PdfPTable table = new PdfPTable(4);
                table.AddCell(new PdfPCell(new Phrase("Сотрудник", font)));
                table.AddCell(new PdfPCell(new Phrase("Донор", font)));
                table.AddCell(new PdfPCell(new Phrase("Дата пожертвования", font)));
                table.AddCell(new PdfPCell(new Phrase("Сумма", font)));

                var reportData = dgReport.ItemsSource as dynamic;
                foreach (var item in reportData)
                {
                    table.AddCell(new PdfPCell(new Phrase(item.EmployeeName, font)));
                    table.AddCell(new PdfPCell(new Phrase(item.DonorName, font)));
                    table.AddCell(new PdfPCell(new Phrase(item.DonationDate.ToString("dd.MM.yyyy"), font)));
                    table.AddCell(new PdfPCell(new Phrase(item.Amount.ToString(), font)));
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

            MessageBox.Show("Отчет успешно сохранен в " + filePath);
        }
    }
}
