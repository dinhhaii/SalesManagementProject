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
using SalesManagement;
using System.Data.SqlClient;
using System.Data;
using System.Collections.ObjectModel;
using Microsoft.Win32;
using System.Text.RegularExpressions;

namespace SalesManagement.ManHinhBan
{
    /// <summary>
    /// Interaction logic for ThemSanPham.xaml
    /// </summary>
    public partial class ThemKhachHang : Window
    {
        ObservableCollection<KhachHang> listKH = new ObservableCollection<KhachHang>();
        SqlConnection sqlConnection = null;

        public ThemKhachHang()
        {
            InitializeComponent();
            //getData();

        }

        //Connect to SQL Server
        public void connectSQL(string sql, out SqlConnection sqlConnection)
        {
            //if (sqlConnection == null)
            sqlConnection = new SqlConnection(sql);
            if (sqlConnection.State == ConnectionState.Closed)
                sqlConnection.Open();
        }

        public void getData()
        {
            //Lấy danh sách khách hàng từ csdl
            connectSQL(App.sqlString, out sqlConnection);
            SqlCommand sqlCom = new SqlCommand();
            sqlCom.CommandType = CommandType.Text;
            sqlCom.CommandText = "select * from KhachHang";
            sqlCom.Connection = sqlConnection;
            SqlDataReader sqlReader = sqlCom.ExecuteReader();
            while (sqlReader.Read())
            {
                KhachHang kh = new KhachHang();
                kh.MaKH = sqlReader.GetString(0).Trim();
                kh.TenKH = sqlReader.GetString(1).Trim();
                kh.GioiTinh = sqlReader.GetString(2).Trim();
                kh.SDT = sqlReader.GetString(3).Trim();
                kh.Email = sqlReader.GetString(4).Trim();
                kh.DiaChi = sqlReader.GetString(5).Trim();

                if (kh != null)
                {
                    listKH.Add(kh);
                }
            }
            sqlReader.Close();
            sqlConnection.Close();
        }

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            getData();
            bool input = false;
            bool duplicate = false;
            //Kiểm tra xem có trùng mã khách hàng không
            for (int i = 0; i < listKH.Count; i++)
            {
                if (listKH[i].MaKH.Trim().ToLower() == txtMaKH.Text.ToLower())
                {
                    duplicate = true;
                    break;
                }

            }
            KhachHang KH = new KhachHang();
            SqlCommand sqlCommand = new SqlCommand();

            if (duplicate == false)
            {
                try
                {
                    //Kết nối tới CSDL
                    connectSQL(App.sqlString, out sqlConnection);

                    sqlCommand.CommandType = CommandType.Text;
                    //tIỀN HÀNH THÊM DỮ LIỆU VÀO SQL
                    string sql = "insert into KhachHang(MaKH,TenKH,GioiTinh,SDT,Email,DiaChi) values(@MaKH,@TenKH,@GioiTinh,@SDT,@Email,@DiaChi)";
                    sqlCommand.CommandText = sql;
                    sqlCommand.Connection = sqlConnection;
                    sqlCommand.Parameters.Add("@MaKH", SqlDbType.NChar).Value = txtMaKH.Text.Trim();
                    sqlCommand.Parameters.Add("@TenKH", SqlDbType.NVarChar).Value = txtTenKH.Text.Trim();
                    ComboBoxItem temp = cbboxGioiTinh.SelectedItem as ComboBoxItem;
                    sqlCommand.Parameters.Add("@GioiTinh", SqlDbType.NVarChar).Value = temp.Content.ToString().Trim();
                    sqlCommand.Parameters.Add("@SDT", SqlDbType.NChar).Value = txtSDT.Text.Trim();
                    sqlCommand.Parameters.Add("@Email", SqlDbType.NChar).Value = txtEmail.Text.Trim();
                    sqlCommand.Parameters.Add("@DiaChi", SqlDbType.NVarChar).Value = txtDiaChi.Text.Trim();
                    input = true;
                }
                catch (Exception)
                {
                    //NẾU NHẬP KHÔNG ĐÚNG BÁO LỖI
                    MessageBox.Show("Thông tin nhập chưa đúng hoặc còn thiếu!!");
                    input = false;
                }
                //Nếu nhập đúng
                if (input == true)
                {
                    int ret = sqlCommand.ExecuteNonQuery();
                    if (ret > 0)
                    {
                        MessageBox.Show("Thêm thành công");
                        txtMaKH.Text = "";
                        txtTenKH.Text = "";
                        txtSDT.Text = "";
                        txtEmail.Text = "";
                        txtDiaChi.Text = "";
                        if (sqlConnection.State == ConnectionState.Open)
                            sqlConnection.Close();
                        sqlCommand.Cancel();
                    }
                    else
                    {
                        MessageBox.Show("Thêm không thành công!!");
                    }
                }

            }
            else
            {
                MessageBox.Show("Thông tin nhập thiếu hoặc chưa đúng. Vui lòng nhập lại!", "Sales Management", MessageBoxButton.OK, MessageBoxImage.Error);
            }


        }

        private void btnExit_Click(object sender, RoutedEventArgs e)
        {
            this.Close();

        }
    }
}
