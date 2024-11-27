using System.Windows.Controls;

namespace BDWPF
{
    public partial class MetallPage : Page
    {
        public MetallPage()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            Manager.MainFrame.Navigate(new Employee());
        }
    }
}
