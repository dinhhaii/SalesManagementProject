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
using System.Collections.ObjectModel;
using System.Data;
using System.Data.SqlClient;
namespace SalesManagement.ManHinhQuanLy
{
    /// <summary>
    /// Interaction logic for DangKyLichLam.xaml
    /// </summary>
    public partial class DangKyLichLam : Window
    {
        SqlConnection sqlConnection = null;
        ObservableCollection<NhanVien> listNV = new ObservableCollection<NhanVien>();
        public DangKyLichLam()
        {
            InitializeComponent();
            getData();
            DataGridDangKyLichLam.ItemsSource = listNV;
        }
        public void connectSQL(string sql, out SqlConnection sqlConnection)
        {
            //if (sqlConnection == null)
            sqlConnection = new SqlConnection(sql);
            if (sqlConnection.State == ConnectionState.Closed)
                sqlConnection.Open();
        }

        public void getData()
        {
            //Lấy danh sách nhân viên từ csdl
            connectSQL(App.sqlString, out sqlConnection);
            SqlCommand sqlCom = new SqlCommand();
            sqlCom.CommandType = CommandType.Text;
            sqlCom.CommandText = "select * from NhanVien";
            sqlCom.Connection = sqlConnection;
            SqlDataReader sqlReader = sqlCom.ExecuteReader();
            while (sqlReader.Read())
            {
                NhanVien nv = new NhanVien();
                nv.MaNV = sqlReader.GetString(0).Trim();
                nv.TenNV = sqlReader.GetString(1).Trim();
                nv.GioiTinh = sqlReader.GetString(2).Trim();
                nv.SDT = sqlReader.GetString(3).Trim();
                if (sqlReader.IsDBNull(4))
                    nv.Email = sqlReader.GetString(4).Trim();
                else
                    nv.Email = "";
                if (sqlReader.IsDBNull(5))
                    nv.DiaChi = sqlReader.GetString(5).Trim();
                else
                    nv.DiaChi = "";
                nv.Luong = sqlReader.GetFloat(6);
                nv.ViTri = sqlReader.GetString(7).Trim();


                listNV.Add(nv);
            }
            sqlReader.Close();
            sqlConnection.Close();
        }
        private void ThemLichLam_Click(object sender, RoutedEventArgs e)
        {
            ThemLichLam dangKy = new ThemLichLam(listNV[DataGridDangKyLichLam.SelectedIndex].MaNV);
            dangKy.ShowDialog();
            
        }

        private void SuaLichLam_Click(object sender, RoutedEventArgs e)
        {
            SuaLichLam suaLich = new SuaLichLam(listNV[DataGridDangKyLichLam.SelectedIndex].MaNV);
            suaLich.ShowDialog();
        }
    }
}
