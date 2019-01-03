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
using System.Data.SqlClient;
using System.Data;

namespace SalesManagement
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        SqlConnection sqlConnection = null;
        List<TaiKhoan> listTaiKhoan = new List<TaiKhoan>();
        private string MaNV { get; set; }

        public MainWindow()
        {
            InitializeComponent();
            txtbox.Text = "admin";
            passwordBox.Password = "1";
            toggleButton.IsChecked = true;
        }

        //Connect to SQL Server
        public void connectSQL(string sql, out SqlConnection sqlConnection)
        {
            //if (sqlConnection == null)
            sqlConnection = new SqlConnection(sql);
            if (sqlConnection.State == ConnectionState.Closed)
                sqlConnection.Open();
        }

        //Lấy danh sách tài khoản từ CSDL
        public void getData()
        {
            //Lấy danh sách tài khoản từ csdl
            connectSQL(App.sqlString, out sqlConnection);
            SqlCommand sqlCom = new SqlCommand();
            sqlCom.CommandType = CommandType.Text;
            sqlCom.CommandText = "select * from TaiKhoan";
            sqlCom.Connection = sqlConnection;
            SqlDataReader sqlReader = sqlCom.ExecuteReader();
            while (sqlReader.Read())
            {
                TaiKhoan tk = new TaiKhoan();
                tk.MaNV = sqlReader.GetString(0);
                tk.TenTaiKhoan = sqlReader.GetString(1);
                tk.MatKhau = sqlReader.GetString(2);
                listTaiKhoan.Add(tk);
            }
            sqlReader.Close();
            sqlConnection.Close();
        }

        //Xử lý sự kiện khi bấm nút ĐĂNG NHẬP
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            //Kết nối đến SQL
            connectSQL(App.sqlString, out sqlConnection);
            //Lấy dữ liệu tài khoản từ CSDL
            getData();

            int i;
            //Duyệt qua danh sách các tài khoản đang tồn tại trong CSDL
            for (i = 0; i < listTaiKhoan.Count; i++)
            {
                //Lấy ra 2 kí tự đầu tiên trong NV
                string temp = listTaiKhoan[i].MaNV.Substring(0, 2);
                if (txtbox.Text == listTaiKhoan[i].TenTaiKhoan.Trim()) //Nếu tên tài khoản tồn tại 
                {                    
                    if (toggleButton.IsChecked == true && temp == "QL") //Nếu đăng nhập là QUẢN LÝ và MaNV có 2 kí tự đầu là QL
                    {
                        //Kiểm tra mật khẩu 
                        if (passwordBox.Password == listTaiKhoan[i].MatKhau.Trim()) 
                        {
                            App.isEmployee = false;
                            MaNV = listTaiKhoan[i].MaNV;
                            showHomeWindow();
                        }
                        else
                        { 
                            passwordBox.Password = "";
                            MessageBox.Show("Mật khẩu chưa đúng. Vui lòng nhập lại!", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Hand);
                        }
                        break;
                    }
                    else if(toggleButton.IsChecked == false && temp == "NV") //Nếu đăng nhập là NHÂN VIÊN và MaNV có 2 kí tự đầu là NV
                    {
                        if (passwordBox.Password == listTaiKhoan[i].MatKhau.Trim())
                        {
                            App.isEmployee = true;
                            MaNV = listTaiKhoan[i].MaNV;
                            showHomeWindow();
                        }
                        else
                        {
                            passwordBox.Password = "";
                            MessageBox.Show("Mật khẩu chưa đúng. Vui lòng nhập lại!", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Hand);
                        }
                        break;
                    }
                    else
                    {
                        passwordBox.Password = "";
                        MessageBox.Show("Mật khẩu chưa đúng. Vui lòng nhập lại!", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Hand);
                        break;
                    }
                }
            }

            //Kiểm tra tài khoản nếu không tồn tại trong danh sách
            if(i == listTaiKhoan.Count)
            {
                txtbox.Text = "";
                passwordBox.Password = "";
                MessageBox.Show("Không tồn tại tên đăng nhập, xin vui lòng kiểm tra lại!", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Hand);
            }

            sqlConnection.Close();
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

        public void showHomeWindow()
        {
            Home window = new Home(MaNV);
            loginwindow.Hide();
            window.Show();
            
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
