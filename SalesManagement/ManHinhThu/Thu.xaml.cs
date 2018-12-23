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
    /// Interaction logic for Nhap.xaml
    /// </summary>
    public partial class Thu : UserControl
    {
        class SanPhamTheoNgay
        {
            public string MaSP1 { get; set; }
            public string TenSP1 { get; set; }
            public string Size1 { get; set; }
            public int SoLuong1 { get; set; }
            public double Gia1 { get; set; }
            public double ThanhTien1 { get; set; }
            public string NgayBan { get; set; }
        }
        
        SqlConnection sqlConnection = null;
        ObservableCollection<SanPham> listSP = new ObservableCollection<SanPham>();
        ObservableCollection<SanPham> listSPSearch = new ObservableCollection<SanPham>();
        ObservableCollection<SP_KH> listSP_KH = new ObservableCollection<SP_KH>();
        ObservableCollection<SanPhamTheoNgay> listSPTN = new ObservableCollection<SanPhamTheoNgay>();


        public Thu()
        {
            InitializeComponent();
            getDataSP_KH();
            getData();
            chuyenDuLieu();
            DataGridNhap.ItemsSource = listSPTN;
            tongTien();
        }

        //Connect to SQL Server
        public void connectSQL(string sql, out SqlConnection sqlConnection)
        {
            //if (sqlConnection == null)
            sqlConnection = new SqlConnection(sql);
            if (sqlConnection.State == ConnectionState.Closed)
                sqlConnection.Open();
        }

        public void chuyenDuLieu()
        {
            for (int j=0;j<listSP.Count;j++)
            {
                SanPhamTheoNgay sptn = new SanPhamTheoNgay();
                sptn.MaSP1 = listSP[j].MaSP;
                sptn.TenSP1 = listSP[j].TenSP;
                sptn.Size1 = listSP[j].Size;
                sptn.Gia1 = listSP[j].Gia;
                sptn.SoLuong1 = 0;
                sptn.ThanhTien1 = 0;
                for (int z = 0; z < listSP_KH.Count; z++)
                {
                    if (listSP_KH[z].MaSP == listSP[j].MaSP)
                    {
                        sptn.ThanhTien1 += ((sptn.Gia1*listSP_KH[z].SoLuong * (100 - listSP_KH[z].KhuyenMai) / 100));
                    }
                }
                for (int z = 0; z < listSP_KH.Count; z++)
                {
                    if (listSP_KH[z].MaSP == listSP[j].MaSP)
                    {
                        sptn.SoLuong1 += listSP_KH[z].SoLuong;
                    }
                }
                listSPTN.Add(sptn);
            }
        }

        public void BindingTheoNgay()
        {
            listSPTN.Clear();
            for (int i=0;i<listSP_KH.Count;i++)
            {
                if (dpick.ToString() == listSP_KH[i].NgayBan.Date.ToString())
                {
                    for (int j=0;j<listSP.Count;j++)
                    {
                        if (listSP_KH[i].MaSP == listSP[j].MaSP)
                        {
                            SanPhamTheoNgay sptn = new SanPhamTheoNgay();
                            sptn.MaSP1 = listSP[j].MaSP;
                            sptn.TenSP1 = listSP[j].TenSP;
                            sptn.Size1 = listSP[j].Size;
                            sptn.SoLuong1 = listSP_KH[i].SoLuong;
                            sptn.Gia1 = listSP[j].Gia;
                            sptn.ThanhTien1 = (sptn.Gia1 * sptn.SoLuong1 * (100 - listSP_KH[i].KhuyenMai) / 100);
                            sptn.NgayBan = listSP_KH[i].NgayBan.Date.ToString().Remove(10);
                            if (sptn != null)
                            {
                                listSPTN.Add(sptn);
                            }
                            break;
                        }
                    }
                }
            }
            
            DataGridNhap.ItemsSource = null;
            tongTien();
            DataGridNhap.ItemsSource = listSPTN;

        }

        public void tongTien()
        {
            double S = 0;
            for (int i=0;i<listSPTN.Count;i++)
            {
                S += listSPTN[i].ThanhTien1;
            }
            TongTien.ActionContent = S.ToString();
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
                spkh.SoLuong = sqlReader.GetInt32(4);
                spkh.NgayBan = sqlReader.GetDateTime(3);
                spkh.KhuyenMai = sqlReader.GetFloat(5);
                listSP_KH.Add(spkh);
            }
            sqlReader.Close();
            sqlConnection.Close();
        }

        public void getData()
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
                sp.Size = sqlReader.GetString(3).Trim();
                sp.Gia = sqlReader.GetFloat(5);
                for (int i = 0; i < listSP_KH.Count; i++)
                {
                    if (sp.MaSP == listSP_KH[i].MaSP)
                    {
                        sp.SoLuong = listSP_KH[i].SoLuong;
                    }
                }
                if ((sp != null && sp.SoLuong > 0))
                {
                    listSP.Add(sp);
                }
            }
                
            sqlReader.Close();
            sqlConnection.Close();
        }

        private void DataGridCell_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            DataGridCell myCell = sender as DataGridCell;
            DataGridRow row = DataGridRow.GetRowContainingElement(myCell);
            SanPhamTheoNgay temp = row.DataContext as SanPhamTheoNgay;
            if (temp != null && temp.NgayBan != null)
            {
                ThongTinKH_SP window = new ThongTinKH_SP(temp.MaSP1,temp.ThanhTien1, temp.NgayBan);
                window.ShowDialog();
            }
            else
            {
                ThongTinKH_SP window = new ThongTinKH_SP(temp.MaSP1, temp.ThanhTien1);
                window.ShowDialog();
            }
        }

        public void refreshData()
        {
            listSPTN.Clear();
            getDataSP_KH();
            getData();
            chuyenDuLieu();
            DataGridNhap.ItemsSource = null;
            DataGridNhap.ItemsSource = listSPTN;
            //DataGridNhap.Items.Refresh();
        }

        //Tìm kiếm
        private void textBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            resetTxtSearch.Visibility = Visibility.Visible;
            if (listSPSearch.Count > 0)
            {
                //Nếu tìm kiếm sản phẩm khác thì reset lại list sản phẩm tìm kiếm
                listSPSearch.Clear();
            }
            if (txtSearch.Text.Trim() == "")
            {
                resetTxtSearch.Visibility = Visibility.Collapsed;
                DataGridNhap.ItemsSource = null;
                DataGridNhap.ItemsSource = listSPTN;

            }
            else
            {
                for (int i = 0; i < listSP.Count; i++)
                {
                    string str = listSP[i].TenSP.Trim().ToLower().Replace(" ", "");
                    //Tách chuỗi nhập vào thành các từ
                    string expression = txtSearch.Text.ToLower();
                    string[] token = expression.Split(new string[] { " " }, StringSplitOptions.RemoveEmptyEntries);

                    int index = 0;
                    //Duyệt qua mảng các từ trong chuỗi nhập
                    for (; index < token.Length; index++)
                    {
                        //Xóa khoảng trắng
                        token[index] = token[index].Trim();
                        //Kiểm tra từ đó xuất hiện trong tên sản phẩm không
                        int pos = listSP[i].TenSP.ToLower().IndexOf(token[index]);
                        //Nếu không xuất hiện thì
                        if (pos < 0)
                        {
                            break;
                        }
                    }
                    if (index == token.Length)
                    {
                        listSPSearch.Add(listSP[i]);
                    }
                }

                DataGridNhap.ItemsSource = null;
                DataGridNhap.ItemsSource = listSPSearch;
            }

        }
        
        private void resetTxtSearch_Click(object sender, RoutedEventArgs e)
        {
            txtSearch.Text = "";
        }

        private void LocaleDatePicker_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            BindingTheoNgay();
        }

        private void Label_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            listSP.Clear();
            listSP_KH.Clear();
            listSPTN.Clear();
            getDataSP_KH();
            getData();
            chuyenDuLieu();
            //DataGridNhap.ItemsSource = null;
            DataGridNhap.ItemsSource = listSPTN;
            tongTien();
        }
    }
}   