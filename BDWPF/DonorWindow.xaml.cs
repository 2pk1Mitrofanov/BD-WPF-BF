using System;
using System.Windows;

namespace BDWPF
{
    public partial class DonorWindow : Window
    {
        private BFEntities1 db; 
        private Donor _donor;

        public DonorWindow(Donor donor)
        {
            InitializeComponent();
            db = new BFEntities1();

            if (donor != null)
            {
                _donor = donor;
                tbName.Text = _donor.Name;
                tbContactInfo.Text = _donor.ContactInfo;
                tbDonationCount.Text = _donor.DonationCount.ToString();
            }
        }

        private void Button_Save(object sender, RoutedEventArgs e)
        {
            if (_donor == null)
            {
                _donor = new Donor
                {
                    Name = tbName.Text,
                    ContactInfo = tbContactInfo.Text,
                    DonationCount = int.Parse(tbDonationCount.Text)
                };
                db.Donor.Add(_donor); 
            }
            else
            {
                _donor.Name = tbName.Text;
                _donor.ContactInfo = tbContactInfo.Text;
                _donor.DonationCount = int.Parse(tbDonationCount.Text);
            }

            db.SaveChanges();
            this.Close(); 
        }

        private void Button_Cancel(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
