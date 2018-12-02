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
                sp.HinhAnhSP = sqlReader.GetString(2).Trim();
                sp.Size = sqlReader.GetString(3).Trim();
                sp.SoLuong = sqlReader.GetInt32(4);
                sp.Gia = sqlReader.GetFloat(5);
                sp.NgayNhap = sqlReader.GetDateTime(6);
                listSP.Add(sp);
            }
            sqlReader.Close();
            sqlConnection.Close();
        }

    }
}
