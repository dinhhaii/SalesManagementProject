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

namespace SalesManagement.ManHinhNhap
{
    /// <summary>
    /// Interaction logic for Nhap.xaml
    /// </summary>
    public partial class Nhap : UserControl
    {
        SqlConnection sqlConnection = null;
        ObservableCollection<SanPham> listSP = new ObservableCollection<SanPham>();
        public bool isDelete { get; set; }
        public bool isEditing { get; set; }

        public Nhap()
        {
            InitializeComponent();
            getData();
            isEditing = false;
            isDelete = false;
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
                if (!sqlReader.IsDBNull(7))
                {
                    sp.DoiTra = sqlReader.GetString(7);
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
            DataGridNhap.ItemsSource = null;
            DataGridNhap.ItemsSource = listSP;
        }

        private void btnDeleteProduct_Click(object sender, RoutedEventArgs e)
        {
            if (!isDelete)
            {
                isDelete = true;
                status.Text = "";
                DataGridNhap.Columns[0].Visibility = Visibility.Visible;
            }
            else
            {
                DataGridNhap.Columns[0].Visibility = Visibility.Collapsed;
                isDelete = false;
            }
           
        }

        private void btnEditProduct_Click(object sender, RoutedEventArgs e)
        {
            isEditing = true;
            status.Text = "";
        }

        private void DataGridCell_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            //Nếu ở chế độ chỉnh sửa
            if (isDelete)
            {
                //Lấy Cell được chọn
                DataGridCell myCell = sender as DataGridCell;       
                DataGridRow row = DataGridRow.GetRowContainingElement(myCell);

                int index = row.GetIndex();
                SanPham itemRow = row.DataContext as SanPham;
                for(int i = 0; i < listSP.Count; i++)
                {
                    if (itemRow == listSP[i])
                    {
                        //listSP.RemoveAt(i);
                        MessageBoxResult result = MessageBox.Show("Bạn có chắc chắn muốn xóa sản phẩm " + itemRow.TenSP + " chứ?", "Sales Management",MessageBoxButton.YesNo,MessageBoxImage.Hand);
                        if (result == MessageBoxResult.Yes)
                        {
                            SqlCommand sqlCmd = new SqlCommand();
                            try
                            {
                                connectSQL(App.sqlString, out sqlConnection);
                                sqlCmd.CommandType = CommandType.Text;
                                sqlCmd.CommandText = "DELETE FROM SanPham WHERE SanPham.MaSP = '" + itemRow.MaSP + "'";
                                sqlCmd.Connection = sqlConnection;
                                int ret = sqlCmd.ExecuteNonQuery();
                                if (ret > 0)
                                {
                                    status.Text = "Xoá thành công";
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
                                sqlCmd.Cancel();
                            }
                        }
                        break;
                    }
                }

                //Reload dữ liệu
                listSP.Clear();
                getData();
                
                DataGridNhap.ItemsSource = null;
                DataGridNhap.ItemsSource = listSP;

            }
        }
    }
}