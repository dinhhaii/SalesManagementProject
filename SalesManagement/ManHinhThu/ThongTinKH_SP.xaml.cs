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
using System.Collections.ObjectModel;
using System.Data.SqlClient;
using System.Data;

namespace SalesManagement.ManHinhThu
{
    /// <summary>
    /// Interaction logic for ChinhSuaSanPham.xaml
    /// </summary>
    public partial class ThongTinKH_SP : Window
    {
        class TTKH_SP
        {
            public string name { get; set; }
            public string phone { get; set; }
            public int quantity { get; set; }
            public string sale { get; set; }
            public double money { get; set; }
            public string date { get; set; }
        }
        public string editMaSP { get; set; }
        public string editDate { get; set; }
        ObservableCollection<SanPham> listSP = new ObservableCollection<SanPham>();
        ObservableCollection<KhachHang> listKH = new ObservableCollection<KhachHang>();
        ObservableCollection<SP_KH> listSP_KH = new ObservableCollection<SP_KH>();
        ObservableCollection<TTKH_SP> listTTKH_SP = new ObservableCollection<TTKH_SP>();

        SqlConnection sqlConnection = null;

        public ThongTinKH_SP(string value, double money)
        {
            InitializeComponent();
            editMaSP = value;
            BindingDuLieu(); // nạp dữ liệu lên giao diện
            ListViewNhap.ItemsSource = listTTKH_SP;

            // Xử lí nạp tên sản phẩm lên giao diện
            int temp = 0;
            for (int i = 0; i < listSP.Count; i++)
            {
                if (editMaSP == listSP[i].MaSP)
                {
                    temp = i;
                    break;
                }
            }
            GroupBoxTenSP.Header = listSP[temp].TenSP;

            TongTien.Text = money.ToString();
        }

        public ThongTinKH_SP(string value,double money, string date)
        {
            InitializeComponent();
            editMaSP = value;
            editDate = date;
            BindingDuLieu(); // nạp dữ liệu lên giao diện
            ListViewNhap.ItemsSource = listTTKH_SP;
            BindingDuLieuTheoNgay();

            // Xử lí nạp tên sản phẩm lên giao diện
            int temp = 0;
            for (int i = 0; i < listSP.Count; i++)
            {
                if (editMaSP == listSP[i].MaSP)
                {
                    temp = i;
                    break;
                }
            }
            GroupBoxTenSP.Header = listSP[temp].TenSP;

            TongTien.Text = money.ToString();
            
        }

        public void BindingDuLieuTheoNgay()
        {
            listTTKH_SP.Clear();
            for (int i=0;i< listSP.Count;i++)
            {
                if (editMaSP == listSP[i].MaSP)
                {
                    for (int j=0;j<listSP_KH.Count;j++)
                    {
                        string date = listSP_KH[j].NgayBan.ToString();
                        string date1 = date.Remove(10);
                        string date2 = date1.Replace('/', '-');
                        if ((listSP[i].MaSP == listSP_KH[j].MaSP) && (date2 == editDate))
                        {
                            for (int k = 0;k<listKH.Count;k++)
                            {
                                if (listSP_KH[j].MaKH == listKH[k].MaKH)
                                {
                                    TTKH_SP ttkhsp = new TTKH_SP();
                                    ttkhsp.name = listKH[k].TenKH;
                                    ttkhsp.phone = listKH[k].SDT;
                                    ttkhsp.quantity = listSP_KH[j].SoLuong;
                                    float temp = listSP_KH[j].KhuyenMai;
                                    ttkhsp.sale = temp.ToString() + '%';
                                    ttkhsp.money = ((listSP[i].Gia * ttkhsp.quantity * (100 - listSP_KH[j].KhuyenMai)) / 100);

                                    listTTKH_SP.Add(ttkhsp);
                                    break;
                                }
                            }
                            
                        }
                        
                    }
                    
                }
                
            }
            ListViewNhap.ItemsSource = null;
            ListViewNhap.ItemsSource = listTTKH_SP;
        }

        public void BindingDuLieu()
        {
            getDataSP();
            getDataSP_KH();
            getDataKH();
            for (int i = 0; i < listSP.Count; i++)
            {
                if (editMaSP == listSP[i].MaSP)
                {
                    for (int j = 0; j < listSP_KH.Count; j++)
                    {
                        if (listSP[i].MaSP == listSP_KH[j].MaSP)
                        {
                            for (int k = 0; k < listKH.Count; k++)
                            {
                                if (listSP_KH[j].MaKH == listKH[k].MaKH)
                                {
                                    TTKH_SP ttkhsp = new TTKH_SP();
                                    ttkhsp.name = listKH[k].TenKH;
                                    ttkhsp.phone = listKH[k].SDT;
                                    ttkhsp.quantity = listSP_KH[j].SoLuong;
                                    float temp = listSP_KH[j].KhuyenMai;
                                    ttkhsp.sale = temp.ToString() + '%';
                                    ttkhsp.money = ((listSP[i].Gia * ttkhsp.quantity * (100 - listSP_KH[j].KhuyenMai)) / 100);
                                    listTTKH_SP.Add(ttkhsp);
                                    break;
                                }
                            }

                        }

                    }

                }

            }
        }

        public void connectSQL(string sql, out SqlConnection sqlConnection)
        {
            //if (sqlConnection == null)
            sqlConnection = new SqlConnection(sql);
            if (sqlConnection.State == ConnectionState.Closed)
                sqlConnection.Open();
        }

        public void getDataSP_KH()
        {
            connectSQL(App.sqlString, out sqlConnection);
            SqlCommand sqlCom = new SqlCommand();
            sqlCom.CommandType = CommandType.Text;
            sqlCom.CommandText = "select * from SP_KH";
            sqlCom.Connection = sqlConnection;
            SqlDataReader sqlReader = sqlCom.ExecuteReader();
            while (sqlReader.Read())
            {
                SP_KH spkh = new SP_KH();
                spkh.MaSP = sqlReader.GetString(0).Trim();
                spkh.MaKH = sqlReader.GetString(1).Trim();
                spkh.SoLuong = sqlReader.GetInt32(4);
                spkh.KhuyenMai = sqlReader.GetFloat(5);
                spkh.NgayBan = sqlReader.GetDateTime(3);
                listSP_KH.Add(spkh);
            }
            sqlReader.Close();
            sqlConnection.Close();
        }

        public void getDataKH()
        {
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
                kh.SDT = sqlReader.GetString(3).Trim();
                if (kh != null)
                {
                    listKH.Add(kh);
                }
            }
            sqlReader.Close();
            sqlConnection.Close();
        }

        public void getDataSP()
        {
            //Lấy danh sách sản phẩm từ csdl
            connectSQL(App.sqlString, out sqlConnection);
            SqlCommand sqlCom = new SqlCommand();
            sqlCom.CommandType = CommandType.Text;
            sqlCom.CommandText = "select * from SanPham";
            sqlCom.Connection = sqlConnection;
            SqlDataReader sqlReader = sqlCom.ExecuteReader();
            while (sqlReader.Read())
            {
                
                SanPham sp = new SanPham();
                sp.MaSP = sqlReader.GetString(0).Trim();
                sp.TenSP = sqlReader.GetString(1).Trim();
                sp.Gia = sqlReader.GetFloat(5);
                if (sp != null)
                {
                    listSP.Add(sp);
                }
            }

            sqlReader.Close();
            sqlConnection.Close();
        }
    }
}
       