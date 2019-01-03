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
using System.Data.SqlClient;
using System.Data;
using System.Collections.ObjectModel;
namespace SalesManagement.ManHinhNhap
{
    /// <summary>
    /// Interaction logic for XoaSanPham.xaml
    /// </summary>
    ///  ObservableCollection<SanPham> listSP = new ObservableCollection<SanPham>();
 
    public partial class XoaSanPham : Window
    {
        SqlConnection sqlConnection = null;
        ObservableCollection<SanPham> listSP = new ObservableCollection<SanPham>();
        public XoaSanPham()
        {
            InitializeComponent();
            getData();
            comboBox1.ItemsSource = listSP;

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

        private void comboBox1_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
           
            for (int i = 0; i < listSP.Count; i++)
            {
                if (comboBox1.SelectedItem == listSP[i])
                {
                    txtMaSP.Text = listSP[i].MaSP;
                    txtTenSP.Text = listSP[i].TenSP;
                    txtSize.Text = listSP[i].Size;
                    txtGia.Text = listSP[i].Gia.ToString();
                    txtSoLuong.Text = listSP[i].SoLuong.ToString();
                    datePicker.Text = listSP[i].NgayNhap.ToString();
                    txtBoxLyDo.Text = listSP[i].DoiTra;
                    if(listSP[i].DoiTra.Trim() != "" )
                    {
                        checkBox1.IsChecked = true;
                    }
                    else
                    {
                        checkBox1.IsChecked = false;
                    }
                    BitmapImage bm = new BitmapImage();
                    bm.BeginInit();
                    bm.UriSource = new Uri(listSP[i].HinhAnhSP.Trim(), UriKind.RelativeOrAbsolute);
                    bm.EndInit();
                    HinhAnhSP.Source = bm;

           

                }       
            }
        }

        private void btnDelete_Click(object sender, RoutedEventArgs e)
        {
            //Duyệt sản phẩm cần xóa
            for(int i=0;i<listSP.Count;i++)
            {
                //Nếu là sản phẩm muốn xóa
                if (comboBox1.SelectedItem == listSP[i])
                {
                    SqlCommand sqlCommand = new SqlCommand();
                    try
                    {
                        //Kết nối tới CSDL
                        connectSQL(App.sqlString, out sqlConnection);
                        
                        sqlCommand.CommandType = CommandType.Text;
                        //Câu lệnh xóa
                        string sql = "delete from SanPham where MaSP='"+ listSP[i].MaSP+ "'";
                        sqlCommand.CommandText = sql;
                        sqlCommand.Connection = sqlConnection;
                        //sqlCommand.Parameters.Add("@maSP", SqlDbType.NChar).Value = listSP[i].MaSP ;
                        int ret = sqlCommand.ExecuteNonQuery();//hàm thực thi
                        if (ret > 0)
                        {
                            MessageBox.Show("Xóa thành công");
                            txtGia.Text = "";
                            txtMaSP.Text = "";
                            txtSoLuong.Text = "";
                            txtTenSP.Text = "";
                            txtSize.Text = "";
                            datePicker.Text = "";
                           
                        }
                        else
                        {
                            MessageBox.Show("Xóa không thành công!!");
                        }

                    }
                    catch (Exception)
                    {
                        MessageBox.Show(" Xóa không thành công");
                    }
                    finally
                    {
                        if (sqlConnection.State == ConnectionState.Open)
                            sqlConnection.Close();
                        sqlCommand.Cancel();
                        //Cập nhật lại danh sách sản phẩm
                        listSP.Clear();
                        getData();                      
                        comboBox1.ItemsSource = null;
                        comboBox1.ItemsSource = listSP;
                        
                    }
                }
            }
           
        }

        private void btnExit_Click(object sender, RoutedEventArgs e)
        {
            xoaSP.Close();
            
        }
    }
  
}
