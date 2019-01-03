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
using System.Data;
using System.Data.SqlClient;
using System.Collections.ObjectModel;
using Microsoft.Win32;
using System.Text.RegularExpressions;
namespace SalesManagement.ManHinhQuanLy
{
    /// <summary>
    /// Interaction logic for QuanLyLuong.xaml
    /// </summary>
    public partial class QuanLyLuong : Window
    {
        ObservableCollection<LichLam> listLichLam = new ObservableCollection<LichLam>();
        ObservableCollection<NhanVien> listNV = new ObservableCollection<NhanVien>();
        SqlConnection sqlConnection = null;
        public QuanLyLuong( )
        {
           
            InitializeComponent();
            getData();
            DataGridLuong.ItemsSource = listNV;
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

public void getLichLam()
        {
            //Lấy danh sách lịch làm từ csdl
            connectSQL(App.sqlString, out sqlConnection);
            SqlCommand sqlCom = new SqlCommand();
            sqlCom.CommandType = CommandType.Text;
            sqlCom.CommandText = "select * from LichLam";
            sqlCom.Connection = sqlConnection;
            SqlDataReader sqlReader = sqlCom.ExecuteReader();
            while (sqlReader.Read())
            {
                LichLam ll = new LichLam();
                ll.MaNV = sqlReader.GetString(0).Trim();
                ll.NgayLam = sqlReader.GetDateTime(1);
                ll.Ca = sqlReader.GetString(2).Trim();
                if (!sqlReader.IsDBNull(3))
                {
                    ll.isDiemDanh = 1;
                }
                else
                {
                    ll.isDiemDanh = 0;

                }
                if (!sqlReader.IsDBNull(4))
                {
                    ll.LyDo = sqlReader.GetString(4);
                }
                else
                {
                    ll.LyDo = "";
                }
                listLichLam.Add(ll);
            }
            sqlReader.Close();
            sqlConnection.Close();
        }
        private void DataGridCell_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            DataGridCell myCell = sender as DataGridCell;
            DataGridRow row = DataGridRow.GetRowContainingElement(myCell);
            NhanVien temp = row.DataContext as NhanVien;
            Luong1Thang1NV luong = new Luong1Thang1NV(temp.MaNV,temp.Luong);
            luong.ShowDialog();

        }
    }
}
