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
using System.ComponentModel;
using SalesManagement;

namespace SalesManagement.ManHinhThongKe
{
    /// <summary>
    /// Interaction logic for ThongKe.xaml
    /// </summary>
    public partial class ThongKe : UserControl
    {
        List<SanPham> listSP = new List<SanPham>();
        List<SP_KH> listSPKH = new List<SP_KH>();
        SqlConnection sqlConnection = null;
        List<KeyValuePair<string, int>> valueList = new List<KeyValuePair<string, int>>();
        public ThongKe()
        {
            InitializeComponent();
            getSP();
            getSP_KH();
            for (int i = 1; i <= 12; i++)
            {
                cbbox.Items.Add(i);
            }

            for (int i = 2016; i <= 2050; i++)
            {
                cbboxnam.Items.Add(i);
            }

            doanhthungay.Text = DateTime.Now.ToString();

            slton.Text = SLSPTon();
            slban.Text = SLSPBan();
            sldoitra.Text = SLSPDoiTra();
            doanhthu.Text = TongDoanhThu();
            
            for(int i=0;i<listSP.Count;i++)
            {
                int SL = 0;
                for(int j=0;j<listSPKH.Count;j++)
                {
                    if(listSP[i].MaSP==listSPKH[j].MaSP)
                    {
                        SL = listSPKH[j].SoLuong + SL;
                    }
                }
                valueList.Add(new KeyValuePair<string, int>(listSP[i].TenSP, SL));
            }
        
        
            //Setting data for column chart
            columnChart.DataContext = valueList;
            //sltonthang.Text = SLSPTonThang();
            //slbanthang.Text = SLSPBanThang();
            //sldoitrathang.Text = SLSPDoiTraThang();
            //doanhthuthang.Text = TongDoanhThuThang();
        }

        //THEO NGÀY ------
        private string SLSPTon()
        {
            int sl = 0;
            for (int i = 0; i < listSP.Count; i++)
            {
                if (doanhthungay.ToString() == listSP[i].NgayNhap.Date.ToString())
                {
                    sl += listSP[i].SoLuong;
                }
            }
            return sl.ToString();
        }

        private string SLSPBan()
        {
            int sl = 0;
            for (int i = 0; i < listSPKH.Count; i++)
            {
                if (listSPKH[i].NgayBan.Date.ToString() == doanhthungay.ToString())
                {
                    sl += listSPKH[i].SoLuong;
                }
            }
            return sl.ToString();
        }

        private string SLSPDoiTra()
        {
            int sl = 0;
            for (int i = 0; i < listSP.Count; i++)
            {
                if (!(listSP[i].DoiTra == null || listSP[i].DoiTra.Trim() == "") && (doanhthungay.ToString() == listSP[i].NgayNhap.Date.ToString()))
                {
                    sl += listSP[i].SoLuong;
                }
            }
            return sl.ToString();
        }

        private string TongDoanhThu()
        {
            double tong = 0;
            for (int i = 0; i < listSPKH.Count; i++)
            {
                if (listSPKH[i].NgayBan.Date.ToString() == doanhthungay.ToString())
                {
                    double gia = 0;
                    foreach (SanPham item in listSP)
                    {
                        if (item.MaSP == listSPKH[i].MaSP)
                        {
                            gia = item.Gia;
                            break;
                        }
                    }
                    tong += gia * (100f - listSPKH[i].KhuyenMai)/100 * listSPKH[i].SoLuong;
                }
            }

            return tong.ToString();
        }

        //THEO THÁNG ------------------------
        private string SLSPTonThang()
        {
            int sl = 0;
            for (int i = 0; i < listSP.Count; i++)
            {
                if ((int)cbbox.SelectedItem == listSP[i].NgayNhap.Month && (int)cbboxnam.SelectedItem == listSP[i].NgayNhap.Year)
                {
                    sl += listSP[i].SoLuong;
                }
            }
            return sl.ToString();
        }

        private string SLSPBanThang()
        {
            int sl = 0;
            for (int i = 0; i < listSPKH.Count; i++)
            {
                if ((int)cbbox.SelectedItem == listSPKH[i].NgayBan.Month && (int)cbboxnam.SelectedItem == listSPKH[i].NgayBan.Year)
                {
                    sl += listSPKH[i].SoLuong;
                }
            }
            return sl.ToString();
        }

