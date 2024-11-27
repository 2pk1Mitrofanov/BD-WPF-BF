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


        private void Button_Cancel(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
