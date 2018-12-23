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

namespace SalesManagement.ManHinhBan
{
    /// <summary>
    /// Interaction logic for KhuyenMai.xaml
    /// </summary>
    public partial class KhuyenMai : Window
    {
        public static string KM { get; set; }
        public KhuyenMai()
        {
            InitializeComponent();
        }

        private void combobox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ComboBoxItem temp = combobox.SelectedItem as ComboBoxItem;
            KM = temp.Content.ToString();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
