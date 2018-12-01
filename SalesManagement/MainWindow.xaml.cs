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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace SalesManagement
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }
        //Xử lý sự kiện khi bấm nút ĐĂNG NHẬP
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (toggleButton.IsChecked == true)
            {
                if (txtbox.Text == "admin" && passwordBox.Password == "123")
                {
                    MessageBox.Show("HELLO Quản lý");
                }
                else
                {
                    txtbox.Text = "";
                    passwordBox.Password = "";
                    MessageBox.Show("Thông tin đăng nhập chưa đúng", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Hand);
                }
            }
            else
            {
                if (txtbox.Text == "admin" && passwordBox.Password == "123")
                {
                    MessageBox.Show("HELLO Nhân viên");
                }
                else
                {
                    txtbox.Text = "";
                    passwordBox.Password = "";
                    MessageBox.Show("Thông tin đăng nhập chưa đúng", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Hand);
                }
            }
        }

        //Đóng chương trình khi tắt một cửa sổ
        private void login_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            Application.Current.Shutdown();
        }

        //Xử lý sự kiện khi bấm phím Enter để đăng nhập
        private void passwordBox_KeyDown(object sender, KeyEventArgs e)
        {
            if(e.Key == Key.Enter)
            {
                Button_Click(sender, e);
            }
        }

        private void txtbox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                Button_Click(sender, e);
            }
        }




        ////Sự kiện khi đưa trỏ chuột vào Textbox
        //private void txtbox_GotMouseCapture(object sender, MouseEventArgs e)
        //{
        //    MaterialDesignThemes.Wpf.HintAssist.SetHint(txtbox, "Username");
        //}
        //private void txtbox_GotKeyboardFocus(object sender, KeyboardFocusChangedEventArgs e)
        //{
        //    MaterialDesignThemes.Wpf.HintAssist.SetHint(txtbox, "Username");
        //}
        //private void passwordBox_GotMouseCapture(object sender, MouseEventArgs e)
        //{
        //    MaterialDesignThemes.Wpf.HintAssist.SetHint(passwordBox, "Password");
        //}
        //private void passwordBox_GotKeyboardFocus(object sender, KeyboardFocusChangedEventArgs e)
        //{
        //    MaterialDesignThemes.Wpf.HintAssist.SetHint(passwordBox, "Password");
        //}
    }
}
