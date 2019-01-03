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
using SalesManagement.ManHinhNhap;

namespace SalesManagement
{
    /// <summary>
    /// Interaction logic for Nhap.xaml
    /// </summary>
    public partial class Nhap : UserControl
    {
        SqlConnection sqlConnection = null;
        ObservableCollection<SanPham> listSP = new ObservableCollection<SanPham>();

        public Nhap()
        {
            InitializeComponent();
            getData();
            DataGridNhap.ItemsSource = listSP;
            
            
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
                if (!sqlReader.IsDBNull(2))
                {
                    sp.HinhAnhSP = sqlReader.GetString(2).Trim();
                }
                else
                    sp.HinhAnhSP = "";
                sp.Size = sqlReader.GetString(3).Trim();
                sp.SoLuong = sqlReader.GetInt32(4);
                sp.Gia = sqlReader.GetFloat(5);
                sp.NgayNhap = sqlReader.GetDateTime(6);
                if (!sqlReader.IsDBNull(7))
                {
                    sp.DoiTra = sqlReader.GetString(7).Trim();
                }
                else
                    sp.DoiTra = "";
                listSP.Add(sp);
            }
            sqlReader.Close();
            sqlConnection.Close();
        }

        private void btnAddProduct_Click(object sender, RoutedEventArgs e)
        {
            ThemSanPham window = new ThemSanPham();
           
            window.ShowDialog();
            
        
        }

        private void btnDeleteProduct_Click(object sender, RoutedEventArgs e)
        {
            
            XoaSanPham xoaSanPham = new XoaSanPham();
            xoaSanPham.ShowDialog();
           
        }

        
        private void btnEditProduct_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
