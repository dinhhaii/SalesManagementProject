using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
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

namespace SalesManagement
{
    /// <summary>
    /// Interaction logic for Home.xaml
    /// </summary>
    public partial class Home : Window
    {
        SqlConnection sqlConnection = null;
        List<NhanVien> listNV = new List<NhanVien>();

        public Home(string MaNV)
        {
            InitializeComponent();
            getData();
            if (App.isEmployee)
            {
                position.Badge = "Nhân viên";
            }
            else
            {
                position.Badge = "Quản lý";
            }

            foreach(NhanVien obj in listNV)
            {
                if(obj.MaNV == MaNV.Trim())
                {
                    userfullname.Content = obj.TenNV;
                    break;
                }
            }
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
            //Lấy danh sách tài khoản từ csdl
            connectSQL(App.sqlString, out sqlConnection);
            SqlCommand sqlCom = new SqlCommand();
            sqlCom.CommandType = CommandType.Text;
            sqlCom.CommandText = "select * from NhanVien";
            sqlCom.Connection = sqlConnection;
            SqlDataReader sqlReader = sqlCom.ExecuteReader();
            while (sqlReader.Read())
            {
                NhanVien nv = new NhanVien();
                if (!sqlReader.IsDBNull(0))
                {
                    nv.MaNV = sqlReader.GetString(0).Trim();
                }
                if (!sqlReader.IsDBNull(1))
                {
                    nv.TenNV = sqlReader.GetString(1).Trim();
                }
                if (!sqlReader.IsDBNull(2))
                {
                    nv.GioiTinh = sqlReader.GetString(2).Trim();
                }
                if (!sqlReader.IsDBNull(3))
                {
                    nv.SDT = sqlReader.GetString(3).Trim();
                }
                if (!sqlReader.IsDBNull(4))
                {
                    nv.Email = sqlReader.GetString(4).Trim();
                }
                if (!sqlReader.IsDBNull(5))
                {
                    nv.DiaChi = sqlReader.GetString(5).Trim();
                }
                if (!sqlReader.IsDBNull(6))
                {
                    nv.Luong = sqlReader.GetFloat(6);
                }
                if (!sqlReader.IsDBNull(7))
                {
                    nv.ViTri = sqlReader.GetString(7).Trim();
                }
                listNV.Add(nv);
            }
            sqlReader.Close();
            sqlConnection.Close();
        }


        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            Application.Current.Shutdown();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            MainWindow window = new MainWindow();
            this.Hide();
            window.ShowDialog();
            this.Close();
        }
    }
}
