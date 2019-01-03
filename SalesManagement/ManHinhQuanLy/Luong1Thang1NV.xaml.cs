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
    /// Interaction logic for Luong1Thang1NV.xaml
    /// </summary>
    public partial class Luong1Thang1NV : Window
    {
        SqlConnection sqlConnection = null;
        ObservableCollection<NhanVien> listNV = new ObservableCollection<NhanVien>();
        ObservableCollection<LichLam> listLichLam = new ObservableCollection<LichLam>();
        public string MaNVLuong { get; set; }
        public double LuongOfNV { get; set; }
        public Luong1Thang1NV(string value,double value2)
        {
            MaNVLuong = value;
            LuongOfNV = value2;
            InitializeComponent();
            getData();
            getLichLam();
            for(int i=0;i<listNV.Count;i++)
            {
                if(value==listNV[i].MaNV)
                {
                    txtTenNV.Text = listNV[i].TenNV;
                }
            }
            var soCaLamTrongThang = 0;
            DateTime day = new DateTime (DateTime.Now.Year, DateTime.Now.Month,1);
            
            for(int i=0;i<listLichLam.Count;i++)
            {
                if(listLichLam[i].MaNV==MaNVLuong)// Lấy nhân viên đó
                {
                    if(listLichLam[i].NgayLam>=day&&listLichLam[i].CoMat==true)//Chỉ xét những ngày từ ngày 1 tháng hiện tại tới hiện tại
                    {
                        soCaLamTrongThang++;
                    }
                }
            }
            txtLuong.Text = soCaLamTrongThang * LuongOfNV + "";
        }

        public void connectSQL(string sql, out SqlConnection sqlConnection)
        {
            //if (sqlConnection == null)
            sqlConnection = new SqlConnection(sql);
            if (sqlConnection.State == ConnectionState.Closed)
                sqlConnection.Open();
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
    }
}