        private string SLSPDoiTraThang()
        {
            int sl = 0;
            for (int i = 0; i < listSP.Count; i++)
            {
                if (!(listSP[i].DoiTra == null || listSP[i].DoiTra.Trim() == "") && ((int)cbbox.SelectedItem == listSP[i].NgayNhap.Month && (int)cbboxnam.SelectedItem == listSP[i].NgayNhap.Year))
                {
                    sl += listSP[i].SoLuong;
                }
            }
            return sl.ToString();
        }

        private string TongDoanhThuThang()
        {
            double tong = 0;
            for (int i = 0; i < listSPKH.Count; i++)
            {
                if ((int)cbbox.SelectedItem == listSPKH[i].NgayBan.Month && (int)cbboxnam.SelectedItem == listSPKH[i].NgayBan.Year)
                {
                    double gia = 0;
                    foreach (SanPham item in listSP)
                    {
                        if (item.MaSP == listSPKH[i].MaSP)
                        {
                            gia = item.Gia;
                            break;
                        }
                    }
                    tong += gia * (100f - listSPKH[i].KhuyenMai)/100 * listSPKH[i].SoLuong;
                }
            }

            return tong.ToString();
        }

        //Connect to SQL Server
        public void connectSQL(string sql, out SqlConnection sqlConnection)
        {
            //if (sqlConnection == null)
            sqlConnection = new SqlConnection(sql);
            if (sqlConnection.State == ConnectionState.Closed)
                sqlConnection.Open();
        }

        public void getSP()
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
                sp.HinhAnhSP = sqlReader.GetString(2).Trim();
                sp.Size = sqlReader.GetString(3).Trim();
                sp.SoLuong = sqlReader.GetInt32(4);
                sp.Gia = sqlReader.GetFloat(5);
                sp.NgayNhap = sqlReader.GetDateTime(6);

                if (!sqlReader.IsDBNull(7))
                {
                    sp.DoiTra = sqlReader.GetString(7);
                }
                else
                    sp.DoiTra = "";
                if (sp != null)
                {
                    listSP.Add(sp);
                }
            }
            sqlReader.Close();
            sqlConnection.Close();
        }

        public void getSP_KH()
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
                spkh.STT = sqlReader.GetString(2).Trim();
                spkh.NgayBan = sqlReader.GetDateTime(3);
                spkh.SoLuong = sqlReader.GetInt32(4);
                spkh.KhuyenMai = sqlReader.GetFloat(5);

                if (spkh != null)
                {
                    listSPKH.Add(spkh);
                }
            }
            sqlReader.Close();
            sqlConnection.Close();
        }



        private void doanhthungay_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            btnflipper1.Content = "THỐNG KÊ THEO NGÀY (" + doanhthungay.ToString().Remove(10).Trim() + ")";
            txtblngay.Text = "THỐNG KÊ THEO NGÀY (" + doanhthungay.ToString().Remove(10).Trim() + ")";

            slton.Text = SLSPTon();
            slban.Text = SLSPBan();
            sldoitra.Text = SLSPDoiTra();
            doanhthu.Text = TongDoanhThu();

        } 

        private void cbbox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            btnflipperthang.Content = "THỐNG KÊ THEO THÁNG (" + cbbox.SelectedItem + "/" + cbboxnam.SelectedItem + ")";
            txtblthang.Text = "THỐNG KÊ THEO THÁNG (" + cbbox.SelectedItem + "/" + cbboxnam.SelectedItem + ")";
            txtboxnam.Visibility = Visibility.Visible;
            cbboxnam.Visibility = Visibility.Visible;
            if (cbboxnam.SelectedItem != null)
            {
                sltonthang.Text = SLSPTonThang();
                slbanthang.Text = SLSPBanThang();
                sldoitrathang.Text = SLSPDoiTraThang();
                doanhthuthang.Text = TongDoanhThuThang();
            }
        }

        private void cbboxnam_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            btnflipperthang.Content = "THỐNG KÊ THEO THÁNG (" + cbbox.SelectedItem + "/" + cbboxnam.SelectedItem + ")";
            txtblthang.Text = "THỐNG KÊ THEO THÁNG (" + cbbox.SelectedItem + "/" + cbboxnam.SelectedItem + ")";
            sltonthang.Text = SLSPTonThang();
            slbanthang.Text = SLSPBanThang();
            sldoitrathang.Text = SLSPDoiTraThang();
            doanhthuthang.Text = TongDoanhThuThang();
        }

    }
}
