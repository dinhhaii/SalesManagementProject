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
    /// Interaction logic for ThanhToan.xaml
    /// </summary>
    public partial class ThanhToan : Window
    {
        public string thanhtien { get; set; }
        public static bool thanhtoanthanhcong { get; set; }

        public ThanhToan(string value)
        {
            InitializeComponent();
            thanhtien = value;
            thanhtoanthanhcong = false;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            double tt;
            double khachdua;

            if(double.TryParse(thanhtien.Substring(0,thanhtien.Length-1), out tt))
            {
                tt = double.Parse(thanhtien.Substring(0, thanhtien.Length - 1), System.Globalization.CultureInfo.InvariantCulture);
            }

            if (double.TryParse(txtbox.Text, out khachdua))
            {
                khachdua = double.Parse(txtbox.Text, System.Globalization.CultureInfo.InvariantCulture);
            }
            
            if(khachdua < tt)
            {
                MessageBox.Show("Số tiền khách đưa thấp hơn tổng tiền hóa đơn!", "Sales Management", MessageBoxButton.OK, MessageBoxImage.Error);
                thanhtoanthanhcong = false;
            }
            else
            {
                double tienthua = khachdua - tt;
                MessageBoxResult result = MessageBox.Show("Bạn chắc chắn muốn thanh toán?", "Sales Management", MessageBoxButton.YesNo, MessageBoxImage.Question);
                if (result == MessageBoxResult.Yes)
                {
                    MessageBox.Show("Tiền trả lại khách: " + tienthua.ToString() + " VNĐ", "Sales Management", MessageBoxButton.OK, MessageBoxImage.Information);
                    thanhtoanthanhcong = true;
                    this.Close();
                }
                else
                {
                    this.Close();
                }
            }

        }

        private void txtbox_KeyDown(object sender, KeyEventArgs e)
        {
            if(e.Key == Key.Enter)
            {
                Button_Click(sender,e);
            }
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
