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
    /// Interaction logic for ChinhSuaKhachHang.xaml
    /// </summary>
    public partial class ChinhSuaKhachHang : Window
    {
        ObservableCollection<KhachHang> listKH = new ObservableCollection<KhachHang>();
        SqlConnection sqlConnection = null;
        public string editMaKH { get; set; }

        public ChinhSuaKhachHang(string makh)
        {
            InitializeComponent();
            editMaKH = makh;
            updateData();
        }

        //Connect to SQL Server
        public void connectSQL(string sql, out SqlConnection sqlConnection)
        {
            //if (sqlConnection == null)
            sqlConnection = new SqlConnection(sql);
            if (sqlConnection.State == ConnectionState.Closed)
                sqlConnection.Open();
        }

        public void getKH()
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

        public void updateData()
        {
            getKH();
            for (int i = 0; i < listKH.Count; i++)
            {
                if (listKH[i].MaKH == editMaKH)
                {
                    txtMaKH.Text = listKH[i].MaKH;
                    txtTenKH.Text = listKH[i].TenKH;
                    txtSDT.Text = listKH[i].SDT;
                    if (listKH[i].GioiTinh.Trim() == "Nam")
                    {
                        cbboxGioiTinh.SelectedIndex = 0;
                    }
                    else
                    {
                        cbboxGioiTinh.SelectedIndex = 1;
                    }
                    txtEmail.Text = listKH[i].Email;
                    txtDiaChi.Text = listKH[i].DiaChi;
                    break;
                }
            }
        }

        private void btnUpdate_Click(object sender, RoutedEventArgs e)
        {
            KhachHang sp = new KhachHang();
            SqlCommand sqlCmd = new SqlCommand();

            //Chinh sua kh trong sp_kh va kh
            try
            {
                //Kết nối đến CSDL
                connectSQL(App.sqlString, out sqlConnection);
                sqlCmd.CommandType = CommandType.Text;
                string sql = "update KhachHang set MaKH=@MaKH ,TenKH=@TenKH, SDT=@SDT, Email=@Email, DiaChi=@DiaChi where MaKH=" + "'" + txtMaKH.Text.Trim() + "'";
                sqlCmd.CommandText = sql;
                sqlCmd.Connection = sqlConnection;
                sqlCmd.Parameters.Add("@MaKH", SqlDbType.NChar).Value = txtMaKH.Text.Trim();
                sqlCmd.Parameters.Add("@TenKH", SqlDbType.NVarChar).Value = txtTenKH.Text.Trim();
                sqlCmd.Parameters.Add("@SDT", SqlDbType.NChar).Value = txtSDT.Text;
                if (txtEmail.Text != "")
                    sqlCmd.Parameters.Add("@Email", SqlDbType.NChar).Value = txtEmail.Text.Trim();
                else
                    sqlCmd.Parameters.Add("@Email", SqlDbType.NChar).Value = "";
                if (txtDiaChi.Text != "")
                    sqlCmd.Parameters.Add("@DiaChi", SqlDbType.NVarChar).Value = txtDiaChi.Text.Trim();
                else
                    sqlCmd.Parameters.Add("@DiaChi", SqlDbType.NVarChar).Value = "";

                int ret = sqlCmd.ExecuteNonQuery();
                if (ret > 0)
                {
                    MessageBox.Show("Cập nhật thành công");
                    sqlCmd.Cancel();
                }

            }
            catch (Exception)
            {
                MessageBox.Show("Cập nhật dữ liệu không thành công!", "Sales Management", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            finally
            {
                sqlCmd.Cancel();
                if (sqlConnection.State == ConnectionState.Open)
                    sqlConnection.Close();
            }

        }

        //Thoát khỏi cửa sổ Cập nhật sản phẩm
        private void btnExit_Click(object sender, RoutedEventArgs e)
        {
            this.Close();

        }

    }
}
